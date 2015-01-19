using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingReport.Models.Models
{
    public class ProductSellingYearlyReport
    {
        [Key]
        public int ProductSellingYearlyReportId { get; set; }
        public int ProductId { get; set; }
        public int CountryId { get; set; }
        public decimal PlannedValue { get; set; }
        public int PlannedPieces { get; set; }
        public decimal SoldValue { get; set; }
        public int SoldPieces { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public virtual Country Country { get; set; }
        public virtual Product Product { get; set; }
    }
}
