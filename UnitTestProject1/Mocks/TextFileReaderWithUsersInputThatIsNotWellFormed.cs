using System.Collections.Generic;
using System.Text;
using MessageSimulator.Core.Infrustructure.IO;

namespace MessageFeedSimulator.Core.Tests.Mocks
{
    public class TextFileReaderWithUsersInputThatIsNotWellFormed : IInputFileReader
    {

        public string LoadFile(string filePath)
        {
            StringBuilder mockFile = new StringBuilder();

            mockFile.AppendLine("Ward follows Alan");
            mockFile.AppendLine("Alan follows Martin  Boom");//Invalid line
            mockFile.AppendLine("Ward follows Martin, Alan");

            return mockFile.ToString();
        }

        public string[] LoadFileAsCollectionOfLines(string filePath)
        {
            List<string> mockFile = new List<string>();

            mockFile.Add("Ward follows Alan");
            mockFile.Add("Alan follows Martin  Boom");//Invalid line
            mockFile.Add("Ward follows Martin, Alan");

            return mockFile.ToArray();
        }
    }
}