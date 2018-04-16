namespace MessageSimulator.Core.Infrustructure.Configuration
{
    /// <summary>
    /// Provides access to application configuration.
    /// </summary>
    public interface IApplicationConfiguration: IInfrustructureType
    {
        /// <summary>
        /// Returns a configuration value given a valid <see cref="key"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A configuration value</returns>
        string GetValue(string key);
    }
}