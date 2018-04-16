using MessageSimulator.Core.Infrustructure.Domain.Exceptions;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions;

namespace MessageSimulator.Core.Domain
{
    /// <summary>
    /// Base class for all <see cref="IMessageFeedSubscriber{TMessageFeed}"/> types.
    /// </summary>
    /// <typeparam name="TMessageFeed"></typeparam>
    public abstract class MessageFeedSubscriber<TMessageFeed> : IMessageFeedSubscriber<TMessageFeed>
        where TMessageFeed : IMessageFeed, new()
    {
        /// <summary>
        /// <para>Creates an instance of <see cref="IMessageFeedSubscriber{TMessageFeed}"/>.</para>
        /// <para></para>
        /// <para>Throws a <see cref="DomainException"/> if <see cref="name"/> is invalid.</para>
        /// </summary>
        /// <param name="name">The name of the <see cref="IMessageFeedSubscriber"/></param>
        protected MessageFeedSubscriber(string name)
        {
            name.ThrowOnNullEmptyOrWhitespace<DomainException>(nameof(name), this.GetType());

            this.Name = name;
            this.MessageFeed = new TMessageFeed();
            this.MessageFeed.RegisterOwner(name);
        }

        public string Name { get; }

        public void OnMessageAdded(Message message)
        {
            this.MessageFeed.UpdateFeedWithSubscriberMessage(message);
        }

        public TMessageFeed MessageFeed { get; private set; }
    }
}