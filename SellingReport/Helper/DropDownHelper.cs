using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SellingReport.Models.Models;

namespace SellingReport.Helper
{
    public class DropDownHelper
    {
        public IEnumerable<SelectListItem> GetMonthsListForDropDown()
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
                    
            if (DateTimeFormatInfo
                .CurrentInfo != null)
            { 
                return DateTimeFormatInfo
                    .CurrentInfo
                    .MonthNames
                    .Select((monthName, index) =>
                        new SelectListItem
                           {
                               Value = (index + 1).ToString(CultureInfo.InvariantCulture),
                               Text = textInfo.ToTitleCase(monthName)
                           }).Where(p=>p.Text != "");

            }
            return DateTimeFormatInfo
                .InvariantInfo
                .MonthNames
                .Select((monthName, index) =>
                    new SelectListItem
                    {
                        Value = (index + 1).ToString(CultureInfo.InvariantCulture),
                        Text = monthName
                    }).Where(p => p.Text != "");
        }

        public IEnumerable<SelectListItem> GetYearsListForDropDown()
        {
            var yearDropdown = new List<SelectListItem>();

            for (int i = 2010; i <= DateTime.Now.Year+1; i++)
            {
                var selListItem = new SelectListItem { Value = i.ToString(CultureInfo.InvariantCulture), Text = i.ToString(CultureInfo.InvariantCulture)};
                yearDropdown.Add(selListItem);
            }
            return yearDropdown;
        }

        public IEnumerable<SelectListItem> GetCountryListForDropDown(IEnumerable<Country> countries)
        {
            return countries.Select(country => new SelectListItem {Value = country.CountryId.ToString(CultureInfo.InvariantCulture), Text = country.Name.ToString(CultureInfo.InvariantCulture)}).ToList();
        }

        public IEnumerable<SelectListItem> GetProductsListForDropDown(IEnumerable<Product> products)
        {
            return products.Select(country => new SelectListItem { Value = country.ProductId.ToString(CultureInfo.InvariantCulture), Text = country.Name.ToString(CultureInfo.InvariantCulture)}).ToList();
            
        }
    }
}
