using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MyUtils
{
    public static class SentenceExtensions
    {
        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }
    }
}
