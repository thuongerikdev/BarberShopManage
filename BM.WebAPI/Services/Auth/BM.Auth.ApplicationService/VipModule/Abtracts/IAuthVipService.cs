using BM.Auth.Dtos.User;
using BM.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Auth.ApplicationService.VipModule.Abtracts
{
    public interface IAuthVipService
    {
        Task<ResponeDto> AuthCreateVip(AuthCreateVip authCreateVip);
        Task<ResponeDto> AuthUpdateVip(AuthUpdateVip authUpdateVip);
        Task<ResponeDto> AuthDeleteVip(int vipID);

        Task<ResponeDto> AuthGetVip(int vipID);
        Task<ResponeDto> AuthGetAllVip();
        public Task<ResponeDto> AuthGetVipByUserID(int userID);
        Task<ResponeDto> AuthGetVipByType(string vipType);
        //Task<ResponeDto> AuthGetVipByType(int vipType);


    }
    public interface IAuthBranchService
    {
        Task<ResponeDto> AuthCreateBranch(AuthCreateBranch authCreateBranch);
        Task<ResponeDto> AuthUpdateBranch(AuthUpdateBranch authUpdateBranch);
        Task<ResponeDto> AuthDeleteBranch(int branchID);
        Task<ResponeDto> AuthGetBranch(int branchID);
        Task<ResponeDto> AuthGetAllBranch();
        public Task<ResponeDto> AuthGetEmpByBranch(int branchID);
    }
    public interface IAuthCusPromoService
    {
        Task<ResponeDto> AuthCreateCusPromo(AuthCreateCusPromo authCreateCusPromo);
        Task<ResponeDto> AuthUpdateCusPromo(AuthUpdateCusPromo authUpdateCusPromo);
        Task<ResponeDto> AuthDeleteCusPromo(int cusPromoID);
        Task<ResponeDto> AuthGetCusPromo(int cusPromoID);
        Task<ResponeDto> AuthGetAllCusPromo();
        //Task<ResponeDto> GetCustomerAndPromotionAsync(int customerID, int promoID);
        Task<ResponeDto> GetCusPromoByCustomerID(int customerID);
        Task<ResponeDto> AuthDecreasePromotion(int cuspromoID);
        Task<ResponeDto> AuthCustomerGetPromo(AuthCreateCusPromo authCreateCusPromo);
        Task<ResponeDto> AuthUpdateCusPromoCount(int cusPromoID);
        Task<ResponeDto> AuthCustomerGetVipPromo(AuthCreateCusPromo authCreateCusPromo);
        Task<ResponeDto> GetVipPromotion(string VipName);
    }
    public interface IAuthPromoService
    {
        Task<ResponeDto> AuthCreatePromo(AuthCreatePromo authCreatePromo);
        Task<ResponeDto> AuthUpdatePromo(AuthUpdatePromo authUpdatePromo);
        Task<ResponeDto> AuthDeletePromo(int promoID);
        Task<ResponeDto> AuthGetPromo(int promoID);
        Task<ResponeDto> AuthGetAllPromo();
        Task<ResponeDto> AuthGetPromoByType(string promoType);
        Task<ResponeDto> AuthGetAllPromoByCustomer(int customerID);
    }
    public interface IAuthCustomerCheckInService
    {
        Task<ResponeDto> AuthCreateCustomerCheckIn(AuthCreateCustomerCheckIn authCreateCustomerCheckIn);
        Task<ResponeDto> AuthUpdateCustomerCheckIn(AuthUpdateCustomerCheckIn authUpdateCustomerCheckIn);
        Task<ResponeDto> AuthDeleteCustomerCheckIn(int checkInID);
        Task<ResponeDto> AuthGetCustomerCheckIn(int checkInID);
        Task<ResponeDto> AuthGetAllCustomerCheckIn();
        Task<ResponeDto> AuthGetCustomerCheckInByCustomerID(int customerID);

    }
}
