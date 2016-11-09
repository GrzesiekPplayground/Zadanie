using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Zadanie.Models
{
    public class JsonData
    {

        private int _firstYear;
        private int _lastYear;

        public static int MaxYear
        {
            get
            {
                int y = 0;
                foreach (var year in GetYearsList())
                {
                    int yearInt = Convert.ToInt32(year);
                    if (yearInt > y)
                    {
                        y = yearInt;
                    }
                }
                return y;
            }
        }
        public static int MinYear
        {
            get
            {
                int y = 9999;
                foreach (var year in GetYearsList())
                {
                    int yearInt = Convert.ToInt32(year);
                    if (yearInt < y && yearInt != 0)
                    {
                        y = yearInt;
                    }
                }
                return y;
            }
        }

        public int FirstYear
        {
            get
            {
                return _firstYear;
            }
            set
            {
                _firstYear = Convert.ToInt32(value);
            }
        }
        public int LastYear
        {
            get
            {
                return _lastYear;
            }
            set
            {
                _lastYear = Convert.ToInt32(value);
            }
        }

        public string JsonString
        {
            get
            {
                var yearsCount = GetYearsCount(_firstYear, _lastYear);
                return JsonConvert.SerializeObject(yearsCount, Formatting.Indented);
            }
        }

        private static string GetYearFromToken(JToken token)
        {
            IList<JToken> attributesTokens = token["attributes"].Children().ToList();
            JToken dateToken = attributesTokens[13];
            string dateString = dateToken.First.ToString();
            string yearString = dateString.Substring(0, 4);

            return yearString;
        }

        private static List<string> GetYearsList()
        {
            List<string> yearsList = new List<string>();

            var jsonText = new WebClient().DownloadString("https://sampleserver3.arcgisonline.com/ArcGIS/rest/services/Earthquakes/Since_1970/MapServer/0/query?where=1%3D1&outFields=*&f=pjson");

            JObject myJObject = JObject.Parse(jsonText);
            IList<JToken> results = myJObject["features"].Children().ToList();

            JToken someToken = results[5];
            IList<JToken> atrToken = someToken["attributes"].Children().ToList();

            for (int i = 0; i < results.Count; i++)
            {
                JToken token = results[i];
                string year = GetYearFromToken(token);
                yearsList.Add(year);
            }

            return yearsList;
        }

        private static Dictionary<string, int> GetYearsCount(int firstYear, int lastYear)
        {
            Dictionary<string, int> yearsCount = new Dictionary<string, int>();

            var myList = GetYearsList();

            foreach (string year in myList)
            {
                int yearInt = Convert.ToInt32(year);
                if (yearInt >= firstYear && yearInt <= lastYear)
                {
                    if (yearsCount.ContainsKey(year))
                    {
                        yearsCount[year]++;
                    }
                    else
                    {
                        yearsCount.Add(year, 1);
                    }
                }

            }

            return yearsCount;
        }
    }
}