using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SellingReport.Models.Models
{
    public sealed class Country
    {
        public Country()
        {
            Holidays = new List<Holiday>();
            ProductSellingPlans = new List<ProductSellingPlan>();
        }
        [Key]
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        private ICollection<Holiday> Holidays { get; set; }
        private ICollection<ProductSellingPlan> ProductSellingPlans { get; set; }

    }
}