namespace MessageSimulator.Core.Domain.Twitter
{
    public class Tweet : Message
    {
        public Tweet(string username, string message)
            : base(username, message)
        {
        }
    }
}