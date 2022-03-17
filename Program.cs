using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace VStoreAPI
{
    public static class Program
    {
        public static class DotEnv
        {
            public static void Load(string filePath)
            {
                if (!File.Exists(filePath))
                    return;

                foreach (var line in File.ReadAllLines(filePath))
                {
                    int index = line.IndexOf('=');
                    string key = line[..index];
                    string value = line[(index + 1)..];

                    if (Environment.GetEnvironmentVariable(key) is null) //If this variable don't exists
                        Environment.SetEnvironmentVariable(key, value);
                    else
                        Console.WriteLine(
                            $"\"the environment variable [\"{key}\"] already exists in your system, " +
                            "be aware about it, .env file will not overwrite it\"");
                }
            }
        }
        public static void Main(string[] args)
        {
            DotEnv.Load("./.env");
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
