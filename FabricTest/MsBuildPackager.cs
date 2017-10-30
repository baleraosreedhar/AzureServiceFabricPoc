using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricTest
{
    public class MsBuildPacker
    {
        public void Pack(string projPath)
        {
            var msbuildPath = GetMsBuildToolPath();
            var process = Process.Start($@"{msbuildPath}\msbuild.exe",
                $@" ""{projPath}"" /t:Package /p:Configuration={Configuration.Current},Platform=x64");

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception("msbuild package failed");
            }
        }

        private string GetMsBuildToolPath()
        {
            return (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0",
                "MSBuildToolsPath", null);
        }
    }
}
