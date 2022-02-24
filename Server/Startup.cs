using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Services;

namespace Server
{
    /// <summary>
    /// Server startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Sets the available services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        /// <summary>
        /// Sets the routing and the available endpoints
        /// </summary>
        /// <param name="app">Application</param>
        /// <param name="env">Environment</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ParserService>();
                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync("Communication with this service is only valid via gRPC");
                    });
            });
        }
    }
}