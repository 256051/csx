

namespace IdentityModel.Protocols
{
    /// <summary>
    /// Log messages and codes
    /// </summary>
    internal static class LogMessages
    {
#pragma warning disable 1591
        // general
        internal const string IDX20000 = "IDX20000: The parameter '{0}' cannot be a 'null' or an empty object.";

        // properties, configuration 
        internal const string IDX20106 = "IDX20106: When setting RefreshInterval, the value must be greater than MinimumRefreshInterval: '{0}'. value: '{1}'.";
        internal const string IDX20107 = "IDX20107: When setting AutomaticRefreshInterval, the value must be greater than MinimumAutomaticRefreshInterval: '{0}'. value: '{1}'.";
        internal const string IDX20108 = "IDX20108: The address specified '{0}' is not valid as per HTTPS scheme. Please specify an https address for security reasons. If you want to test with http address, set the RequireHttps property  on IDocumentRetriever to false.";

        // configuration retrieval errors
        internal const string IDX20803 = "IDX20803: Unable to obtain configuration from: '{0}'.";
        internal const string IDX20804 = "IDX20804: Unable to retrieve document from: '{0}'.";
        internal const string IDX20805 = "IDX20805: Obtaining information from metadata endpoint: '{0}'.";
        internal const string IDX20806 = "IDX20806: Unable to obtain an updated configuration from: '{0}'. Returning the current configuration.";
        internal const string IDX20807 = "IDX20807: Unable to retrieve document from: '{0}'. HttpResponseMessage: '{1}', HttpResponseMessage.Content: '{2}'.";
#pragma warning restore 1591
    }
}
