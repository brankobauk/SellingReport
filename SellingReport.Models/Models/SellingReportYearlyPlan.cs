using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingReport.Models.Models
{
    public class SellingReportYearlyPlan
    {
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal PlannedValue { get; set; }
    }
}
