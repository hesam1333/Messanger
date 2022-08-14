
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Messenger.Domain
{
    public class Startup
    {
        public Startup(IConfiguration Configuration , IWebHostEnvironment env )
        {
            this.Configuration = Configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment env { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMemoryCache();
            services.AddResponseCaching();
            services.AddHttpContextAccessor();


            AddCore(services, Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebSockets();
        }



        private void AddCore(IServiceCollection services, IConfiguration configuration)
        {

        }


    }
}