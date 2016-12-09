using System;
using System.IO.Abstractions;
using ForcePush.CliParsing;
using Ninject;
using Ninject.Extensions.Conventions;

namespace ForcePush
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = new StandardKernel();
            container.Bind(x => x.FromAssemblyContaining<IFileSystem>().SelectAllClasses().BindAllInterfaces());
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().BindToSelf());

            var binder = container.Get<ArgumentBinder>();
            var runner = container.Get<SalesForcePackager>();

            Console.WriteLine("ForcePush - SalesForce metadata packager.");

            //#if RELEASE
            if (args.Length == 0)
            {
                binder.Hint<CommandLineArgs>().ForEach(Console.WriteLine);
                return;
            }
            //#endif

            try
            {
                var commandLineArgs = binder.Bind<CommandLineArgs>(args);
                runner.CreateSalesforceDelta(commandLineArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                binder.Hint<CommandLineArgs>().ForEach(Console.WriteLine);
                Environment.ExitCode = -1;
            }
        }
    }
}