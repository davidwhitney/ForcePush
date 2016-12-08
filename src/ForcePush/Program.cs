using System;
using System.IO.Abstractions;
using Ninject;
using Ninject.Extensions.Conventions;

namespace ForcePush
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new StandardKernel();
            container.Bind(x => x.FromAssemblyContaining<IFileSystem>().SelectAllClasses().BindAllInterfaces());
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().BindToSelf());
            var runner = container.Get<SalesForcePackager>();

            try
            {
                var repo = @"C:\dev\repo";
                var metadataDirectory = "src";
                var targetBranch = "master";
                var sourceBranch = "feature/gitdiff";
                var outputLocation = @"c:\dev\temp.zip";

                runner.CreateSalesforceDelta(repo, targetBranch, sourceBranch, outputLocation, metadataDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.ExitCode = -1;
            }
        }
    }
}