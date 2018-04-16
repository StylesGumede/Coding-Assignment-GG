using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public class TwitterUserCollectionTests
    {
        private IInfrustructureFactory _infrustructureFactory;
        private StringBuilder _userInputFileMock;
        private readonly string _usersInputFilePath = $@"{Environment.CurrentDirectory}\InputFile\user.txt";

        public TwitterUserCollectionTests()
        {
            this.InitializeTestContext();
        }

        private void InitializeTestContext()
        {
            this._infrustructureFactory = new InfrustructureFactory();
            this._userInputFileMock = new StringBuilder();

            this._userInputFileMock.AppendLine("Ward follows Alan");
            this._userInputFileMock.AppendLine("Alan follows Martin");
            this._userInputFileMock.AppendLine("Ward follows Martin, Alan");
        }

        [TestMethod]
        public void TestMustFailToInitializeGivenInvalidConstructorArgument_ApplicationConfiguration()
        {
            try
            {
                TwitterUserData target = new TwitterUserData(null,
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
                
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"{nameof(IApplicationConfiguration)} can not be null.\nThrown at " +
                                $"'{typeof(TwitterUserData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestMustFailToInitializeGivenInvalidConstructorArgument_TextFileReader()
        {
            try
            {
                TwitterUserData target = new TwitterUserData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    null);

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"{nameof(IInputFileReader)} can not be null.\nThrown at " +
                                $"'{typeof(TwitterUserData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestGetUsers_MustFailGivenInvalidArguments_FilePath_Null()
        {
            try
            {
                TwitterUserData target = new TwitterUserData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
                target.GetUsers(null);

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"filePath can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterUserData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestGetUsers_MustFailGivenInvalidArguments_FilePath_WhitespaceString()
        {
            try
            {
                TwitterUserData target = new TwitterUserData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
                target.GetUsers("  ");

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"filePath can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterUserData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestGetUsers_MustFailGivenInvalidArguments_FilePath_EmptyString()
        {
            try
            {
                TwitterUserData target = new TwitterUserData(
                    this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                    this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
                target.GetUsers(string.Empty);

                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DataAccessException));
                Assert.AreEqual($"filePath can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterUserData).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestGetUsers_MustFailGivenInvalidArguments_FilePath_InvalidFilePath()
        {
            TwitterUserData target = new TwitterUserData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());
            target.GetUsers("something that does not exist!!!!!");
        }

        [TestMethod]
        public void TestGetUsers_MustRaiseEventGivenAnInputFileThatContainsIncorrectData()
        {
            TwitterUserData target = new TwitterUserData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                new TextFileReaderWithUsersInputThatIsNotWellFormed());

            bool eventRaised = false;

            target.Notification += (data) =>
            {
                DataAccessEventArgs eventArgs = (DataAccessEventArgs)data;

                Assert.AreEqual($"The following line in file '{this._usersInputFilePath}' is incorrect:\n\t" +
                                $"Alan follows Martin  Boom\nThe line will not be processed as it " +
                                $"does not meet the rule: [USER_NAME][SPACE_CHAR]['follows'][SPACE_CHAR][COMMA_DELIMITED_LIST_OF_USER_NAMES] " +
                                $"e.g. Zuma follows CCMA, Careers24, PNet", eventArgs.Message);

                eventRaised = true;
            };

            target.GetUsers(this._usersInputFilePath);

            if (!eventRaised)
                Assert.Fail("Expected event was not raised.");
        }


        [TestMethod]
        public void TestGetUsers_MustRaiseEventGivenAnInputFileThatContainsIncorrectData2()
        {
            TwitterUserData target = new TwitterUserData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                new TextFileReaderWithUsersInputThatIsNotWellFormed());

            bool eventRaised = false;

            target.Notification += (data) =>
            {
                DataAccessEventArgs eventArgs = (DataAccessEventArgs)data;

                Assert.AreEqual($"The following line in file '{this._usersInputFilePath}' is incorrect:\n\t" +
                                $"Alan follows Martin  Boom\nThe line will not be processed as it does not " +
                                $"meet the rule: [USER_NAME][SPACE_CHAR]['follows'][SPACE_CHAR][COMMA_DELIMITED_LIST_OF_USER_NAMES] " +
                                $"e.g. Zuma follows CCMA, Careers24, PNet", eventArgs.Message);

                eventRaised = true;
            };

            target.GetUsers(this._usersInputFilePath);

            if (!eventRaised)
                Assert.Fail("Expected event was not raised.");
        }

        [TestMethod]
        public void TestGetUsers_MustLoadAllUsersFromGivenValidInputFile()
        {
            TwitterUserData target = new TwitterUserData(
                this._infrustructureFactory.CreateInstanceOf<IApplicationConfiguration>(),
                this._infrustructureFactory.CreateInstanceOf<IInputFileReader>());

            IEnumerable<TwitterUser> users = target.GetUsers(this._usersInputFilePath);

            Assert.IsNotNull(users);
            Assert.AreEqual(3, users.Count());
        }
    }
}
