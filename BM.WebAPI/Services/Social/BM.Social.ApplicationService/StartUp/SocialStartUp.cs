using BM.Social.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BM.Constant.Database;
using CloudinaryDotNet;
using BM.Social.ApplicationService.SocialModule.Abtracts;
using BM.Social.ApplicationService.SocialModule.Implements.Src;
using StackExchange.Redis;
using BM.Social.ApplicationService.SocialModule.Implements.Social;
using BM.Social.ApplicationService.SocialModule.Implements.Business;
using Microsoft.Extensions.Logging;
using BM.Shared.ApplicationService;

namespace BM.Social.ApplicationService.StartUp
{
    public static class SocialStartUp
    {
        public static void ConfigureSocial(this WebApplicationBuilder builder, string? assemblyName)
        {
            // Database configuration
            builder.Services.AddDbContext<SocialDbContext>(
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
            builder.Services.AddSignalR();

            // Cấu hình Cloudinary
            var cloudinaryConfig = builder.Configuration.GetSection("Cloudinary");
            var cloudName = cloudinaryConfig["CloudName"];
            var apiKey = cloudinaryConfig["ApiKey"];
            var apiSecret = cloudinaryConfig["ApiSecret"];

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new ArgumentException("Cloudinary configuration is missing or invalid in appsettings.json");
            }

            var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(cloudinaryAccount);

            // Đăng ký Cloudinary như một singleton
            builder.Services.AddSingleton(cloudinary);

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

            builder.Services.AddScoped<ILogger<CloudinaryService>, Logger<CloudinaryService>>();

            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

            // Register services
            builder.Services.AddScoped<ISocialBlogService, SocialBlogService>();
            builder.Services.AddScoped<ISocialBussiness, SocialBusiness>();
            builder.Services.AddScoped<ISocialCommentService , SocialCommentService>();
            builder.Services.AddScoped<ISocialGroupService, SocialGroupService>();
            builder.Services.AddScoped<ISocialGroupUserService, SocialGroupUserService>();
            builder.Services.AddScoped<ISocialMessageService, SocialMessageService>();
            builder.Services.AddScoped<ISocialBlogTopicService, SocialBlogTopicService>();
            builder.Services.AddScoped<ISocialBlogContentService, SocialBlogContentService>();
            builder.Services.AddScoped<ISocialBlogImageService, SocialBlogImageService>();


            //builder.Services.AddScoped<ISocialSrcBlogService, SocialSrcBlogService>();
            builder.Services.AddScoped<ISocialSrcService, SocialSrcService>();
            //builder.Services.AddScoped<ISocialSrcMessageService, SocialSrcMessageService>();


            // JWT configuration
        }
    }
}