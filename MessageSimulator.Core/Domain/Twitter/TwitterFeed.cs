using System;
using System.Text;

namespace MessageSimulator.Core.Domain.Twitter
{
    public class TwitterFeed : MessageFeed
    {
        public TwitterFeed()
        {
        }

        public void AddTweet(Tweet tweet)
        {
            this.AddMessage(tweet);

            this.BroadcastAddedMessage(tweet);
        }

        public void UpdateFeedWithFollowerTweet(Tweet tweet)
        {
            if (tweet == null)
                throw new ArgumentNullException(nameof(tweet));

            this.AddMessage(tweet);
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns a formatted string of all the <see cref="Tweet"/>s in this <see cref="TwitterFeed"/>
        ///  </returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var tweet in this.Messages)
                stringBuilder.AppendLine($"\t@{tweet.Owner}: {tweet.Text}");

            return stringBuilder.ToString();
        }
    }
}