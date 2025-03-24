using BM.Booking.Dtos.CRUDdtos;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BookingModule.Abtracts
{
    public interface IBookingReviewService
    {
        public Task<ResponeDto> BookingCreateReview(BookingCreateReviewDto bookingCreateReviewDto);
        public Task<ResponeDto> BookingUpdateReview(BookingUpdateReviewDto bookingUpdateReviewDto);
        public Task<ResponeDto> BookingDeleteReview(int reviewID);
        public Task<ResponeDto> BookingGetReview(int reviewID);
        public Task<ResponeDto> BookingGetAllReview();
    }
}
