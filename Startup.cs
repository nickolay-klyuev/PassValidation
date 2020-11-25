using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace PassValidation
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { 
                endpoints.MapGet("/ping", async context =>
                {
                    context.Response.StatusCode = 200;
                    await context.Response.CompleteAsync();
                });
                endpoints.MapGet("/validatePassword", async context =>
                {
                    string password = context.Request.Query["password"].ToString();
                    if (string.IsNullOrEmpty(password))
                    {
                        context.Response.StatusCode = 400;
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        PasswordStatus status = new PasswordStatus{ status = PasswordValidator(password) };
                        await context.Response.WriteAsync(JsonSerializer.Serialize(status));
                    }
                });
            });
        }

        public class PasswordStatus
        {
            public bool status { get; set; }
        }

        public static bool PasswordValidator(string password)
        {   
            return 
                !(password.Length != 16 || 
                password.IndexOfAny(".,_-!?*=".ToCharArray()) == -1 ||
                !password.Any(char.IsDigit) ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsLower));
        }
    }
}
