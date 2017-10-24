using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyUtils.Extensions.StringExtension;

namespace MyUtils.Extensions.UriExtension
{
    /// <summary>
    /// URi Extensions
    /// </summary>
    public static class Ext
    {
        /// <summary>
        /// Converts a string to <see cref="Uri" />
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>

        public static Uri ToUri(this string uri)
        {
            StringExtension.GaurdExtensions.GuardNull(uri, nameof(uri));

            return new UriBuilder(uri).Uri;
        }

        /// <summary>
        /// Converts a string to a relative <see cref="Uri" />, with the other given string representing base uri
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>

        public static Uri ToUri(this string uri, string baseUri)
        {
            StringExtension.GaurdExtensions.GuardNull(uri, nameof(uri));
            StringExtension.GaurdExtensions.GuardNull(baseUri, nameof(baseUri));

            return new Uri(ToUri(baseUri), new Uri(uri, UriKind.Relative));
        }

        /// <summary>
        /// Converts a string to relative <see cref="Uri" />, with the given base <see cref="Uri" />
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <returns></returns>

        public static Uri ToUri(this string uri, Uri baseUri)
        {
            StringExtension.GaurdExtensions.GuardNull(uri, nameof(uri));
            StringExtension.GaurdExtensions.GuardNull(baseUri, nameof(baseUri));

            return new Uri(baseUri, new Uri(uri, UriKind.Relative));
        }

        /// <summary>
        /// Returns URL encoded version of a string
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>

        public static string UrlEncode(this string data)
        {
            StringExtension.GaurdExtensions.GuardNull(data, nameof(data));

            return WebUtility.UrlEncode(data);
        }

        /// <summary>
        /// Returns URL decoded version of a string
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>

        public static string UrlDecode(this string data)
        {
            StringExtension.GaurdExtensions.GuardNull(data, nameof(data));

            return WebUtility.UrlDecode(data);
        }

        /// <summary>
        /// Sets the given query parameter to the given value in a uri string
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>

        public static string SetQueryParameter(this string uri, string key, string value)
        {
            StringExtension.GaurdExtensions.GuardNull(uri, nameof(uri));
            StringExtension.GaurdExtensions.GuardNull(key, nameof(key));

            if (value == null)
                value = string.Empty;

            // Find existing parameter
            var existingMatch = Regex.Match(uri, $@"[?&]({Regex.Escape(key)}=?.*?)(?:&|/|$)");

            // Parameter already set to something
            if (existingMatch.Success)
            {
                var group = existingMatch.Groups[1];

                // Remove existing
                uri = uri.Remove(group.Index, group.Length);

                // Insert new one
                uri = uri.Insert(group.Index, $"{key}={value}");

                return uri;
            }
            // Parameter hasn't been set yet
            else
            {
                // See if there are other parameters
                bool hasOtherParams = uri.IndexOf('?') >= 0;

                // Prepend either & or ? depending on that
                char separator = hasOtherParams ? '&' : '?';

                // Assemble new query string
                return uri + separator + key + '=' + value;
            }
        }

        /// <summary>
        /// Sets the given query parameter to the given value in a <see cref="Uri" />
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>

        public static Uri SetQueryParameter(this Uri uri, string key, string value)
            => ToUri(SetQueryParameter(uri.ToString(), key, value));
    }
}
