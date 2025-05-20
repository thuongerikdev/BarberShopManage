using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Booking.Dtos.CRUDdtos
{
    public class BookingCreateServiceDto
    {
        public string servName { get; set; }
        public string servDescription { get; set; }
        public double servPrice { get; set; }
        public string servStatus { get; set; }
        public IFormFile servImage { get; set; }
    }
    public class BookingUpdateServiceDto
    {
        public int servID { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public double servPrice { get; set; }
        public string servStatus { get; set; }
        public string servImage { get; set; }
    }
    public class BookingReadServiceDto
    {
        public int servID { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public double servPrice { get; set; }
        public string servStatus { get; set; }
        public string servImage { get; set; }
    }


    public class BookingCreateServiceDetailDto
    {
      
        public int servID { get; set; }
        public double servPrice { get; set; }
        public IFormFile servImage { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public string servStatus { get; set; }
    }
    public class BookingUpdateServiceDetailDto : BookingCreateServiceDetailDto
    {
        public int serviceDetailID { get; set; }
   
    }
    public class BookingCreateServiceDetailDescriptionDto
    {
       
        public int serviceDetailID { get; set; }
        public IFormFile servImage { get; set; }
        public string servName { get; set; }
        public string servDescription { get; set; }
        public string servStatus { get; set; }
        public string servType { get; set; }
    }
    public class BookingUpdateServiceDetailDescriptionDto : BookingCreateServiceDetailDescriptionDto
    {
        public int serviceDetailDescriptionID { get; set; }
    }
}
