using System;
using MessageSimulator.Core.Infrustructure.Data;

namespace MessageSimulator.Core.Infrustructure.ErrorHandling.Exceptions
{
    /// <summary>
    /// Custom <see cref="Exception"/> raised in <see cref="IDataAccess"/> types.
    /// </summary>
    [Serializable]
    public class DataAccessException : Exception
    {
        public DataAccessException(string message) : base(message)
        {
        }
    }
}