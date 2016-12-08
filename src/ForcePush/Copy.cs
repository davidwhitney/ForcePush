using System.IO;
using System.Runtime.ConstrainedExecution;

namespace ForcePush
{
    public static class Copy
    {
        public static void Tree(string source, string destination)
        {
            foreach (var filePath in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            {
                File(source, destination, filePath);
            }
        }

        public static void File(string source, string destination, string filePath)
        {
            var relativePath = filePath.Replace(source, "");

            CreateRelativePathInDestination(destination, relativePath);

            relativePath = relativePath.TrimStart('\\');
            var destinationPath = Path.Combine(destination, relativePath);
            System.IO.File.Copy(filePath, destinationPath);
        }

        private static void CreateRelativePathInDestination(string destination, string relativePath)
        {
            var relativeDirectory = (Path.GetDirectoryName(relativePath) ?? "").TrimStart('\\');
            if (!string.IsNullOrWhiteSpace(relativeDirectory))
            {
                var dirParts = relativeDirectory.Split('\\');
                string current = destination;
                foreach (var dirctoryPart in dirParts)
                {
                    current = Path.Combine(current, dirctoryPart);
                    if (!Directory.Exists(current))
                    {
                        Directory.CreateDirectory(current);
                    }
                }
            }
        }
    }
}