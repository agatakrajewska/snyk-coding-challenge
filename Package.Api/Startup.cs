using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Package.Api.Middleware;
using Package.Api.RetrieveDependencies;
using Swashbuckle.AspNetCore.Swagger;

namespace Package.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public string AppEnvironment => Environment.EnvironmentName;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = $"Package.Api v1 - ({AppEnvironment})", Version = "v1" });
            });


            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddOptions<NpmjsConfiguration>()
                .Bind(Configuration.GetSection("Npmjs"));

            services.AddSingleton<INpmjsService, NpmjsService>();
            services.AddHttpClient<INpmjsHttpService, NpmjsHttpService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseHealthChecks("/health");


            app.UseSwagger(options =>
            {
                if (!string.IsNullOrWhiteSpace(Configuration["SwaggerBaseUrl"]))
                {
                    options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.BasePath = Configuration["SwaggerBaseUrl"];
                    });
                }
                options.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../api/swagger/v1/swagger.json", $"Package.Api v1 - ({AppEnvironment})");
                c.RoutePrefix = "api";
            });


            app.UseMvc();
        }
    }
}
