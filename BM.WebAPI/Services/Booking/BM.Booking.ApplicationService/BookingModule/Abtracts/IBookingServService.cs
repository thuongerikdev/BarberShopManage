using BM.Booking.Dtos.CRUDdtos;
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
    public interface IBookingServiceDetailService
    {
        public Task<ResponeDto> BookingCreateServiceDetail(BookingCreateServiceDetailDto bookingCreateServiceDetailDto);
        public Task<ResponeDto> BookingUpdateServiceDetail(BookingUpdateServiceDetailDto bookingUpdateServiceDetailDto);
        public Task<ResponeDto> BookingDeleteServiceDetail(int serviceDetailID);
        public Task<ResponeDto> BookingGetServiceDetailByID(int serviceDetailID);
        public Task<ResponeDto> BookingGetAllServiceDetail();
        public Task<ResponeDto> BookingGetServiceDetailByServiceID(int serviceID);
        Task<ResponeDto> BookingGetServiceDetailByServiceName(string serviceName);
        Task<ResponeDto> BookingUpdatePriceOfServiceDetail(int serviceDetailID, double price);
    }
    public interface IBookingServiceDetailDescriptionService
    {
        public Task<ResponeDto> BookingCreateServiceDetailDescription(BookingCreateServiceDetailDescriptionDto createBookingServiceDetailDescriptionDto);
        public Task<ResponeDto> BookingUpdateServiceDetailDescription(BookingUpdateServiceDetailDescriptionDto bookingUpdateServiceDetailDescriptionDto);
        public Task<ResponeDto> BookingDeleteServiceDetailDescription(int serviceDetailDescriptionID);
        public Task<ResponeDto> BookingGetServiceDetailDescriptionByID(int serviceDetailDescriptionID);
        public Task<ResponeDto> BookingGetAllServiceDetailDescription();
        Task<ResponeDto> BookingGetServiceDetailDescriptionByServiceDetailID(int serviceDetailID);
    }
}
