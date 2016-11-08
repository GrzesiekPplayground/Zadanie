using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Zadanie.Models
{

    public class Earthquake
    {
        // Rok wybrany z listy
        public string selectedYear { get; set; }
        
        // Zwroc liste wszystkich lat z podanego przedziali jako listę stringów
        public static List<SelectListItem> GetAllYearsList(int minYear, int maxYear)
        {
            var resultList = new List<SelectListItem>();
            for (int y = minYear; y <= maxYear; y++)
            {
                var year = Convert.ToString(y);
                resultList.Add(new SelectListItem { Text = year, Value = year });
            }
            return resultList;
        }

    }
}