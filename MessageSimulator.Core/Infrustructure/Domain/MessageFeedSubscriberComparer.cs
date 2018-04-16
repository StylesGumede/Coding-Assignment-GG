using System.Collections.Generic;
using MessageSimulator.Core.Domain;

namespace MessageSimulator.Core.Infrustructure.Domain
{
    /// <summary>
    /// Custom <see cref="IComparer{T}"/> used in a <see cref="SortedSet{T}"/>
    /// </summary>
    public class MessageFeedSubscriberComparer : IComparer<IMessageFeedSubscriber>
    {
        public int Compare(IMessageFeedSubscriber x, IMessageFeedSubscriber y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}