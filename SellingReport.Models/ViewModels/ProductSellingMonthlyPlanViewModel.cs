using System.Collections.Generic;
using System.Web.Mvc;
using SellingReport.Models.Models;

namespace SellingReport.Models.ViewModels
{
    public class ProductSellingMonthlyPlanViewModel
    {
        public ProductSellingMonthlyPlan ProductSellingMonthlyPlan { get; set; }
        public IEnumerable<SelectListItem> Month { get; set; }
        public IEnumerable<SelectListItem> Year { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
