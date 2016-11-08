using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.Mvc;
using Zadanie.Models;

namespace Zadanie.Controllers
{
    public class HomeController : Controller
    {
        private string _rootPath
        {
            get
            {
                return Server.MapPath("~");
            }
        }

        private string _tableAdress
        {
            get
            {
                return "[" + _rootPath + "Data\\Earthquakes1970" + "]";
            }
        }

        private int _minYear
        {
            get
            {
                var minYearQuery = string.Format("SELECT MIN(LEFT(YYYYMMDD, 4)) FROM {0}", _tableAdress);

                var minYearRow = myData(minYearQuery);
                return Convert.ToInt32((minYearRow[0])[0]);
            }
        }

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
            var year = "1970;";
            var query = string.Format("SELECT LEFT(YYYYMMDD, 4) AS Year, Count(*) FROM {1} WHERE (YYYYMMDD LIKE '{0}%') GROUP BY Year", year, _tableAdress);
            //var minYearQuery = string.Format("SELECT MIN(LEFT(YYYYMMDD, 4)) FROM {0}", _tableAdress);
            //var maxYearQuery = string.Format("SELECT MAX(LEFT(YYYYMMDD, 4)) FROM {0}", _tableAdress);

            ViewBag.rows = myData(query);

            ViewBag.minYear = _minYear;
            ViewBag.maxYear = _maxYear;

            return View();
        }

        [HttpPost]
        public ActionResult Backend3(Earthquake eq)
        {
            var year = eq.selectedYear;
            var query = string.Format("SELECT LEFT(YYYYMMDD, 4) AS Year, Count(*) FROM {1} WHERE (YYYYMMDD LIKE '{0}%') GROUP BY Year", year, _tableAdress);

            ViewBag.rows = myData(query);

            ViewBag.minYear = _minYear;
            ViewBag.maxYear = _maxYear;

            return View();
        }

        public ActionResult Backend4()
        {
            var query = string.Format("SELECT * FROM {0} WHERE (Num_Deaths > 1000) ORDER BY Num_Deaths", _tableAdress);
            var rows = myData(query);

            ViewBag.rows = rows;

            return View();
        }

        private List<DataRow> myData(string query)
        {
            var connectionString = string.Format("Provider=VFPOLEDB.1;Data Source={0}\\Data\\;Extended Properties=dBASE IV;User ID=;Password=;", _rootPath);
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();

            OleDbCommand cmd = new OleDbCommand(query, conn);

            DataTable dt = new DataTable();

            dt.Load(cmd.ExecuteReader());

            conn.Close();

            var rows = new List<DataRow>();

            foreach (DataRow row in dt.Rows)
            {
                rows.Add(row);
            }

            return rows;
        }

    }
}