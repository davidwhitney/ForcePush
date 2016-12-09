using System;
using System.IO.Abstractions;
using ForcePush.CliParsing;
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

            var binder = container.Get<ArgumentBinder>();
            var runner = container.Get<SalesForcePackager>();

            if (args.Length == 0)
            {
                var help = binder.Hint<CommandLineArgs>();
                help.ForEach(Console.WriteLine);
                return;
            }

            try
            {
                var paramz = binder.Bind<CommandLineArgs>(args);

                runner.CreateSalesforceDelta(paramz);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.ExitCode = -1;
            }
        }

    }
}