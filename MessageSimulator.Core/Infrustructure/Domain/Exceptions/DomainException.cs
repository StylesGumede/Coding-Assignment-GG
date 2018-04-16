using System;

namespace MessageSimulator.Core.Infrustructure.Domain.Exceptions
{
    /// <summary>
    /// Custom <see cref="Exception"/> raised in Domain types.
    /// </summary>
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}