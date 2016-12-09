using System;
using System.IO;
using System.IO.Abstractions;

namespace ForcePush.Diffing
{
    public class DiffCollector
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICmd _commandRunner;

        public DiffCollector(IFileSystem fileSystem, ICmd commandRunner)
        {
            _fileSystem = fileSystem;
            _commandRunner = commandRunner;
        }

        public GitDiff RetrieveChanges(string gitDirectory, string firstBranch, string secondBranch)
        {
            if (string.IsNullOrWhiteSpace(gitDirectory)) throw new ArgumentNullException(nameof(gitDirectory));
            if (!_fileSystem.Directory.Exists(gitDirectory)) throw new DirectoryNotFoundException($"Cannot find directory '{gitDirectory}'.");

            var results = _commandRunner.Execute($"git diff --name-only {firstBranch}...{secondBranch}", gitDirectory);
            var gitDiff = new GitDiff {RootPath = gitDirectory, Branch = secondBranch};
            gitDiff.AddRange(results);
            return gitDiff;
        }
    }
}