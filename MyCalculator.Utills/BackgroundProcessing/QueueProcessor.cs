using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace MyUtils.BackgroundProcessing
{
    /// <summary>
    /// Background queueu processor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.IDisposable" />
    public abstract class QueueProcessor<T> : IDisposable where T : class
    {
        //private static readonly ILogger<QueueProcessor> Logger = FocusLog.GetCurrentClassLogger();

        #region Fields

        /// <summary>
        /// The interval (in seconds) to use for the queue timer.
        /// </summary>
        protected int _sleepInterval;

        /// <summary>
        /// The timer
        /// </summary>
        private System.Timers.Timer _timer;
        /// <summary>
        /// The thread count
        /// </summary>
        private int _threadCount;
        /// <summary>
        /// The manual reset event
        /// </summary>
        private ManualResetEvent _manualResetEvent;

        /// <summary>
        /// The stopped
        /// </summary>
        protected volatile bool _stopped;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProcessor{T}"/> class.
        /// </summary>
        /// <param name="sleepInterval">The sleep interval.</param>
        protected QueueProcessor(int sleepInterval)
        {
            _sleepInterval = sleepInterval;
            if (sleepInterval > 0)
            {
                _timer = new System.Timers.Timer();
                _timer.Interval = sleepInterval * 1000;
                _timer.Elapsed += timer_Elapsed;
            }

            _manualResetEvent = new ManualResetEvent(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_timer != null)
            {
                _timer.Enabled = false;
                _timer.Dispose();
            }

            if (_manualResetEvent != null)
                _manualResetEvent.Close();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _stopped = false;
            ThreadPool.QueueUserWorkItem(ProcessQueue);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public virtual void Stop()
        {
            _stopped = true;
            //Logger.Info("Service stopped.");
            _manualResetEvent.WaitOne(60000, false);
        }

        /// <summary>
        /// Handles the Elapsed event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="ea">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void timer_Elapsed(object sender, ElapsedEventArgs ea)
        {
            _timer.Enabled = false;

            if (!_stopped)
                ThreadPool.QueueUserWorkItem(ProcessQueue);
        }

        /// <summary>
        /// Processes the queue.
        /// </summary>
        /// <param name="state">The state.</param>
        private void ProcessQueue(object state)
        {
            try
            {
                Interlocked.Increment(ref _threadCount);
                _manualResetEvent.Reset();
               // Logger.Debug(string.Format("Starting the queue, thread count is {0}", _threadCount));

                bool firstIteration = true;

                bool @break = false;
                for (; ; )
                {
                    if (_stopped)
                        break;
                    try
                    {
                        if (firstIteration)
                        {
                            ResetVolatileData();
                            firstIteration = false;
                        }

                        T item = GetWorkItem();
                        if (_stopped)
                        {
                            if (item != null)
                                UnlockItemWithoutProcessing(item);
                            @break = true;
                        }
                        else if (item != null)
                        {
                            Interlocked.Increment(ref _threadCount);

                            //Logger.Debug(string.Format("Got work item {1}, thread count is {0}", _threadCount, item));
                            //Logger.Debug(string.Format("Starting task for work item {0}", item));

                            var task = Task.Factory.StartNew((obj) => ProcessItem((T)obj), item);

                            task.ContinueWith(t =>
                            {
                                //Logger.Debug(string.Format("Finished process item for item {0}.", t.AsyncState));
                                if (Interlocked.Decrement(ref _threadCount) == 0)
                                {
                                   // Logger.Debug("Thread count was 0, triggering the manual reset event.");
                                    _manualResetEvent.Set();
                                }
                               // Logger.Debug(string.Format("Thread count is now {0}", _threadCount));
                            });
                        }
                        else
                        {
                           // Logger.Debug("Did not get an item, setting up timer.");
                            if (_timer != null)
                                _timer.Enabled = true;
                            @break = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.Error("An error ocurred in ProcessQueue", ex);
                        //Logger.Error(ex.Message, ex);
                        //Logger.Error(ex.StackTrace, ex);
                        //Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
                        if (_timer != null && !_stopped)
                            _timer.Enabled = true;

                        @break = true;
                    }

                    if (@break)
                        break;
                }

                if (Interlocked.Decrement(ref _threadCount) == 0)
                {
                   // Logger.Info(string.Format("Thread count is now {0}", _threadCount));
                    _manualResetEvent.Set();
                }
                else if (_timer == null)
                {
                   // Logger.Info(string.Format("Thread count is now {0}", _threadCount));
                    _manualResetEvent.WaitOne();
                }
                //else
                   // Logger.Info(string.Format("Thread count is now {0}", _threadCount));
            }
            catch (Exception ex)
            {
               // Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Resets the volatile data.
        /// </summary>
        protected abstract void ResetVolatileData();

        /// <summary>
        /// Gets the work item.
        /// </summary>
        /// <returns></returns>
        protected abstract T GetWorkItem();

        /// <summary>
        /// Processes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        protected abstract void ProcessItem(T item);

        /// <summary>
        /// Unlocks the item without processing.
        /// </summary>
        /// <param name="item">The item.</param>
        protected abstract void UnlockItemWithoutProcessing(T item);
    }
}
