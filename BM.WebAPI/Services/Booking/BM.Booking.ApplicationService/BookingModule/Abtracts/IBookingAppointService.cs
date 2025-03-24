using BM.Booking.Dtos.CRUDdtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BookingModule.Abtracts
{
    public interface IBookingAppointService
    {
        public Task<ResponeDto> BookingCreateAppoint(List<BookingCreateAppointDto> bookingCreateAppointDto);
        public Task<ResponeDto> BookingUpdateAppoint(BookingUpdateAppointDto bookingUpdateAppointDto);
        public Task<ResponeDto> BookingDeleteAppoint(int appointID);
        public Task<ResponeDto> BookingGetAppoint(int appointID);
        public Task<ResponeDto> BookingGetAllAppoint();
        public Task<ResponeDto> BookingGetAppointByOrderID(int orderID);
    }
}
