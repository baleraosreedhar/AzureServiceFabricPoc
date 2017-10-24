using System;
using System.Diagnostics;

namespace MyUtils
{
    public static class Ensure
    {
        [DebuggerStepThrough]
        public static void ArgumentNotNull(object argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }
    }
}
