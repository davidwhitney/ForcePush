using System.IO.Compression;

namespace ForcePush.Packaging
{
    public class FilePackager
    {
        public void Package(string directory, string outputPath)
        {
            ZipFile.CreateFromDirectory(directory, outputPath);
        }
    }
}