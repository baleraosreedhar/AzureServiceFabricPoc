using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyUtils.Extensions.StringExtension
{
    /// <summary>
    /// string extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the string is either null, empty or whitespace
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is blank; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBlank(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Determines whether the string is neither null, empty or whitespace
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is not blank] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotBlank(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Determines whether the string only consists of digits
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string str)
        {
            GaurdExtensions.GuardNull(str, nameof(str));

            return str.ToCharArray().All(char.IsDigit);
        }

        /// <summary>
        /// Determines whether the string only consists of letters
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is alphabetic; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphabetic(this string str)
        {
            GaurdExtensions.GuardNull(str, nameof(str));

            return str.ToCharArray().All(char.IsLetter);
        }

        /// <summary>
        /// Determines whether the string only consists of letters and/or digits
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphanumeric(this string str)
        {
            GaurdExtensions.GuardNull(str, nameof(str));

            return str.ToCharArray().All(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Returns null if the given string is either null, empty or whitespace, otherwise returns the same string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string NullIfBlank(this string str)
        {
            return IsBlank(str) ? null : str;
        }

        /// <summary>
        /// Returns an empty string if the given string is null, otherwise returns the same string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string EmptyIfNull(this string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// Returns an empty string if the given string is either null, empty or whitespace, otherwise returns the same string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string EmptyIfBlank(this string str)
        {
            return IsBlank(str) ? string.Empty : str;
        }

        /// <summary>
        /// Formats the given string identically to <see cref="string.Format(string,object[])" />
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string Format(this string str, params object[] args)
        {
            GaurdExtensions.GuardNull(str, nameof(str));

            return string.Format(str, args);
        }

        /// <summary>
        /// Removes all leading occurrences of a substring in the given string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string TrimStart(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            while (str.StartsWith(sub, comparison))
                str = str.Substring(sub.Length);

            return str;
        }

        /// <summary>
        /// Removes all trailing occurrences of a substring in the given string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string TrimEnd(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            while (str.EndsWith(sub, comparison))
                str = str.Substring(0, str.Length - sub.Length);

            return str;
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a substring in the given string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string Trim(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            return str.TrimStart(sub).TrimEnd(sub);
        }

        /// <summary>
        /// Reverses order of characters in a string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            GaurdExtensions.GuardNull(str, nameof(str));

            if (str.Length <= 1)
                return str;

            var sb = new StringBuilder(str.Length);
            for (var i = str.Length - 1; i >= 0; i--)
                sb.Append(str[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Returns a string formed by repeating the given string given number of times
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Repeat(this string str, int count)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardMin(count, 0, nameof(count));

            if (count == 0)
                return string.Empty;

            // Optimization
            if (count == 1)
                return str;
            if (count == 2)
                return str + str;
            if (count == 3)
                return str + str + str;

            // StringBuilder for count >= 4
            var sb = new StringBuilder(str, str.Length*count);
            for (var i = 2; i <= count; i++)
                sb.Append(str);

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string formed by repeating the given character given number of times
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Repeat(this char c, int count)
        {
            GaurdExtensions.GuardMin(count, 0, nameof(count));

            if (count == 0)
                return string.Empty;

            return new string(c, count);
        }

        /// <summary>
        /// Truncates a string leaving only the given number of characters from the start of the string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Take(this string str, int count)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardMin(count, 0, nameof(count));

            if (count == 0) return string.Empty;
            if (count >= str.Length) return str;
            return str.Substring(0, count);
        }

        /// <summary>
        /// Truncates a string dropping the given number of characters from the start of the string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Skip(this string str, int count)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardMin(count, 0, nameof(count));

            if (count == 0) return str;
            if (count >= str.Length) return string.Empty;
            return str.Substring(count);
        }

        /// <summary>
        /// Truncates a string leaving only the given number of characters from the end of the string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string TakeLast(this string str, int count)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardMin(count, 0, nameof(count));

            if (count == 0) return string.Empty;
            if (count >= str.Length) return str;
            return Skip(str, str.Length - count);
        }

        /// <summary>
        /// Truncates a string dropping the given number of characters from the end of the string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string SkipLast(this string str, int count)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardMin(count, 0, nameof(count));

            if (count == 0) return str;
            if (count >= str.Length) return string.Empty;
            return Take(str, str.Length - count);
        }

        /// <summary>
        /// Removes all occurrences of the given substrings from a string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="substrings">The substrings.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string Except(this string str, IEnumerable<string> substrings,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(substrings, nameof(substrings));

            foreach (var sub in substrings)
            {
                var index = str.IndexOf(sub, comparison);
                while (index >= 0)
                {
                    str = str.Remove(index, sub.Length);
                    index = str.IndexOf(sub, comparison);
                }
            }

            return str;
        }

        /// <summary>
        /// Removes all occurrences of the given substrings from a string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="substrings">The substrings.</param>
        /// <returns></returns>
        public static string Except(this string str, params string[] substrings)
            => Except(str, (IEnumerable<string>) substrings);

        /// <summary>
        /// Removes all occurrences of the given characters from a string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="characters">The characters.</param>
        /// <returns></returns>
        public static string Except(this string str, IEnumerable<char> characters)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(characters, nameof(characters));

            var charArray = characters as char[] ?? characters.ToArray();
            var pos = str.IndexOfAny(charArray);
            while (pos >= 0)
            {
                str = str.Remove(pos, 1);
                pos = str.IndexOfAny(charArray);
            }
            return str;
        }

        /// <summary>
        /// Removes all occurrences of the given characters from a string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="characters">The characters.</param>
        /// <returns></returns>
        public static string Except(this string str, params char[] characters)
            => Except(str, (IEnumerable<char>) characters);

        /// <summary>
        /// Prepends a string if the given string doesn't start with it already
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string EnsureStartsWith(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            return str.StartsWith(sub, comparison) ? str : sub + str;
        }

        /// <summary>
        /// Appends a string if the given string doesn't end with it already
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            return str.EndsWith(sub, comparison) ? str : str + sub;
        }

        /// <summary>
        /// Retrieves a substring that ends at the position of first occurrence of the given other string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string SubstringUntil(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            var index = str.IndexOf(sub, comparison);
            if (index < 0) return str;
            return str.Substring(0, index);
        }

        /// <summary>
        /// Retrieves a substring that starts at the position of first occurrence of the given other string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string SubstringAfter(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            var index = str.IndexOf(sub, comparison);
            if (index < 0) return string.Empty;
            return str.Substring(index + sub.Length, str.Length - index - sub.Length);
        }

        /// <summary>
        /// Retrieves a substring that ends at the position of last occurrence of the given other string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparsion">The comparsion.</param>
        /// <returns></returns>
        public static string SubstringUntilLast(this string str, string sub,
            StringComparison comparsion = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            var index = str.LastIndexOf(sub, comparsion);
            if (index < 0) return str;
            return str.Substring(0, index);
        }

        /// <summary>
        /// Retrieves a substring that starts at the position of last occurrence of the given other string
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="sub">The sub.</param>
        /// <param name="comparsion">The comparsion.</param>
        /// <returns></returns>
        public static string SubstringAfterLast(this string str, string sub,
            StringComparison comparsion = StringComparison.Ordinal)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(sub, nameof(sub));

            var index = str.LastIndexOf(sub, comparsion);
            if (index < 0) return string.Empty;
            return str.Substring(index + sub.Length, str.Length - index - sub.Length);
        }

        /// <summary>
        /// Discards blank strings from a sequence
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static IEnumerable<string> ExceptBlank(this IEnumerable<string> enumerable)
        {
            GaurdExtensions.GuardNull(enumerable, nameof(enumerable));

            return enumerable.Where(IsNotBlank);
        }

        /// <summary>
        /// Splits string using given separators, discarding empty entries
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separators">The separators.</param>
        /// <returns></returns>
        public static string[] Split(this string str, params string[] separators)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(separators, nameof(separators));

            return str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Splits string using given separators, discarding empty entries
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separators">The separators.</param>
        /// <returns></returns>
        public static string[] Split(this string str, params char[] separators)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(separators, nameof(separators));

            return str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Returns a string formed by joining elements of a sequence using the given separator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string JoinToString<T>(this IEnumerable<T> enumerable, string separator)
        {
            GaurdExtensions.GuardNull(enumerable, nameof(enumerable));
            GaurdExtensions.GuardNull(separator, nameof(separator));

            return string.Join(separator, enumerable);
        }

        /// <summary>
        /// Parses the string into an object of given type using a <see cref="GaurdExtensions.ParseDelegate{T}" /> handler
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        public static T Parse<T>(this string str, GaurdExtensions.ParseDelegate<T> handler)
        {
            GaurdExtensions.GuardNull(str, nameof(str));
            GaurdExtensions.GuardNull(handler, nameof(handler));

            return handler(str);
        }

        ///// <summary>
        /////     Parses the string into an object of given type using a <see cref="TryParseDelegate{T}" /> handler or returns
        /////     default value if unsuccessful
        ///// </summary>
        //////public static T ParseOrDefault<T,  TResult>(this string str, TryParseDelegate<T> handler,
        //////    T defaultValue = default(T), TResult defaultResult = default(TResult))
        //////{
        //////    GuardNull(handler, nameof(handler));

        //////    return handler(str, out TResult) ?defaultValue;
        ////}
    }


}