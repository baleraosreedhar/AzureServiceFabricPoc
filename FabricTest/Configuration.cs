using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricTest
{
    public static class Configuration
    {
        public static string Current
        {
            get
            {
#if DEBUG
                var mode = "Debug";
#else
                var mode = "Release";
#endif
                return mode;
            }
        }
    }
