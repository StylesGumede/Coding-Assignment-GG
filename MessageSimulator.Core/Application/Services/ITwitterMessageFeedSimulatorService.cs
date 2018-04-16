using MessageSimulator.Core.Infrustructure.Application;

namespace MessageSimulator.Core.Application.Services
{
    /// <summary>
    /// Twitter message feed <see cref="IApplicationService"/>.
    /// </summary>
    public interface ITwitterMessageFeedSimulatorService: IApplicationService
    {
        /// <summary>
        /// Runs a Twitter message feed simulation.
        /// </summary>
        /// <param name="request">A valid <see cref="TwitterMessageFeedSimulatorServiceRequest"/></param>
        /// <returns></returns>
        TwitterMessageFeedSimulatorServiceResponse RunSimulation(
            TwitterMessageFeedSimulatorServiceRequest request);
    }
}