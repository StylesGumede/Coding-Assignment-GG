using System.Collections.Generic;
using System.Text;
using MessageSimulator.Core.Infrustructure.IO;

namespace MessageFeedSimulator.Core.Tests.Mocks
{
    public class TextFileReaderWithMessagesInputThatIsNotWellFormed : IInputFileReader
    {

        public string LoadFile(string filePath)
        {
            StringBuilder mockFile = new StringBuilder();

            mockFile.AppendLine("");
            mockFile.AppendLine("Alan> If you have a procedure with 10 parameters, you probably missed some.");
            mockFile.AppendLine("Ward> There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.");
            mockFile.AppendLine("Alan> Random numbers should not be generated with a method chosen at random.");
            mockFile.AppendLine("Alan> Regular expressions are difficult!!!");
            mockFile.AppendLine("Martin>> Black Label is great!");//Invalid line

            return mockFile.ToString();
        }

        public string[] LoadFileAsCollectionOfLines(string filePath)
        {
            List<string> mockFile = new List<string>();

            mockFile.Add("");
            mockFile.Add("Alan> If you have a procedure with 10 parameters, you probably missed some.");
            mockFile.Add("Ward> There are only two hard things in Computer Science: cache invalidation, naming things and off-by-1 errors.");
            mockFile.Add("Alan> Random numbers should not be generated with a method chosen at random.");
            mockFile.Add("Alan> Regular expressions are difficult!!!");
            mockFile.Add("Martin>> Black Label is great!");//Invalid line

            return mockFile.ToArray();
        }
    }
}