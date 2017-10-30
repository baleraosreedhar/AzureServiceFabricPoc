using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FabricTest
{
    public class Initialize
    {
        private FabricApplicationDeployer _deployer;

        public static Uri ApplicationUri { get; private set; }

       
        public async Task SetUp()
        {
            var pathFinder = new PathFinder();
            var paths = pathFinder.Find("SfApplication");

            var applicationType = "SfApplicationType";
            var applicationTypeVersion = "1.0.0";

            var msBuildPackager = new MsBuildPacker();
            msBuildPackager.Pack(paths.SfProj);

            _deployer = new FabricApplicationDeployer(paths.SfPackagePath, applicationType, applicationTypeVersion);
            ApplicationUri = await _deployer.DeployAsync()
                                        .ConfigureAwait(false);
        }

        
        public async Task Kill()
        {
            await _deployer.RemoveAsync()
                .ConfigureAwait(false);
        }
    }
    public class ProjectPaths
    {
        public string SfProj { get; set; }

        public string SfPackagePath { get; set; }
    }
    public class PathFinder
    {
        public ProjectPaths Find(string projectName)
        {
            var codeBase = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var slnRoot = codeBase.Substring(0, codeBase.IndexOf(@"\FabricTest", StringComparison.Ordinal));

            var applicationRoot = $@"{slnRoot}\{projectName}";

            var applicationSfProj = $@"{applicationRoot}\{projectName}.sfproj";
            var packagePath = $@"{applicationRoot}\pkg\{Configuration.Current}";

            return new ProjectPaths()
            {
                SfProj = applicationSfProj,
                SfPackagePath = packagePath
            };
        }
    }
}
