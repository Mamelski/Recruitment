using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Recruitment.API.Options;
using Recruitment.API.Services;

namespace Recruitment.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Recruitment.API", Version = "v1"});
            });
            
            // This way we dont need to pass IOptions<HashCalculatorOptions> and instead we can simply use IHashCalculatorOptions
            services
                .Configure<HashCalculatorOptions>(Configuration.GetSection(nameof(HashCalculatorOptions)))
                .AddSingleton<IHashCalculatorOptions>(provider => provider.GetRequiredService<IOptions<HashCalculatorOptions>>().Value);

            services
                .AddTransient<IHashService, HashService>()
                .AddHttpClient<IHashService, HashService>((provider, client) =>
                {
                    var options = provider.GetRequiredService<IHashCalculatorOptions>();
                    client.BaseAddress = new Uri(options.BaseUrl);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recruitment.API v1"));
            }
            
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    // Hide error, this can be handled differently
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Ups! something went wrong");
                });
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}