using System;
using System.Text.RegularExpressions;
using MessageSimulator.Core.Infrustructure.Configuration;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Exceptions;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions;

namespace MessageSimulator.Core.Infrustructure.Data
{
    /// <summary>
    /// Base class for <see cref="IDataAccess"/> types. 
    /// Provides notifications to subscribers on events that 
    /// occurr.
    /// </summary>
    public abstract class DataAccessBase: INotifiable
    {
        #region Constuctors

        protected DataAccessBase(IApplicationConfiguration applicationConfiguration)
        {
            applicationConfiguration.ThrowOnNull<DataAccessException,
                IApplicationConfiguration>(this.GetType());

            this.ApplicationConfiguration = applicationConfiguration;
        }
        

        #endregion

        #region Protected Properties

        /// <summary>
        /// Application configuration accessible to derived types of <see cref="DataAccessBase"/>
        /// </summary>
        protected IApplicationConfiguration ApplicationConfiguration { get; set; }

        #endregion

        #region Events

        public event Action<EventArgs> Notification;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="Notification"/> event with an <see cref="message"/>
        /// </summary>
        /// <param name="message"></param>
        protected void RaiseNotification(string message)
        {
            this.Notification?.Invoke(new DataAccessEventArgs(message));
        }

        /// <summary>
        /// Validates a string against a given <see cref="regularExpression"/>. 
        /// Raises an event if there are any discrepancies found.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="regularExpression"></param>
        /// <param name="line"></param>
        /// <param name="ruleWording"></param>
        /// <returns></returns>
        protected bool IsLineValid(string filePath, string regularExpression, string line, string ruleWording = null)
        {
            Match formatRuleMatch = Regex.Match(line, regularExpression, RegexOptions.Singleline);
            if (formatRuleMatch.Success)
                return true;

            if (string.IsNullOrWhiteSpace(ruleWording))
            {
                this.RaiseNotification($"The following line in file '{filePath}' is " +
                                       $"incorrect:\n{$"\t{line}"}\nThe line will not be processed.");
            }
            else
            {
                this.RaiseNotification($"The following line in file '{filePath}' is " +
                                       $"incorrect:\n{$"\t{line}"}\n" +
                                       $"The line will not be processed as it does not meet the rule: {ruleWording}");
            }

            return false;
        }

        #endregion
    }
}