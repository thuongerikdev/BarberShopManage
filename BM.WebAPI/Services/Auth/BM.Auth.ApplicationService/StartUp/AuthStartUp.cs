using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.ApplicationService.UserModule.Implements;
using BM.Auth.Infrastructure;
using BM.Constant.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BM.Auth.ApplicationService.StartUp
{
    public static class AuthStartUp
    {
        public static void ConfigureAuth(this WebApplicationBuilder builder, string? assemblyName)
        {

            builder.Services.AddDbContext<AuthDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("Default"),
                        options =>
                        {
                            options.MigrationsAssembly(assemblyName);
                            options.MigrationsHistoryTable(
                                DbSchema.TableMigrationsHistory,
                                DbSchema.Auth
                            );
                        }
                    );
                },
                ServiceLifetime.Scoped
            );

            builder.Services.AddScoped<IAuthPositionService, AuthPositionService>();
            builder.Services.AddScoped<IAuthEmpService, AuthEmpService>();
            builder.Services.AddScoped<IAuthSpecService, AuthSpecService>();
            builder.Services.AddScoped<IAuthUserService, AuthUserService>();
            builder.Services.AddScoped<IAuthCustomerService, AuthCustomerService>();
            builder.Services.AddScoped<IAuthScheduleService, AuthScheduleService>();
            builder.Services.AddScoped<IAuthScheEmpService, AuthScheEmpService>();


            var secretKey = builder.Configuration["Jwt:SecretKey"] ?? "A_very_long_and_secure_secret_key_1234567890";
            var key = Encoding.UTF8.GetBytes(secretKey);
            // Thay đổi secret key của bạn

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie()
            //.AddGoogle( GoogleDefaults.AuthenticationScheme ,   googleOptions =>
            //{
            //    googleOptions.ClientId = builder.Configuration.GetSection("Google:ClientId").Value ?? "811958613194 - aq0eag0lc78brobjetprjdvoikpv0c3m.apps.googleusercontent.com";
            //    googleOptions.ClientSecret = builder.Configuration.GetSection("Google:ClientSecret").Value ?? "GOCSPX - sKsTl1MtmbXot_J3MUnX - TZxdM5o";

            //})

            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });



        }
    }
}
