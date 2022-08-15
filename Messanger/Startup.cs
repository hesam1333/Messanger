
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Messenger.Middelware;
using Messenger.Brockers;
using Messenger.Domain;
using Messenger.Services;

namespace Messenger
{
    public class Startup
    {
        public Startup(IConfiguration Configuration, IWebHostEnvironment env)
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
            app.UseMiddleware<SocketMiddleware>();
        }



        private void AddCore(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(HubPool));
            services.AddTransient(typeof(ApplicationService));
            services.AddTransient(typeof(ISerializationBroker), typeof(SerializationBrocker));
            services.AddTransient(typeof(ISocketBrocker), typeof(SocketBrocker));
        }


    }
}