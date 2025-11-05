using Serilog;
using System.Diagnostics;

namespace Core
{
    public static class Logging
    {
        public static void ConfigureLogging()
        {
            var projectName = Process.GetCurrentProcess().ProcessName;
            DateTime utcDate = DateTime.UtcNow;
            var currentDateTime = utcDate.ToString("en-GB");

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File($"logs/{projectName}_{currentDateTime}.txt", rollingInterval: RollingInterval.Day)
            .Enrich.WithProperty("Framework", "CSharpTestFramework")
            .CreateLogger();
        }
        
    }
}
