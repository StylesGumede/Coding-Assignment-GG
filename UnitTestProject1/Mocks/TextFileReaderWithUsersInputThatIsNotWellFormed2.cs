using System.Collections.Generic;
using System.Text;
using MessageSimulator.Core.Infrustructure.IO;

namespace MessageFeedSimulator.Core.Tests.Mocks
{
    public class TextFileReaderWithUsersInputThatIsNotWellFormed2 : IInputFileReader
    {

        public string LoadFile(string filePath)
        {
            StringBuilder mockFile = new StringBuilder();

            mockFile.AppendLine("");
            mockFile.AppendLine("Ward follows Alan");
            mockFile.AppendLine("Alan follows Martin");
            mockFile.AppendLine("Ward follows Martin, Alan");
            mockFile.AppendLine("Piet follows Bob, Alan");
            mockFile.AppendLine("Petrus follows Pitso, Mike,,");//Invalid line

            return mockFile.ToString();
        }

        public string[] LoadFileAsCollectionOfLines(string filePath)
        {
            List<string> mockFile = new List<string>();

            mockFile.Add("Ward follows Alan");
            mockFile.Add("Alan follows Martin");
            mockFile.Add("Ward follows Martin, Alan");
            mockFile.Add("Piet follows Bob, Alan");
            mockFile.Add("Petrus follows Pitso, Mike,,");//Invalid line

            return mockFile.ToArray();
        }
    }
}