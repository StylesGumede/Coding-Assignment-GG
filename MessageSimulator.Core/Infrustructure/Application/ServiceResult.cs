using MessageSimulator.Core.Application.Services;

namespace MessageSimulator.Core.Infrustructure.Application
{
    /// <summary>
    /// Enumeration used by <see cref="IApplicationService"/> types to 
    /// report on the outcome of a <see cref="IApplicationService"/> 
    /// operation
    /// </summary>
    public enum ServiceResult
    {
        Default,
        Exception,
        Success
    }
}