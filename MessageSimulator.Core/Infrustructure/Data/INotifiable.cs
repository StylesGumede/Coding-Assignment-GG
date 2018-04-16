using System;

namespace MessageSimulator.Core.Infrustructure.Data
{
    /// <summary>
    /// Defines a <see cref="Notification"/> event so that implementing types 
    /// can raise events to subscribing types.
    /// </summary>
    public interface INotifiable
    {
        event Action<EventArgs> Notification;
    }
}