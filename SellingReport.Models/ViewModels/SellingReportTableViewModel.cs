using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SellingReport.Models.Models;

namespace SellingReport.Models.ViewModels
{
    public class SellingReportTableViewModel
    {
        public string Date { get; set; }
        public List<SellingReportTable> SellingReportTable { get; set; }
        public SellingReportMonthlyTable SellingReportMonthlyTable { get; set; }
        public List<SellingReportYearlyTable> SellingReportYearlyTable { get; set; }
    }
}
