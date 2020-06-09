using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Nats.Client.Api
{
    /// <summary>
    /// Program class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="args">arguments of string[].</param>
        public static void Main(string[] args)
            => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}