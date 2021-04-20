using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

using Npgsql;

using GTGrimServer.Database.Controllers;
using GTGrimServer.Utils;
using GTGrimServer.Config;
using GTGrimServer.Filters;
using GTGrimServer.Services;

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

            AddJWTAuthentication(services);

            services.AddMvc(options =>
            {
                var settings = new XmlWriterSettings() { Indent = true, IndentChars = string.Empty, NewLineChars = "\n", NewLineHandling = NewLineHandling.Replace, OmitXmlDeclaration = false };
                options.OutputFormatters.Add(new XmlSerializerOutputFormatterNamespace(settings));
            }).AddXmlSerializerFormatters();


            // Grim Related Services
            services.Configure<GameServerOptions>(Configuration.GetSection(GameServerOptions.GameServer));
            services.AddSingleton<PlayerManager>();

            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(Configuration["Database:ConnectionString"]));
            services.AddSingleton<UserDBManager>();
            services.AddSingleton<FriendDBManager>();
            services.AddSingleton<UserSpecialDBManager>();
            services.AddSingleton<BbsBoardDBManager>();
            services.AddSingleton<CourseDBManager>();
            services.AddSingleton<ActionLogDBManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("Init: Configuring HTTP server");
            
            InitDatabase(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseAuthorization();

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

        public void InitDatabase(IServiceProvider services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.GetService<UserDBManager>().CreateTableIfNeeded();
            services.GetService<FriendDBManager>().CreateTableIfNeeded();
            services.GetService<UserSpecialDBManager>().CreateTableIfNeeded();
            services.GetService<CourseDBManager>().CreateTableIfNeeded();
            services.GetService<BbsBoardDBManager>().CreateTableIfNeeded();
            services.GetService<ActionLogDBManager>().CreateTableIfNeeded();
        }

        public void AddJWTAuthentication(IServiceCollection services)
        {
            var authBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            authBuilder.AddJwtBearer(jwt =>
            {
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                };
                
                jwt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["X-gt-token"];
                        return Task.CompletedTask;
                    },
                };
            });
        }
    }
}
