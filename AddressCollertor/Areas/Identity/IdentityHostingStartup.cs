using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AddressCollector.Areas.Identity.IdentityHostingStartup))]
namespace AddressCollector.Areas.Identity
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