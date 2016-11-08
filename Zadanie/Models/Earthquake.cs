using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Zadanie.Models
{
    
    public class Earthquake
    {

        public string year { get; set; }
        public string selectedYear { get; set; }

        //public static IEnumerable<SelectListItem> GetYearsSelectItems()
        //{
        //    yield return new SelectListItem { Text = "1970", Value = "1970" };
        //    yield return new SelectListItem { Text = "1980", Value = "1980" };
        //}

        //public static List<SelectListItem> xGetYearsSelectItems;
        
        public static List<SelectListItem> yList()
        {
            var someList = new List<SelectListItem>();
            for (int y = 1970; y < 2003; y++)
            {
                var year = Convert.ToString(y);
                someList.Add(new SelectListItem { Text = year, Value = year });
            }
            return someList;
        }


    }
}