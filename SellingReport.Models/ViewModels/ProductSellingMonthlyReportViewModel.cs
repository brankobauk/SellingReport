using System.Collections.Generic;
using System.Web.Mvc;
using SellingReport.Models.Models;

namespace SellingReport.Models.ViewModels
{
    public class ProductSellingMonthlyReportViewModel
    {
        public ProductSellingMonthlyReport ProductSellingMonthlyReport { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<HolidayDates> HolidayDates { get; set; }
    }
}
