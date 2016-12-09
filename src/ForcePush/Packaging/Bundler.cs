using System.IO;
using ForcePush.Diffing;
using ForcePush.Output;

namespace ForcePush.Packaging
{
    public class Bundler
    {
        private readonly IOutput _output;

        public Bundler(IOutput output)
        {
            _output = output;
        }

        public string CreateTempDirectoryFromDiff(GitDiff diff)
        {
            // checkout correct branch
            var tempDirectory = CopyTree(diff);
            return tempDirectory;
        }

        private string CopyTree(GitDiff diff)
        {
            var windowsPaths = diff.ToWindowsPaths();
            var tempDirectory = TempDirectory.Create("ForcePushBundler");

            foreach (var path in windowsPaths)
            {
                var filePath = Path.GetFullPath(path);

                var relativePath = filePath.Replace(diff.RootPath, "");
                var relativeDirectory = (Path.GetDirectoryName(relativePath) ?? "").TrimStart('\\');

                Copy.CreateRelativePathInDestination(tempDirectory, relativePath);

                var fileName = Path.GetFileName(path);
                var fullPath = Path.Combine(tempDirectory, relativeDirectory, fileName);
                var destFileName = Path.Combine(tempDirectory, fullPath);
                File.Copy(path, destFileName);
            }

            _output.WriteLine($"Copyed modified files into staging area '{tempDirectory}'.");
            return tempDirectory;
        }
    }
}