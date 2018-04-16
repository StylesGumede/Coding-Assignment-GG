using System.Collections.Generic;
using MessageSimulator.Core.Domain;
using MessageSimulator.Core.Infrustructure.Data;

namespace MessageSimulator.Core.Data.Interfaces
{
    /// <summary>
    /// Exposes functionality to read <see cref="TUser"/> data 
    /// from an external data source.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public interface IUserData<TUser>: 
        IDataAccess, INotifiable 
        where TUser : IMessageFeedSubscriber
    {
        /// <summary>
        /// Returns a collection of <see cref="TUser"/>s given a valid 
        /// <see cref="filePath"/>.
        /// </summary>
        /// <param name="filePath">File path of a file containing <see cref="TUser"/> data.</param>
        /// <returns>A collection of <see cref="TUser"/>s</returns>
        IEnumerable<TUser> GetUsers(string filePath);
    }
}