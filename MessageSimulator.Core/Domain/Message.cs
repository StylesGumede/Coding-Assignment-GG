using System;

namespace MessageSimulator.Core.Domain
{
    public abstract class Message : EventArgs, IMessage
    {
        protected Message(string owner, string text)
        {
            this.Owner = owner;
            this.Text = text;
        }

        public string Owner { get; }
        public string Text { get; }
    }
}