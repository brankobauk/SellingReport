using System;
using System.ComponentModel.DataAnnotations;

namespace SellingReport.Models.Models
{
    public class Holiday
    {
        [Key]
        public int HolidayId { get; set; }
        public int CountryId { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public virtual Country Country { get; set; }
        
    }
}