using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Asakabank.Base;
using Asakabank.UserApi.Base;
using Asakabank.UserApi.Core;
using Asakabank.UserApi.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Asakabank.UserApi {
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

            //var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            //services.AddDbContext<DataContext>(options =>
            //                                       options.UseNpgsql(
            //                                           Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"),
            //                                           assembly =>
            //                                               assembly.MigrationsAssembly("Asakabank.UserApi"))
            //);
            services.AddDbContext<DataContext>(options => {
                options
                    .UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                               assembly =>
                                   assembly.MigrationsAssembly("Asakabank.UserApi"));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IDbRepository, DbRepository>();
            services.AddTransient<IBlogService, BlogService>();

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Asakabank.UserApi", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider) {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.Migrate();
            }

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Asakabank.UserApi v1"));

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