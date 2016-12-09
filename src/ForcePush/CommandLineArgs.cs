using ForcePush.CliParsing;

namespace ForcePush
{
    public class CommandLineArgs
    {
        public string Repo { get; set; } = @"C:\dev\euromoney.events";
        public string TargetBranch { get; set; } = "master";
        public string SourceBranch { get; set; } = "feature/gitdiff";
        public string OutputLocation { get; set; } = @"c:\dev\Delta.zip";
        [Optional] public string MetadataDirectory { get; set; } = "src";
    }
}