using System;
using System.IO.Abstractions;
using ForcePush.CliParsing;
using ForcePush.Output;
using Ninject;
using Ninject.Extensions.Conventions;

namespace ForcePush
{
    public class Program
    {
        public static StandardKernel Container { get; set; }
        public static ArgumentBinder Binder => Container.Get<ArgumentBinder>();
        public static SalesForcePackager Runner => Container.Get<SalesForcePackager>();
        public static IOutput Output => Container.Get<IOutput>();

        static void Main(string[] args)
        {
            var commandLineArgs = new CommandLineArgs();

            Container = CreateContainer();
            Output.WriteLine("ForcePush - SalesForce metadata packager.");

            #if RELEASE
            if (args.Length == 0)
            {
                Binder.Hint<CommandLineArgs>().ForEach(Output.WriteLine);
                return;
            }
            #endif

            try
            {
                commandLineArgs = Binder.Bind<CommandLineArgs>(args);
                Runner.CreateSalesforceDelta(commandLineArgs);
            }
            catch (Exception ex)
            {
                Output.WriteLine(commandLineArgs.Debug ? ex.ToString() : ex.Message);
                Binder.Hint<CommandLineArgs>().ForEach(Output.WriteLine);
                Environment.ExitCode = -1;
            }
        }

        private static StandardKernel CreateContainer()
        {
            var container = new StandardKernel();
            container.Bind(x => x.FromAssemblyContaining<IFileSystem>().SelectAllClasses().BindAllInterfaces());
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().BindAllInterfaces());
            container.Bind(x => x.FromThisAssembly().SelectAllClasses().BindToSelf());
            return container;
        }
    }
}