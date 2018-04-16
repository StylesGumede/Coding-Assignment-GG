using System;

namespace MessageSimulator.Core.Infrustructure.Data
{
    /// <summary>
    /// <see cref="EventArgs"/> for events raised in <see cref="IDataAccess"/> types.
    /// </summary>
    public class DataAccessEventArgs:EventArgs
    {
        public DataAccessEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Event message
        /// </summary>
        public string Message { get; set; }
    }
}