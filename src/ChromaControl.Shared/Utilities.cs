using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ChromaControl.Shared
{
    /// <summary>
    /// The utilities class
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Allocates a console window
        /// </summary>
        [DllImport("kernel32")]
        private static extern void AllocConsole();

        /// <summary>
        /// The application id
        /// </summary>
        internal static string ApplicationId = "Default";

        /// <summary>
        /// The application guid
        /// </summary>
        internal static string ApplicationGuid = "";

        /// <summary>
        /// Initializes the environment
        /// </summary>
        /// <param name="applicationId">The application id</param>
        /// <param name="applicationGuid">The application id</param>
        /// <param name="args">The command line arguments</param>
        /// <returns>If sucessful</returns>
        public static bool InitializeEnvironment(string applicationId, string applicationGuid, string[] args)
        {
            ApplicationId = applicationId;
            ApplicationGuid = applicationGuid;

            var mutex = new Mutex(true, applicationId, out var result);

            if (!result)
            {
                return false;
            }

            if (Debugger.IsAttached || args.Length > 0 && args[0] == "--console")
            {
                AllocConsole();
            }

            return true;
        }

        /// <summary>
        /// Creates the default host
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>A host builder</returns>
        public static IHostBuilder CreateDefaultHost(string[] args)
        {
            var logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ChromaControl\\logs");

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddFile(Path.Combine(logFolder, $"{ApplicationId}.log"), append: true);
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                });
        }
    }
}
