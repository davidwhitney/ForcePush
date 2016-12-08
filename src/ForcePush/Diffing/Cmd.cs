using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ForcePush.Diffing
{
    public class Cmd : ICmd
    {
        public List<string> Execute(string command, string workingDirectory = "")
        {

            var p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "cmd.exe",
                    Arguments = "/c " + command
                }
            };

            if (!string.IsNullOrWhiteSpace(workingDirectory))
            {
                p.StartInfo.WorkingDirectory = workingDirectory;
            }

            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            var errorOutput = p.StandardError.ReadToEnd();
            p.WaitForExit();

            if (!string.IsNullOrWhiteSpace(errorOutput))
            {
                throw new Exception("Cmd.exe Operation errored with message:\r\n\r\n" + errorOutput);
            }

            return output.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}