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
        public DateTime GetLastActivityDate()
        {
            return DateTime.Now;
        }
    }
}
