using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SellingReport.Models.Models;

namespace SellingReport.Models.ViewModels
{
    public class VariableHolidayViewModel
    {
        public VariableHoliday VariableHoliday { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
