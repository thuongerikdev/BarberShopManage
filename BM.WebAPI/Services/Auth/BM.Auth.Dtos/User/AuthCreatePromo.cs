using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BM.Auth.Dtos.User
{
    public class AuthCreatePromo
    {
      
        public string promoName { get; set; }
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public int pointToGet { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
        public string promoStatus { get; set; }
        public string promoType { get; set;}
        public IFormFile promoImage { get; set; }
    }
    public class AuthUpdatePromo { 
        public int promoID { get; set; }
        public string promoName { get; set; }
        public string promoDescription { get; set; }
        public double promoDiscount { get; set; }
        public int pointToGet { get; set; }
        public DateTime promoStart { get; set; }
        public DateTime promoEnd { get; set; }
        public string promoStatus { get; set; }
        public string promoType { get; set; }
        public IFormFile promoImage { get; set; }
    }
}
