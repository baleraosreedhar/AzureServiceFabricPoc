﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyUtils.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class DownloadHelper
    {
        /// <summary>
        /// Downloads the and extract all.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        public static async Task DownloadAndExtractAll(string uri, string dest)
        {
            var tmpFile = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString() + ".zip";
            Console.WriteLine("Downloading from " + uri);
            using (var webClient = new HttpClient() { Timeout = new TimeSpan(1, 0, 0), MaxResponseContentBufferSize = 1024 * 1024 * 50 })
            {
                var bytes = await webClient.GetByteArrayAsync(uri);
                File.WriteAllBytes(tmpFile, bytes);
                Console.WriteLine("Downloaded");
            }
            using (var fileStream = new FileStream(tmpFile, FileMode.Open))
            using (var archive = new ZipArchive(fileStream))
            {
                foreach (var x in archive.Entries)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(dest + x.FullName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(dest + x.FullName));
                    if (x.Length == 0 && string.IsNullOrEmpty(Path.GetExtension(x.FullName)))
                        continue;
                    using (var entryStream = x.Open())
                    using (var destStream = File.OpenWrite(dest + x.FullName))
                    {
                        entryStream.CopyTo(destStream);
                    }
                }
            }
            File.Delete(tmpFile);
        }

        /// <summary>
        /// Downloads the and extract.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Could not find file '" + file.Item1 + "'.</exception>
        public static async Task DownloadAndExtract(
            string uri,
            params Tuple<string, string>[] files)
        {
            var tmpFile = Path.GetTempPath() + "pomelo_" + Guid.NewGuid().ToString() + ".zip";
            Console.WriteLine("Downloading from " + uri);
            using (var webClient = new HttpClient() { Timeout = new TimeSpan(1, 0, 0), MaxResponseContentBufferSize = 1024 * 1024 * 50 })
            {
                var bytes = await webClient.GetByteArrayAsync(uri);
                File.WriteAllBytes(tmpFile, bytes);
                Console.WriteLine("Downloaded");
            }
            using (var fileStream = new FileStream(tmpFile, FileMode.Open))
            using (var archive = new ZipArchive(fileStream))
            {
                foreach (var file in files)
                {
                    var entry = archive.GetEntry(file.Item1);
                    if (entry == null)
                        throw new Exception("Could not find file '" + file.Item1 + "'.");

                    Directory.CreateDirectory(Path.GetDirectoryName(file.Item2));

                    using (var entryStream = entry.Open())
                    using (var dllStream = File.OpenWrite(file.Item2))
                    {
                        entryStream.CopyTo(dllStream);
                    }
                }
            }
            File.Delete(tmpFile);
        }
    }
}
