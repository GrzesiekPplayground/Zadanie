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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backend3()
        {
            var year = "1970;";
            var query = string.Format("SELECT LEFT(YYYYMMDD, 4) AS Year, Count(*) FROM {1} WHERE (YYYYMMDD LIKE '{0}%') GROUP BY Year", year, _tableAdress);
            var rows = myData(query);
            ViewBag.rows = myData(query);

            return View();
        }

        [HttpPost]
        public ActionResult Backend3(Earthquake eq)
        {
            var year = eq.selectedYear;
            var query = string.Format("SELECT LEFT(YYYYMMDD, 4) AS Year, Count(*) FROM {1} WHERE (YYYYMMDD LIKE '{0}%') GROUP BY Year", year, _tableAdress);
            var rows = myData(query);
            ViewBag.rows = myData(query);

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