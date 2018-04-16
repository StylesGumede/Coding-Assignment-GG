namespace MessageSimulator.Core.Domain
{
    /// <summary>
    /// Extends the basic <see cref="IMessageFeedSubscriber"/> interface.
    /// </summary>
    /// <typeparam name="TMessageFeed"></typeparam>
    public interface IMessageFeedSubscriber<TMessageFeed>: IMessageFeedSubscriber
        where TMessageFeed : IMessageFeed, new()
    {
        /// <summary>
        /// Generic <see cref="TMessageFeed"/> based on the <see cref="IMessageFeed"/> interface.
        /// </summary>
        TMessageFeed MessageFeed { get; }

        /// <summary>
        /// Event handler message to handle added messages to the <see cref="IMessageFeedSubscriber"/>'s 
        /// feed.
        /// </summary>
        /// <param name="message">A valid message with an owner.</param>
        void OnMessageAdded(Message message);
    }

    /// <summary>
    /// Defines a basic <see cref="IMessageFeedSubscriber"/> structure.
    /// </summary>
    public interface IMessageFeedSubscriber
    {
        string Name { get; }
    }
}