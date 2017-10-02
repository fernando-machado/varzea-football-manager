using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using VarzeaFootballManager.Api.Extensions;
using VarzeaFootballManager.Domain.Core;
using VarzeaFootballManager.Domain.Jogadores;
using VarzeaFootballManager.Persistence.Repositorios;
using VarzeaFootballManager.Api.Mappers;

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
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initialize instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configure services of Application
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <remarks>This method gets called by the runtime. Use this method to add services to the container.</remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMvc().AddJsonOptions(c =>
            {
                c.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                c.SerializerSettings.Converters.Insert(0, new StringEnumConverter { CamelCaseText = true });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Version = "v1", Title = "API do App Futebol de Várzea", Description = "Uma api para gerenciar futebol amador." });

                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "VarzeaFootballManager.Api.xml"));

                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });

            services.AddMongoDb();

            AutomapperConfig.RegisterMappings();

            services.AddScoped<IRepository<Jogador>, Repository<Jogador>>();
            services.AddScoped<IRepositoryAsync<Jogador>, RepositoryAsync<Jogador>>();
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/html";

                        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            //LogException(error.Error, context);
                            byte[] message = System.Text.Encoding.UTF8.GetBytes("<h2>An error has occured in the website.</h2>");
                            await context.Response.Body.WriteAsync(message, 0, message.Length).ConfigureAwait(false);
                        }
                    });
                });
            }

            //app.UseStaticFiles();

            app.UseMvc(
                //routes =>
                //{
                //    routes.MapRoute(
                //        name: "default",
                //        template: "{controller=Home}/{action=Index}/{id?}"
                //    );
                //}
            );
            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API do App Futebol de Várzea V1");
            });
        }
    }
}
