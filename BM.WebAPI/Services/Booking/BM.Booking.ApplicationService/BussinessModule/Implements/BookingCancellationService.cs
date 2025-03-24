using BM.Booking.ApplicationService.BussinessModule.Implements;
using BM.Booking.Dtos.BussinessDtos;
using BM.Booking.Infrastructure;
using BM.Shared.ApplicationService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class BookingCancellationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<BookingHub> _hubContext;

    public BookingCancellationService(
        IServiceProvider serviceProvider,
        IHubContext<BookingHub> hubContext)
    {
        _serviceProvider = serviceProvider;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
                var authDataSend = scope.ServiceProvider.GetRequiredService<AuthDataSend>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailCancelBooking>();

                // Tính thời gian ngưỡng (30 phút trước từ hiện tại)
                var threshold = DateTime.UtcNow.AddMinutes(-30);

                // Sử dụng biểu thức có thể dịch sang SQL
                var expiredOrders = await dbContext.BookingOrders
                    .Where(o => o.orderStatus != "Completed" && o.orderStatus != "Cancelled"
                           && o.createAt < threshold)
                    .ToListAsync(stoppingToken);

                foreach (var order in expiredOrders)
                {
                    order.orderStatus = "Cancelled";
                    var appointments = await dbContext.BookingAppointments
                        .Where(a => a.orderID == order.orderID)
                        .ToListAsync(stoppingToken);
                    foreach (var appointment in appointments)
                    {
                        appointment.appStatus = "Cancelled";
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);

                    var customerResponse = await authDataSend.GetUserToOtherDomain(order.custID);
                    var customerData = customerResponse?.Data as AuthUser;
                    string customerEmail = customerData?.email ?? "default@example.com";

                    await emailService.SendCancelEmail(customerEmail, order.orderID, order.createAt);

                    await _hubContext.Clients.All.SendAsync(
                        "ReceiveCancellation",
                        order.orderID,
                        $"Đơn hàng {order.orderID} đã bị hủy do quá 30 phút kể từ khi tạo.",
                        cancellationToken: stoppingToken
                    );
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

}