using System.IO;
using ForcePush.Diffing;

namespace ForcePush.Packaging
{
    public class Bundler
    {
        public string CreateTempDirectoryFromDiff(GitDiff diff)
        {
            var windowsPaths = diff.ToWindowsPaths();
            var tempDirectory = TempDirectory.Create();

            foreach (var path in windowsPaths)
            {
                var filePath = Path.GetFullPath(path);

                var relativePath = filePath.Replace(diff.RootPath, "");
                var relativeDirectory = (Path.GetDirectoryName(relativePath) ?? "").TrimStart('\\');

                if (!string.IsNullOrWhiteSpace(relativeDirectory))
                {
                    var dirParts = relativeDirectory.Split('\\');
                    string current = tempDirectory;
                    foreach (var dirctoryPart in dirParts)
                    {
                        current = Path.Combine(current, dirctoryPart);
                        if (!Directory.Exists(current))
                        {
                            Directory.CreateDirectory(current);
                        }
                    }
                }


                // BUG: CREATE DIRECTORY STRUCTURE TOO!
                var fileName = Path.GetFileName(path);
                var destFileName = Path.Combine(tempDirectory, fileName);
                File.Copy(path, destFileName);
            }

            return tempDirectory;
        }
    }
}