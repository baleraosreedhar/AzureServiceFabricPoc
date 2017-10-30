using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Fabric;
using System.Threading.Tasks;
using System.Fabric.Health;
using System.Linq;

namespace FabricTest
{
    [TestClass]
    public class ApplicationHealthTests
    {
        private FabricClient _fabricClient;
        [TestMethod]
        public async System.Threading.Tasks.Task TestMethod1Async()
        {
            var applicationHealth = await GetApplicationHealth()
                                       .ConfigureAwait(false);

            Assert.AreEqual(applicationHealth.AggregatedHealthState, HealthState.Ok);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task ShouldHaveHealthyMyStatefullService()
        {
            var uriBuilder = new UriBuilder(Initialize.ApplicationUri);
            uriBuilder.Path += "/TechnicCharacterCountService";
            var serviceHealthState = await GetServiceHealthState(uriBuilder.Uri).ConfigureAwait(false);
            Assert.AreEqual(serviceHealthState, (HealthState.Ok));
        }
        private async Task<ApplicationHealth> GetApplicationHealth()
        {
            var applicationHealth =
                await _fabricClient.HealthManager
                    .GetApplicationHealthAsync(Initialize.ApplicationUri)
                    .ConfigureAwait(false);

            return applicationHealth;
        }

        private async Task<HealthState?> GetServiceHealthState(Uri serviceUri)
        {
            return (await GetApplicationHealth().ConfigureAwait(false))
                        .ServiceHealthStates
                        .FirstOrDefault(x => x.ServiceName == serviceUri)
                        ?.AggregatedHealthState;
        }
    }
}
