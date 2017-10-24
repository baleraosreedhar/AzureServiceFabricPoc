using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUtils.Extensions.LinqExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LinqExtensionsExt
    {
        /// <summary>
        /// Selects the try.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static IEnumerable<SelectTryResult<TSource, TResult>> SelectTry<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector)
        {
            foreach (TSource element in enumerable)
            {
                SelectTryResult<TSource, TResult> returnedValue;
                try
                {
                    returnedValue = new SelectTryResult<TSource, TResult>(element, selector(element), null);
                }
                catch (Exception ex)
                {
                    returnedValue = new SelectTryResult<TSource, TResult>(element, default(TResult), ex);
                }
                yield return returnedValue;
            }
        }

        /// <summary>
        /// Called when [caught exception].
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> OnCaughtException<TSource, TResult>(this IEnumerable<SelectTryResult<TSource, TResult>> enumerable, Func<Exception, TResult> exceptionHandler)
        {
            return enumerable.Select(x => x.CaughtException == null ? x.Result : exceptionHandler(x.CaughtException));
        }

        /// <summary>
        /// Called when [caught exception].
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> OnCaughtException<TSource, TResult>(this IEnumerable<SelectTryResult<TSource, TResult>> enumerable, Func<TSource, Exception, TResult> exceptionHandler)
        {
            return enumerable.Select(x => x.CaughtException == null ? x.Result : exceptionHandler(x.Source, x.CaughtException));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        public class SelectTryResult<TSource, TResult>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SelectTryResult{TSource, TResult}"/> class.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="result">The result.</param>
            /// <param name="exception">The exception.</param>
            internal SelectTryResult(TSource source, TResult result, Exception exception)
            {
                Source = source;
                Result = result;
                CaughtException = exception;
            }

            /// <summary>
            /// Gets the source.
            /// </summary>
            /// <value>
            /// The source.
            /// </value>
            public TSource Source { get; private set; }

            /// <summary>
            /// Gets the result.
            /// </summary>
            /// <value>
            /// The result.
            /// </value>
            public TResult Result { get; private set; }

            /// <summary>
            /// Gets the caught exception.
            /// </summary>
            /// <value>
            /// The caught exception.
            /// </value>
            public Exception CaughtException { get; private set; }
        }
    }
}
