using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NewsService.Models;
using NewsService.Repository;
using NewsService.Services;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace NewsService
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
            services.AddControllers();
            services.AddSingleton(new NewsContext(Configuration));
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, Services.NewsService>();
            //register all dependencies here
            //Implement token validation logic
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(op =>
                {
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer=Configuration["Audience:Iss"],
                        ValidAudience=Configuration["Audience:Auidence"],
                        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Audience:Secret"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            //Add extension method i.e Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ValidateToken(IConfiguration configuration, IServiceCollection services)
        {
            //Implement validate token logic
        }
    }
}
