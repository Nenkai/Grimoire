using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

using Npgsql;

using GTGrimServer.Database.Controllers;
using GTGrimServer.Utils;
using GTGrimServer.Config;
using GTGrimServer.Filters;

namespace GTGrimServer
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
            Console.WriteLine("Init: Configuring service provider");
            services.AddControllers();

            // Config stuff
            services.Configure<GameServerOptions>(Configuration.GetSection(GameServerOptions.GameServer));

            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(Configuration["Database:ConnectionString"]));
            services.AddSingleton<UserDBManager>();

            services.AddMvc(options =>
            {
                var settings = new XmlWriterSettings() { OmitXmlDeclaration = false };
                options.OutputFormatters.Add(new XmlSerializerOutputFormatterNamespace(settings));
                //options.Filters.Add<PDIClientAttribute>();
            }).AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Init: Configuring HTTP server");
            var db = app.ApplicationServices.GetService<UserDBManager>();
            db.CreateTableIfNeeded();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;
                if (url.Contains("/init//regionlist.xml"))
                    context.Request.Path = "/init/regionlist.xml";

                await next();
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
