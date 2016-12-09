using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using ForcePush.Diffing;
using ForcePush.Test.Unit.Fakes;
using NUnit.Framework;

namespace ForcePush.Test.Unit.Diffing
{
    [TestFixture]
    public class DiffCollectorTests
    {
        private DiffCollector _differ;
        private MockFileSystem _fakeFilesystem;
        private FakeCmd _fakeCommandRunner;

        [SetUp]
        public void SetUp()
        {
            _fakeFilesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"c:\repo\file1.js", new MockFileData("file1")},
                {@"c:\repo\file2.js", new MockFileData("file2")},
            });

            _fakeCommandRunner = new FakeCmd();

            _differ = new DiffCollector(_fakeFilesystem, _fakeCommandRunner);
        }

        [TestCase]
        public void RetrieveChanges_PathNotFound_Throws()
        {
            var ex = Assert.Throws<DirectoryNotFoundException>(() => _differ.RetrieveChanges(@"c:\doesnotexist", "", ""));

            Assert.That(ex.Message, Does.Contain(@"c:\doesnotexist"));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void RetrieveChanges_GivenNull_ThrowsUsefulError(string path)
        {
            Assert.Throws<ArgumentNullException>(() => _differ.RetrieveChanges(path, "", ""));
        }

        [Test]
        public void RetrieveChanges_GivenValidPath_ReturnsExpectedChangedFile()
        {
            _fakeCommandRunner.Add("addedfile.txt");

            var diff = _differ.RetrieveChanges(@"c:\repo", "master", "other");

            Assert.That(diff, Does.Contain("addedfile.txt"));
        }

        [Test]
        public void RetrieveChanges_GivenValidPath_ExecutesGitDiffForBranches()
        {
            _differ.RetrieveChanges(@"c:\repo", "master", "other");

            Assert.That(_fakeCommandRunner.LastCommand, Is.EqualTo("git diff --name-only master...other"));
        }

        [Test]
        public void RetrieveChanges_GivenValidPath_RemembersWhereFilesWereStored()
        {
            var diff = _differ.RetrieveChanges(@"c:\repo", "master", "other");

            Assert.That(diff.RootPath, Is.EqualTo(@"c:\repo"));
        }

        [Test]
        public void RetrieveChanges_GivenValidPath_BranchIsTracked()
        {
            var diff = _differ.RetrieveChanges(@"c:\repo", "master", "other");

            Assert.That(diff.Branch, Is.EqualTo("other"));
        }
    }
}