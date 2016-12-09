using System.IO;
using System.IO.Abstractions;
using ForcePush.Diffing;
using ForcePush.ManifestCreation;
using ForcePush.Packaging;

namespace ForcePush
{
    public class SalesForcePackager
    {
        private readonly DiffCollector _differ;
        private readonly Bundler _bundler;
        private readonly FilePackager _packager;
        private readonly PackageXmlGenerator _manifestGenerator;
        private readonly IFileSystem _fs;

        public SalesForcePackager(DiffCollector differ, Bundler bundler, FilePackager packager, PackageXmlGenerator manifestGenerator, IFileSystem fs)
        {
            _differ = differ;
            _bundler = bundler;
            _packager = packager;
            _manifestGenerator = manifestGenerator;
            _fs = fs;
        }

        public string CreateSalesforceDelta(CommandLineArgs args)
        {
            var changes = _differ.RetrieveChanges(args.Repo, args.TargetBranch, args.SourceBranch);
            var tempDirectory = _bundler.CreateTempDirectoryFromDiff(changes);
            var metaDataRoot = Path.Combine(tempDirectory, args.MetadataDirectory);

            var manifest = _manifestGenerator.GenerateFor(metaDataRoot);
            var manifestXml = _manifestGenerator.Serialize(manifest);

            _fs.File.WriteAllText(_fs.Path.Combine(metaDataRoot, "package.xml"), manifestXml);
            _packager.Package(metaDataRoot, args.OutputLocation);

            return args.OutputLocation;
        }
    }
}
