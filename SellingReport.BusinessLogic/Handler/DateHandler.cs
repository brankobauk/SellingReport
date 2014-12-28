using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SellingReport.Models.Models;

namespace SellingReport.BusinessLogic.Handler
{
    public class DateHandler
    {
        public string GetLastActivityDate(IEnumerable<ProductSellingReport> productSellingReport)
        {
            var sellingReport = productSellingReport.OrderByDescending(p => p.Date).FirstOrDefault();
            if (sellingReport != null) return sellingReport.Date.ToLongDateString();
            return DateTime.Now.ToLongDateString();
        }
    }
}
