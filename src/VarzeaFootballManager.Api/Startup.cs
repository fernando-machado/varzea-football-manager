using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace VarzeaFootballManager.Api
{
    /// <summary>
    /// Startup Application
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration Root
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Initialize instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="env">Hosting environment</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        /// <summary>
        /// Configure services of Application
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            //var pathToDoc = Configuration["Swagger:Path"];
            

            // Add framework services.
            services.AddMvc();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "API Gerenciador App Futebol de Várzea",
                    Description = "Uma api para gerenciar futebol amador.",
                    TermsOfService = "None"
                });

                var xmlCommentsFilePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "VarzeaFootballManager.Api.xml");
                options.IncludeXmlComments(xmlCommentsFilePath);

                options.DescribeAllEnumsAsStrings();
            });

            //services.AddScoped<ISearchProvider, SearchProvider>();
        }
        
        /// <summary>
        /// Configure services of Application
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Hosting Environment</param>
        /// <param name="loggerFactory">Logger Factory</param>
        /// <remarks>This method gets called by the runtime. Use this method to configure the HTTP request pipeline.</remarks>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler(builder =>
            //    {
            //        builder.Run(async context =>
            //        {
            //            context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            //            context.Response.ContentType = "text/html";

            //            var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            //            if (error != null)
            //            {
            //                //LogException(error.Error, context);
            //                await context.Response.WriteAsync("<h2>An error has occured in the website.</h2>").ConfigureAwait(false);
            //            }
            //        });
            //    });
            //}

            //app.UseStaticFiles();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}"
            //    );
            //});

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gerenciador App Futebol de Várzea V1");
            });
        }
    }
}
