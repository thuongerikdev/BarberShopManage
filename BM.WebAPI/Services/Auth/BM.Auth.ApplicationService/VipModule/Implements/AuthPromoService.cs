using BM.Auth.ApplicationService.Common;
using BM.Auth.ApplicationService.VipModule.Abtracts;
using BM.Auth.Domain;
using BM.Auth.Dtos;
using BM.Auth.Dtos.User;
using BM.Auth.Infrastructure;
using BM.Constant;
using BM.Shared.ApplicationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.VipModule.Implements
{
    public class AuthPromoService : AuthServiceBase, IAuthPromoService
    {
        protected readonly ICloudinaryService _cloudinaryService;
        public AuthPromoService(ILogger<AuthPromoService> logger, AuthDbContext dbContext, ICloudinaryService cloudinaryService) : base(logger, dbContext)
        {
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ResponeDto> AuthCreatePromo(AuthCreatePromo authCreatePromo)
        {
            _logger.LogInformation("AuthCreatePromo");
            try
            {
                var img = await _cloudinaryService.UploadImageAsync(authCreatePromo.promoImage);
                if (img == null)
                {
                    return ErrorConst.Error(500, "Tải ảnh lên thất bại");
                }
                var promo = new AuthPromotion
                {
                    promoName = authCreatePromo.promoName,
                    promoDescription = authCreatePromo.promoDescription,
                    promoDiscount = authCreatePromo.promoDiscount,
                    promoStatus = authCreatePromo.promoStatus,
                    promoStart = authCreatePromo.promoStart,
                    promoEnd = authCreatePromo.promoEnd,
                    promoType = authCreatePromo.promoType,
                    promoImage = img,
                    pointToGet = authCreatePromo.pointToGet,
                };
                _dbContext.Promos.Add(promo);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Tạo khuyến mãi thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthUpdatePromo(AuthUpdatePromo authUpdatePromo)
        {
            _logger.LogInformation("AuthUpdatePromo");
            try
            {
                var img = await _cloudinaryService.UploadImageAsync(authUpdatePromo.promoImage);
                if (img == null)
                {
                    return ErrorConst.Error(500, "Tải ảnh lên thất bại");
                }

                var promo = await _dbContext.Promos.FindAsync(authUpdatePromo.promoID);
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi");
                }
                promo.promoName = authUpdatePromo.promoName;
                promo.promoDescription = authUpdatePromo.promoDescription;
                promo.promoDiscount = authUpdatePromo.promoDiscount;
                promo.promoStatus = authUpdatePromo.promoStatus;
                promo.promoStart = authUpdatePromo.promoStart;
                promo.promoEnd = authUpdatePromo.promoEnd;
                promo.promoType = authUpdatePromo.promoType;
                promo.pointToGet = authUpdatePromo.pointToGet;
                promo.promoImage = img;
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Cập nhật khuyến mãi thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthDeletePromo(int promoID)
        {
            _logger.LogInformation("AuthDeletePromo");
            try
            {
                var promo = await _dbContext.Promos.FindAsync(promoID);
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi");
                }
                _dbContext.Promos.Remove(promo);
                await _dbContext.SaveChangesAsync();
                return ErrorConst.Success("Xóa khuyến mãi thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetPromo(int promoID)
        {
            _logger.LogInformation("AuthGetPromo");
            try
            {
                var promo = await _dbContext.Promos.FindAsync(promoID);
                if (promo == null)
                {
                    return ErrorConst.Error(500, "Không tìm thấy khuyến mãi");
                }
                return ErrorConst.Success("Lấy khuyến mãi thành công", promo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }

        }
        public async Task<ResponeDto> AuthGetAllPromo()
        {
            _logger.LogInformation("AuthGetAllPromo");
            try
            {
                var promos = await _dbContext.Promos.ToListAsync();
                return ErrorConst.Success("Lấy danh sách khuyến mãi thành công", promos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetPromoByType(string promoType)
        {
            _logger.LogInformation("AuthGetPromoByType");
            try
            {
                var promos = await _dbContext.Promos.Where(x => x.promoType == promoType).ToListAsync();
                return ErrorConst.Success("Lấy danh sách khuyến mãi theo loại thành công", promos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ErrorConst.Error(500, ex.Message);
            }
        }
        public async Task<ResponeDto> AuthGetAllPromoByCustomer(int customerID)
        {
            _logger.LogInformation("AuthGetAllPromoByCustomer for customer {CustomerID}", customerID);
            try
            {
                // Eagerly load promotions with their associated AuthCusPromos
                var promos = await _dbContext.Promos
                    .Include(p => p.AuthCusPromos)
                    .Select(p => new PromoWithOwnershipDto
                    {
                        PromoID = p.promoID,
                        PromoName = p.promoName,
                        PromoDescription = p.promoDescription,
                        PromoDiscount = p.promoDiscount,
                        PointToGet = p.pointToGet,
                        PromoStart = p.promoStart,
                        PromoEnd = p.promoEnd,
                        PromoStatus = p.promoStatus,
                        PromoType = p.promoType,
                        PromoImage = p.promoImage,
                        IsAssociatedWithCustomer = p.AuthCusPromos.Any(cp => cp.customerID == customerID),
                        CustomerPromos = p.AuthCusPromos
                            .Where(cp => cp.customerID == customerID)
                            .Select(cp => new AuthCusPromoDto
                            {
                                CusPromoID = cp.cusPromoID,
                                CustomerID = cp.customerID,
                                PromoID = cp.promoID,
                                CusPromoStatus = cp.cusPromoStatus,
                                PromoCount = cp.promoCount,
                                CreateAt = cp.createAt,
                                UpdateAt = cp.updateAt
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return ErrorConst.Success("Lấy danh sách khuyến mãi thành công", promos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving promotions for customer {CustomerID}", customerID);
                return ErrorConst.Error(500, ex.Message);
            }
        }

    }
}
