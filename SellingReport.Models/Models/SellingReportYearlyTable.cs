﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingReport.Models.Models
{
    public class SellingReportYearlyTable
    {
        public string Name { get; set; }
        public List<ProductSellingYearlyReport> ProductSellingYearlyReport { get; set; }
    }
}
