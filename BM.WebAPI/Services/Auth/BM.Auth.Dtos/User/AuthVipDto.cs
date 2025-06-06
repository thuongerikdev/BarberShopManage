﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BM.Auth.Dtos.User
{
    public class AuthCreateVip
    {
        public string vipType { get; set; }
        public string vipStatus { get; set; }
        public double vipCost { get; set; }
        public double vipDiscount { get; set; }
        public IFormFile Image { get; set; }
    }
    public class AuthUpdateVip
    {
        public int vipID { get; set; }
        public string vipType { get; set; }
        public string vipStatus { get; set; }
        public double vipCost { get; set; }
        public double vipDiscount { get; set; }
        public IFormFile Image { get; set; }
    }

    public class AuthCreateBranch
    {
        public string branchName { get; set; }
        public string branchType { get; set; }
        public string branchStatus { get; set; }
        public double branchArea { get; set; }
        public string branchHotline { get; set; }
        public string startWork { get; set; }
        public string endWork { get; set; }
        public string location { get; set; }
        public IFormFile? branchImage { get; set; }
    }
    public class AuthUpdateBranch
    {
        public int branchID { get; set; }
        public string branchName { get; set; }
        public string branchType { get; set; }
        public string branchStatus { get; set; }
        public double branchArea { get; set; }
        public string branchHotline { get; set; }
        public string startWork { get; set; }
        public string endWork { get; set; }
        public string location { get; set; }
        public IFormFile? branchImage { get; set; }
    }
    public class AuthCreateCusPromo {
        public int customerID { get; set; }
        public int promoID { get; set; }
        public int promoCount { get; set; }

        public string cusPromoStatus { get; set; }
    }
    public class AuthUpdateCusPromo
    {
        public int cusPromoID { get; set; }
        public int customerID { get; set; }
        public int promoID { get; set; }
        public int promoCount { get; set; }
        public string cusPromoStatus { get; set; }
    }
    public class AuthCreateCustomerCheckIn
    {
      
        public int customerID { get; set; }
        //public DateTime checkInDate { get; set; }
        public string checkInStatus { get; set; }
        public string checkInType { get; set; }
    }
    public class AuthUpdateCustomerCheckIn
    {
        public int customerCheckInID { get; set; }
        public int customerID { get; set; }
        public DateTime checkInDate { get; set; }
        public string checkInStatus { get; set; }
        public string checkInType { get; set; }
    }

    





}
