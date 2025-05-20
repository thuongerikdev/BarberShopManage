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
using BM.Booking.ApplicationService.BussinessModule.Abtracts;
using BM.Booking.ApplicationService.BussinessModule.Implements;
using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.BookingModule.Implements;
using BM.Booking.ApplicationService.PaymentModule.Abtracts;
using BM.Booking.ApplicationService.PaymentModule.Implements;
using BM.Shared.ApplicationService;
using BM.Booking.ApplicationService.BookingModule.Implements.BookingSale;
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
            builder.Services.AddSignalR();

            builder.Services.AddHostedService<BookingCancellationService>();

            builder.Services.AddScoped<IBookingBussinessService, BookingBussinessService>();

            builder.Services.AddScoped<IBookingAppointService, BookingAppointService>();
            builder.Services.AddScoped<IBookingOrderService, BookingOrderService>();
            //builder.Services.AddScoped<IBookingPromoService, BookingPromoService>();    
            builder.Services.AddScoped<IBookingServService, BookingServService>();
            builder.Services.AddScoped<IBookingServiceDetailService, BookingServiceDetailService>();
            builder.Services.AddScoped<IBookingServiceDetailDescriptionService, BookingServiceDetailDescriptionService>();
            builder.Services.AddScoped<IBookingReviewService, BookingReviewService>();
            //builder.Services.AddScoped<IBookingServProService, BookingServProService>();

            builder.Services.AddScoped<IBookingBussinessService, BookingBussinessService>();
            builder.Services.AddScoped<IBookingInvoiceService , BookingInvoiceService>();

            builder.Services.AddScoped<ISendOrderData, BookingBussinessService>();

            builder.Services.AddScoped<IBookingProductService, BookingProductService>();
            //builder.Services.AddScoped<IBookingProductDescriptionService, BookingProductDescriptionService>();
            builder.Services.AddScoped<IBookingCategoryService, BookingCategoryService>();
            builder.Services.AddScoped<IBookingProductDetailService, BookingProductDetailService>();
            builder.Services.AddScoped<IBookingOrderProductService, BookingOrderProductService>();
            builder.Services.AddScoped<IBookingSupplierService, BookingSupplierService>();
            builder.Services.AddScoped<IBookingProductImageService, BookingProductImageService>();
            builder.Services.AddScoped<IBookingSupplierProductDetailService, BookingSupplierProductDetailService>();
           





        }
    }
}































