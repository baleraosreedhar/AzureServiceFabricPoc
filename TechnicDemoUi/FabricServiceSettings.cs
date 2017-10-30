using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicDemoUi
{
    /// <summary>
    /// 
    /// </summary>
    public class FabricServiceSettings
    {
        /// <summary>
        /// Gets or sets the azure fabric client connection.
        /// </summary>
        /// <value>
        /// The azure fabric client connection.
        /// </value>
        public string AzureFabricClientConnection { get; set; }
        /// <summary>
        /// Gets or sets the azure fabric cluster connection.
        /// </summary>
        /// <value>
        /// The azure fabric cluster connection.
        /// </value>
        public string AzureFabricClusterConnection { get; set; }
        /// <summary>
        /// Gets or sets the name of the azure application.
        /// </summary>
        /// <value>
        /// The name of the azure application.
        /// </value>
        public string AzureApplicationName { get; set; }
        /// <summary>
        /// Gets or sets the name of the azure service.
        /// </summary>
        /// <value>
        /// The name of the azure service.
        /// </value>
        public string AzureServiceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the azure resource.
        /// </summary>
        /// <value>
        /// The name of the azure resource.
        /// </value>
        public string AzureResourceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the azure resource action.
        /// </summary>
        /// <value>
        /// The name of the azure resource action.
        /// </value>
        public string AzureResourceActionName { get; set; }
        /// <summary>
        /// Gets or sets the kind of the partion.
        /// </summary>
        /// <value>
        /// The kind of the partion.
        /// </value>
        public string PartionKind { get; set; }
        /// <summary>
        /// Gets or sets the partition key.
        /// </summary>
        /// <value>
        /// The partition key.
        /// </value>
        public string PartitionKey { get; set; }
        /// <summary>
        /// Gets or sets the filter terms.
        /// </summary>
        /// <value>
        /// The filter terms.
        /// </value>
        public string FilterTerms { get; set; }

        /// <summary>
        /// Gets or sets the name of the azure count service.
        /// </summary>
        /// <value>
        /// The name of the azure count service.
        /// </value>
        public string AzureCountServiceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the azure count service resource.
        /// </summary>
        /// <value>
        /// The name of the azure count service resource.
        /// </value>
        public string AzureCountServiceResourceName { get; set; }
        /// <summary>
        /// Gets or sets the name of the azure count service resource action.
        /// </summary>
        /// <value>
        /// The name of the azure count service resource action.
        /// </value>
        public string AzureCountServiceResourceActionName { get; set; }
    }
}
