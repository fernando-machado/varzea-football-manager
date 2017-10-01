using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace VarzeaFootballManager.Api
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Default entry point of Application
        /// </summary>
        /// <param name="args">Arguments</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Build web host with startup configurations
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Returns WebHost created</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
