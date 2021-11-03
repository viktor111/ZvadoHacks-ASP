using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ZvadoHacks.Areas.Identity.IdentityHostingStartup))]
namespace ZvadoHacks.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}