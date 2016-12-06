using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ForcePush
{
    public class Cmd : ICmd
    {
        public List<string> Execute(string command)
        {
            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    FileName = "cmd.exe",
                    Arguments = "/c " + command
                }
            };
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}