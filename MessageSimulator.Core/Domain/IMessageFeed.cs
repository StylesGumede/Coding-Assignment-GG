using System;
using System.Collections.Generic;

namespace MessageSimulator.Core.Domain
{
    /// <summary>
    /// Defines the basic structure of a Message Feed. 
    /// Exposes methods for <see cref="IMessageFeedSubscriber"/>s to subscribe and 
    /// unsubscribe to a <see cref="IMessageFeed"/>.
    /// </summary>
    public interface IMessageFeed
    {
        /// <summary>
        /// A collection of subscribers that are subscribed to the <see cref="IMessageFeed"/>.
        /// </summary>
        IEnumerable<string> Subscribers { get; }

        /// <summary>
        /// The owner of the <see cref="IMessageFeed"/>
        /// </summary>
        string Owner { get; }

        /// <summary>
        /// An event that is fired when a message is added to the <see cref="IMessageFeed"/>.
        /// </summary>
        event Action<Message> MessageAdded;

        /// <summary>
        /// Registers a subscriber with the feed.
        /// </summary>
        /// <typeparam name="TMessageFeed">Generic Message feed type</typeparam>
        /// <param name="subscriber">The subscriber interested in registering with 
        /// the <see cref="IMessageFeed"/></param>
        void RegisterSubscriber<TMessageFeed>(IMessageFeedSubscriber<TMessageFeed> subscriber)
            where TMessageFeed : IMessageFeed, new();

        /// <summary>
        /// Uregisters a subscriber with the feed.
        /// </summary>
        /// <typeparam name="TMessageFeed">Generic Message feed type</typeparam>
        /// <param name="subscriber">The subscriber interested in unregistering with 
        /// the <see cref="IMessageFeed"/></param>
        void UnregisterSubscriber<TMessageFeed>(IMessageFeedSubscriber<TMessageFeed> subscriber)
            where TMessageFeed : IMessageFeed, new();

        /// <summary>
        /// Updates <see cref="IMessageFeed"/> with subscribers <see cref="Message"/>
        /// </summary>
        /// <param name="message">A valid message with an owner.</param>
        void UpdateFeedWithSubscriberMessage(Message message);

        /// <summary>
        /// Assigns an owner to the <see cref="IMessageFeed"/>.
        /// </summary>
        /// <param name="name">Name of <see cref="IMessageFeed"/> owner.</param>
        void RegisterOwner(string name);
    }
}