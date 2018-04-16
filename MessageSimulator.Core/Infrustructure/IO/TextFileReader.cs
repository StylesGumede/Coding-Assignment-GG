using System.IO;

namespace MessageSimulator.Core.Infrustructure.IO
{
    public class TextFileReader : IInputFileReader
    {
        public string[] LoadFileAsCollectionOfLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}