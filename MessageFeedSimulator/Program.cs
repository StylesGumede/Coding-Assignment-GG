using System;
using MessageSimulator.Core.Application.Services;
using MessageSimulator.Core.Data;
using MessageSimulator.Core.Data.Factories;
using MessageSimulator.Core.Data.Interfaces;
using MessageSimulator.Core.Domain.Twitter;
using MessageSimulator.Core.Infrustructure;
using MessageSimulator.Core.Infrustructure.Application;
using MessageSimulator.Core.Infrustructure.Data;

namespace MessageFeedSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            IInfrustructureFactory infrustructureFactory = new InfrustructureFactory();
            IDataFactory dataFactory = new DataFactory(infrustructureFactory);

            IUserData<TwitterUser> userCollection = dataFactory.CreateInstanceOf<IUserData<TwitterUser>>();
            IMessageData<Tweet> messageCollection = dataFactory.CreateInstanceOf<IMessageData<Tweet>>();

            userCollection.Notification += OnUserCollectionNotification;
            messageCollection.Notification += OnMessageCollectionNotification;

            TwitterMessageFeedSimulatorService service = new TwitterMessageFeedSimulatorService(
                userCollection,
                messageCollection);

            TwitterMessageFeedSimulatorServiceResponse response = service.RunSimulation(new TwitterMessageFeedSimulatorServiceRequest
            {
                UsersInputFilePath = args[0],
                MessagesInputFilePath = args[1]
            });

            Console.WriteLine();

            switch (response.ServiceResult)
            {
                case ServiceResult.Exception:
                    Console.WriteLine(response.Message);
                    break;
                case ServiceResult.Success:
                    Console.WriteLine(response.Message);
                    break;
            }

            Console.Read();
        }

        private static void OnMessageCollectionNotification(EventArgs obj)
        {
            DataAccessEventArgs eventArgs = (DataAccessEventArgs)obj;

            Console.WriteLine(eventArgs.Message);
        }

        private static void OnUserCollectionNotification(System.EventArgs obj)
        {
            DataAccessEventArgs eventArgs = (DataAccessEventArgs) obj;

            Console.WriteLine(eventArgs.Message);
        }
    }
}
