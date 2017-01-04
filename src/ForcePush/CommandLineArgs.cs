using ForcePush.CliParsing;

namespace ForcePush
{
    public class CommandLineArgs
    {
        [Required, Annotation("The full path to your SalesForce repository")]
        public string Repo { get; set; }

        [Required, Annotation("Source of the incoming changes, your git branch name.")]
        public string SourceBranch { get; set; }

        [Annotation("Defaults to master")]
        public string TargetBranch { get; set; } = "master";

        [Annotation("The full path and filename to your output file. Defaults to Delta.zip")]
        public string OutputLocation { get; set; } = @"Delta.zip";

        [Annotation("If your SalesForce metadata is not in your repository root, provide a path here.")]
        public string MetadataDirectory { get; set; } = "src";

        [Annotation("Include stack traces in errors.")]
        public bool Debug { get; set; } = false;
    }
}