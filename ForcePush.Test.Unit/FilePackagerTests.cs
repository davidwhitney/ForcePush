using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NUnit.Framework;

namespace ForcePush.Test.Unit
{
    [TestFixture]
    public class FilePackagerTests
    {
        private FilePackager _fp;
        private string _tempDir;

        [SetUp]
        public void SetUp()
        {
            _fp = new FilePackager();
            _tempDir = TempDirectory.Create("FilePackagerTests");
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_tempDir, true);
        }

        [Test]
        public void Package_DiffContainsFile_FileIsAddedToRepoCorrectly()
        {
            var diff = RepoContainsDiffFor(@"FakeDiffDirectory");

            var path = _fp.Package(diff, _tempDir + "\\Test.zip");

            using (var zipFile = ZipFile.OpenRead(path))
            {
                var firstFile = zipFile.Entries.Single().FullName;
                Assert.That(firstFile, Is.EqualTo("SomeCode.txt"));
            }
        }

        private GitDiff RepoContainsDiffFor(string directory)
        {
            var root = CopyFakeRepoToTempDirectory(directory);

            var tempDirWithoutDrive = _tempDir.Replace(root, "\\");
            var filesInTempDir = Directory.GetFiles(_tempDir);
            var testFiles = new List<string>();
            foreach (var file in filesInTempDir)
            {
                testFiles.Add(Path.Combine(tempDirWithoutDrive, file));
            }

            var gd = new GitDiff {RootPath = _tempDir };
            gd.AddRange(testFiles.Select(file => "/" + file.Replace(root, "").Replace(@"\", "/")));
            return gd;
        }

        private string CopyFakeRepoToTempDirectory(string directory)
        {
            var cwd = (Path.GetDirectoryName(GetType().Assembly.CodeBase) ?? "").Replace(@"file:\", "");
            var root = Directory.GetDirectoryRoot(cwd);
            var source = Path.Combine(cwd, directory);
            Copy.Tree(source, _tempDir);

            //foreach (var canonicalPath in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            //{
            //    var fileName = Path.GetFileName(canonicalPath);
            //    var destinationPath = Path.Combine(_tempDir, fileName);
            //    File.Copy(canonicalPath, destinationPath);
            //}
            return root;
        }
    }
}
