using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.ApplicationService.UserModule.Implements;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.ApplicationService.VipModule.Implements;
using BM.Auth.Domain;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Constant.Database;
using BM.Shared.ApplicationService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace BM.Auth.ApplicationService.StartUp
{
    public static class AuthStartUp
    {
        public static void ConfigureAuth(this WebApplicationBuilder builder, string? assemblyName)
        {
            // Database configuration
            builder.Services.AddDbContext<AuthDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("Default"),
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(assemblyName);
                            sqlOptions.MigrationsHistoryTable(
                                DbSchema.TableMigrationsHistory,
                                DbSchema.Auth
                            );
                        }
                    );
                },
                ServiceLifetime.Scoped
            );

            //builder.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = builder.Configuration["Redis:ConnectionString"]; // Ví dụ: "localhost:6379"
            //    options.InstanceName = "SampleInstance";
            //});

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.Configure<EmailSettings>(
            builder.Configuration.GetSection("MailSettings"));

            // Register services
            builder.Services.AddScoped<IAuthPositionService, AuthPositionService>();
            builder.Services.AddScoped<IAuthEmpService, AuthEmpService>();
            builder.Services.AddScoped<IAuthSpecService, AuthSpecService>();
            builder.Services.AddScoped<IAuthUserService, AuthUserService>();
            builder.Services.AddScoped<IAuthCustomerService, AuthCustomerService>();
            builder.Services.AddScoped<IAuthScheduleService, AuthScheduleService>();
            builder.Services.AddScoped<IAuthScheEmpService, AuthScheEmpService>();
            builder.Services.AddScoped<IAuthRoleService, AuthRoleService>();
            builder.Services.AddScoped<IAuthRolePermissionService, AuthRolePermissionService>();
            builder.Services.AddScoped<IAuthPermissionService, AuthPermissionService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IEmailCancelBooking , EmailService>();
            builder.Services.AddScoped<AuthDataSend , AuthUserService>();
            builder.Services.AddScoped<IGetCustomerByOrUserID , AuthCustomerService>();

            builder.Services.AddScoped<IAuthBranchService, AuthBranchService>();
            builder.Services.AddScoped<IAuthVipService, AuthVipService>();
            builder.Services.AddScoped<IAuthPromoService, AuthPromoService>();
            builder.Services.AddScoped<IAuthCusPromoService,  AuthCusPromoService>();
            builder.Services.AddScoped<IGetPromotionShared, AuthCusPromoService>();




            // JWT configuration
            var secretKey = builder.Configuration["Jwt:SecretKey"] ?? "A_very_long_and_secure_secret_key_1234567890";
            var key = Encoding.UTF8.GetBytes(secretKey);

            // Authentication setup
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // "Bearer"
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;       // "Bearer" (fixes the error)
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set to true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Consider enabling in production
                    ValidateAudience = false // Consider enabling in production
                };
            })

            ;

            // Authorization policies
            builder.Services.AddAuthorization(options =>
            {
                foreach (var permission in PermissionConstants.Permissions)
                {
                    options.AddPolicy(permission.Key, policy => policy.RequireClaim("Permission", permission.Value));
                }
            });
        }
    }
}