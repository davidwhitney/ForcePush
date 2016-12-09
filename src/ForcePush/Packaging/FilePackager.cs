using System.IO.Compression;
using ForcePush.Output;

namespace ForcePush.Packaging
{
    public class FilePackager
    {
        private readonly IOutput _output;

        public FilePackager(IOutput output)
        {
            _output = output;
        }

        public void Package(string directory, string outputPath)
        {
            _output.WriteLine($"Packaging assets from '{directory}' to '{outputPath}'.");
            ZipFile.CreateFromDirectory(directory, outputPath);
        }
    }
}