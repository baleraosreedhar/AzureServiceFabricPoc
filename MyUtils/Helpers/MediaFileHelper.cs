////using System;
////using System.Collections.Generic;
////using System.Diagnostics;
////using System.IO;
////using System.Linq;
////using System.Runtime.InteropServices;
////using System.Threading.Tasks;
////usingMyUtilsn;
////usingMyUtilsn.Media;

////namespace MyUtils.Helpers
////{
////    public enum OSType
////    {
////        Windows,
////        OSX,
////        Linux
////    }

////    public static class Platform
////    {
////        public static OSType OS
////        {
////            get
////            {
////#if !NET451
////                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
////                    return OSType.Windows;
////                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
////                    return OSType.OSX;
////                else
////                    return OSType.Linux;
////#else
////                if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32S || Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.WinCE)
////                    return OSType.Windows;
////                else if (Environment.OSVersion.Platform == PlatformID.MacOSX)
////                    return OSType.OSX;
////                else
////                    return OSType.Linux;
////#endif
////            }
////        }
////    }

////    public enum Quality
////    {
////        Smallest,
////        Medium,
////        Best
////    };

////    /// <summary>
////    /// 
////    /// </summary>
////    public abstract class MediaFile
////    {
////        /// <summary>
////        /// Initializes a new instance of the <see cref="Domain.MediaFile"/> class.
////        /// </summary>
////        protected MediaFile()
////        {
////            ID = Guid.NewGuid();
////        }

////        /// <summary>
////        /// Gets or sets the source.
////        /// </summary>
////        /// <value>
////        /// The source.
////        /// </value>
////        protected string _source { get; set; }

////        /// <summary>
////        /// Gets or sets a value indicating whether this instance is temporary.
////        /// </summary>
////        /// <value>
////        /// <c>true</c> if this instance is temporary; otherwise, <c>false</c>.
////        /// </value>
////        public bool IsTemp { get; set; }

////        /// <summary>
////        /// 文件路径
////        /// </summary>
////        /// <value>
////        /// The source.
////        /// </value>
////        public string Source => _source;

////        /// <summary>
////        /// Gets or sets the identifier.
////        /// </summary>
////        /// <value>
////        /// The identifier.
////        /// </value>
////        public Guid ID { get; set; }
////        /// <summary>
////        /// Gets or sets all bytes.
////        /// </summary>
////        /// <value>
////        /// All bytes.
////        /// </value>
////        public virtual byte[] AllBytes { get; set; }

////        /// <summary>
////        /// 另存为
////        /// </summary>
////        /// <param name="path">The path.</param>
////        public void SaveAs(string path)
////        {
////            System.IO.File.WriteAllBytes(path, AllBytes);
////        }

////        /// <summary>
////        /// Saves as asynchronous.
////        /// </summary>
////        /// <param name="path">The path.</param>
////        /// <returns></returns>
////        public Task SaveAsAsync(string path)
////        {
////            return Task.Factory.StartNew(() => SaveAs(path));
////        }

////        /// <summary>
////        /// Moves to.
////        /// </summary>
////        /// <param name="path">The path.</param>
////        public void MoveTo(string path)
////        {
////            System.IO.File.WriteAllBytes(path, AllBytes);
////            System.IO.File.Delete(_source);
////            _source = path;
////        }

////        /// <summary>
////        /// Moves to asynchronous.
////        /// </summary>
////        /// <param name="path">The path.</param>
////        /// <returns></returns>
////        public Task MoveToAsync(string path)
////        {
////            return Task.Factory.StartNew(() => MoveTo(path));
////        }
////    }

////    public class Image : MediaFile
////    {
////        /// <summary>
////        /// 构造函数
////        /// </summary>
////        /// <param name="bytes">图片文件字节集</param>
////        /// <param name="extension">扩展名（如.png）</param>
////        public Image(byte[] bytes, string extension)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + extension;
////            System.IO.File.WriteAllBytes(fname, bytes);
////            _source = fname;
////            var info = MediaHelper.GetImageInfo(fname);
////            Width = info.Width;
////            Height = info.Height;
////        }

////        public Image(string src)
////        {
////            _source = src;
////            var info = MediaHelper.GetImageInfo(src);
////            Width = info.Width;
////            Height = info.Height;
////        }

////        public int Width { get; set; }

////        public int Height { get; set; }

////        public override byte[] AllBytes
////        {
////            get
////            {
////                return System.IO.File.ReadAllBytes((string)base._source);
////            }
////        }

////        /// <summary>
////        /// 转换图片格式
////        /// </summary>
////        /// <param name="Extension">目标图片扩展名，如(.png)</param>
////        /// <returns></returns>
////        public Image Convert(string Extension)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Extension;
////            if (!MediaHelper.ImageFormatConvert(_source, fname))
////                return null;
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> ConvertAsync(string Extension)
////        {
////            return Task.Factory.StartNew(() => Convert(Extension));
////        }

////        /// <summary>
////        /// 转换图片格式
////        /// </summary>
////        /// <param name="Extension">目标图片扩展名，如(.png)</param>
////        /// <param name="Width">宽度（px）</param>
////        /// <param name="Height">高度（px）</param>
////        /// <returns></returns>
////        public Image Convert(string Extension, int Width, int Height)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Extension;
////            if (!MediaHelper.ImageFormatConvert(_source, fname, Width, Height))
////                return null;
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> ConvertAsync(string Extension, int Width, int Height)
////        {
////            return Task.Factory.StartNew(() => Convert(Extension, Width, Height));
////        }

////        /// <summary>
////        /// 转换图片格式
////        /// </summary>
////        /// <param name="Extension">扩展名（如.png）</param>
////        /// <param name="Width">宽度（px）</param>
////        /// <returns></returns>
////        public Image ConvertByWidth(string Extension, int Width)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Extension;
////            if (!MediaHelper.ImageFormatConvert(_source, fname, Width, Width * this.Height / this.Width))
////                return null;
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> ConvertByWidthAsync(string Extension, int Width, int Height)
////        {
////            return Task.Factory.StartNew(() => ConvertByWidth(Extension, Width));
////        }

////        /// <summary>
////        /// 转换图片格式
////        /// </summary>
////        /// <param name="Extension">扩展名（如.png）</param>
////        /// <param name="Height">高度（px）</param>
////        /// <returns></returns>
////        public Image ConvertByHeight(string Extension, int Height)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Extension;
////            if (!MediaHelper.ImageFormatConvert(_source, fname, Height * this.Width / this.Height, Height))
////                return null;
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> ConvertByHeightAsync(string Extension, int Height)
////        {
////            return Task.Factory.StartNew(() => ConvertByHeight(Extension, Height));
////        }

////        /// <summary>
////        /// 调整大小
////        /// </summary>
////        /// <param name="Width">宽度（px）</param>
////        /// <param name="Height">高度（px）</param>
////        /// <returns></returns>
////        public Image Resize(int Width, int Height)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.ImageFormatConvert(_source, fname, Width, Height))
////                return null;
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> ResizeAsync(int Width, int Height)
////        {
////            return Task.Factory.StartNew(() => Resize(Width, Height));
////        }

////        /// <summary>
////        /// 根据宽度等比例调整尺寸
////        /// </summary>
////        /// <param name="Width">宽度（px）</param>
////        /// <returns></returns>
////        public Image ResizeByWidth(int Width)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.ImageFormatConvert(_source, fname, Width, Width * this.Height / this.Width))
////                return null;
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> ResizeByWidthAsync(int Width)
////        {
////            return Task.Factory.StartNew(() => ResizeByWidth(Width));
////        }

////        /// <summary>
////        /// 根据高度等比例调整尺寸
////        /// </summary>
////        /// <param name="Height">高度（px）</param>
////        /// <returns></returns>
////        public Image ResizeByHeight(int Height)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.ImageFormatConvert(_source, fname, Height * this.Width / this.Height, Height))
////                return null;
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> ResizeByHeightAsync(int Height)
////        {
////            return Task.Factory.StartNew(() => ResizeByHeight(Height));
////        }

////        ~Image()
////        {
////            if (IsTemp)
////            {
////                try
////                {
////                    System.IO.File.Delete((string)base._source);
////                }
////                catch
////                {
////                }
////            }
////        }
////    }

////    public class VideoInfo
////    {
////        /// <summary>
////        /// 片长
////        /// </summary>
////        public TimeSpan Duration { get; set; }

////        /// <summary>
////        /// 比特率
////        /// </summary>
////        public int Biterate { get; set; }

////        /// <summary>
////        /// 宽(px)
////        /// </summary>
////        public int Width { get; set; }

////        /// <summary>
////        /// 高(px)
////        /// </summary>
////        public int Height { get; set; }
////    }



////    public class Video : MediaFile
////    {
////        public readonly VideoInfo Info;

////        /// <summary>
////        /// 构造函数
////        /// </summary>
////        /// <param name="bytes">视频文件字节集</param>
////        /// <param name="extension">扩展名（如.png）</param>
////        public Video(byte[] bytes, string extension)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + extension;
////            System.IO.File.WriteAllBytes(fname, bytes);
////            _source = fname;
////            Info = MediaHelper.GetImageInfo(fname);
////        }

////        /// <summary>
////        /// 
////        /// </summary>
////        /// <param name="src">视频源文件</param>
////        public Video(string src)
////        {
////            if (!System.IO.File.Exists(src))
////                throw new Exception(src + " Not Found.");
////            _source = src;
////            Info = MediaHelper.GetVideoInfo(src);
////        }

////        /// <summary>
////        /// 获取截图
////        /// </summary>
////        /// <param name="timeoff">时间（秒）</param>
////        /// <returns></returns>
////        public Image GetFrame(double timeoff = 1.0)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            System.IO.File.WriteAllBytes((string)fname, MediaHelper.GetFrame((string)base._source, timeoff));
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> GetFrameAsync(double timeoff = 1.0)
////        {
////            return Task.Factory.StartNew(() => GetFrame(timeoff));
////        }

////        /// <summary>
////        /// 获取截图
////        /// </summary>
////        /// <param name="width">宽(px)</param>
////        /// <param name="height">高(px)</param>
////        /// <param name="timeoff">时间（秒）</param>
////        /// <returns></returns>
////        public Image GetFrame(int width, int height, double timeoff = 1.0)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            System.IO.File.WriteAllBytes((string)fname, MediaHelper.GetFrame((string)base._source, width, height, timeoff));
////            var ret = new Image(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Image> GetFrameAsync(int width, int height, double timeoff = 1.0)
////        {
////            return Task.Factory.StartNew(() => GetFrame(width, height, timeoff));
////        }

////        /// <summary>
////        /// 抽取整个影片帧
////        /// </summary>
////        /// <param name="timeoff">时间（秒）</param>
////        /// <returns></returns>
////        public List<Image> GetFrames(int timeoff = 1)
////        {
////            var result = new List<Image>();
////            for (var i = 0; i < Info.Duration.TotalSeconds; i += timeoff)
////            {
////                try
////                {
////                    var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
////                    System.IO.File.WriteAllBytes(fname, MediaHelper.GetFrame(_source, i));
////                    var frame = new Image(fname);
////                    frame.IsTemp = true;
////                    result.Add(frame);
////                }
////                catch (Exception e)
////                {
////                    System.Diagnostics.Debug.WriteLine("System caught an exception:");
////                    System.Diagnostics.Debug.WriteLine(e.ToString());
////                }
////            }
////            return result;
////        }

////        public Task<List<Image>> GetFramesAsync(int timeoff = 1)
////        {
////            return Task.Factory.StartNew(() => GetFrames(timeoff));
////        }

////        /// <summary>
////        /// 抽取整个影片帧
////        /// </summary>
////        /// <param name="width">宽(px)</param>
////        /// <param name="height">高(px)</param>
////        /// <param name="timeoff">时间（秒）</param>
////        /// <returns></returns>
////        public List<Image> GetFrames(int width, int height, int timeoff = 1)
////        {
////            var result = new List<Image>();
////            for (var i = 0; i <= Info.Duration.TotalSeconds; i += timeoff)
////            {
////                var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
////                System.IO.File.WriteAllBytes(fname, MediaHelper.GetFrame(_source, width, height, i));
////                var frame = new Image(fname);
////                frame.IsTemp = true;
////                result.Add(frame);
////            }
////            return result;
////        }

////        public Task<List<Image>> GetFramesAsync(int width, int height, int timeoff = 1)
////        {
////            return Task.Factory.StartNew(() => GetFrames(width, height, timeoff));
////        }

////        /// <summary>
////        /// 从影片中抽取指定数目的帧
////        /// </summary>
////        /// <param name="Count">数量</param>
////        /// <returns></returns>
////        public List<Image> GetFramesByCount(int Count)
////        {
////            var result = new List<Image>();
////            double sec = (int)Info.Duration.TotalSeconds;
////            var timeoff = sec / Count;
////            for (double i = 0.0; i < Info.Duration.TotalSeconds; i += timeoff)
////            {
////                try
////                {
////                    var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
////                    System.IO.File.WriteAllBytes(fname, MediaHelper.GetFrame(_source, i));
////                    var frame = new Image(fname);
////                    frame.IsTemp = true;
////                    result.Add(frame);
////                }
////                catch (Exception e)
////                {
////                    System.Diagnostics.Debug.WriteLine("System caught an exception:");
////                    System.Diagnostics.Debug.WriteLine(e.ToString());
////                }
////            }
////            return result;
////        }

////        public Task<List<Image>> GetFramesByCountAsync(int Count)
////        {
////            return Task.Factory.StartNew(() => GetFramesByCount(Count));
////        }

////        /// <summary>
////        /// 从影片中抽取指定数目的帧
////        /// </summary>
////        /// <param name="Count">数量</param>
////        /// <param name="width">宽度(px)</param>
////        /// <param name="height">高度(px)</param>
////        /// <returns></returns>
////        public List<Image> GetFramesByCount(int Count, int width, int height)
////        {
////            var result = new List<Image>();
////            double sec = (int)Info.Duration.TotalSeconds;
////            var timeoff = sec / Count;
////            for (var i = 0.0; i < Info.Duration.TotalSeconds; i += timeoff)
////            {
////                try
////                {
////                    var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
////                    System.IO.File.WriteAllBytes(fname, MediaHelper.GetFrame(_source, width, height, i));
////                    var frame = new Image(fname);
////                    frame.IsTemp = true;
////                    result.Add(frame);
////                }
////                catch (Exception e)
////                {
////                    System.Diagnostics.Debug.WriteLine("System caught an exception:");
////                    System.Diagnostics.Debug.WriteLine(e.ToString());
////                }
////            }
////            return result;
////        }

////        public Task<List<Image>> GetFramesByCountAsync(int Count, int width, int height)
////        {
////            return Task.Factory.StartNew(() => GetFramesByCount(Count, width, height));
////        }

////        /// <summary>
////        /// 视频文件字节集
////        /// </summary>
////        public override byte[] AllBytes
////        {
////            get { return System.IO.File.ReadAllBytes((string)base._source); }
////        }

////        /// <summary>
////        /// 向后连接一个视频
////        /// </summary>
////        /// <param name="file">目标文件</param>
////        /// <returns></returns>
////        public Video PushBack(Video file, Quality Quality)
////        {
////            var src = new List<string>();
////            src.Add(_source);
////            src.Add(file._source);
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            try
////            {
////                MediaHelper.Concat(src, fname, Quality);
////            }
////            catch
////            {
////                return null;
////            }
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> PushBackAsync(Video file, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => PushBack(file, Quality));
////        }

////        /// <summary>
////        /// 向前连接视频文件
////        /// </summary>
////        /// <param name="file">目标文件</param>
////        /// <returns></returns>
////        public Video PushFront(Video file, Quality Quality)
////        {
////            var src = new List<string>();
////            src.Add(file._source);
////            src.Add(_source);
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            try
////            {
////                MediaHelper.Concat(src, fname, Quality);
////            }
////            catch
////            {
////                return null;
////            }
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> PushFrontAsync(Video file, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => PushFront(file, Quality));
////        }

////        /// <summary>
////        /// 截取视频
////        /// </summary>
////        /// <param name="Begin">起始时间（秒）</param>
////        /// <param name="Length">截取时间</param>
////        /// <returns></returns>
////        public Video Intercept(double Begin, TimeSpan Length, Quality Quality)
////        {
////            if (Begin + Length.TotalSeconds > Info.Duration.TotalSeconds)
////                throw new Exception("超出了影片长度");
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            var flag = false;
////            try
////            {
////                flag = MediaHelper.Intercept(_source, fname, Begin, Length, Quality);
////            }
////            catch
////            {
////                return null;
////            }
////            if (!flag) return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> InterceptAsync(double Begin, TimeSpan Length, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => Intercept(Begin, Length, Quality));
////        }

////        /// <summary>
////        /// 切分影片
////        /// </summary>
////        /// <param name="Interval">周期</param>
////        /// <returns></returns>
////        public List<Video> Split(TimeSpan Interval, Quality Quality)
////        {
////            var ret = new List<Video>();
////            var TotalSeconds = Info.Duration.TotalSeconds;
////            var Begin = 0;
////            while (TotalSeconds > 0)
////            {
////                if (Interval.TotalSeconds > TotalSeconds)
////                {
////                    Interval = Interval - new TimeSpan(0, 0, System.Convert.ToInt32(TotalSeconds));
////                }
////                var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////                var tmp = MediaHelper.Intercept(_source, fname, Begin, Interval, Quality);
////                if (tmp)
////                {
////                    var tmp2 = new Video(fname);
////                    tmp2.IsTemp = true;
////                    ret.Add(tmp2);
////                }
////                TotalSeconds -= Interval.TotalSeconds;
////                Begin += int.Parse(Interval.TotalSeconds.ToString());
////            }
////            return ret;
////        }

////        public Task<List<Video>> SplitAsync(TimeSpan Interval, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => Split(Interval, Quality));
////        }

////        /// <summary>
////        /// 转换格式
////        /// </summary>
////        /// <param name="format">扩展名（带有点）</param>
////        /// <returns></returns>
////        public Video Convert(string format, Quality Quality)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + format;
////            if (!MediaHelper.FormatConvert(Source, fname, Quality))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ConvertAsync(string format, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => Convert(format, Quality));
////        }

////        /// <summary>
////        /// 转换格式并限制尺寸
////        /// </summary>
////        /// <param name="format">扩展名（带有点）</param>
////        /// <param name="width">宽度(px)</param>
////        /// <param name="height">高度(px)</param>
////        /// <returns></returns>
////        public Video Convert(string format, int width, int height, Quality Quality)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + format;
////            if (!MediaHelper.FormatConvert(Source, fname, width, height, Quality))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ConvertAsync(string format, int width, int height, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => Convert(format, width, height, Quality));
////        }

////        /// <summary>
////        /// 压缩应聘啊
////        /// </summary>
////        /// <param name="width">宽度(px)</param>
////        /// <param name="height">高度(px)</param>
////        /// <returns></returns>
////        public Video Compress(int width, int height, Quality Quality)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, width, height, Quality))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> CompressAsync(int width, int height, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => Compress(width, height, Quality));
////        }

////        /// <summary>
////        /// 导出240P
////        /// </summary>
////        /// <returns></returns>
////        public Video ExportTo240P()
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, System.Convert.ToInt32(Info.Width * 240 / Info.Height), 240, Quality.Smallest))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ExportTo240PAsync()
////        {
////            return Task.Factory.StartNew(() => ExportTo240P());
////        }


////        /// <summary>
////        /// 导出360P
////        /// </summary>
////        /// <returns></returns>
////        public Video ExportTo360P()
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, System.Convert.ToInt32(Info.Width * 360 / Info.Height), 360, Quality.Smallest))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ExportTo360PAsync()
////        {
////            return Task.Factory.StartNew(() => ExportTo360P());
////        }

////        /// <summary>
////        /// 导出480P
////        /// </summary>
////        /// <returns></returns>
////        public Video ExportTo480P()
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, System.Convert.ToInt32(Info.Width * 480 / Info.Height), 480, Quality.Smallest))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ExportTo480PAsync()
////        {
////            return Task.Factory.StartNew(() => ExportTo480P());
////        }

////        /// <summary>
////        /// 导出720P
////        /// </summary>
////        /// <returns></returns>
////        public Video ExportTo720P()
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, System.Convert.ToInt32(Info.Width * 720 / Info.Height), 720, Quality.Medium))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ExportTo720PAsync()
////        {
////            return Task.Factory.StartNew(() => ExportTo720P());
////        }

////        /// <summary>
////        /// 导出1080P
////        /// </summary>
////        /// <returns></returns>
////        public Video ExportTo1080P()
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, System.Convert.ToInt32(Info.Width * 1080 / Info.Height), 1080, Quality.Best))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ExportTo1080PAsync()
////        {
////            return Task.Factory.StartNew(() => ExportTo1080P());
////        }

////        /// <summary>
////        /// 导出2K
////        /// </summary>
////        /// <returns></returns>
////        public Video ExportTo2K()
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, System.Convert.ToInt32(Info.Width * 1440 / Info.Height), 1440, Quality.Best))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ExportTo2KPAsync()
////        {
////            return Task.Factory.StartNew(() => ExportTo2K());
////        }

////        /// <summary>
////        /// 导出4K
////        /// </summary>
////        /// <returns></returns>
////        public Video ExportTo4K()
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.FormatConvert(Source, fname, System.Convert.ToInt32(Info.Width * 2160 / Info.Height), 2160, Quality.Best))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> ExportTo4KPAsync()
////        {
////            return Task.Factory.StartNew(() => ExportTo4K());
////        }

////        /// <summary>
////        /// 截取影片
////        /// </summary>
////        /// <param name="Begin">开始时间（秒）</param>
////        /// <param name="Length">截取时常</param>
////        /// <returns></returns>
////        public Video Intercept(int Begin, TimeSpan Length, Quality Quality)
////        {
////            var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(_source);
////            if (!MediaHelper.Intercept(Source, fname, Begin, Length, Quality))
////                return null;
////            var ret = new Video(fname);
////            ret.IsTemp = true;
////            return ret;
////        }

////        public Task<Video> InterceptAsync(int Begin, TimeSpan Length, Quality Quality)
////        {
////            return Task.Factory.StartNew(() => Intercept(Begin, Length, Quality));
////        }

////        ~Video()
////        {
////            if (IsTemp)
////            {
////                try
////                {
////                    System.IO.File.Delete((string)base._source);
////                }
////                catch
////                {
////                }
////            }
////        }
////    }

////    public static class MediaHelper
////    {
////        public static string[] Args =
////        {
////            "-qscale 32 -ab 56 -ar 11025 -b 500k  -r 15",
////            "-qscale 8 -ab 72 -ar 22050 -b 800k  -r 25.97",
////            "-qscale 2 -ab 96 -ar 44100 -b 1500k  -r 29.97"
////        };

////        private static string FfmpegPath;

////        static MediaHelper()
////        {
////            var TmpPath = Path.GetTempPath();

////            if (System.Platform.OS == OSType.Windows)
////            {
////                FfmpegPath = TmpPath + @"ffmpeg.exe";
////                if (!File.Exists(FfmpegPath))
////                {
////                    DownloadHelper.DownloadAndExtract("https://github.com/PomeloResources/Ffmpeg/archive/windows.zip",
////                        Tuple.Create("Ffmpeg-windows/ffmpeg.exe", FfmpegPath)).Wait();
////                }
////            }
////            else if (System.Platform.OS == OSType.OSX)
////            {
////                FfmpegPath = TmpPath + @"ffmpeg";
////                if (!File.Exists(FfmpegPath))
////                {
////                    DownloadHelper.DownloadAndExtract("https://github.com/PomeloResources/Ffmpeg/archive/osx.zip",
////                        Tuple.Create("Ffmpeg-osx/ffmpeg", FfmpegPath)).Wait();
////                }
////            }
////            else
////            {
////                FfmpegPath = TmpPath + @"ffmpeg";
////                if (!File.Exists(FfmpegPath))
////                {
////                    DownloadHelper.DownloadAndExtract("https://github.com/PomeloResources/Ffmpeg/archive/linux.zip",
////                        Tuple.Create("Ffmpeg-linux/ffmpeg", FfmpegPath)).Wait();
////                }
////            }
////            if (System.Platform.OS != OSType.Windows)
////            {
////                Process p = new Process();
////                p.StartInfo = new ProcessStartInfo
////                {
////                    UseShellExecute = false,
////                    FileName = "chmod",
////                    Arguments = "u+x \"" + FfmpegPath + "\"",
////                    RedirectStandardError = true
////                };
////                Console.WriteLine("chmoding");
////                p.Start();
////                p.WaitForExit();
////                Console.WriteLine(p.StandardError.ReadToEnd());
////            }
////        }

       
////        public static bool FormatConvert(string src, string dest, int width, int height, Quality Quality)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            if (!System.IO.File.Exists(src))
////            {
////                throw new Exception("The video file is not exist: " + src);
////            }
////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            FilestartInfo.Arguments = " -i \"" + src + "\" " + Args[Convert.ToInt32(Quality)] + " -s " + width + "x" + height + " \"" + dest + "\"";
////            try
////            {
////                var proc = System.Diagnostics.Process.Start(FilestartInfo);
////                proc.WaitForExit();
////            }
////            catch
////            {
////                return false;
////            }
////            return true;
////        }

////        public static bool ImageFormatConvert(string src, string dest, int width, int height)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            if (!System.IO.File.Exists(src))
////            {
////                throw new Exception("The video file is not exist: " + src);
////            }
////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            FilestartInfo.Arguments = " -i \"" + src + "\" " + " -f image2 -s " + width + "x" + height + " \"" + dest + "\"";
////            try
////            {
////                var proc = System.Diagnostics.Process.Start(FilestartInfo);
////                proc.WaitForExit();
////            }
////            catch
////            {
////                return false;
////            }
////            return true;
////        }
       
////        public static bool FormatConvert(string src, string dest, Quality Quality)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            if (!System.IO.File.Exists(src))
////            {
////                throw new Exception("The video file is not exist: " + src);
////            }
////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            FilestartInfo.Arguments = " -i \"" + src + "\" " + Args[Convert.ToInt32(Quality)] + " \"" + dest + "\"";
////            try
////            {
////                var proc = System.Diagnostics.Process.Start(FilestartInfo);
////                proc.WaitForExit();
////            }
////            catch
////            {
////                return false;
////            }
////            return true;
////        }

////        public static bool ImageFormatConvert(string src, string dest)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            if (!System.IO.File.Exists(src))
////            {
////                throw new Exception("The video file is not exist: " + src);
////            }
////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            FilestartInfo.Arguments = " -i \"" + src + "\" -f image2 " + " \"" + dest + "\"";
////            try
////            {
////                var proc = System.Diagnostics.Process.Start(FilestartInfo);
////                proc.WaitForExit();
////            }
////            catch
////            {
////                return false;
////            }
////            return true;
////        }

       
////        public static byte[] GetFrame(string src, int width, int height, double timeoff = 1.0)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            if (!System.IO.File.Exists(src))
////            {
////                throw new Exception("The video file is not exist: " + src);
////            }
////            var _timeoff = timeoff.ToString("0.000");
////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            var PictureName = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
////            FilestartInfo.Arguments = String.Format("-i \"{0}\" -y -f image2 -ss {1} -t 0.001 -s {2}x{3} '{4}'", src, _timeoff, width, height, PictureName);
////            var proc = System.Diagnostics.Process.Start(FilestartInfo);
////            proc.WaitForExit();
////            var file = System.IO.File.ReadAllBytes(PictureName);
////            try
////            {
////                System.IO.File.Delete(PictureName);
////            }
////            catch
////            {
////            }
////            return file;
////        }

       
////        public static byte[] GetFrame(string src, double timeoff = 1.0)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            if (!System.IO.File.Exists(src))
////            {
////                throw new Exception("The video file is not exist: " + src);
////            }
////            var _timeoff = timeoff.ToString("0.000");
////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            var PictureName = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
////            FilestartInfo.Arguments = String.Format("-i \"{0}\" -ss {1} -t 0.001 -f image2 \"{2}\"", src, _timeoff, PictureName);
////            var proc = System.Diagnostics.Process.Start(FilestartInfo);
////            proc.WaitForExit();
////            var file = System.IO.File.ReadAllBytes(PictureName);
////            try
////            {
////                System.IO.File.Delete(PictureName);
////            }
////            catch
////            {
////            }
////            return file;
////        }

        
////        public static VideoInfo GetVideoInfo(string src)
////        {
////            using (System.Diagnostics.Process ffmpeg = new System.Diagnostics.Process())
////            {
////                String duration;
////                String result;
////                StreamReader errorreader;
////                ffmpeg.StartInfo.UseShellExecute = false;
////                ffmpeg.StartInfo.RedirectStandardError = true;
////                ffmpeg.StartInfo.FileName = FfmpegPath;
////                ffmpeg.StartInfo.Arguments = "-i \"" + src + "\"";
////                ffmpeg.Start();
////                errorreader = ffmpeg.StandardError;
////                ffmpeg.WaitForExit();
////                result = errorreader.ReadToEnd();
////                System.Diagnostics.Debug.WriteLine(result);
////                var ret = new VideoInfo();
////                duration = result.Substring(result.IndexOf("Duration: ") + ("Duration: ").Length, ("00:00:00").Length);
////                var tmp = duration.Split(':');
////                ret.Duration = new TimeSpan(int.Parse(tmp[0]), int.Parse(tmp[1]), int.Parse(tmp[2]));
////                var tmp2 = result.Split('\n');
////                for (var i = 0; i < tmp2.Length; i++)
////                {
////                    if (tmp2[i].IndexOf("Duration") >= 0)
////                    {
////                        var tmp3 = tmp2[i].Split(',');
////                        foreach (var s in tmp3)
////                        {
////                            if (s.IndexOf("bitrate:") >= 0)
////                            {
////                                ret.Biterate = Convert.ToInt32(s.Replace("bitrate:", "").Replace("kb/s", "").Trim(' '));
////                            }
////                        }
////                    }
////                    if (tmp2[i].IndexOf("Video:") >= 0)
////                    {
////                        var tmp3 = tmp2[i].Split(',');
////                        foreach (var s in tmp3)
////                        {
////                            if (s.IndexOf("SAR") >= 0)
////                            {
////                                var ss = s.Trim(' ');
////                                ret.Width = Convert.ToInt32(ss.Split(' ')[0].Split('x')[0]);
////                                ret.Height = Convert.ToInt32(ss.Split(' ')[0].Split('x')[1]);
////                            }
////                        }
////                    }
////                }
////                return ret;
////            }
////        }

////        public static VideoInfo GetImageInfo(string src)
////        {
////            using (System.Diagnostics.Process ffmpeg = new System.Diagnostics.Process())
////            {
////                String result;
////                StreamReader errorreader;
////                ffmpeg.StartInfo.UseShellExecute = false;
////                ffmpeg.StartInfo.RedirectStandardError = true;
////                ffmpeg.StartInfo.FileName = FfmpegPath;
////                ffmpeg.StartInfo.Arguments = "-i \"" + src + "\"";
////                ffmpeg.Start();
////                errorreader = ffmpeg.StandardError;
////                ffmpeg.WaitForExit();
////                result = errorreader.ReadToEnd();
////                System.Diagnostics.Debug.WriteLine(result);
////                var ret = new VideoInfo();
////                var tmp2 = result.Split('\n');
////                for (var i = 0; i < tmp2.Length; i++)
////                {
////                    if (tmp2[i].IndexOf("Video:") >= 0)
////                    {
////                        var tmp3 = tmp2[i].Split(',');
////                        foreach (var s in tmp3)
////                        {
////                            if (s.IndexOf("x") >= 0)
////                            {
////                                try
////                                {
////                                    var ss = s.Trim(' ');
////                                    ret.Width = Convert.ToInt32(ss.Split(' ')[0].Split('x')[0]);
////                                    ret.Height = Convert.ToInt32(ss.Split(' ')[0].Split('x')[1]);
////                                }
////                                catch { }
////                            }
////                        }
////                    }
////                }
////                return ret;
////            }
////        }

       
////        public static bool Concat(List<string> src, string dest, Quality Quality)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            var tmp = new Queue<string>();
////            foreach (var s in src)
////            {
////                var flag = false;
////                var fname = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString().Replace("-", "") + ".ts";
////                try
////                {
////                    flag = FormatConvert(s, fname, Quality);
////                }
////                catch
////                {
////                }
////                if (flag)
////                    tmp.Enqueue(fname);
////            }
////            var files = "";
////            foreach (var s in tmp)
////            {
////                files += s + "|";
////            }
////            files = files.Trim('|');

////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            FilestartInfo.Arguments = String.Format("\"concat: {0}\" -acodec copy -vcodec copy -absf aac_adtstoasc \"{1}\"", files, dest);
////            try
////            {
////                var proc = System.Diagnostics.Process.Start(FilestartInfo);
////                proc.WaitForExit();
////            }
////            catch
////            {
////                return false;
////            }
////            return true;
////        }

       
////        public static bool Intercept(string src, string dest, double Begin, TimeSpan Length, Quality Quality)
////        {
////            if (!System.IO.File.Exists(FfmpegPath))
////            {
////                throw new Exception("Ffmpeg not found: " + FfmpegPath);
////            }
////            if (!System.IO.File.Exists(src))
////            {
////                throw new Exception("The video file is not exist: " + src);
////            }
////            System.Diagnostics.ProcessStartInfo FilestartInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
////            FilestartInfo.Arguments = String.Format("-i \"{0}\" " + Args[Convert.ToInt32(Quality)] + " -ss {1} -t {2} \"{3}\"", src, Begin, Length, dest);
////            try
////            {
////                var proc = System.Diagnostics.Process.Start(FilestartInfo);
////                proc.WaitForExit();
////            }
////            catch
////            {
////                return false;
////            }
////            return true;
////        }
////    }
////}
