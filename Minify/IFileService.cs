namespace Minify
{
    public interface IFileService
    {
        /// <summary>
        /// Returns true if the specified file exists.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        bool Exists(string fileName);

        /// <summary>
        /// Saves the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        void Save(string fileName, string content);
    }
}