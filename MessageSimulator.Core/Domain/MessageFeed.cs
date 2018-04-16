using System;
using System.Collections.Generic;
using MessageSimulator.Core.Infrustructure.Domain.Exceptions;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions;

namespace MessageSimulator.Core.Domain
{
    /// <summary>
    /// Base class for all <see cref="IMessageFeed"/> types.
    /// </summary>
    public abstract class MessageFeed : IMessageFeed
    {
        #region Private Fields

        private readonly List<Message> _messages;
        private readonly HashSet<string> _subscribers;

        #endregion

        #region Constructors

        protected MessageFeed()
        {
            this._messages = new List<Message>();
            this._subscribers = new HashSet<string>();
        }

        #endregion

        #region Events

        public event Action<Message> MessageAdded;

        #endregion

        #region Public Properties

        public string Owner { get; private set; }
        public IEnumerable<Message> Messages { get { return this._messages; } }
        public IEnumerable<string> Subscribers { get { return this._subscribers; } }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Will broad cast/notify all subscribers of this <see cref="IMessageFeed"/>
        /// </summary>
        /// <param name="message">A valid message with an owner.</param>
        protected void BroadcastAddedMessage(Message message)
        {
            this.MessageAdded?.Invoke(message);
        }

        /// <summary>
        /// Adds a valid <see cref="Message"/> to the <see cref="IMessageFeed"/>
        /// </summary>
        /// <param name="message">A valid message with an owner.</param>
        protected void AddMessage(Message message)
        {
            this._messages.Add(message);
        }

        #endregion

        #region Public Methods

        public void RegisterSubscriber<TMessageFeed>(IMessageFeedSubscriber<TMessageFeed> subscriber)
            where TMessageFeed : IMessageFeed, new()
        {
            subscriber.ThrowOnNull<DomainException, 
                IMessageFeedSubscriber<TMessageFeed>>(this.GetType(), "Invalid subscriber");

            if(this._subscribers.Contains(subscriber.Name))
                return;

            if(this.Owner == subscriber.Name)
                return;

            this._subscribers.Add(subscriber.Name);
            this.MessageAdded += subscriber.OnMessageAdded;
        }

        public void UnregisterSubscriber<TMessageFeed>(IMessageFeedSubscriber<TMessageFeed> subscriber)
            where TMessageFeed : IMessageFeed, new()
        {
            subscriber.ThrowOnNull<DomainException,
                IMessageFeedSubscriber<TMessageFeed>>(this.GetType(), "Invalid subscriber");

            if (!this._subscribers.Contains(subscriber.Name))
                return;

            this._subscribers.Remove(subscriber.Name);
            this.MessageAdded -= subscriber.OnMessageAdded;
        }

        public void UpdateFeedWithSubscriberMessage(Message message)
        {
            this._messages.Add(message);
        }

        public void RegisterOwner(string name)
        {
            this.Owner = name;
        }

        #endregion
    }
}