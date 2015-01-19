﻿using System.Collections.Generic;
using System.Web.Mvc;
using SellingReport.Models.Models;

namespace SellingReport.Models.ViewModels
{
    public class ProductSellingYearlyReportViewModel
    {
        public ProductSellingYearlyReport ProductSellingYearlyReport { get; set; }
        public IEnumerable<SelectListItem> Month { get; set; }
        public IEnumerable<SelectListItem> Year { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
    }
}
