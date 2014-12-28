using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SellingReport.Models.Models
{
    public class ProductSellingReport
    {
        public int ProductSellingReportId { get; set; }
        public int ProductId { get; set; }
        public int CountryId { get; set; }
        public int SoldPieces { get; set; }
        public decimal SoldValue { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }
        public virtual Country Country { get; set; }
        public virtual Product Product { get; set; }

    }
}
