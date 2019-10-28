using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Runtime.Remoting.Contexts;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AngularTest1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetSehins()
        {
            DataTable dtResult = new DataTable();

            string ConString = "Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP(10) ZAICD, ZUBAN, NAME, KISYU FROM M0100", con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                //dtResult = new DataTable();
                //DataColumn dc;
                //dc = new DataColumn("選択", typeof(bool));
                //dtResult.Columns.Add(dc);
                ada.Fill(dtResult);

                //DataColumn dc;
                //dc = new DataColumn("Column1", typeof(bool));
                //DataTableへ追加
                //dtResult.Columns.Add(dc);
                con.Close();
            }

            List<Models.SeihinModels> listSehin = new List<Models.SeihinModels>();
            foreach (DataRow row in dtResult.Rows)
            {
                Models.SeihinModels sehin = new Models.SeihinModels
                {
                    Code = (string)row[0],
                    Zuban = (string)row[1],
                    Name = (string)row[2],
                    Kisyu = (string)row[3]
                };

                listSehin.Add(sehin);
            }

            Models.SeihinModels[] result = listSehin.ToArray<Models.SeihinModels>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpPost]
        public void PostData(Models.SeihinModels[] listSehins)
        {
            //var array = (Models.SeihinModels[])listSehins;

            if (listSehins != null)
            {
                var test1 = listSehins.Count();
                var test2 = "";
                //return Json(new { success = false });
            }
        }
    }
}