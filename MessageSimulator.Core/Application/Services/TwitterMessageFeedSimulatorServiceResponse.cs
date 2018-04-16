using MessageSimulator.Core.Infrustructure.Application;

namespace MessageSimulator.Core.Application.Services
{
    /// <summary>
    /// <see cref="ITwitterMessageFeedSimulatorService"/> response.
    /// </summary>
    public class TwitterMessageFeedSimulatorServiceResponse
    {
        public ServiceResult ServiceResult { get; set; } = ServiceResult.Default;
        public string Message { get; set; } = string.Empty;
    }
}