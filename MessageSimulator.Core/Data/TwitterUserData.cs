using System.Collections.Generic;
using System.Linq;
using MessageSimulator.Core.Data.Extensions;
using MessageSimulator.Core.Data.Interfaces;
using MessageSimulator.Core.Domain.Twitter;
using MessageSimulator.Core.Infrustructure.Configuration;
using MessageSimulator.Core.Infrustructure.Data;
using MessageSimulator.Core.Infrustructure.Domain;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Exceptions;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions;
using MessageSimulator.Core.Infrustructure.IO;

namespace MessageSimulator.Core.Data
{
    public class TwitterUserData : DataAccessBase, IUserData<TwitterUser>
    {
        #region Private Fields

        private readonly IInputFileReader _inputFileReader;

        #endregion

        #region Constructor

        public TwitterUserData(
            IApplicationConfiguration applicationConfiguration, 
            IInputFileReader inputFileReader):base(applicationConfiguration)
        {
            inputFileReader.ThrowOnNull<DataAccessException,
                IInputFileReader, TwitterUserData>();

            this._inputFileReader = inputFileReader;
        }

        #endregion

        #region Public Methods

        public IEnumerable<TwitterUser> GetUsers(string filePath)
        {
            filePath.ThrowOnNullEmptyOrWhitespace<DataAccessException, TwitterUserData>(nameof(filePath));

            string[] inputFile = this._inputFileReader.LoadFileAsCollectionOfLines(filePath);

            string regularExpression = this.ApplicationConfiguration
                .GetValue("TWITTER_USER_INPUT_FILE_FORMAT");

            SortedSet<TwitterUser> sortedUserSet = new SortedSet<TwitterUser>(new MessageFeedSubscriberComparer());

            foreach (string line in inputFile)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;//This is an assumption

                if (!this.IsLineValid(filePath, regularExpression, line,
                    this.ApplicationConfiguration.GetValue("TWITTER_USER_INPUT_FILE_RULE")))
                    continue;

                string username = line.ExtractUsernameFromLine("follows");

                TwitterUser twitterUser = null;

                twitterUser = twitterUser.CreateUser(sortedUserSet, username);

                IEnumerable<string> usernamesOfUsersThatUserFollows = line.ExtractUsersFollowedByUser(username,
                    "follows", ',');

                FollowUsersThatUserFollows(sortedUserSet, twitterUser, usernamesOfUsersThatUserFollows);

                sortedUserSet.Add(twitterUser);
            }

            return sortedUserSet;
        }

        #endregion

        #region Private Methods

        private static void FollowUsersThatUserFollows(SortedSet<TwitterUser> sortedUserSet, TwitterUser twitterUser, 
            IEnumerable<string> usernamesOfUsersThatUserFollows)
        {
            foreach (string userNameOfUserFollowed in usernamesOfUsersThatUserFollows)
            {
                TwitterUser userFollowed = sortedUserSet.FirstOrDefault(x => x.Name == userNameOfUserFollowed);

                if (userFollowed == null)
                {
                    userFollowed = new TwitterUser(userNameOfUserFollowed);
                    sortedUserSet.Add(userFollowed);
                }

                userFollowed.AddFollower(twitterUser);
            }
        }

        #endregion
    }
}