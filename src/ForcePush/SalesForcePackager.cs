using System.IO;
using ForcePush.Diffing;
using ForcePush.Packaging;

namespace ForcePush
{
    public class SalesForcePackager
    {
        private readonly DiffCollector _differ;
        private readonly Bundler _bundler;
        private readonly FilePackager _packager;

        public SalesForcePackager(DiffCollector differ, Bundler bundler, FilePackager packager)
        {
            _differ = differ;
            _bundler = bundler;
            _packager = packager;
        }

        public string CreateSalesforceDelta(CommandLineArgs args)
        {
            var changes = _differ.RetrieveChanges(args.Repo, args.TargetBranch, args.SourceBranch);
            var tempDirectory = _bundler.CreateTempDirectoryFromDiff(changes);
            _packager.Package(Path.Combine(tempDirectory, args.MetadataDirectory), args.OutputLocation);

            return args.OutputLocation;
        }
    }
}
