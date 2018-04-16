using System.Configuration;

namespace MessageSimulator.Core.Infrustructure.Configuration
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}