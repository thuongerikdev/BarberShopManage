using BM.Booking.ApplicationService.BookingModule.Abtracts;
using BM.Booking.ApplicationService.Common;
using BM.Booking.Domain;
using BM.Booking.Dtos.CRUDdtos;
using BM.Booking.Infrastructure;
using BM.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.ApplicationService.BookingModule.Implements
{
    public class BookingReviewService : BookingServiceBase, IBookingReviewService
    {
        public BookingReviewService(ILogger<BookingReviewService> logger, BookingDbContext bookingDbContext) : base(logger, bookingDbContext)
        {
        }
        public async Task<ResponeDto> BookingCreateReview(BookingCreateReviewDto bookingCreateReviewDto)
        {
            _logger.LogInformation("BookingCreateReview called");
            try
            {
                var review = new BookingReview
                {
                    reviewContent = bookingCreateReviewDto.reviewContent,
                    reviewDate = bookingCreateReviewDto.reviewDate,
                    reviewStatus = bookingCreateReviewDto.reviewStatus,
                    appID = bookingCreateReviewDto.appID,
                    rating = bookingCreateReviewDto.rating
                };
                _dbContext.BookingReviews.Add(review);

                //var appointment = await _dbContext.BookingAppointments.FindAsync(bookingCreateReviewDto.appID);
                //if (appointment == null)
                //{
                //    return ErrorConst.Error(500, "Không tìm thấy cuộc hẹn");
                //}
               

                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo review thành công", review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingUpdateReview(BookingUpdateReviewDto bookingUpdateReviewDto)
        {
            _logger.LogInformation("BookingUpdateReview called");
            try
            {
                var review = await _dbContext.BookingReviews.FindAsync(bookingUpdateReviewDto.reviewID);
                if (review == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy review");
                }
                review.reviewContent = bookingUpdateReviewDto.reviewContent;
                review.reviewDate = bookingUpdateReviewDto.reviewDate;
                review.reviewStatus = bookingUpdateReviewDto.reviewStatus;
                review.appID = bookingUpdateReviewDto.appID;
                review.rating = bookingUpdateReviewDto.rating;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật review thành công", review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingDeleteReview(int reviewID)
        {
            _logger.LogInformation("BookingDeleteReview called");
            try
            {
                var review = await _dbContext.BookingReviews.FindAsync(reviewID);
                if (review == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy review");
                }
                _dbContext.BookingReviews.Remove(review);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa review thành công", review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> BookingGetReview(int reviewID)
        {
            _logger.LogInformation("BookingGetReview called");
            try
            {
                var review = await _dbContext.BookingReviews.FindAsync(reviewID);
                if (review == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy review");
                }
                return ErrorConst.Success("Lấy review thành công", review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        } 

            public async Task<ResponeDto> BookingGetAllReview()
            {
                _logger.LogInformation("BookingGetAllReview called");
                try
                {
                    var reviews = await _dbContext.BookingReviews.ToListAsync();
                    return ErrorConst.Success("Lấy danh sách review thành công", reviews);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return ErrorConst.Error(500, ex.Message);
                }
            }
        }
    }
