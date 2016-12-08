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

        public string CreateSalesforceDelta(string repoPath, string targetBranch, string sourceBranch, string outputPath)
        {
            var changes = _differ.RetrieveChanges(repoPath, targetBranch, sourceBranch);
            var tempDirectory = _bundler.CreateTempDirectoryFromDiff(changes);
            _packager.Package(tempDirectory, outputPath);

            return outputPath;
        }
    }
}
