using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PactNet01.ProviderApi.FooIntegration;

namespace PactNet01.ProviderApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DotNetEnv.Env.TraversePath().Load();
        }
        
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(provider => new FooApiClient(FooApiConfiguration.BaseUri));

            services.AddControllers();

            services.AddSwaggerGen();

           // services.AddSwaggerGen(options =>
           //                        {
           //                            options.SwaggerDoc("v1", new OpenApiInfo
           //                                {
           //                                    Version = "v1",
           //                                    Title = "Something API",
           //                                    Description = "Something API description goes here",
           //                                    TermsOfService = new Uri("http://google.com/terms"),
           //                                    Contact = new OpenApiContact
           //                                        {
           //                                            Name = "John Smith",
           //                                            Email = "jsmith@foo.com",
           //                                            Url = new Uri("https://twitter.com/foo")
           //                                        },
           //                                    License = new OpenApiLicense
           //                                        {
           //                                            Name = "Use MIT",
           //                                            Url = new Uri("https://example.com/license")
           //                                        }
           //                                });
           //                        });
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
                             {
                                 options.SwaggerEndpoint("/swagger/v1/swagger.json", "Provider API");
                                 options.RoutePrefix = string.Empty;
                             });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
