namespace MessageSimulator.Core.Application.Services
{
    /// <summary>
    /// <see cref="ITwitterMessageFeedSimulatorService"/> request.
    /// </summary>
    public class TwitterMessageFeedSimulatorServiceRequest
    {
        public string UsersInputFilePath { get; set; } = string.Empty;
        public string MessagesInputFilePath { get; set; } = string.Empty;
    }
}