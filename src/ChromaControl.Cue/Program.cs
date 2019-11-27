using Microsoft.Extensions.Hosting;
using RGBKit.Core;
using ChromaControl.Shared;

namespace ChromaControl.Cue
{
    /// <summary>
    /// The Chroma Control service program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The program entry point
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public static void Main(string[] args)
        {
            if (!Utilities.InitializeEnvironment("ChromaControl.Cue", "00000000-0000-0000-0000-000000000000", args))
                return;

            Utilities.CreateDefaultHost(args)
                .ConfigureRGBKit(rgbKit =>
                {
                    rgbKit.UseCue();
                })
                .Build().Run();
        }
    }
}
