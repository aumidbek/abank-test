using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Asakabank.Base;
using Asakabank.IdentityApi.Core;
using Asakabank.IdentityApi.Helpers;
using Asakabank.IdentityApi.Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Asakabank.IdentityApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers(options => {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                    new SlugifyParameterTransformer()));
                options.Filters.Add(new ErrorFilter());
            });

            var tokenKey = Configuration.GetValue<string>("TokenKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);

            services.AddAuthentication(x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x => {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            
            services.AddDbContext<DataContext>(options => {
                options
                    .UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                        assembly =>
                            assembly.MigrationsAssembly("Asakabank.IdentityApi"));
            });

            services.AddScoped<IDbRepository, DbRepository>();
            services.AddSingleton<IJwtRefreshManager>(x =>
                new JwtRefreshManager(key, x.GetService<IJwtAuthenticationManager>()));
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IJwtAuthenticationManager>(x =>
                new JwtAuthenticationManager(tokenKey, x.GetService<IRefreshTokenGenerator>(),
                    x.GetService<IDbRepository>()));


            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Asakabank.IdentityApi", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Asakabank.IdentityApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private class SlugifyParameterTransformer : IOutboundParameterTransformer {
            public string TransformOutbound(object value) => value == null
                ? null
                : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }

        private class ErrorFilter : ExceptionFilterAttribute {
            public override async Task OnExceptionAsync(ExceptionContext context) {
                var exception = context.Exception;
                var response = $"{{\"error\": \"{exception.Message}{exception.InnerException?.Message}\"}}";
                await using var responseWriter = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
                context.HttpContext.Response.ContentLength = Encoding.UTF8.GetBytes(response).Length + 3;
                await responseWriter.WriteAsync(response);
            }
        }
    }
}