using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BussinessModule.Implements
{
    public class BookingHub : Hub
    {
        public async Task SendCancellationNotification(int orderId, string message)
        {
            await Clients.All.SendAsync("ReceiveCancellation", orderId, message);
        }
    }
}
