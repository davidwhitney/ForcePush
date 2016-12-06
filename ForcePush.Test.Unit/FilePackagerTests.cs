using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace ForcePush.Test.Unit
{
    [TestFixture]
    public class FilePackagerTests
    {
        private FilePackager _fp;
        private MockFileSystem _fakeFilesystem;

        [SetUp]
        public void SetUp()
        {
            _fakeFilesystem = new MockFileSystem();
            _fp = new FilePackager();
        }

        [Test]
        public void Package_DiffContainsFile_FileIsAddedToRepoCorrectly()
        {
            var diff = RepoContainsDiffFor(@"FakeDiffDirectory");

            var zipFile = _fp.Package(diff);
            var firstFile = zipFile.Entries.Single().FullName;

            Assert.That(firstFile, Is.EqualTo("FakeDiffDirectory\\SomeCode.txt"));
        }

        private GitDiff RepoContainsDiffFor(string directory)
        {
            var root = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());

            var testDir = Path.Combine(Directory.GetCurrentDirectory(), directory);
            var testDirWithoutDrive = testDir.Replace(root, "\\");

            var filesInTestDir = Directory.GetFiles(testDir);
            var testFiles = new List<string>();
            foreach (var file in filesInTestDir)
            {
                testFiles.Add(Path.Combine(testDirWithoutDrive, file));
            }

            var gd = new GitDiff {RootPath = testDir };
            gd.AddRange(testFiles.Select(file => "/" + file.Replace(root, "").Replace(@"\", "/")));
            return gd;
        }
    }
}
