using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingReport.Models.Models
{
    public class SellingReportTable
    {
        public string Name { get; set; }
        public string MonthlyPlan { get; set; }
        public string MonthlyPlanToDate { get; set; }
        public string SoldPieces { get; set; }
        public string SoldPercentage { get; set; }
        public byte[] Image { get; set; }
        public bool OnPlan { get; set; }
    }
}
