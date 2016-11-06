using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.Mvc;

namespace Zadanie.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backend3()
        {
            var querry = "SELECT LEFT(YYYYMMDD, 4) AS Year, Count(*) FROM d:\\data\\Earthquakes1970 GROUP BY Year";
            var rows = myData(querry);
            ViewBag.rows = myData(querry);

            return View();
        }

        public ActionResult Backend4()
        {
            var querry = "SELECT * FROM d:\\data\\Earthquakes1970 WHERE (Num_Deaths > 1000) ORDER BY Num_Deaths";
            var rows = myData(querry);

            ViewBag.rows = rows;

            return View();
        }

        private List<DataRow> myData(string querry)
        {
            OleDbConnection conn = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=c:\\Data\\;Extended Properties=dBASE IV;User ID=;Password=;");
            conn.Open();

            OleDbCommand cmd = new OleDbCommand(querry, conn);

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