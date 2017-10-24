using System;

namespace MyUtils.Extensions.StringExtension
{
    /// <summary>
    /// Gaurd extension for checking lenths
    /// </summary>
    public static class GaurdExtensions
    {
        /// <summary>
        /// Generic string parse delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public delegate T ParseDelegate<out T>(string str);

        /// <summary>
        /// Generic string try-parse delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str">The string.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public delegate bool TryParseDelegate<T>(string str, out T result);

        /// <summary>
        /// Guards the null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void GuardNull<T>(
            T value, string name,
            string message = null)
        {
            if (value != null)
                return;

            if (message == null)
                message = $"Parameter [{name}] cannot be null";

            throw new ArgumentNullException(name, message);
        }

        /// <summary>
        /// Guards the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void GuardRange<T>(T value, T min, T max, string name, string message = null)
            where T : IComparable
        {
            if (value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0)
                return;

            if (message == null)
                message = $"Parameter [{name}] needs to be inside range [{min} -> {max}]";

            throw new ArgumentOutOfRangeException(name, message);
        }

        /// <summary>
        /// Guards the minimum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void GuardMin<T>(T value, T min, string name, string message = null) where T : IComparable
        {
            if (value.CompareTo(min) >= 0)
                return;

            if (message == null)
                message = $"Parameter [{name}] needs to be greater than or equal to [{min}]";

            throw new ArgumentOutOfRangeException(name, message);
        }

        /// <summary>
        /// Guards the maximum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void GuardMax<T>(T value, T max, string name, string message = null) where T : IComparable
        {
            if (value.CompareTo(max) <= 0)
                return;

            if (message == null)
                message = $"Parameter [{name}] needs to be lesser than or equal to [{max}]";

            throw new ArgumentOutOfRangeException(name, message);
        }

        /// <summary>
        /// Guards the condition.
        /// </summary>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void GuardCondition( bool condition,
            string name, string message = null)
        {
            if (!condition)
                return;

            if (message == null)
                message = $"Parameter [{name}] is invalid";

            throw new ArgumentException(message, name);
        }

       
    }
}