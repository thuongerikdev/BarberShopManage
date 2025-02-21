using BM.Booking.Dtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BookingModule.Abtracts
{
    public interface IBookingServService
    {
        public Task<ResponeDto> BookingCreateService (BookingCreateServiceDto bookingCreateServiceDto);
        public Task<ResponeDto> BookingUpdateService(BookingUpdateServiceDto bookingUpdateServiceDto);
        public Task<ResponeDto> BookingDeleteService(int serviceID);
        public Task<ResponeDto> BookingGetService(int serviceID);
        public Task<ResponeDto> BookingGetAllService();
    }
}
