using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Architecture2.Common.Win.WinService.Interface;

namespace Architecture2.Common.Win.WinService
{
    public static class Runner
    {
        public static void Run<TService, TAppRunner>() where TService: ServiceBaseEx, new() where TAppRunner: IAppRunner, new()
        {
            var args = Environment.GetCommandLineArgs();
            // runs the app as a  console application if the command argument "-console" is used
            if (RunAsConsoleIfRequested<TAppRunner>(args))
                return;
            // uses "-install" and "-uninstall" to manage the service.
            if (ManageServiceIfRequested(args))
                return;
            using (var service = new TService())
            {
                using (var runner = new TAppRunner())
                {
                    service.SetAppRunner(runner);
                    ServiceBase.Run(service);                                
                }
            }
        }

        private static readonly string[] InstallArguments = { "/i", "/install", "-i", "-install" };
        private static readonly string[] UninstallArguments = { "/u", "/uninstall", "-u", "-uninstall" };
        private const string ConsoleArgument = "-console";

        private static void AttachConsole()
        {
            NativeMethods.AllocConsole();
        }

        private static bool RunAsConsoleIfRequested<T>(IEnumerable<string> args) where T : IAppRunner, new()
        {
            if (!args.Contains(ConsoleArgument))
                return false;
            AttachConsole();
            var a = Environment.GetCommandLineArgs().Where(WherePredicate).ToArray();
            using (var service = new T())
            {
                service.OnStart(a);
                Console.WriteLine("Your service named '{0}' is up and running.\r\nPress 'ENTER' to stop it.", service.GetType().FullName);
                Console.ReadLine();
                service.OnStop();
                Console.WriteLine("Service is stopped. Press any key to exit console");                
            }
            Console.ReadKey();
            return true;
        }

        private static bool WherePredicate(string name)
        {
            return name != ConsoleArgument;
        }

        private static bool ManageServiceIfRequested(string[] arguments)
        {
            try
            {
                if (!arguments.Any(x => InstallArguments.Contains(x)) && !arguments.Any(x => UninstallArguments.Contains(x)))
                    return false;
                AttachConsole();
                var serviceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstallUtil.InstallLog");
                if (File.Exists(serviceFile))
                    File.Delete(serviceFile);
                if (arguments.Any(x => InstallArguments.Contains(x)))
                    ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                else if (arguments.Any(x => UninstallArguments.Contains(x)))
                    ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
                else
                    return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: {0}", exception.Message);
            }
            Console.WriteLine("Service configuration is done. Press any key to exit console");
            Console.ReadKey();
            return true;
        }


    }
}