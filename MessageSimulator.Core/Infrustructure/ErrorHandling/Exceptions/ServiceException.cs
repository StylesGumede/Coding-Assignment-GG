using System;
using MessageSimulator.Core.Infrustructure.Application;

namespace MessageSimulator.Core.Infrustructure.ErrorHandling.Exceptions
{
    /// <summary>
    /// Custom <see cref="Exception"/> raised in <see cref="IApplicationService"/> types.
    /// </summary>
    public class ServiceException: Exception
    {
        public ServiceException(string message) : base(message)
        {
        }
    }
}