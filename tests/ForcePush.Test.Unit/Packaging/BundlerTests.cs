using System.IO;
using ForcePush.Packaging;
using ForcePush.Test.Unit.Fakes;
using NUnit.Framework;

namespace ForcePush.Test.Unit.Packaging
{
    [TestFixture]
    public class BundlerTests
    {
        private string _tempDir;
        private Bundler _fp;

        [SetUp]
        public void SetUp()
        {
            _fp = new Bundler(new FakeOutput());
            _tempDir = TempDirectory.Create("FilePackagerTests");
        }

        [Test]
        public void Package_DiffContainsFile_FileIsAddedToRepoCorrectly()
        {
            var diff = FakeDiffCreator.FakeDiffFor(@"FakeDiffDirectory", _tempDir);

            var path = _fp.CreateTempDirectoryFromDiff(diff);
            var filesInPath = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            Assert.That(filesInPath.Length, Is.EqualTo(1));
        }
        
        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_tempDir, true);
        }
    }
}
