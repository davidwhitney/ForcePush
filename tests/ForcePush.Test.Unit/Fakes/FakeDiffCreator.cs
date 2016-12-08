using System.Collections.Generic;
using System.IO;
using System.Linq;
using ForcePush.Diffing;
using ForcePush.Packaging;

namespace ForcePush.Test.Unit.Fakes
{
    public class FakeDiffCreator
    {
        public static GitDiff FakeDiffFor(string path, string tempDirectory)
        {
            var root = CopyFakeRepoToTempDirectory(path, tempDirectory);

            var tempDirWithoutDrive = tempDirectory.Replace(root, "\\");
            var filesInTempDir = Directory.GetFiles(tempDirectory);
            var testFiles = new List<string>();
            foreach (var file in filesInTempDir)
            {
                testFiles.Add(Path.Combine(tempDirWithoutDrive, file));
            }

            var gd = new GitDiff { RootPath = tempDirectory };
            gd.AddRange(testFiles.Select(file => "/" + file.Replace(root, "").Replace(@"\", "/")));
            return gd;
        }
        private static string CopyFakeRepoToTempDirectory(string directory, string tempDirectory)
        {
            var cwd = (Path.GetDirectoryName(typeof(FakeDiffCreator).Assembly.CodeBase) ?? "").Replace(@"file:\", "");
            var root = Directory.GetDirectoryRoot(cwd);
            var source = Path.Combine(cwd, directory);
            Copy.Tree(source, tempDirectory);
            return root;
        }
    }
}