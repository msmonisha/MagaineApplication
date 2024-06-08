using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagazineStore
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
           .ConfigureServices((hostContext, services) =>
           {
               services.AddHttpClient();
               services.AddTransient<IMagazine, Magazine>();
           }).UseConsoleLifetime();

            var host = builder.Build();

            try
            {
                var myService = host.Services.GetRequiredService<IMagazine>();
                var result = await myService.GetSubscriberDatas();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
       
    }
}
