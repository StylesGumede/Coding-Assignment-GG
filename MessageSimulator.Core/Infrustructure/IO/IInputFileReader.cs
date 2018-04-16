namespace MessageSimulator.Core.Infrustructure.IO
{
    /// <summary>
    /// Provides functionality to read an external file.
    /// </summary>
    public interface IInputFileReader: IInfrustructureType
    {
        /// <param name="filePath"></param>
        /// <returns>Returns of string collection of all lines in a 
        /// successfully read <see cref="filePath"/></returns>
        string[] LoadFileAsCollectionOfLines(string filePath);
    }
}