using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace ForcePush
{
    public class GitDiff : List<string>
    {
        public string RootPath { get; set; }

        public List<string> ToWindowsPaths()
        {
            var root = Directory.GetDirectoryRoot(RootPath);
            return this.Select(file => root + file.Replace("/", "\\")).ToList();
        }
    }
}