using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace AngularTest1.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetSehins(string param)
        {
            var test1 = param;


            DataTable dtResult = new DataTable();

            string ConString = "Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOKCD + ' - ' + NAME FROM M0200 WHERE TOKCD LIKE '" + param+"%'", con);
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

            List<string> listSehin = new List<string>();
            foreach (DataRow row in dtResult.Rows)
            {
                /*
                Models.SeihinModels sehin = new Models.SeihinModels
                {
                    Code = (string)row[0],
                    Zuban = (string)row[1],
                    Name = (string)row[2],
                    Kisyu = (string)row[3]
                };*/

                listSehin.Add((string)row[0]);
            }

           string[] result = listSehin.ToArray<string>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}