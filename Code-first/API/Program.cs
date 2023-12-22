using API;
using Microsoft.AspNetCore.Server.Kestrel.Core;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }



    public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)

             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.ConfigureKestrel(options =>
                 {
                     options.ConfigureEndpointDefaults(lo => lo.Protocols = HttpProtocols.Http2);
                 });

                 webBuilder.UseStartup<Startup>();
             });
}

