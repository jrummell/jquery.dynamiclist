using System.IO;

namespace Minify
{
    public class FileService : IFileService
    {
        public bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        public void Save(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
        }
    }
}