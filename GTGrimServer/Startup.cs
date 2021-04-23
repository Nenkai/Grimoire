using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Threading;

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
        public ILogger<Startup> Logger { get; private set; }
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
            services.AddSingleton<PhotoDBManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            Logger = logger;
            Logger.LogInformation("Init: Configuring HTTP server");

            try
            {
                InitDatabase(app.ApplicationServices);
            }
            catch (Exception e)
            {
                Logger.LogCritical($"Init: Unable to init database. Make sure that the SQL service is running, and a connection to it is possible.");
                throw;
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                VerifyDevSecrets();
                    
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

            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                    logger.LogWarning($"Unimplemented endpoint: {context.HttpContext.Request.Path}");
            });

            Logger.LogInformation("Init: Done configuring host.");
        }

        public void InitDatabase(IServiceProvider services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            int attempts = 3;
            bool initialized = false;
            do
            {
                try
                {
                    CreateTables(services);
                    initialized = true;
                    break;
                }
                catch (Exception e)
                {
                    attempts--;
                    if (attempts > 0)
                    {
                        Logger.LogError(e, "Could not initialize database, trying {count} more times...", attempts);
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                }
            } while (!initialized && attempts != 0);

            if (!initialized)
                throw new Exception("Unable to initialize database.");
        }

        private void CreateTables(IServiceProvider services)
        {
            services.GetService<UserDBManager>().CreateTable();
            services.GetService<FriendDBManager>().CreateTable();
            services.GetService<UserSpecialDBManager>().CreateTable();
            services.GetService<CourseDBManager>().CreateTable();
            services.GetService<BbsBoardDBManager>().CreateTable();
            services.GetService<ActionLogDBManager>().CreateTable();
            services.GetService<PhotoDBManager>().CreateTable();
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

        private void VerifyDevSecrets()
        {
            if (string.IsNullOrEmpty(Configuration["Database:ConnectionString"]))
            {
                throw new ArgumentException("Init: Db connection string missing in user secrets. (Database:ConnectionString)");
            }

            if (string.IsNullOrEmpty(Configuration["Jwt:Key"]))
            {
                throw new ArgumentException("Init:Jwt encryption key missing in user secrets. (Jwt:Key)");
            }

            if (string.IsNullOrEmpty(Configuration["Jwt:Issuer"]))
            {
                throw new ArgumentException("Init:Jwt issuer missing in user secrets. (Jwt:Issuer)");
            }

            if (string.IsNullOrEmpty(Configuration["Jwt:Audience"]))
            {
                throw new ArgumentException("Init:Jwt audience key missing in user secrets. (Jwt:Audience)");
            }

            Logger.LogInformation("Init: User Secrets OK");
        }
    }
}
