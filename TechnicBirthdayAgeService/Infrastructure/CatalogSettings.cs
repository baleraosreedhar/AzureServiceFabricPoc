namespace TechnicBirthdayAgeService.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class CatalogSettings
    {
        /// <summary>
        /// Gets or sets the pic base URL.
        /// </summary>
        /// <value>
        /// The pic base URL.
        /// </value>
        public string PicBaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the event bus connection.
        /// </summary>
        /// <value>
        /// The event bus connection.
        /// </value>
        public string EventBusConnection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use customization data].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use customization data]; otherwise, <c>false</c>.
        /// </value>
        public bool UseCustomizationData { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [azure storage enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [azure storage enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool AzureStorageEnabled { get; set; }
    }
}