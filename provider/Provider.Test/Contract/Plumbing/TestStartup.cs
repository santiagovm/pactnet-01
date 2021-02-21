using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PactNet01.Provider.Test.Contract.Plumbing
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            // this is required to load controllers from startup and avoid HTTP 404 error
            // https://stackoverflow.com/a/58079778/725987
            services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ProviderStateMiddleware>();
            base.Configure(app, env);
        }
    }
}
