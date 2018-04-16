using System;
using System.Linq;
using MessageSimulator.Core.Domain.Twitter;
using MessageSimulator.Core.Infrustructure.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageFeedSimulator.Core.Tests.Domain.Tests
{
    [TestClass]
    public class TwitterUserTests
    {
        [TestMethod]
        public void TestMustFailToInitializeGivenInvalidConstructorArgument_Name_Null()
        {
            try
            {
                TwitterUser target = new TwitterUser(null);

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual($"name can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterUser).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestMustFailToInitializeGivenInvalidConstructorArgument_Name_Empty()
        {
            try
            {
                TwitterUser target = new TwitterUser(string.Empty);

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual($"name can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterUser).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestMustFailToInitializeGivenInvalidConstructorArgument_Name_Whitespace()
        {
            try
            {
                TwitterUser target = new TwitterUser("  ");

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual($"name can not be null, empty or contain whitespaces.\nThrown at " +
                                $"'{typeof(TwitterUser).FullName}'", exception.Message);
            }
        }

        [TestMethod]
        public void TestAddFollower_MustFailToAddFollower_Null_Follower()
        {
            try
            {
                TwitterUser target = new TwitterUser("Paul");
                target.AddFollower(null);

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual("Invalid subscriber", exception.Message);
            }
        }

        [TestMethod]
        public void TestRemoveFollower_MustFailToRemoveFollower_Null_Follower()
        {
            try
            {
                TwitterUser target = new TwitterUser("Paul");
                target.RemoveFollower(null);

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual("Invalid subscriber", exception.Message);
            }
        }

        [TestMethod]
        public void TestAddFollower_MustNotAddDuplicateFollower()
        {
            TwitterUser target = new TwitterUser("Mike");
            target.AddFollower(new TwitterUser("Paul"));
            target.AddFollower(new TwitterUser("Paul"));

            Assert.AreEqual(1, target.MessageFeed.Subscribers.Count());
            Assert.AreEqual("Paul", target.MessageFeed.Subscribers.First());
        }

        [TestMethod]
        public void TestAddFollower_MustNotSelfFollow()
        {
            TwitterUser target = new TwitterUser("Mike");
            target.AddFollower(target);

            Assert.AreEqual(0, target.MessageFeed.Subscribers.Count());
        }

        [TestMethod]
        public void TestRemoveFollower_MustNotFailRemovingFollowerThatIsNotFollowing()
        {
            TwitterUser target = new TwitterUser("Mike");
            target.AddFollower(new TwitterUser("Paul"));

            target.RemoveFollower(new TwitterUser("Ghost"));

            Assert.AreEqual(1, target.MessageFeed.Subscribers.Count());
            Assert.AreEqual("Paul", target.MessageFeed.Subscribers.First());
        }

        [TestMethod]
        public void TestAddFollower_MustAddFollowerSuccessfully()
        {
            TwitterUser target=new TwitterUser("Mike");
            target.AddFollower(new TwitterUser("Paul"));

            Assert.AreEqual(1,target.MessageFeed.Subscribers.Count());
            Assert.AreEqual("Paul", target.MessageFeed.Subscribers.First());
        }

        [TestMethod]
        public void TestRemoveFollower_MustRemoveFollowerSuccessfully()
        {
            TwitterUser target = new TwitterUser("Mike");

            TwitterUser follower = new TwitterUser("Paul");

            target.AddFollower(follower);

            Assert.AreEqual(1, target.MessageFeed.Subscribers.Count());
            Assert.AreEqual("Paul", target.MessageFeed.Subscribers.First());

            target.RemoveFollower(follower);

            Assert.AreEqual(0, target.MessageFeed.Subscribers.Count());
        }

        [TestMethod]
        public void TestTweet_MustFailToTweetGivenInvalidText_Message_Null()
        {
            try
            {
                TwitterUser target = new TwitterUser("Paul");
                target.Tweet(null);

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual("Can not add invalid tweet. ", exception.Message);
            }
        }

        [TestMethod]
        public void TestTweet_MustFailToTweetGivenInvalidText_Message_Empty()
        {
            try
            {
                TwitterUser target = new TwitterUser("Paul");
                target.Tweet(string.Empty);

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual("Can not add invalid tweet. ", exception.Message);
            }
        }

        [TestMethod]
        public void TestTweet_MustFailToTweetGivenInvalidText_Message_Whitespace()
        {
            try
            {
                TwitterUser target = new TwitterUser("Paul");
                target.Tweet(" ");

                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(DomainException));
                Assert.AreEqual("Can not add invalid tweet. ", exception.Message);
            }
        }

        [TestMethod]
        public void TestTweet_MustTweetSuccessully()
        {
            TwitterUser target = new TwitterUser("Paul");
            target.Tweet("Hello there!, how do you do!");

            Assert.AreEqual("Paul\r\n\t@Paul: Hello there!, how do you do!", target.ReadFeed());
        }

        [TestMethod]
        public void TestTweet_MustTweetSuccessullyAndBroadcastToFollowers()
        {
            TwitterUser target = new TwitterUser("Paul");

            TwitterUser follower1 = new TwitterUser("Donald");
            follower1.Tweet("Make America great again!!");

            TwitterUser follower2 = new TwitterUser("Obama");
            follower2.Tweet("Ya'll gonna miss me!!");

            target.AddFollower(follower1);
            target.AddFollower(follower2);

            target.Tweet("Hello there!, how do you do!");

            Assert.AreEqual("Paul\r\n\t@Paul: Hello there!, how do you do!", target.ReadFeed());
            Assert.AreEqual("Donald\r\n\t@Donald: Make America great again!!\r\n\t@Paul: Hello there!, how do you do!", follower1.ReadFeed());
            Assert.AreEqual("Obama\r\n\t@Obama: Ya'll gonna miss me!!\r\n\t@Paul: Hello there!, how do you do!", follower2.ReadFeed());
        }
    }
}
