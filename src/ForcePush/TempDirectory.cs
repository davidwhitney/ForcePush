using System;
using System.IO;
using System.IO.Abstractions;

namespace ForcePush
{
    public static class TempDirectory
    {
        public static IFileSystem Fs = new FileSystem();

        public static string Create(string prefix = "")
        {
            var tempDir = Fs.Path.GetTempPath();
            var guid = Guid.NewGuid();
            var identifier = guid.ToString().Substring(0, 6);
            var formattableString = !string.IsNullOrWhiteSpace(prefix) ? prefix + "\\" + identifier : identifier;
            var path = Path.Combine(tempDir, formattableString);
            Fs.Directory.CreateDirectory(path);
            return path;
        }
    }
}