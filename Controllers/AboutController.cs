using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json.Linq;

using System.IO;
using System.Data.OleDb;

namespace AngularTest1.Controllers
{
    public class AboutController : Controller
    {
        string ConString = Models.MasterModels.ConString;
            //"Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
        // GET: About
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetTokuiCD(string param)
        {
            var test1 = param;


            DataTable dtResult = new DataTable();

            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOKCD + '  ,  ' + NAME FROM M0200 WHERE TOKCD + '  ,  ' + NAME LIKE '%" + param + "%'", con);
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

            List<string> listTokuiCD = new List<string>();
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

                listTokuiCD.Add((string)row[0]);
            }

            string[] result = listTokuiCD.ToArray<string>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNouNyu(string param)
        {

            string strsql = "";
            if (param.Contains(" - "))
            {
                string[] searchKey = param.Split(new string[] { " - " }, StringSplitOptions.None);

                strsql = "SELECT NOUCD + '  ,  ' + NAME FROM M0220 WHERE TOKCD='" + searchKey[1] + "' AND NOUCD + '  ,  ' + NAME LIKE '%"+ searchKey[0] + "%' ";
            }
            else strsql = "SELECT NOUCD + '  ,  ' + NAME FROM M0220 WHERE TOKCD='" + param + "'";


            DataTable dtResult = new DataTable();
            //string ConString = "Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand(strsql, con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }
            List<string> listNounyu = new List<string>();
            foreach (DataRow row in dtResult.Rows)
            {
                listNounyu.Add((string)row[0]);
            }
            string[] result = listNounyu.ToArray<string>();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSehin(string param)
        {
           string strSQL = "";
            if(!param.Contains(" - "))
            {
                strSQL = "SELECT ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU, TANKA FROM M0100 "
                    + "WHERE ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU LIKE '%" + param.ToUpper() + "%'";

                string[] multiP = param.Split(' ');
                if (multiP.Count() > 0)
                {
                    strSQL = "SELECT ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU, TANKA FROM M0100 WHERE ZAICD=ZAICD ";
                    foreach (string value in multiP)
                    {
                        strSQL += " AND ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU LIKE '%" + value.ToUpper() + "%' ";
                    }
                }
            }else
            {
                string[] searchKey = param.Split(new string[] { " - " }, StringSplitOptions.None);

                string kubun = "A";

                if (searchKey[1] == "0") kubun = "A";
                else kubun = "B";

                strSQL = "SELECT ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU, TANKA FROM M0100 "
                    + "WHERE ZAIKB='" + kubun + "' ";
                    //"AND ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU LIKE '%" + searchKey[0].ToUpper() + "%'";


                string[] multiP = searchKey[0].Split(' ');
                if (multiP.Count() > 0)
                {
                    //strSQL = "SELECT ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU, TANKA FROM M0100 WHERE ZAICD=ZAICD ";
                    foreach (string value in searchKey[0].Split(' '))
                    {
                        strSQL += " AND ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU LIKE '%" + value.ToUpper() + "%' ";
                    }
                }
                else
                {
                    strSQL += " AND ZAICD + '  ,  ' + ZUBAN + '  ,  ' + NAME + '  ,  ' + KISYU LIKE '%" + searchKey[0].ToUpper() + "%' ";
                }


            }


            DataTable dtResult = new DataTable();
            //string ConString = "Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }
            List<string> listNounyu = new List<string>();
            var a = dtResult.Rows.Count;

            foreach (DataRow row in dtResult.Rows)
            {
                double kisyu = double.Parse(row[1].ToString());
                listNounyu.Add((string)row[0] + "  ,  " + kisyu);
            }
            string[] result = listNounyu.ToArray<string>();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTantou(string param)
        {
            DataTable dtResult = new DataTable();
            //string ConString = "Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("SELECT SAGCD + '  ,  ' + NAME FROM M0400 WHERE "
                   + "SAGCD + '  ,  ' + NAME LIKE '%" + param + "%'", con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }
            List<string> listNounyu = new List<string>();

            foreach (DataRow row in dtResult.Rows)
            {
                listNounyu.Add((string)row[0]);
            }
            string[] result = listNounyu.ToArray<string>();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSehins()
        {
            DataTable dtResult = new DataTable();

            //string ConString = "Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
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

        public string registerOrder(Models.JyuucyuuModels newYuucyu)
        {
            return newYuucyu.inputTESCWORK();
        }

        [System.Web.Http.HttpPost]
        public void setLangSel(string lang)
        {
            Properties.Settings.Default.langSel = lang;
            Session["langSel"] = lang;
        }

        public string getLangSel()
        {
            //if (Session["langSel"] != null) return Session["langSel"].ToString();
            return Properties.Settings.Default.langSel;
        }

        public JsonResult GetCYUMNDBF()
        {


            //string strcon = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \\10.121.21.16\Rainbow; Extended Properties = dBASE IV;";

            string strcon = Models.MasterModels.ConStringDBF;

            string sqlINS = "SELECT CYUNO,CYUDT,ZUBAN,SNAME,KISYU,TANI,TANKA,CYUSU,NOUKI,TOKCD,NOUCD,SFLG FROM CYUMNDF.DBF WHERE CYUDT > '20190901' AND CYUNO > '180000'";// SFLG IS NULL";

            DataTable dtResult = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(strcon))
            {
                OleDbCommand cmd = new OleDbCommand(sqlINS, conn);
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dtResult);
                conn.Close();
            }

            List<Models.DBFCYUMNModels> listCYUMN = new List<Models.DBFCYUMNModels>();
            foreach (DataRow row in dtResult.Rows)
            {
                Models.DBFCYUMNModels cyumn = new Models.DBFCYUMNModels
                {
                    Cyuno = row["CYUNO"].ToString(),
                    Cyudt = row["CYUDT"].ToString(),
                    Zuban = row["ZUBAN"].ToString(),
                    Sname = row["SNAME"].ToString(),
                    Kisyu = row["KISYU"].ToString(),
                    Tani = row["TANI"].ToString(),
                    Tanka = row["TANKA"].ToString(),
                    Cyusu = row["CYUSU"].ToString(),
                    Nouki = row["NOUKI"].ToString(),
                    Tokcd = row["TOKCD"].ToString(),
                    Noucd = row["NOUCD"].ToString(),
                    Sflg = row["SFLG"].ToString()
                };

                listCYUMN.Add(cyumn);
            }

            Models.DBFCYUMNModels[] result = listCYUMN.ToArray<Models.DBFCYUMNModels>();

            Session["listCyumn"] = listCYUMN;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult convertCyuToJyu(Models.DBFCYUMNModels cyumn)
        {
            Models.JyuucyuuModels jyucyu = new Models.JyuucyuuModels();
            jyucyu.Jyuucyuno = "";


            jyucyu.Tokuicode = "00001";
            jyucyu.Tokuiname = "(株)玉吉製作所大田原工場";
            jyucyu.Nounyucode = "00001";
            jyucyu.Nonyuname = "大田原工場";


            jyucyu.Seihinname = cyumn.Sname;
            jyucyu.Seihinzuban = cyumn.Zuban;
            jyucyu.Seihinkisyu = cyumn.Kisyu;
            jyucyu.Quantity = cyumn.Cyusu;
            jyucyu.Tanka = cyumn.Tanka;

            //find by SQL

            DataTable dtResult = cyumn.findSehinCode();
            string result = "";

            var rowC = dtResult.Rows.Count;
            string rtani;
            double rtanka, ctanka;


            if (rowC < 1)
            {
                result = "Error : Can not find this sehin";
            }

            if (rowC == 1)
            {
                jyucyu.Seihinname = dtResult.Rows[0]["NAME"].ToString().Trim();
                jyucyu.Seihinkisyu = dtResult.Rows[0]["KISYU"].ToString().Trim();
                rtani = dtResult.Rows[0]["TANI"].ToString().Trim();
                rtanka = double.Parse(dtResult.Rows[0]["TANKA"].ToString());

                if (cyumn.Sname == null) cyumn.Sname = "";
                if (cyumn.Kisyu == null) cyumn.Kisyu = "";
                if (cyumn.Tani == null) cyumn.Tani = "";
                if (cyumn.Tanka == null) ctanka = 0;
                else ctanka = double.Parse(cyumn.Tanka);

                if (jyucyu.Seihinname != cyumn.Sname) result += "Name is not correct, ";
                if (jyucyu.Seihinkisyu != cyumn.Kisyu.ToString()) result += "Kisyu is not correct, ";
                if (rtani != cyumn.Tani.ToString()) result += "Tani is not correct, ";
                if (rtanka != ctanka) result += "Tanka is not correct, ";

                if (result != "") result = "Error: " + result.Substring(0, result.Length - 2);
                
                jyucyu.Tanka = rtanka.ToString();
                jyucyu.Seihincode = dtResult.Rows[0]["ZAICD"].ToString().Trim();
            }

            else result = "Error : Have many sehin Code";
            
            jyucyu.Nouki = cyumn.Nouki;
            jyucyu.Nounyu = "";//early nouki 2 days

            jyucyu.Bumoncode = "999";
            jyucyu.Bumonname = "データCONV";
            jyucyu.Tantoucode = "209";//by Login User
            jyucyu.Tantouname = "和久井清美";//by Login User
            jyucyu.Cyumonno1 = cyumn.Cyuno;
            jyucyu.Cyumonno2 = "";
            jyucyu.Seizou = "";
            jyucyu.Bikou = "";
            jyucyu.Status = result; //"Checking";

            return Json(jyucyu, JsonRequestBehavior.AllowGet);
        }

        public JsonResult savetoJyuEDI(Models.JyuucyuuModels JyuuProcess)
        {
            JyuuProcess.updateInf();

            return this.GetJyuProcess();
        }

        public JsonResult GetJyuProcess()
        {
            DataTable dtResult = new DataTable();

            //string ConStringEx = "Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";
            string ConStringEx = Models.MasterModels.ConStringEx;

            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                SqlCommand cmd = new SqlCommand("SELECT JYUNO,TOKCD,TNAME,NOUCD,NNAME,SEICD,ZUBAN,NAME,KISYU,JYUSU,JTANKA,NOUKI,YDATE,BMNCD,JTANCD,CYUNO,EDANO,SEIZO,BIKOU,NDATE,STATUS FROM D1000_EDI WHERE STATUS IS NOT NULL AND STATUS != 'FN' AND STATUS != 'DEL'", con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);                
                con.Close();
            }

            List<Models.JyuucyuuModels> listJyu = new List<Models.JyuucyuuModels>();
            foreach (DataRow row in dtResult.Rows)
            {
                Models.JyuucyuuModels jyucyu = new Models.JyuucyuuModels
                {
                    selected = false,
                    Jyuucyuno = row["JYUNO"].ToString(),                    
                    Tokuicode = row["TOKCD"].ToString(),
                    Tokuiname = row["TNAME"].ToString(),
                    Nounyucode = row["NOUCD"].ToString(),
                    Nonyuname = row["NNAME"].ToString(),
                    Seihincode = row["SEICD"].ToString(),
                    Seihinzuban = row["ZUBAN"].ToString(),
                    Seihinname = row["NAME"].ToString(),
                    Seihinkisyu = row["KISYU"].ToString(),
                    Quantity = row["JYUSU"].ToString(),
                    Tanka = row["JTANKA"].ToString(),
                    Nouki = row["NOUKI"].ToString(),
                    Nounyu = row["YDATE"].ToString(),
                    Bumoncode = row["BMNCD"].ToString(),
                    Tantoucode = row["JTANCD"].ToString(),
                    Cyumonno1 = row["CYUNO"].ToString(),
                    Cyumonno2 = row["EDANO"].ToString(),
                    Seizou = row["SEIZO"].ToString(),
                    Bikou = row["BIKOU"].ToString(),
                    Ndate = row["NDATE"].ToString(),
                    Status = row["STATUS"].ToString(),
                };

                listJyu.Add(jyucyu);
            }

            Models.JyuucyuuModels[] result = listJyu.ToArray<Models.JyuucyuuModels>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string findName(string tokuiC,string NounyuC)
        {
            string tokuiN = "", NounyuN = "";
            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("SELECT M0200.TOKCD TOKUIC, M0200.NAME TOKUIN, M0220.NOUCD NOUNYUC,M0220.NAME NOUNYUN"
                    + " FROM M0200 LEFT JOIN M0220 ON M0200.TOKCD = M0220.TOKCD AND M0220.NOUCD = '" + NounyuC + "'"
                    + " WHERE M0200.TOKCD = '" + tokuiC + "'", con);
                con.Open();

                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }

            if (dtResult.Rows.Count == 1)
            {
                tokuiN = dtResult.Rows[0]["TOKUIN"].ToString();
                NounyuN = dtResult.Rows[0]["NOUNYUN"].ToString();
                if (NounyuN == "") NounyuN = "NG: Can not find Delivery";
            }
            else return "NG: Can not find Customer - NG: Can not find Delivery"; 
            return tokuiN + " - " + NounyuN;
        }

        public Models.JyuucyuuModels convertCyu(Models.DBFCYUMNModels cyumn)
        {
            //Models.JyuucyuuModels result = new Models.JyuucyuuModels();

            //

            Models.JyuucyuuModels jyucyu = new Models.JyuucyuuModels();
            jyucyu.Jyuucyuno = "";


            jyucyu.Tokuicode = "00" + cyumn.Tokcd;
            jyucyu.Nounyucode = "00" + cyumn.Noucd;

            string temp = this.findName(jyucyu.Tokuicode, jyucyu.Nounyucode);

            jyucyu.Tokuiname = temp.Split(new string[] { " - " }, StringSplitOptions.None)[0];            
            jyucyu.Nonyuname = temp.Split(new string[] { " - " }, StringSplitOptions.None)[1];


            jyucyu.Seihinname = cyumn.Sname;
            jyucyu.Seihinzuban = cyumn.Zuban;
            jyucyu.Seihinkisyu = cyumn.Kisyu;
            jyucyu.Quantity = cyumn.Cyusu;
            jyucyu.Tanka = cyumn.Tanka;

            //find by SQL
            DataTable dtResult = cyumn.findSehinCode();
            string result = "";

            var rowC = dtResult.Rows.Count;
            string rtani;
            double rtanka, ctanka;


            if (rowC < 1)
            {
                result = "NG: Can not find this sehin";
            }

            if (rowC == 1)
            {
                jyucyu.Seihinname = dtResult.Rows[0]["NAME"].ToString().Trim();
                jyucyu.Seihinkisyu = dtResult.Rows[0]["KISYU"].ToString().Trim();
                rtani = dtResult.Rows[0]["TANI"].ToString().Trim();
                rtanka = double.Parse(dtResult.Rows[0]["TANKA"].ToString());

                if (cyumn.Sname == null) cyumn.Sname = "";
                if (cyumn.Kisyu == null) cyumn.Kisyu = "";
                if (cyumn.Tani == null) cyumn.Tani = "";
                if (cyumn.Tanka == null) ctanka = 0;
                else ctanka = double.Parse(cyumn.Tanka);

                if (jyucyu.Seihinname != cyumn.Sname) result += "NG: Name is not correct, ";
                if (jyucyu.Seihinkisyu != cyumn.Kisyu.ToString()) result += "NG: Kisyu is not correct, ";
                if (rtani != cyumn.Tani.ToString()) result += "NG: Tani is not correct, ";
                if (rtanka != ctanka) result += "NG: Tanka is not correct, ";

                if (result != "") result = result.Substring(0, result.Length - 2);

                jyucyu.Tanka = rtanka.ToString();
                jyucyu.Seihincode = dtResult.Rows[0]["ZAICD"].ToString().Trim();
            }

            else result = "NG: Have many sehin Code";

            jyucyu.Nouki = cyumn.Nouki;

            jyucyu.Nounyu = new DateTime(Int32.Parse(cyumn.Nouki.Substring(0, 4)), Int32.Parse(cyumn.Nouki.Substring(4, 2)), Int32.Parse(cyumn.Nouki.Substring(6, 2))).AddDays(-2).ToString("yyyyMMdd");//early nouki 2 days

            jyucyu.Bumoncode = "999";
            jyucyu.Bumonname = "データCONV";
            jyucyu.Tantoucode = "209";//by Login User
            jyucyu.Tantouname = "和久井清美";//by Login User
            jyucyu.Cyumonno1 = cyumn.Cyuno;
            jyucyu.Cyumonno2 = "";
            jyucyu.Seizou = "";
            jyucyu.Bikou = "";
            jyucyu.Status = result; //"Checking";

            return jyucyu;
        }

        public void importEDI(Models.JyuucyuuModels newYuucyu)
        {
            if (newYuucyu.checkExists()) return;
            
            var result = "";

            int maxJYU, insertRow;
            string ConStringEx = Models.MasterModels.ConStringEx;
                //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";

            //Get maxJYU
            maxJYU = 0;
            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                SqlCommand cmd = new SqlCommand("SELECT D1000_EDI.SEICD maxJYU FROM D1000_EDI WHERE JYUNO = '000000'", con);
                con.Open();

                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }
            maxJYU = Int32.Parse(dtResult.Rows[0]["maxJYU"].ToString());
            //maxSEI = Int32.Parse(dtResult.Rows[0]["maxSEI"].ToString());

            insertRow = 1;

            //if (insertRow == 1) return "test";

            using (SqlConnection connection = new SqlConnection(ConStringEx))
            {
                SqlCommand command = new SqlCommand("UPDATE D1000_EDI SET SEICD='" + (maxJYU + insertRow) + "' WHERE JYUNO='000000'", connection);
                //SqlCommand command = new SqlCommand("UPDATE D1000_EDI SET SEICD='462379' WHERE JYUNO='000000'", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            /*
            using (SqlConnection connection = new SqlConnection(ConString))
            {
                SqlCommand command = new SqlCommand("UPDATE D1100_EDI SET ZAICD='" + (maxSEI + insertRow) + "' WHERE SIYNO='000000'", connection);
                //SqlCommand command = new SqlCommand("UPDATE D1100_EDI SET ZAICD='445159' WHERE SIYNO='000000'", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            */

            //確定受注
            string insertSQL = "INSERT INTO D1000_EDI VALUES " +
                "('" + (maxJYU + 1) + "','','" + newYuucyu.Tokuicode + "','" + newYuucyu.Tokuiname + "','" + newYuucyu.Nounyucode + "','" + newYuucyu.Nonyuname + "'," +
                "'" + newYuucyu.Seihincode + "','" + newYuucyu.Seihinzuban + "','" + newYuucyu.Seihinname + "','" + newYuucyu.Seihinkisyu + "','" + newYuucyu.Quantity + "','" + newYuucyu.Tanka + "'," +
                "'" + newYuucyu.Nouki.Replace("-", "").Substring(0, 8) + "','" + newYuucyu.Nounyu.Replace("-", "").Substring(0, 8) + "'," +
                "'999','"+newYuucyu.Tantoucode+"','" + newYuucyu.Cyumonno1 + "','" + newYuucyu.Cyumonno2 + "','" + newYuucyu.Seizou + "','" + newYuucyu.Bikou + "','0'" +
                ",'','','',0,0," +
                "'" + DateTime.Now.ToString("yyyyMMdd") + "','" + newYuucyu.Status + "')";
            insertSQL = insertSQL.Replace("''", "null");
            using (SqlConnection connection = new SqlConnection(ConStringEx))
            {
                SqlCommand command = new SqlCommand(insertSQL, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

            /*
            string strcon = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \\10.121.21.16\Rainbow; Extended Properties = dBASE IV;";
            string sqlINS = "UPDATE CYUMNDF.DBF SET SFLG='2' WHERE CYUNO = '" + CYUNO + "'";// SFLG IS NULL";

            using (OleDbConnection conn = new OleDbConnection(strcon))
            {
                OleDbCommand cmd = new OleDbCommand(sqlINS, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            */

        }

        public JsonResult importToD1000Ex(List<Models.DBFCYUMNModels> listcyumn)
        {
            /*
            this.GetCYUMNDBF();

            listcyumn = (List<Models.DBFCYUMNModels>) Session["listCyumn"];

            foreach (Models.DBFCYUMNModels cyumn in listcyumn)
            {
                Models.JyuucyuuModels jyucyu = this.convertCyu(cyumn);
                if (!jyucyu.checkExists())
                {
                    this.importEDI(jyucyu);
                } 
            }*/

            return this.GetJyuProcess();
        }

        public string findPIC(string JTANCD)
        {

            string ConStringEx = Models.MasterModels.ConString;
                //"Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";

            DataTable dtResult = new DataTable();
            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                SqlCommand cmd = new SqlCommand("SELECT NAME FROM M0400 WHERE SAGCD='"+ JTANCD + "'", con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }
            if (dtResult.Rows.Count > 0) return dtResult.Rows[0]["NAME"].ToString();

            return "NG: NOT FOUND";
        }

        public JsonResult completeEDI(string listselectedProcess, string listJyuProcess)
        {
            string ConStringEx = Models.MasterModels.ConStringEx;
            DataTable dtResult = new DataTable();

            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                dtResult = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT * FROM D1000_EDI " +
                    " WHERE JYUNO " + listselectedProcess, con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();

                foreach (DataRow row in dtResult.Rows)
                {
                    Models.JyuucyuuModels jyucyu = new Models.JyuucyuuModels
                    {
                        Jyuucyuno = row["JYUNO"].ToString(),
                        Tokuicode = row["TOKCD"].ToString(),
                        Tokuiname = row["TNAME"].ToString(),
                        Nounyucode = row["NOUCD"].ToString(),
                        Nonyuname = row["NNAME"].ToString(),
                        Seihincode = row["SEICD"].ToString(),
                        Seihinzuban = row["ZUBAN"].ToString(),
                        Seihinname = row["NAME"].ToString(),
                        Seihinkisyu = row["KISYU"].ToString(),
                        Quantity = row["JYUSU"].ToString(),
                        Tanka = row["JTANKA"].ToString(),
                        Nouki = row["NOUKI"].ToString(),
                        Nounyu = row["YDATE"].ToString(),
                        Bumoncode = row["BMNCD"].ToString(),
                        Tantoucode = row["JTANCD"].ToString(),
                        Cyumonno1 = row["CYUNO"].ToString(),
                        Cyumonno2 = row["EDANO"].ToString(),
                        Seizou = row["SEIZO"].ToString(),
                        Bikou = row["BIKOU"].ToString(),
                        Ndate = row["NDATE"].ToString(),
                        Status = row["STATUS"].ToString()
                    };

                    jyucyu.inputTESCWORK();
                }
            }
            List<Models.JyuucyuuModels> listJyu = new List<Models.JyuucyuuModels>();
            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                dtResult = new DataTable();
                SqlCommand cmd = new SqlCommand("SELECT JYUNO,TOKCD,TNAME,NOUCD,NNAME,SEICD,ZUBAN,NAME,KISYU,JYUSU,JTANKA,NOUKI,YDATE,BMNCD,JTANCD,CYUNO,EDANO,SEIZO,BIKOU,NDATE,STATUS FROM D1000_EDI " +
                    " WHERE JYUNO " + listJyuProcess, con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();                
                foreach (DataRow row in dtResult.Rows)
                {
                    Models.JyuucyuuModels jyucyu = new Models.JyuucyuuModels
                    {
                        Jyuucyuno = row["JYUNO"].ToString(),
                        Tokuicode = row["TOKCD"].ToString(),
                        Tokuiname = row["TNAME"].ToString(),
                        Nounyucode = row["NOUCD"].ToString(),
                        Nonyuname = row["NNAME"].ToString(),
                        Seihincode = row["SEICD"].ToString(),
                        Seihinzuban = row["ZUBAN"].ToString(),
                        Seihinname = row["NAME"].ToString(),
                        Seihinkisyu = row["KISYU"].ToString(),
                        Quantity = row["JYUSU"].ToString(),
                        Tanka = row["JTANKA"].ToString(),
                        Nouki = row["NOUKI"].ToString(),
                        Nounyu = row["YDATE"].ToString(),
                        Bumoncode = row["BMNCD"].ToString(),
                        Tantoucode = row["JTANCD"].ToString(),
                        Cyumonno1 = row["CYUNO"].ToString(),
                        Cyumonno2 = row["EDANO"].ToString(),
                        Seizou = row["SEIZO"].ToString(),
                        Bikou = row["BIKOU"].ToString(),
                        Ndate = row["NDATE"].ToString(),
                        Status = row["STATUS"].ToString(),
                    };

                    listJyu.Add(jyucyu);
                }
            }
            
            Models.JyuucyuuModels[] result = listJyu.ToArray<Models.JyuucyuuModels>();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult deleteSelectedOrder(List<Models.JyuucyuuModels> listJyuProcess)
        {
            if (listJyuProcess != null)
            {
                string cond = " IN (";
                foreach (Models.JyuucyuuModels jyucyu in listJyuProcess)
                {
                    cond += "'" + jyucyu.Jyuucyuno + "',";

                    if (jyucyu.selected == true)
                    {
                        //string ConStringEx = Models.MasterModels.ConStringEx;
                        //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";

                        //Update FN on EDI
                        string updateSQL = "UPDATE D1000_EDI SET STATUS = 'DEL' WHERE CYUNO='" + jyucyu.Cyumonno1 + "' AND JYUNO='" + jyucyu.Jyuucyuno + "'";
                        using (SqlConnection connection = new SqlConnection(Models.MasterModels.ConStringEx))
                        {
                            SqlCommand command = new SqlCommand(updateSQL, connection);
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                            command.Connection.Close();
                        }
                        
                    }
                }

                cond = cond.Substring(0, cond.Length - 1) + ")";

                DataTable dtResult = new DataTable();

                string ConStringEx = Models.MasterModels.ConStringEx;
                //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";
                using (SqlConnection con = new SqlConnection(ConStringEx))
                {
                    SqlCommand cmd = new SqlCommand("SELECT JYUNO,TOKCD,TNAME,NOUCD,NNAME,SEICD,ZUBAN,NAME,KISYU,JYUSU,JTANKA,NOUKI,YDATE,BMNCD,JTANCD,CYUNO,EDANO,SEIZO,BIKOU,NDATE,STATUS FROM D1000_EDI " +
                        " WHERE JYUNO " + cond, con);
                    con.Open();
                    SqlDataAdapter ada = new SqlDataAdapter(cmd);
                    ada.Fill(dtResult);
                    con.Close();
                }

                List<Models.JyuucyuuModels> listJyu = new List<Models.JyuucyuuModels>();
                foreach (DataRow row in dtResult.Rows)
                {
                    Models.JyuucyuuModels jyucyu = new Models.JyuucyuuModels
                    {
                        Jyuucyuno = row["JYUNO"].ToString(),
                        Tokuicode = row["TOKCD"].ToString(),
                        Tokuiname = row["TNAME"].ToString(),
                        Nounyucode = row["NOUCD"].ToString(),
                        Nonyuname = row["NNAME"].ToString(),
                        Seihincode = row["SEICD"].ToString(),
                        Seihinzuban = row["ZUBAN"].ToString(),
                        Seihinname = row["NAME"].ToString(),
                        Seihinkisyu = row["KISYU"].ToString(),
                        Quantity = row["JYUSU"].ToString(),
                        Tanka = row["JTANKA"].ToString(),
                        Nouki = row["NOUKI"].ToString(),
                        Nounyu = row["YDATE"].ToString(),
                        Bumoncode = row["BMNCD"].ToString(),
                        Tantoucode = row["JTANCD"].ToString(),
                        Cyumonno1 = row["CYUNO"].ToString(),
                        Cyumonno2 = row["EDANO"].ToString(),
                        Seizou = row["SEIZO"].ToString(),
                        Bikou = row["BIKOU"].ToString(),
                        Ndate = row["NDATE"].ToString(),
                        Status = row["STATUS"].ToString(),
                    };

                    listJyu.Add(jyucyu);
                }

                Models.JyuucyuuModels[] result = listJyu.ToArray<Models.JyuucyuuModels>();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else return new JsonResult();
        }



        public string checkData(Models.JyuucyuuModels JyuuProcess)
        {
            return JyuuProcess.checkERROR();
        }

        public void syncEDIDATA()
        {
            Models.JyuucyuuModels jyucyu = new Models.JyuucyuuModels();

            jyucyu.syncEDI();

            //return this.GetJyuProcess();
        }

        public string validateDATA(Models.JyuucyuuModels JyuuProcess)
        {
            return JyuuProcess.checkERROR();
        }

    }
}