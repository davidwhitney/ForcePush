using System.IO;
using ForcePush.Diffing;

namespace ForcePush.Packaging
{
    public class Bundler
    {
        public string CreateTempDirectoryFromDiff(GitDiff diff)
        {
            // checkout correct branch

            var tempDirectory = CopyTree(diff);

            return tempDirectory;
        }

        private static string CopyTree(GitDiff diff)
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
            return tempDirectory;
        }
    }
}