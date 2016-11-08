using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Zadanie.Models
{
    public class Earthquake
    {

        public string year { get; set; }
        public string selectedYear { get; set; }

        public static IEnumerable<SelectListItem> GetYearsSelectItems()
        {
            yield return new SelectListItem { Text = "1970", Value = "1970" };
            yield return new SelectListItem { Text = "1980", Value = "1980" };
        }


    }
}