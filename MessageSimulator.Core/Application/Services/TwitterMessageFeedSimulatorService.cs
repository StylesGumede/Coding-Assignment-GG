using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MessageSimulator.Core.Data;
using MessageSimulator.Core.Data.Interfaces;
using MessageSimulator.Core.Domain.Twitter;
using MessageSimulator.Core.Infrustructure.Application;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Exceptions;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Extensions;

namespace MessageSimulator.Core.Application.Services
{
    public class TwitterMessageFeedSimulatorService : ITwitterMessageFeedSimulatorService
    {
        #region Private Fields

        private readonly IUserData<TwitterUser> _userCollection;
        private readonly IMessageData<Tweet> _messageCollection;

        #endregion

        #region Constructor

        public TwitterMessageFeedSimulatorService(
            IUserData<TwitterUser> userCollection, 
            IMessageData<Tweet> messageCollection)
        {
            this._userCollection = userCollection;
            this._messageCollection = messageCollection;
        }

        #endregion

        #region Public Methods

        public TwitterMessageFeedSimulatorServiceResponse RunSimulation(
            TwitterMessageFeedSimulatorServiceRequest request)
        {
            TwitterMessageFeedSimulatorServiceResponse response = new TwitterMessageFeedSimulatorServiceResponse();

            try
            {
                this.ValidateMembers(request);

                IEnumerable<TwitterUser> users = this._userCollection.GetUsers(request.UsersInputFilePath);

                IEnumerable<Tweet> messages = this._messageCollection.GetMessages(request.MessagesInputFilePath);

                this.AssociateMessagesWithUsers(messages, users);

                response.Message = this.GetAllUserFeeds(users);

                response.ServiceResult = ServiceResult.Success;

            }
            catch (Exception exception)
            {
                response.ServiceResult = ServiceResult.Exception;
                response.Message = $"The following exception occurred | {exception.Message}";
            }

            return response;
        }

        #endregion

        #region Private Methods

        private void ValidateMembers(TwitterMessageFeedSimulatorServiceRequest request)
        {
            request.ThrowOnNull<ServiceException,
                TwitterMessageFeedSimulatorServiceRequest,
                TwitterMessageFeedSimulatorService>();

            this._userCollection.ThrowOnNull<ServiceException,
                IUserData<TwitterUser>,
                TwitterMessageFeedSimulatorService>();

            this._messageCollection.ThrowOnNull<ServiceException,
                IMessageData<Tweet>,
                TwitterMessageFeedSimulatorService>();
        }

        private string GetAllUserFeeds(IEnumerable<TwitterUser> users)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (TwitterUser twitterUser in users)
            {
                stringBuilder.AppendLine(twitterUser.ReadFeed());
            }

            return stringBuilder.ToString();
        }

        private void AssociateMessagesWithUsers(IEnumerable<Tweet> messages, IEnumerable<TwitterUser> users)
        {
            foreach (Tweet message in messages)
            {
                TwitterUser user = users.FirstOrDefault(x => x.Name == message.Owner);

                if(user == null)
                    continue;

                user.Tweet(message.Text);
            }
        }

        #endregion
    }
}