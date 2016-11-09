using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Zadanie.Models
{
    public class JsonData
    {

        private int _firstYear;
        private int _lastYear;

        // Minimalny i maksymalny rok na podstawie listy z GetYearsList
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

        // Pierwszy i ostatni rok wybrane z listy w widoku
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

        // String w foracie Json
        public string JsonString
        {
            get
            {
                var yearsCount = GetYearsCount(_firstYear, _lastYear);
                return JsonConvert.SerializeObject(yearsCount, Formatting.Indented);
            }
        }


        // Z obiektu JToken wybierz rok.
        private static string GetYearFromToken(JToken token)
        {
            // Z Token wybierz "attributes"
            IList<JToken> attributesTokens = token["attributes"].Children().ToList();

            // pobierz token daty - 13 pozycja listy
            JToken dateToken = attributesTokens[13]; 

            // z tokena daty pobierz wartość jako string i wybierz z niego rok
            string dateString = dateToken.First.ToString();
            string yearString = dateString.Substring(0, 4);

            return yearString;
        }

        // Pobierz listę lat
        private static List<string> GetYearsList()
        {
            // Lista, w której będą przechowywane lata
            List<string> yearsList = new List<string>();

            // Pobierz dane z URL do stringa
            var jsonText = new WebClient().DownloadString("https://sampleserver3.arcgisonline.com/ArcGIS/rest/services/Earthquakes/Since_1970/MapServer/0/query?where=1%3D1&outFields=*&f=pjson");

            // Konwertuj dane ze stringa do obiektu. Z całego pliku pobierz "features" jako listę obiektów JToken
            JObject myJObject = JObject.Parse(jsonText);
            IList<JToken> results = myJObject["features"].Children().ToList();

            // Z każdego obiektu listy results pobierz rok i zapis go do listy.
            for (int i = 0; i < results.Count; i++)
            {
                JToken token = results[i];
                string year = GetYearFromToken(token);
                yearsList.Add(year);
            }

            return yearsList;
        }

        // Zlicz trzęsienia zemi w podanych latach
        private static Dictionary<string, int> GetYearsCount(int firstYear, int lastYear)
        {
            Dictionary<string, int> yearsCount = new Dictionary<string, int>();

            foreach (string year in GetYearsList())
            {
                int yearInt = Convert.ToInt32(year);
                if (yearInt >= firstYear && yearInt <= lastYear) // Jeśli rok zawiera sie w podanym przedziale
                {
                    if (yearsCount.ContainsKey(year)) // Jeśli rok jest już na liście, zwiększ liczbę trzęsień dla danego roku o 1.
                    {
                        yearsCount[year]++;
                    }
                    else // Jeśli nie ma, dodaj z odpowiadającą liczbą trzśień ziemi = 1;
                    {
                        yearsCount.Add(year, 1);
                    }
                }

            }

            return yearsCount;
        }
    }
}