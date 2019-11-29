using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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

            string ConString = Models.MasterModels.ConString;
                //"Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
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

        public void getSeiEDI(int kubun, bool kaitenCheck)
        {
            Models.SeisanModels seisan = new Models.SeisanModels();

            seisan.syncEDI(kubun, kaitenCheck);

            //return this.GetJyuProcess();
        }

        public JsonResult GetSeiProcess()
        {
            DataTable dtResult = new DataTable();

            string ConStringEx = Models.MasterModels.ConStringEx;
                //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                SqlCommand cmd = new SqlCommand("SELECT SIYNO,SIYKB,TOKCD,TNAME,ZAIKB,ZAICD,ZUBAN,NAME,KISYU,SEISU,JTANKA,NOUKI,NAIJI,HIKSU,TMKBN,SKKBN,NDATE,STATUS FROM [TESCex].[dbo].[D1100_EDI] WHERE STATUS IS NOT NULL AND STATUS != 'FN'", con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }

            List<Models.SeisanModels> listSei = new List<Models.SeisanModels>();
            foreach (DataRow row in dtResult.Rows)
            {
                Models.SeisanModels seisan = new Models.SeisanModels
                {
                    selected = false,
                    SIYNO = row["SIYNO"].ToString(),
                    SIYKB = row["SIYKB"].ToString(),
                    TOKCD = row["TOKCD"].ToString(),
                    TNAME = row["TNAME"].ToString(),
                    ZAIKB = row["ZAIKB"].ToString(),
                    ZAICD = row["ZAICD"].ToString(),
                    ZUBAN = row["ZUBAN"].ToString(),
                    NAME = row["NAME"].ToString(),
                    KISYU = row["KISYU"].ToString(),
                    SEISU = row["SEISU"].ToString(),
                    JTANKA = row["JTANKA"].ToString(),
                    NOUKI = row["NOUKI"].ToString(),
                    NAIJI = row["NAIJI"].ToString(),
                    HIKSU = row["SEISU"].ToString(),
                    TMKBN = row["TMKBN"].ToString(),
                    SKKBN = row["SKKBN"].ToString(),
                    NDATE = row["NDATE"].ToString(),
                    Status = row["STATUS"].ToString()
                };
                listSei.Add(seisan);
            }
            Models.SeisanModels[] result = listSei.ToArray<Models.SeisanModels>();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string delJyuProcess(Models.JyuucyuuModels Jyuu)
        {

            string ConStringEx = Models.MasterModels.ConStringEx;
                //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";
            
            //Update FN on EDI
            string updateSQL = "UPDATE D1000_EDI SET STATUS = 'DEL' WHERE CYUNO='" + Jyuu.Cyumonno1 + "' AND JYUNO='" + Jyuu.Jyuucyuno + "'";
            using (SqlConnection connection = new SqlConnection(ConStringEx))
            {
                SqlCommand command = new SqlCommand(updateSQL, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }          

            return "";
        }

        public JsonResult deleteSelectedOrder(string listselectedProcess, string listJyuProcess)
        {
            string ConStringEx = Models.MasterModels.ConStringEx;

            string updateSQL = "UPDATE D1000_EDI SET STATUS = 'DEL' WHERE JYUNO " + listselectedProcess;
            using (SqlConnection connection = new SqlConnection(ConStringEx))
            {
                SqlCommand command = new SqlCommand(updateSQL, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }


            DataTable dtResult = new DataTable();

            
            //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                SqlCommand cmd = new SqlCommand("SELECT JYUNO,TOKCD,TNAME,NOUCD,NNAME,SEICD,ZUBAN,NAME,KISYU,JYUSU,JTANKA,NOUKI,YDATE,BMNCD,JTANCD,CYUNO,EDANO,SEIZO,BIKOU,NDATE,STATUS FROM D1000_EDI " +
                    " WHERE JYUNO " + listJyuProcess, con);
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

        public JsonResult deleteSelectedSeiyo(string listselectedProcess, string listSeiProcess)
        {
            string ConStringEx = Models.MasterModels.ConStringEx;

            using (SqlConnection connection = new SqlConnection(ConStringEx))
            {
                SqlCommand command = new SqlCommand("DELETE [TESCex].[dbo].D1100_EDI WHERE SIYNO " + listselectedProcess, connection);
                //SqlCommand command = new SqlCommand("UPDATE D1000_EDI SET SEICD='462379' WHERE JYUNO='000000'", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }

            return this.GetSeiProcess();


            return new JsonResult();


            string updateSQL = "UPDATE D1000_EDI SET STATUS = 'DEL' WHERE JYUNO " + listselectedProcess;
            using (SqlConnection connection = new SqlConnection(ConStringEx))
            {
                SqlCommand command = new SqlCommand(updateSQL, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }


            DataTable dtResult = new DataTable();


            //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                SqlCommand cmd = new SqlCommand("SELECT JYUNO,TOKCD,TNAME,NOUCD,NNAME,SEICD,ZUBAN,NAME,KISYU,JYUSU,JTANKA,NOUKI,YDATE,BMNCD,JTANCD,CYUNO,EDANO,SEIZO,BIKOU,NDATE,STATUS FROM D1000_EDI " +
                    " WHERE JYUNO " + listSeiProcess, con);
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



        public string getParamByID(string id)
        {
            string strURL = Request.UrlReferrer.ToString();
            string result;
            string[] param;
            if (strURL.Contains("?"))
            {
                result = strURL.Split('?')[1];
                if (result.Contains("&"))
                {
                    param = result.Split('&');
                    foreach(string parameter in param){
                        if (parameter.Split('=')[0] == id)
                        {
                            return parameter.Split('=')[1];
                        }
                        else result = "";
                    }
                }
                else
                {
                    if (result.Split('=')[0] == id)
                    {
                        return result.Split('=')[1];
                    }
                    else result = "";
                }
            }

            return "";
        }

        public string getRootDirectory(string id)
        {
            string strURL = Request.UrlReferrer.ToString();
            string result;
            string[] param;
            if (strURL.Contains("?"))
            {
                result = strURL.Split('?')[1];
                if (result.Contains("&"))
                {
                    param = result.Split('&');
                    foreach (string parameter in param)
                    {
                        if (parameter.Split('=')[0] == id)
                        {
                            return parameter.Split('=')[1];
                        }
                        else result = "";
                    }
                }
                else
                {
                    if (result.Split('=')[0] == id)
                    {
                        return result.Split('=')[1];
                    }
                    else result = "";
                }
            }

            return "";
        }

        public string updateSeiProcess(Models.SeisanModels seiProcess, string updateID)
        {
            return seiProcess.updateSeiProcess(updateID);
        }

        public string delSeiProcess(Models.SeisanModels seiProcess)
        {
            seiProcess.delSeiProcess();
            return "";
        }

        public bool createSeiProcess(Models.SeisanModels seiProcess)
        {
            return seiProcess.createSeiProcess();
            //return "";
        }

        public string TWSeiProcess(string listseiSelected)
        {
            DataTable dtResult = new DataTable();

            string ConStringEx = Models.MasterModels.ConStringEx;
            //"Data Source=10.121.21.15;Initial Catalog=TESCex;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(ConStringEx))
            {
                SqlCommand cmd = new SqlCommand("SELECT SIYNO,SIYKB,TOKCD,TNAME,ZAIKB,ZAICD,ZUBAN,NAME,KISYU,SEISU,JTANKA,NOUKI,NAIJI,HIKSU,TMKBN,SKKBN,NDATE,STATUS FROM [TESCex].[dbo].[D1100_EDI] "
                    + " WHERE SIYNO " + listseiSelected, con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }
            
            foreach (DataRow row in dtResult.Rows)
            {
                Models.SeisanModels seisan = new Models.SeisanModels
                {
                    selected = false,
                    SIYNO = row["SIYNO"].ToString(),
                    SIYKB = row["SIYKB"].ToString(),
                    TOKCD = row["TOKCD"].ToString(),
                    TNAME = row["TNAME"].ToString(),
                    ZAIKB = row["ZAIKB"].ToString(),
                    ZAICD = row["ZAICD"].ToString(),
                    ZUBAN = row["ZUBAN"].ToString(),
                    NAME = row["NAME"].ToString(),
                    KISYU = row["KISYU"].ToString(),
                    SEISU = row["SEISU"].ToString(),
                    JTANKA = row["JTANKA"].ToString(),
                    NOUKI = row["NOUKI"].ToString(),
                    NAIJI = row["NAIJI"].ToString(),
                    HIKSU = row["SEISU"].ToString(),
                    TMKBN = row["TMKBN"].ToString(),
                    SKKBN = row["SKKBN"].ToString(),
                    NDATE = row["NDATE"].ToString(),
                    Status = row["STATUS"].ToString()
                };
                seisan.TWSeiProcess();
            }
            return "";

            /*
            if (listSeiProcess != null)
            {
                foreach (Models.SeisanModels ss in listSeiProcess)
                {
                    if (ss.selected)
                    {
                        ss.TWSeiProcess();
                    }
                }
            }
            return "";*/
        }

        

        public string changedateNouki(string dateNouki, string dateNounyu)
        {
            string result = "";

            DateTime nouki;                                               
            DateTime nounyu;

            try
            {
                nouki = new DateTime(Int32.Parse(dateNouki.Substring(0, 4)), Int32.Parse(dateNouki.Substring(4, 2)), Int32.Parse(dateNouki.Substring(6, 2)));
            }
            catch (Exception)
            {
                return "NG: PLEASE CORRECT NOUKIDATE";
            }

            int dayNouki = Int32.Parse(dateNouki.Substring(6, 2));

            if (nouki < DateTime.Now) return "NG: NOUKIDATE MUST BE GREATER THAN TODAY";

            if (Models.MasterModels.checkIsHoliday(nouki))
            {
                return "NG: NOUKIDATE is holidays";
            }            

            else
            {
                if(dateNounyu == "" || dateNounyu == null)
                {
                    for(int i = -1; i > -30; i--)
                    {
                        nounyu = nouki.AddDays(i);
                        if (!Models.MasterModels.checkIsHoliday(nounyu))
                        {
                            return nounyu.ToString("yyyyMMdd");
                        }
                    }
                }
                else
                {
                    try
                    {
                        nounyu = new DateTime(Int32.Parse(dateNounyu.Substring(0, 4)), Int32.Parse(dateNounyu.Substring(4, 2)), Int32.Parse(dateNounyu.Substring(6, 2)));
                    }
                    catch (Exception)
                    {
                        nounyu = DateTime.Now;
                    }

                    if (nounyu >= nouki || Models.MasterModels.checkIsHoliday(nounyu) || nounyu <= DateTime.Today)
                    {
                        for (int i = -1; i > -30; i--)
                        {
                            nounyu = nouki.AddDays(i);
                            if (!Models.MasterModels.checkIsHoliday(nounyu))
                            {
                                return nounyu.ToString("yyyyMMdd");
                            }
                        }
                    }
                    else return nounyu.ToString("yyyyMMdd");
                }                
            }
            return result;
        }

        public string changedateNounyu(string dateNouki, string dateNounyu)
        {
            DateTime nouki;
            DateTime nounyu;

            try
            {
                nouki = new DateTime(Int32.Parse(dateNouki.Substring(0, 4)), Int32.Parse(dateNouki.Substring(4, 2)), Int32.Parse(dateNouki.Substring(6, 2)));
            }
            catch (Exception)
            {
                return "NG: PLEASE CORRECT NOUKIDATE";
            }

            int dayNouki = Int32.Parse(dateNouki.Substring(6, 2));

            if (nouki < DateTime.Now) return "NG: NOUKIDATE MUST BE GREATER THAN TODAY";

            if (Models.MasterModels.checkIsHoliday(nouki))
            {
                return "NG: NOUKIDATE IS HOLIDAYS";
            }

            else
            {
                if (dateNounyu == "" || dateNounyu == null)
                {
                    return "NG: PLEASE CORRECT NOUNYUDATE";
                }
                else
                {
                    try
                    {
                        nounyu = new DateTime(Int32.Parse(dateNounyu.Substring(0, 4)), Int32.Parse(dateNounyu.Substring(4, 2)), Int32.Parse(dateNounyu.Substring(6, 2)));
                    }
                    catch (Exception)
                    {
                        return "NG: PLEASE CORRECT NOUNYUDATE";
                    }

                    if (nounyu >= nouki)
                    {
                        return "NG: NOUNYUDATE MUST BE LESS THAN NOUKI";
                    }
                    if(nounyu <= DateTime.Now) return "NG: NOUNYUDATE MUST BE GREATER THAN TODAY";
                    else
                    {
                        if (Models.MasterModels.checkIsHoliday(nounyu))
                        {
                            return "NG: NOUNYUDATE IS HOLIDAYS";
                        }
                        else return "";
                    }
                }
            }
        }


        public bool checkIsHoliday(DateTime dt)
        {
            DataTable dtResult = new DataTable();
            //string ConStringEx = Models.MasterModels.ConStringEx;
                //"Data Source=10.121.21.15;Initial Catalog=TESC;Persist Security Info=True;User ID=TESCWIN;";
            using (SqlConnection con = new SqlConnection(Models.MasterModels.ConString))
            {
                SqlCommand cmd = new SqlCommand("SELECT [DAYKB] FROM [TESC].[dbo].[M0700] WHERE CALYM =" + dt.ToString("yyyyMM"), con);
                con.Open();
                SqlDataAdapter ada = new SqlDataAdapter(cmd);
                ada.Fill(dtResult);
                con.Close();
            }
            string dayKB = dtResult.Rows[0]["DAYKB"].ToString();
            return dayKB.ElementAt(dt.Day - 1) == '2';
        }

        public JsonResult TESTBYDUC(string testV)
        {
            string path = Models.MasterModels.dirServer + "CYUMNDF.csv";

            //string path = @"C:\Rainbow\CYUMNDF.csv";

            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string strsql = "select * from CYUMNDF.csv where ZUBAN='GA6-6713-001'";

            switch (testV)
            {
                case "0":
                    string cnstr = @"Driver={Microsoft Text Driver (*.txt; *.csv)}; DBQ="+pathOnly;
                    OdbcConnection cn = new OdbcConnection(cnstr);

                    cn.Open();
                    OdbcCommand cmd = new OdbcCommand("select * from CYUMNDF.csv where ZUBAN='GA6-6713-001'", cn);

                    OdbcDataReader reader = cmd.ExecuteReader();

                    List<Models.DBFCYUMNModels> listCYUMN = new List<Models.DBFCYUMNModels>();

                    while (reader.Read())
                    {
                        Models.DBFCYUMNModels cyumn = new Models.DBFCYUMNModels
                        {
                            Cyuno = reader["CYUNO"].ToString(),
                            Cyudt = reader["CYUDT"].ToString(),
                            Zuban = reader["ZUBAN"].ToString(),
                            Sname = reader["SNAME"].ToString(),
                            Kisyu = reader["KISYU"].ToString(),
                            Tani = reader["TANI"].ToString(),
                            Tanka = reader["TANKA"].ToString(),
                            Cyusu = reader["CYUSU"].ToString(),
                            Nouki = reader["NOUKI"].ToString(),
                            Tokcd = reader["TOKCD"].ToString(),
                            Noucd = reader["NOUCD"].ToString(),
                            Sflg = reader["SFLG"].ToString()
                        };

                        listCYUMN.Add(cyumn);
                    }

                    reader.Close();
                    cn.Close();

                    Models.DBFCYUMNModels[] result = listCYUMN.ToArray<Models.DBFCYUMNModels>();

                    Session["listCyumn"] = listCYUMN;

                    return Json(result, JsonRequestBehavior.AllowGet);
                    cn.Close();
                    break;
                case "1":
                    string sql = @"SELECT * FROM [" + fileName + "] WHERE SFLG='' OR SFLG IS NULL";

                    using (OleDbConnection con = new OleDbConnection(
                              @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                              ";Extended Properties=\"Text;HDR=Yes\""))
                    using (OleDbCommand command = new OleDbCommand(sql, con))
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        DataTable dtResult = new DataTable();
                        dtResult.Locale = CultureInfo.CurrentCulture;
                        adapter.Fill(dtResult);
                        listCYUMN = new List<Models.DBFCYUMNModels>();

                        //List<DBFCYUMNModels> listCYUMN = new List<DBFCYUMNModels>();
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
                    }
                    result = listCYUMN.ToArray<Models.DBFCYUMNModels>();

                    Session["listCyumn"] = listCYUMN;

                    return Json(result, JsonRequestBehavior.AllowGet);
                    break;
                case "2":
                    // 読み込みたいCSVファイルのパスを指定して開く
                    
                    using (var reader3 = new StreamReader(path, Encoding.GetEncoding("shift_jis")))
                    //Console.WriteLine(reader3.ReadToEnd(),Encoding.GetEncoding("UTF-8"));//日本語化ける
                    //Console.WriteLine(reader3.ReadToEnd(),Encoding.GetEncoding("Shift-JIS"));//日本語化ける
                    //Console.WriteLine(reader3.ReadToEnd(),Encoding.GetEncoding("Shift_JIS"));//日本語化ける
                    //Console.WriteLine(reader3.ReadToEnd(),Encoding.GetEncoding("ASCII"));//日本語化ける
                    //Console.WriteLine(reader3.ReadToEnd(),Encoding.GetEncoding("CP932"));//サポート外

                    {
                        listCYUMN = new List<Models.DBFCYUMNModels>();
                        while (!reader3.EndOfStream)
                        {
                            string line = reader3.ReadLine();
                            if (!String.IsNullOrWhiteSpace(line))
                            {
                                
                                string[] values = line.Split(',');

                                var slfgV = values[values.Length - 1].Replace("\"","").Trim();
                                
                                if (values.Length == 18)
                                {
                                    if (slfgV == "" || slfgV == null)
                                    {
                                        Models.DBFCYUMNModels cyumn = new Models.DBFCYUMNModels
                                        {
                                            Cyuno = values[0].ToString().Replace("\"", ""),
                                            Cyudt = values[2].ToString().Replace("\"", ""),
                                            Zuban = values[4].ToString().Substring(1, values[4].Length - 2).Replace("\"\"", "\""),
                                            Sname = values[5].ToString().Replace("\"", ""),
                                            Kisyu = values[6].ToString().Replace("\"", ""),
                                            Tani = values[7].ToString().Replace("\"", ""),
                                            Tanka = values[8].ToString().Replace("\"", ""),
                                            Cyusu = values[9].ToString().Replace("\"", ""),
                                            Nouki = values[10].ToString().Replace("\"", ""),
                                            Tokcd = values[15].ToString().Replace("\"", ""),
                                            Noucd = values[16].ToString().Replace("\"", ""),
                                            Sflg = values[17].ToString().Replace("\"", "")
                                        };
                                        listCYUMN.Add(cyumn);
                                    }
                                }
                                else
                                {

                                    int i = values.Length - 18;
                                    if (slfgV == "" || slfgV == null)
                                    {
                                        string zubanV = "";
                                        for (int j = 0; j <= i; j++)
                                        {
                                            zubanV += values[4+j].ToString() + ",";
                                        }

                                        Models.DBFCYUMNModels cyumn = new Models.DBFCYUMNModels
                                        {
                                            Cyuno = values[0].ToString().Replace("\"", ""),
                                            Cyudt = values[2].ToString().Replace("\"", ""),
                                            Zuban = zubanV.Substring(1, zubanV.Length - 3).Replace("\"\"","\""),
                                            Sname = values[5+i].ToString().Replace("\"", ""),
                                            Kisyu = values[6+1].ToString().Replace("\"", ""),
                                            Tani = values[7+i].ToString().Replace("\"", ""),
                                            Tanka = values[8+i].ToString().Replace("\"", ""),
                                            Cyusu = values[9+i].ToString().Replace("\"", ""),
                                            Nouki = values[10+i].ToString().Replace("\"", ""),
                                            Tokcd = values[15+i].ToString().Replace("\"", ""),
                                            Noucd = values[16+i].ToString().Replace("\"", ""),
                                            Sflg = values[17+i].ToString().Replace("\"", "")
                                        };
                                        listCYUMN.Add(cyumn);
                                    }
                                }       
                            }
                        }
                        result = listCYUMN.ToArray<Models.DBFCYUMNModels>();
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }


                    break;

                case "3":
                    DbConnection connection = new OleDbConnection();
                    connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;"      // プロバイダ設定
                                                                                            //= "Provider=Microsoft.Jet.OLEDB.4.0;"     // Jetでやる場合
                        + "Data Source=" + pathOnly + "\\; "          // ソースファイル指定
                        + "Extended Properties=\"Text;HDR=YES;FMT=Delimited\"";
                    connection.Open();
                    DbCommand cmd1;

                    cmd1 = connection.CreateCommand();
                    cmd1.CommandText = strsql;
                    DbDataReader reader2 = cmd1.ExecuteReader();

                    listCYUMN = new List<Models.DBFCYUMNModels>();
                    while (reader2.Read())
                    {
                        Models.DBFCYUMNModels cyumn = new Models.DBFCYUMNModels
                        {
                            Cyuno = reader2["CYUNO"].ToString(),
                            Cyudt = reader2["CYUDT"].ToString(),
                            Zuban = reader2["ZUBAN"].ToString(),
                            Sname = reader2["SNAME"].ToString(),
                            Kisyu = reader2["KISYU"].ToString(),
                            Tani = reader2["TANI"].ToString(),
                            Tanka = reader2["TANKA"].ToString(),
                            Cyusu = reader2["CYUSU"].ToString(),
                            Nouki = reader2["NOUKI"].ToString(),
                            Tokcd = reader2["TOKCD"].ToString(),
                            Noucd = reader2["NOUCD"].ToString(),
                            Sflg = reader2["SFLG"].ToString()
                        };

                        listCYUMN.Add(cyumn);
                    }

                    result = listCYUMN.ToArray<Models.DBFCYUMNModels>();

                    Session["listCyumn"] = listCYUMN;

                    cmd1.Dispose();
                    connection.Dispose();

                    return Json(result, JsonRequestBehavior.AllowGet);
                    break;
                default:
                    return new JsonResult();
                    break;
            }
            return new JsonResult();
            /*
            //string header = isFirstRowHeader ? "Yes" : "No";
            string path = Models.MasterModels.dirServer +  "CYUMNDF.csv";
            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

//            string result = "";
            string cnstr = @"Driver={Microsoft Text Driver (*.txt; *.csv)}; DBQ=C:\Rainbow";
            OdbcConnection cn = new OdbcConnection(cnstr);

            cn.Open();

            OdbcCommand cmd = new OdbcCommand("select * from CYUMNDF.csv where ZUBAN='GA6-6713-001'", cn);

            OdbcDataReader row = cmd.ExecuteReader();

            List<Models.DBFCYUMNModels> listCYUMN = new List<Models.DBFCYUMNModels>();

            while (row.Read())
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

            row.Close();
            cn.Close();

            Models.DBFCYUMNModels[] result = listCYUMN.ToArray<Models.DBFCYUMNModels>();

            Session["listCyumn"] = listCYUMN;

            return Json(result, JsonRequestBehavior.AllowGet);
            

            /*
            using (OleDbConnection connection = new OleDbConnection(@"Driver={Microsoft Text Driver (*.txt; *.csv)}; DBQ="+pathOnly))

            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dtResult = new DataTable();
                dtResult.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dtResult);

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

            return new JsonResult();

            /*
            string result = "";
            string cnstr = @"Driver={Microsoft Text Driver (*.txt; *.csv)}; DBQ=C:\Rainbow";
            OdbcConnection cn = new OdbcConnection(cnstr);

            cn.Open();

            OdbcCommand cmd = new OdbcCommand("select * from CYUMNDF.csv where ZUBAN='GA6-6713-001'", cn);

            OdbcDataReader r = cmd.ExecuteReader();
                       

            while (r.Read())
            {
                result += "<span>";
                result += r["CYUNO"] + ", ";
                result += r["SOUHU"] + ", ";
                result += r["CYUDT"] + ", ";
                result += r["BCODE"] + ", ";
                result += r["ZUBAN"] + ", ";
                result += r["SNAME"] + ", ";
                result += r["KISYU"] + ", ";
                result += r["TANI"] + ", ";
                result += r["TANKA"] + ", ";
                result += r["CYUSU"] + ", ";
                result += r["NOUKI"] + ", ";
                result += r["HKBN"] + ", ";
                result += r["SEIZO"] + ", ";
                result += r["HINSYU"] + ", ";
                result += r["NKBN"] + ", ";
                result += r["TOKCD"] + ", ";
                result += r["NOUCD"] + ", ";
                result += r["SFLG"] + "\n";
                result += "</span>";
            }

            r.Close();
            cn.Close();

            return result;

            /*
            string result = "";
            //string filePath = @"\\10.121.21.16\Rainbow";
            string filePath = @"C:\Rainbow";

            string query = string.Empty;
            
            query = "SELECT * FROM " + fileName + " WHERE SFLG=''";

            // 32-bit
            // OdbcConnection conn = new OdbcConnection("Driver=Microsoft Text Driver (*.txt, *.csv);Dbq=" + filePath + ";Extensions=csv;");
            // 64-bit
            OdbcConnection conn = new OdbcConnection("Driver=Microsoft Access Text Driver (*.txt, *.csv);Dbq=" + filePath + ";Extensions=csv;");

            conn.Open();
            OdbcCommand cmd = new OdbcCommand(query, conn);
            OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
            DataSet mydata = new DataSet("CSVData");
            adapter.Fill(mydata);
            conn.Close();
            conn.Dispose();

            var a = mydata.Tables[0].Rows.Count;

            return result;
            */
        }

        public bool checkExistTokuiN(string tokuiN)
        {
            return Models.MasterModels.checkExistTokuiN(tokuiN);
        }
        
        public bool checkExistZaiCD(string zaicd, string kubun)
        {
            return Models.MasterModels.checkExistZaiCD(zaicd, kubun);
        }

        public bool checkExistZaiCDByName(string name)
        {
            return Models.MasterModels.checkExistZaiCDByName(name);
        }

        public bool checkExistTantouCD(string tantouCD)
        {
            return Models.MasterModels.checkExistTantouCD(tantouCD);
        }

        public bool checkExistNounyuN(string nounyuN, string tokuiC)
        {
            return Models.MasterModels.checkExistTokuiN(nounyuN, tokuiC);
        }

    }
}