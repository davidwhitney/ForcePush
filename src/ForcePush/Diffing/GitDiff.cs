using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ForcePush.Diffing
{
    public class GitDiff : List<string>
    {
        public string RootPath { get; set; }
        public string Branch { get; set; }

        public List<string> ToWindowsPaths()
        {
            return this.Select(file => Path.Combine(RootPath, file.Replace("/", "\\"))).ToList();
        }
    }
}