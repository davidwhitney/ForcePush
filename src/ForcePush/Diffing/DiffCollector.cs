using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using ForcePush.Output;

namespace ForcePush.Diffing
{
    public class DiffCollector
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICmd _commandRunner;
        private readonly IOutput _output;

        public DiffCollector(IFileSystem fileSystem, ICmd commandRunner, IOutput output)
        {
            _fileSystem = fileSystem;
            _commandRunner = commandRunner;
            _output = output;
        }

        public GitDiff RetrieveChanges(string gitDirectory, string firstBranch, string secondBranch)
        {
            if (string.IsNullOrWhiteSpace(gitDirectory)) throw new ArgumentNullException(nameof(gitDirectory));
            if (!_fileSystem.Directory.Exists(gitDirectory)) throw new DirectoryNotFoundException($"Cannot find directory '{gitDirectory}'.");

            _output.WriteLine("Collecting Git-Diff...");

            var branchCheck = _commandRunner.Execute("git status");
            if (branchCheck.First() != $"On branch {secondBranch}")
            {
                _commandRunner.Execute($"git checkout -f {secondBranch}");
            }

            var results = _commandRunner.Execute($"git diff --name-only {firstBranch}...{secondBranch}", gitDirectory);
            var gitDiff = new GitDiff {RootPath = gitDirectory, Branch = secondBranch};
            gitDiff.AddRange(results);
            return gitDiff;
        }
    }
}