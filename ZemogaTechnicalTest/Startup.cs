using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZemogaTechnicalTest.Data;
using ZemogaTechnicalTest.Interfaces;
using ZemogaTechnicalTest.Repositories;
using ZemogaTechnicalTest.Tools;

namespace ZemogaTechnicalTest
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
            services.AddHttpContextAccessor();
            services.AddCors(options => {
                options.AddDefaultPolicy(builder => builder.WithOrigins("*").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Interfaces implementations
            services.AddTransient<PostInterface, PostRepository>();
            services.AddTransient<AuthenticationInterface, AuthenticationRepository>();
            services.AddTransient<UserInterface, UserRepository>();
            #endregion
            services.AddDbContext<ZemogaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("zemogaTest")));

            // This service is configured for define the global variables of the project. It is defined using the appsettings
            Action<GlobalVariables> globalVariables = (gl =>
            {
                gl.InitialStatus = Int32.Parse(Configuration["GlobalVariables:InitialStatus"]);
                gl.PendingStatus = Int32.Parse(Configuration["GlobalVariables:PendingStatus"]);
                gl.PublishedStatus = Int32.Parse(Configuration["GlobalVariables:PublishedStatus"]);
                gl.RejectedStatus = Int32.Parse(Configuration["GlobalVariables:RejectedStatus"]);
                gl.DeletedStatus = Int32.Parse(Configuration["GlobalVariables:DeletedStatus"]);
                gl.WriterRole = Int32.Parse(Configuration["GlobalVariables:WriterRole"]);
                gl.EditorRole = Int32.Parse(Configuration["GlobalVariables:EditorRole"]);
            });
            services.Configure(globalVariables);
            services.AddSingleton(s => s.GetRequiredService<IOptions<GlobalVariables>>().Value);

            // JWT Authentication configuration
            services.Configure<JwtAuthentication>(Configuration.GetSection("JwtAuthentication"));
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            //services.AddDatabaseDeveloperPageExceptionFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers.Add("access-control-allow-headers", "content-type");
                    context.Response.Headers.Add("access-control-allow-origin", "*");
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = error.Error;
                        await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            StatusCode = 400,
                            ErrorMessage = ex.Message
                        }));
                    }
                });
            });
            
            app.UseCors();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
        {
            private readonly IOptions<JwtAuthentication> _jwtAuthentication;

            public ConfigureJwtBearerOptions(IOptions<JwtAuthentication> jwtAuthentication)
            {
                _jwtAuthentication = jwtAuthentication ?? throw new System.ArgumentNullException(nameof(jwtAuthentication));
            }

            public void PostConfigure(string name, JwtBearerOptions options)
            {
                var jwtAuthentication = _jwtAuthentication.Value;

                options.ClaimsIssuer = jwtAuthentication.ValidIssuer;
                options.IncludeErrorDetails = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtAuthentication.ValidIssuer,
                    ValidAudience = jwtAuthentication.ValidAudience,
                    IssuerSigningKey = jwtAuthentication.SymmetricSecurityKey,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            }
        }
    }
}
