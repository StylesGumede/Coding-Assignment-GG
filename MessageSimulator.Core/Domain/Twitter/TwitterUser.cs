using System.Text;
using MessageSimulator.Core.Infrustructure.Domain.Exceptions;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions;

namespace MessageSimulator.Core.Domain.Twitter
{
    /// <summary>
    /// Reprensents the concept of a Twitter user.
    /// </summary>
    public class TwitterUser : MessageFeedSubscriber<TwitterFeed>
    {
        /// <summary>
        /// Creates an instance of <see cref="TwitterUser"/>.
        /// </summary>
        /// <param name="name"></param>
        public TwitterUser(string name) : base(name)
        {
        }

        /// <summary>
        /// Adds a <see cref="TwitterUser"/> as a follower of this <see cref="TwitterUser"/>.
        /// </summary>
        /// <param name="user">A <see cref="TwitterUser"/> interested in 
        /// following this <see cref="TwitterUser"/>.</param>
        public void AddFollower(TwitterUser user)
        {
            this.MessageFeed.RegisterSubscriber(user);
        }

        /// <summary>
        /// Removes a <see cref="TwitterUser"/> as a follower of this <see cref="TwitterUser"/>.
        /// </summary>
        /// <param name="user">A <see cref="TwitterUser"/> interested in 
        /// unfollowing this <see cref="TwitterUser"/>.</param>
        public void RemoveFollower(TwitterUser user)
        {
            this.MessageFeed.UnregisterSubscriber(user);
        }

        /// <summary>
        /// <para>Adds a tweet to this<see cref="TwitterUser"/>'s tweets. </para>
        /// <para>Throws a <see cref="DomainException"/> if the tweet is invalid</para>
        /// </summary>
        /// <param name="message">The tweet</param>
        public void Tweet(string message)
        {
            message.ThrowOnNullEmptyOrWhitespace<DomainException>(nameof(message), 
                "Can not add invalid tweet. ");

            Tweet tweet = new Tweet(this.Name, message);

            this.MessageFeed.AddTweet(tweet);
        }

        /// <summary>
        /// Adds a tweet coming from a <see cref="TwitterUser"/> followed by this <see cref="TwitterUser"/>
        /// </summary>
        /// <param name="tweet">A <see cref="Tweet"/></param>
        public void OnFollowerTweeted(Tweet tweet)
        {
            this.MessageFeed.UpdateFeedWithSubscriberMessage(tweet);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// Returns a formated string with all the <see cref="Tweet"/>s from this 
        /// <see cref="TwitterUser"/> and the <see cref="Tweet"/>s from the <see cref="TwitterUser"/>s 
        /// that this <see cref="TwitterUser"/> follows.
        /// </returns>
        public string ReadFeed()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(this.Name);
            stringBuilder.AppendLine(this.MessageFeed.ToString());

            return stringBuilder.ToString().Trim();
        }
    }
}