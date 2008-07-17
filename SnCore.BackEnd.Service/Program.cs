using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using SnCore.Data.Hibernate;
using SnCore.BackEndServices;
using Microsoft.CommandLine;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace SnCore.BackEnd.Service
{
    class ProgramCommandLineArguments
    {
        [Argument(ArgumentType.AtMostOnce, DefaultValue = true, HelpText = "Run in debug mode on command line.")]
        public bool debug = false;
    }

    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    AllocConsole();

                    AssemblyName name = Assembly.GetExecutingAssembly().GetName();
                    Console.WriteLine("{0}: {1}", name.Name, name.Version);
                    ProgramCommandLineArguments pargs = new ProgramCommandLineArguments();
                    if (Parser.ParseArgumentsWithUsage(args, pargs))
                    {
                        if (pargs.debug)
                        {
                            ServiceEngine serviceEngine = new ServiceEngine();

                            Console.CancelKeyPress += delegate
                            {
                                Console.WriteLine("Stopping service, please wait ...");
                                serviceEngine.StopOnConsole();
                            };

                            serviceEngine.RunOnConsole();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Press any key to continue ...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Press any key to continue ...");
                    Console.ReadKey();
                }
                finally
                {
                    FreeConsole();
                }
            }
            else
            {
                ServiceEngine serviceEngine = new ServiceEngine();
                serviceEngine.RunAsService();
            }
        }
    }
}