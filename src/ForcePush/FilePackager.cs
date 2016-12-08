using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Schema;

namespace ForcePush
{
    public class FilePackager
    {
        public string Package(GitDiff diff, string outputTarget)
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

            ZipFile.CreateFromDirectory(tempDirectory, outputTarget);
            return outputTarget;
        }

    }
}