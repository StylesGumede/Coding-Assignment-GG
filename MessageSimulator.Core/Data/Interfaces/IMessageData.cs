using System.Collections.Generic;
using MessageSimulator.Core.Domain;
using MessageSimulator.Core.Infrustructure.Data;

namespace MessageSimulator.Core.Data.Interfaces
{
    /// <summary>
    /// Exposes functionality to read <see cref="TMessage"/> data 
    /// from an external data source.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageData<TMessage> :
        IDataAccess, INotifiable 
        where TMessage : IMessage
    {
        /// <summary>
        /// Returns a collection of <see cref="TMessage"/>s given a valid 
        /// <see cref="filePath"/>.
        /// </summary>
        /// <param name="filePath">File path of a file containing <see cref="TMessage"/> data.</param>
        /// <returns>A collection of <see cref="TMessage"/>s</returns>
        IEnumerable<TMessage> GetMessages(string filePath);
    }
}