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
    }
    public interface IAuthPromoService
    {
        Task<ResponeDto> AuthCreatePromo(AuthCreatePromo authCreatePromo);
        Task<ResponeDto> AuthUpdatePromo(AuthUpdatePromo authUpdatePromo);
        Task<ResponeDto> AuthDeletePromo(int promoID);
        Task<ResponeDto> AuthGetPromo(int promoID);
        Task<ResponeDto> AuthGetAllPromo();
    }
}
