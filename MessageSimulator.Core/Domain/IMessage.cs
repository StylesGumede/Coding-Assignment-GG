namespace MessageSimulator.Core.Domain
{
    /// <summary>
    /// Defines the basic structure of a message.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The owner of the message.
        /// </summary>
        string Owner { get; }

        /// <summary>
        /// The actual message.
        /// </summary>
        string Text { get; }
    }
}