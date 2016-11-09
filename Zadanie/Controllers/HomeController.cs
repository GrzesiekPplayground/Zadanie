using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Net.Mime;
using System.Web.Mvc;
using Zadanie.Models;

namespace Zadanie.Controllers
{
    public class HomeController : Controller
    {
        // scieżka do lokalizacji root
        private string _rootPath
        {
            get
            {
                return Server.MapPath("~");
            }
        }

        // scieżka do pliku dbf
        private string _tableAdress
        {
            get
            {
                return "[" + _rootPath + "Data\\Earthquakes1970" + "]";
            }
        }

        // najmniejsza wartość roku z tabeli
        private int _minYear
        {
            get
            {
                var minYearQuery = string.Format("SELECT MIN(LEFT(YYYYMMDD, 4)) FROM {0}", _tableAdress);

                var minYearRow = myData(minYearQuery);
                return Convert.ToInt32((minYearRow[0])[0]);
            }
        }

        // najwieksza wartość roku z tabeli
        private int _maxYear
        {
            get
            {
                var maxYearQuery = string.Format("SELECT MAX(LEFT(YYYYMMDD, 4)) FROM {0}", _tableAdress);

                var maxYearRow = myData(maxYearQuery);
                return Convert.ToInt32((maxYearRow[0])[0]);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backend3()
        {
            // Utwórz pustą listę dal widoku.
            ViewBag.rows = new List<DataRow>();

            ViewBag.minYear = _minYear;
            ViewBag.maxYear = _maxYear;

            return View();
        }

        [HttpPost]
        public ActionResult Backend3(Earthquake eq)
        {
            // Pobierz wybrany z listy rok z modelu i wybierze z dbf kolumy z rokiem i liczą trzęsień ziemi
            var year = eq.selectedYear;
            var query = string.Format("SELECT LEFT(YYYYMMDD, 4) AS Year, Count(*) FROM {1} WHERE (YYYYMMDD LIKE '{0}%') GROUP BY Year", year, _tableAdress);

            // Prześlij wybrane dane do widoku
            ViewBag.rows = myData(query);

            ViewBag.minYear = _minYear;
            ViewBag.maxYear = _maxYear;

            return View();
        }

        public ActionResult Backend4()
        {
            // wybierz z dbf wiersze w ktorych Num_Death wynosi więcej niż 1000.
            var query = string.Format("SELECT * FROM {0} WHERE (Num_Deaths > 1000) ORDER BY Num_Deaths", _tableAdress);
            var rows = myData(query);

            ViewBag.rows = rows;

            return View();
        }

        public ActionResult Json(JsonData j)
        {

            ViewBag.minYear = _minYear;
            ViewBag.maxYear = _maxYear;

            return View();
        }

        public ActionResult DownloadJson(JsonData j)
        {
            var filePath = _rootPath + "Data\\JData.json";
            System.IO.File.WriteAllText(filePath, j.JsonString);

            ViewBag.jsonString = j.JsonString;
            ViewBag.filePath = filePath;

            return File(filePath, MediaTypeNames.Text.Plain, "plik.json");         
        }

        // Pobierz z dbf dane postaci listy obiektów DataRow na podstawie zapytania SQL
        private List<DataRow> myData(string query)
        {
            // Otworz polaczenie z dbf. Jesli występuje błąd, prawdopodobnie nie zainstalowano sterownika VFPOLEDB.
            var connectionString = string.Format("Provider=VFPOLEDB.1;Data Source={0}\\Data\\;Extended Properties=dBASE IV;User ID=;Password=;", _rootPath);
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();

            // Wykonaj zapytanie SQL
            OleDbCommand cmd = new OleDbCommand(query, conn);

            // Załaduj dane do DataTable
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            conn.Close();

            // Przerzuć dane z DataTable do listy DataRow
            var rows = new List<DataRow>();
            foreach (DataRow row in dt.Rows)
            {
                rows.Add(row);
            }

            // Zwróć listę DataRow
            return rows;
        }

    }
}