using System.Collections.Generic;
using MessageSimulator.Core.Data.Extensions;
using MessageSimulator.Core.Data.Interfaces;
using MessageSimulator.Core.Domain.Twitter;
using MessageSimulator.Core.Infrustructure.Configuration;
using MessageSimulator.Core.Infrustructure.Data;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Exceptions;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions;
using MessageSimulator.Core.Infrustructure.IO;

namespace MessageSimulator.Core.Data
{
    public class TwitterMessageData : DataAccessBase,IMessageData<Tweet>
    {
        #region Private Fields

        private readonly IInputFileReader _inputFileReader;

        #endregion

        #region Constructor

        public TwitterMessageData(
            IApplicationConfiguration applicationConfiguration,
            IInputFileReader inputFileReader):base(applicationConfiguration)
        {
            inputFileReader.ThrowOnNull<DataAccessException,
                IInputFileReader, TwitterMessageData>();

            this._inputFileReader = inputFileReader;
        }

        #endregion

        #region Public Methods

        public IEnumerable<Tweet> GetMessages(string filePath)
        {
            filePath.ThrowOnNullEmptyOrWhitespace<DataAccessException, TwitterMessageData>(nameof(filePath));

            List<Tweet> tweets = new List<Tweet>();

            string[] messagesFile = this._inputFileReader.LoadFileAsCollectionOfLines(filePath);

            string regularExpression =
                this.ApplicationConfiguration.GetValue("TWITTER_MESSAGE_INPUT_FILE_FORMAT");

            foreach (string line in messagesFile)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;//This is an assumption

                if (!this.IsLineValid(filePath, regularExpression, line,
                    this.ApplicationConfiguration.GetValue("TWITTER_MESSAGE_INPUT_FILE_RULE")))
                    continue;

                string username = line.ExtractUsernameFromLine(">");
                string message = line.ExtractMessageFromLine(username);

                if (string.IsNullOrWhiteSpace(message))
                    continue;

                if (message.Length > 140)
                {
                    this.RaiseNotification($"\n'{filePath}' contains the following tweet that is longer than 140 " +
                                           $"characters:\n\n{message}\n\nThe Tweet will be ignored.");
                    continue;
                }

                tweets.Add(new Tweet(username, message));
            }

            return tweets;
        }

        #endregion
    }
}