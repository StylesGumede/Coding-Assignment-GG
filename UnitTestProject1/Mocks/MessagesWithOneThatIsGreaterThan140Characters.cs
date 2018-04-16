using System.Collections.Generic;
using System.Text;
using MessageSimulator.Core.Infrustructure.IO;

namespace MessageFeedSimulator.Core.Tests.Mocks
{
    public class MessagesWithOneThatIsGreaterThan140Characters : IInputFileReader
    {

        public string LoadFile(string filePath)
        {
            StringBuilder mockFile = new StringBuilder();

            mockFile.AppendLine("");
            mockFile.AppendLine("Alan> If you have a procedure with 10 parameters, you probably missed some.");
            mockFile.AppendLine("Ward> There are only two hard things in Computer Science: cache invalidation, " +
                                "naming things and off-by-1 errors.");
            mockFile.AppendLine("Alan> Random numbers should not be generated with a method chosen at random.");
            mockFile.AppendLine("Alan> People always fear change.  People feared electricity when it was " +
                                "invented, didn’t they?  People feared coal, they feared gas-powered " +
                                "engines.  There will always be ignorance, and ignorance leads to fear.  " +
                                "But with time, people will come to accept their silicon masters.");
            mockFile.AppendLine("Martin> Black Label is great!");

            return mockFile.ToString();
        }

        public string[] LoadFileAsCollectionOfLines(string filePath)
        {
            List<string> mockFile = new List<string>();

            mockFile.Add("Alan> If you have a procedure with 10 parameters, you probably missed some.");
            mockFile.Add("Ward> There are only two hard things in Computer Science: cache invalidation, " +
                         "naming things and off-by-1 errors.");
            mockFile.Add("Alan> Random numbers should not be generated with a method chosen at random.");
            mockFile.Add("Alan> People always fear change.  People feared electricity when it was " +
                         "invented, didn’t they?  People feared coal, they feared gas-powered " +
                         "engines.  There will always be ignorance, and ignorance leads to fear.  " +
                         "But with time, people will come to accept their silicon masters.");
            mockFile.Add("Martin> Black Label is great!");

            return mockFile.ToArray();
        }
    }
}