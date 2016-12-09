using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Humanizer;

namespace ForcePush.ManifestCreation
{
    public class PackageXmlGenerator
    {
        private readonly IFileSystem _fs;

        public PackageXmlGenerator(IFileSystem fs)
        {
            _fs = fs;
        }

        public Package GenerateFor(string repo)
        {
            var packageTypes = new List<PackageTypes>();

            foreach (var directory in _fs.Directory.GetDirectories(repo))
            {
                var dir = directory.Split('\\').Last(x => !string.IsNullOrWhiteSpace(x));
                var packageName = dir.Singularize() ?? dir;
                packageName = packageName.Pascalize() ?? dir;

                var specialClasses = new List<string> {"Class", "Component", "Page", "Trigger"};
                packageName = specialClasses.Contains(packageName) ? "Apex" + packageName : packageName;

                packageTypes.Add(new PackageTypes
                {
                    name = packageName,
                    members = new[] {"*"}
                });
            }

            return new Package
            {
                types = packageTypes.ToArray(),
                version = 36.0m
            };
        }

        public string Serialize(Package package)
        {
            var serialzier = new XmlSerializer(typeof(Package));
            var buffer = new StringBuilder();
            using (var writer = new StringWriter(buffer))
            {
                serialzier.Serialize(writer, package);
            }

            buffer.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            return buffer.ToString();
        }
    }

}