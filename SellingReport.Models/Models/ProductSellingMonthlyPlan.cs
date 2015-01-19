using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingReport.Models.Models
{
    public class ProductSellingMonthlyPlan
    {
        [Key]
        public int ProductSellingMonthlyPlanId { get; set; }
        public decimal PlannedValue { get; set; }
        public int CountryId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public virtual Country Country { get; set; }
    }
}
