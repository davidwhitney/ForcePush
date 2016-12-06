using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;

namespace ForcePush
{
    public class FilePackager
    {
        public ZipArchive Package(GitDiff diff)
        {
            var windowsPaths = diff.ToWindowsPaths();
            var tempDirectory = GetTempDir();

            foreach (var path in windowsPaths)
            {
                var fileName = Path.GetFileName(path);
                var filePath = Path.GetFullPath(path);
                // BUG: CREATE DIRECTORY STRUCTURE TOO!
                var destFileName = Path.Combine(tempDirectory, fileName);
                File.Copy(path, destFileName);
            }

            ZipFile.CreateFromDirectory(tempDirectory, @"c:\test.zip");

            return null;
        }
        
        private string GetTempDir()
        {
            var tempDir = Path.GetTempPath();
            var guid = Guid.NewGuid();
            var temp = Path.Combine(tempDir, $"ForcePush\\{guid.ToString().Substring(0, 6)}");
            Directory.CreateDirectory(temp);
            return temp;
        }
    }
}