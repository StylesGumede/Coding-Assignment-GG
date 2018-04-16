using System;
using MessageSimulator.Core.Application.Services;
using MessageSimulator.Core.Data.Factories;
using MessageSimulator.Core.Data.Interfaces;
using MessageSimulator.Core.Domain.Twitter;
using MessageSimulator.Core.Infrustructure;
using MessageSimulator.Core.Infrustructure.Application;
using MessageSimulator.Core.Infrustructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageFeedSimulator.Core.Tests.Application
{
    [TestClass]
    public class TwitterMessageFeedSimulatorServiceTests
    {
        private IInfrustructureFactory _infrustructureFactory;
        private IDataFactory _dataFactory;
        private readonly string _messagesInputFilePath = $@"{Environment.CurrentDirectory}\tweet.txt";
        private readonly string _usersInputFilePath = $@"{Environment.CurrentDirectory}\user.txt";

        public TwitterMessageFeedSimulatorServiceTests()
        {
            this.InitializeTestContext();
        }

        private void InitializeTestContext()
        {
            this._infrustructureFactory = new InfrustructureFactory();
            this._dataFactory = new DataFactory(this._infrustructureFactory);
        }

        [TestMethod]
        public void TestReturnExceptionGivenInvalidConstructorArguments_Null_IUserCollection()
        {
            TwitterMessageFeedSimulatorService target = new TwitterMessageFeedSimulatorService(
                null,
                this._dataFactory.CreateInstanceOf<IMessageData<Tweet>>());

            TwitterMessageFeedSimulatorServiceResponse response = target.RunSimulation(new TwitterMessageFeedSimulatorServiceRequest());

            Assert.IsNotNull(response);
            Assert.AreEqual(ServiceResult.Exception, response.ServiceResult);
            Assert.AreEqual("The following exception occurred | IUserData`1 can not be null.\nThrown at " +
                            "'MessageSimulator.Core.Application.Services.TwitterMessageFeedSimulatorService'", response.Message);
        }

        [TestMethod]
        public void TestReturnExceptionGivenInvalidConstructorArguments_Null_IMessagesCollection()
        {
            TwitterMessageFeedSimulatorService target = new TwitterMessageFeedSimulatorService(
                this._dataFactory.CreateInstanceOf<IUserData<TwitterUser>>(),
                null);

            TwitterMessageFeedSimulatorServiceResponse response = target.RunSimulation(new TwitterMessageFeedSimulatorServiceRequest());

            Assert.IsNotNull(response);
            Assert.AreEqual(ServiceResult.Exception, response.ServiceResult);
            Assert.AreEqual("The following exception occurred | IMessageData`1 can not be null.\nThrown at " +
                            "'MessageSimulator.Core.Application.Services.TwitterMessageFeedSimulatorService'", response.Message);
        }

        [TestMethod]
        public void TestReturnExceptionGivenInvalidConstructorArguments_Null_Request()
        {
            TwitterMessageFeedSimulatorService target = new TwitterMessageFeedSimulatorService(
                this._dataFactory.CreateInstanceOf<IUserData<TwitterUser>>(),
                this._dataFactory.CreateInstanceOf<IMessageData<Tweet>>());

            TwitterMessageFeedSimulatorServiceResponse response = target.RunSimulation(null);

            Assert.IsNotNull(response);
            Assert.AreEqual(ServiceResult.Exception, response.ServiceResult);
            Assert.AreEqual($"The following exception occurred | {nameof(TwitterMessageFeedSimulatorServiceRequest)} can not be null.\nThrown at " +
                            "'MessageSimulator.Core.Application.Services.TwitterMessageFeedSimulatorService'", response.Message);
        }

        [TestMethod]
        public void TestRunSimulation_MustReturnSuccessGivenValidUserAndMessagesInputFiles()
        {
            TwitterMessageFeedSimulatorService target = new TwitterMessageFeedSimulatorService(
                this._dataFactory.CreateInstanceOf<IUserData<TwitterUser>>(),
                this._dataFactory.CreateInstanceOf<IMessageData<Tweet>>());

            TwitterMessageFeedSimulatorServiceResponse response = target.RunSimulation(
                new TwitterMessageFeedSimulatorServiceRequest
            {
                MessagesInputFilePath = this._messagesInputFilePath,
                UsersInputFilePath = this._usersInputFilePath
            });

            Assert.IsNotNull(response);
            Assert.AreEqual(ServiceResult.Success, response.ServiceResult);
            Assert.AreEqual("Alan\r\n\t" +
                            "@Alan: If you have a procedure with 10 parameters, you probably missed some.\r\n\t" +
                            "@Alan: Random numbers should not be generated with a method chosen at random.\r\n" +
                            "Martin\r\nWard\r\n\t@Alan: If you have a procedure with 10 parameters, you " +
                            "probably missed some.\r\n\t" +
                            "@Ward: There are only two hard things in Computer Science: cache invalidation, " +
                            "naming things and off-by-1 errors.\r\n\t" +
                            "@Alan: Random numbers should not be generated with a method chosen at random.\r\n",
                            response.Message);
        }
    }
}
