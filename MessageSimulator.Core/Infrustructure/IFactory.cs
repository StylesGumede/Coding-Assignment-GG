namespace MessageSimulator.Core.Infrustructure
{
    /// <summary>
    /// Defines a single generic factory method to create an instance of a given generic type.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="T"/>
        /// </summary>
        /// <typeparam name="T">Generic type parameter</typeparam>
        /// <returns>An instance of <see cref="T"/></returns>
        T CreateInstanceOf<T>() where T : class;
    }
}
