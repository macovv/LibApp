using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Books;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation.AspNetCore;
using Infrastructure.JwtService;
using Infrastructure.UserAccessor;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace LibApp.Api
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
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddMediatR(typeof(List.Handler).Assembly);

            services.AddDefaultIdentity<AppUser>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMvc().AddFluentValidation(cfg =>
                {
                    cfg.RegisterValidatorsFromAssemblyContaining<Add>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(
                        options => options.SerializerSettings.ReferenceLoopHandling
                            = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //CreateUserRoles(services).Wait();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }

        private async Task CreateUserRoles(IServiceCollection services)
        {
            var roleManager = services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
            var userManager = services.BuildServiceProvider().GetService<UserManager<AppUser>>();

            if (!(await roleManager.RoleExistsAsync("Admin")))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!(await roleManager.RoleExistsAsync("User")))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            AppUser user = await userManager.FindByEmailAsync("szampon@gmail.com");
            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}
