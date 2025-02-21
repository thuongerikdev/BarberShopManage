using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BM.Booking.Infrastructure;
using BM.Constant.Database;
namespace BM.Booking.ApplicationService.StartUp
{
    public static class BookingStartUp
    {
        public static void ConfigureBooking(this WebApplicationBuilder builder, string? assemblyName)
        {

            builder.Services.AddDbContext<BookingDbContext>(
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

            //builder.Services.AddScoped<IAuthPositionService, AuthPositionService>();
            //builder.Services.AddScoped<IAuthEmpService, AuthEmpService>();
            //builder.Services.AddScoped<IAuthSpecService, AuthSpecService>();
            //builder.Services.AddScoped<IAuthUserService, AuthUserService>();





        }
    }
}































