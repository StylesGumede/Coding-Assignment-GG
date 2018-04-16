using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessageFeedSimulator.Core.Tests.Mocks;
using MessageSimulator.Core.Data;
using MessageSimulator.Core.Domain.Twitter;
using MessageSimulator.Core.Infrustructure;
using MessageSimulator.Core.Infrustructure.Configuration;
using MessageSimulator.Core.Infrustructure.Data;
using MessageSimulator.Core.Infrustructure.ErrorHandling.Exceptions;
using MessageSimulator.Core.Infrustructure.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageFeedSimulator.Core.Tests.Data.Tests
{
    [TestClass]
    public class TwitterMessageCollectionTests
    {
        private IInfrustructureFactory _infrustructureFactory;
        private readonly string _messagesInputFilePath = $@"{Environment.CurrentDirectory}\InputFile\tweet.txt";

        public TwitterMessageCollectionTests()
        {
            this.InitializeTestContext();
        }

        private void InitializeTestContext()
        {
            this._infrustructureFactory = new InfrustructureFactory();
        }

        [TestMethod]
        public void TestMustFailToInitializeGivenInvalidConstructorArgument_ApplicationConfiguration()
        {
            try
            {
                TwitterMessageData target = new TwitterMessageData(null,
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"{nameof(IApplicationConfiguration)} can not be null.\nThrown at " +
                                $"'{typeof(TwitterMessageData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestMustFailToInitializeGivenInvalidConstructorArgument_TextFileReader()
        {
            try
            {
                TwitterMessageData target = new TwitterMessageData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    null);

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"{nameof(IInputFileReader)} can not be null.\nThrown at " +
                                $"'{typeof(TwitterMessageData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestGetMessages_MustFailGivenInvalidArguments_FilePath_Null()
        {
            try
            {
                TwitterMessageData target = new TwitterMessageData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
                target.GetMessages(null);

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"filePath can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterMessageData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestGetMessages_MustFailGivenInvalidArguments_FilePath_WhitespaceString()
        {
            try
            {
                TwitterMessageData target = new TwitterMessageData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
                target.GetMessages("  ");

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"filePath can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterMessageData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestGetMessages_MustFailGivenInvalidArguments_FilePath_EmptyString()
        {
            try
            {
                TwitterMessageData target = new TwitterMessageData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
                target.GetMessages(string.Empty);

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"filePath can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterMessageData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestGetMessages_MustFailGivenInvalidArguments_FilePath_InvalidFilePath()
        {
            TwitterMessageData target = new TwitterMessageData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
            target.GetMessages("something that does not exist!!!!!");
        }

        [TestMethod]
        public void TestGetUsers_MustRaiseEventGivenAnInputFileThatContainsInvalidData()
        {
            TwitterMessageData target = new TwitterMessageData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                new TextFileReaderWithMessagesInputThatIsNotWellFormed());

            bool eventRaised = false;

            target.Notification += (data) =>
            {
                DataAccessEventArgs eventArgs = (DataAccessEventArgs)data;

                Assert.AreEqual($"The following line in file '{this._messagesInputFilePath}' is incorrect:\n\t" +
                                $"Martin>> Black Label is great!\nThe line will not be processed as " +
                                $"it does not meet the rule: [USER_NAME]>[SPACE_CHAR][TWEET] " +
                                $"e.g. 'Zuma> Is unemployed and worried!'", eventArgs.Message);
                eventRaised = true;
            };

            target.GetMessages(this._messagesInputFilePath);

            if(!eventRaised)
                Assert.Fail("Expected event was not raised.");
        }

        [TestMethod]
        public void TestGetUsers_MustRaiseEventGivenAnInputFileThatContainsAMessageThatIsGreaterThan140Characters()
        {
            TwitterMessageData target = new TwitterMessageData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                new MessagesWithOneThatIsGreaterThan140Characters());

            bool eventRaised = false;

            target.Notification += (data) =>
            {
                DataAccessEventArgs eventArgs = (DataAccessEventArgs)data;

                Assert.AreEqual($"\n'{this._messagesInputFilePath}' contains the following tweet that is longer than 140 characters:\n\n" +
                                $"People always fear change.  People feared electricity when it was invented, didn’t they?  " +
                                $"People feared coal, they feared gas-powered engines.  There will always be ignorance, and ignorance " +
                                $"leads to fear.  But with time, people will come to accept their silicon masters.\n\n" +
                                $"The Tweet will be ignored.", eventArgs.Message);
                eventRaised = true;
            };

            target.GetMessages(this._messagesInputFilePath);

            if (!eventRaised)
                Assert.Fail("Expected event was not raised.");
        }

        [TestMethod]
        public void TestGetMessages_MustLoadAllMessagesFromGivenValidInputFile()
        {
            TwitterMessageData target = new TwitterMessageData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());

            IEnumerable<Tweet> tweets = target.GetMessages(this._messagesInputFilePath);

            Assert.IsNotNull(tweets);
            Assert.AreEqual(3, tweets.Count());
        }

    }
}
