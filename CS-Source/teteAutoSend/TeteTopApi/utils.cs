using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using TeteTopApi.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using QWeiboSDK;
using System.IO;
using System.Web.Security;
using System.Net;

namespace TeteTopApi
{
    public class utils
    {
        /// <summary>
        /// 获取通知类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMsgType(string str)
        {
            Regex reg = new Regex(@"""msg"":{""([^""]*)"":", RegexOptions.IgnoreCase);
            if (reg.IsMatch(str))
            {
                return reg.Match(str).Groups[1].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 将JSON转换为Trade对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Trade GetTrade(string str)
        {
            Trade trade = new Trade();

            trade.BuyNick = GetValueByProperty(str, "buyer_nick");
            trade.Modified = GetValueByProperty(str, "modified");
            trade.Nick = GetValueByProperty(str, "seller_nick");
            trade.Oid = GetValueByPropertyNum(str, "oid");
            trade.Payment = GetValueByProperty(str, "payment");
            trade.Status = GetValueByProperty(str, "status");
            trade.Tid = GetValueByPropertyNum(str, "tid");

            return trade;
        }

        /// <summary>
        /// 发送微博消息
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="content"></param>
        /// <param name="filepath"></param>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        public void SendMicroBlog(string nick, string content, string filepath, string key, string secret)
        {
            try
            {
                string appKey = "d3225497956249cbb13a7cb7375d62bd";
                string appSecret = "6cf7a3274cb676328e77dff3e203061d";

                //发送微博
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter("content", content));

                //身份验证
                OauthKey oauthKey = new OauthKey();
                oauthKey.customKey = appKey;
                oauthKey.customSecrect = appSecret;
                oauthKey.tokenKey = key;
                oauthKey.tokenSecrect = secret;

                //图片信息
                List<Parameter> files = new List<Parameter>();
                if (filepath != "")
                {
                    files.Add(new Parameter("pic", DownPic(filepath)));
                    //files.Add(new Parameter("pic", filepath));
                }

                Console.Write("send weibo msg...[" + filepath + "]-[" + content + "]\r\n");

                QWeiboRequest request = new QWeiboRequest();
                int nKey = 0;
                if (request.AsyncRequest("http://open.t.qq.com/api/t/add_pic", "POST", oauthKey, parameters, files, new AsyncRequestCallback(RequestCallback), out nKey))
                {

                }
            }
            catch (Exception e)
            {
            }
        }

        private string DownPic(string url)
        {
            string strHtml = string.Empty;

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream reader = response.GetResponseStream();

            string time = DateTime.Now.ToString("yyyy-MM-dd");

            //创建文件夹
            if (!Directory.Exists("pic/" + time + "/"))
            {
                Directory.CreateDirectory("pic/" + time + "/");
            }

            string fileName = "pic/" + time + "/" + MD5(url) + ".jpg";

            if (!File.Exists(fileName))
            {
                FileStream writer = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[512];
                int c = 0; //实际读取的字节数
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                writer.Close();
            }

            return fileName;
        }

        public static string MD5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        /// <summary>
        /// 发送微博的回调函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        protected void RequestCallback(int key, string content)
        {
            Encoding utf8 = Encoding.GetEncoding(65001);
            Encoding defaultChars = Encoding.Default;
            byte[] temp = utf8.GetBytes(content);
            byte[] temp1 = Encoding.Convert(utf8, defaultChars, temp);
            string result = defaultChars.GetString(temp1);

            //更新微博发送记录和写入发送日志
            Console.Write(result + "\r\n");
        }

        /// <summary>
        /// 将JSON转换为Item对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Item GetItem(string str)
        {
            Item item = new Item();

            item.ID = GetValueByPropertyNum(str, "num_iid");

            return item;
        }

        /// <summary>
        /// 根据属性名获取JSON中的属性值（字符）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetValueByProperty(string str, string prop)
        {
            Regex reg = new Regex(@"""" + prop + @""":""([^""]*)""", RegexOptions.IgnoreCase);
            if (reg.IsMatch(str))
            {
                return reg.Match(str).Groups[1].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据属性名获取JSON中的属性值（字符）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetValueByProperty(string str, string prop, int index)
        {
            Regex reg = new Regex(@"""" + prop + @""":""([^""]*)""", RegexOptions.IgnoreCase);
            if (reg.IsMatch(str))
            {
                return reg.Matches(str)[index].Groups[1].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据属性名获取JSON中的属性值（数字）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetValueByPropertyNum(string str, string prop)
        {
            Regex reg = new Regex(@"""" + prop + @""":([^"",}]*)", RegexOptions.IgnoreCase);
            if (reg.IsMatch(str))
            {
                return reg.Match(str).Groups[1].ToString();
            }
            else
            {
                return "";
            }
        }



        /// <summary>
        /// 获取订单物流状态中的签收日期(此处需要XML的数据格式)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string GetShippingEndTime(string str)
        {
            Regex reg = new Regex(@"<status_time>([^<]*)</status_time></transit_step_info></trace_list></logistics_trace_search_response>", RegexOptions.IgnoreCase);
            if (reg.IsMatch(str))
            {
                string time = reg.Match(str).Groups[1].ToString();
                if (time == "")
                {
                    //<status_time>2011-11-15 18:20:52</status_time></transit_step_info><transit_step_info><status_time></status_time></transit_step_info></trace_list></logistics_trace_search_response>
                    Regex regNew = new Regex(@"<status_time>([^<]*)</status_time></transit_step_info><transit_step_info><status_time></status_time></transit_step_info></trace_list></logistics_trace_search_response>", RegexOptions.IgnoreCase);
                    time = regNew.Match(str).Groups[1].ToString();
                    return time;
                }
                else
                {
                    return time;
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 执行SQL语句返回DataTable
        /// </summary>
        /// <param name="dbstring">SQL语句</param>
        /// <returns>返回结果的DataTable</returns>
        public static DataTable ExecuteDataTable(string dbstring)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(dbstring);
            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
            }
            catch (Exception e)
            {
                db = null;
            }

            return dt;
        }

        /// <summary>
        /// 执行无返回的SQL语句
        /// </summary>
        /// <param name="dbstring">SQL语句</param>
        /// <returns>是否成功，0失败，1成功</returns>
        public static int ExecuteNonQuery(string dbstring)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(dbstring);

            try
            {
                return db.ExecuteNonQuery(dbCommand);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="dbstring"></param>
        /// <returns></returns>
        public static string ExecuteString(string dbstring)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(dbstring);
            DataTable dt = null;
            try
            {
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
            }
            catch (Exception e)
            {

            }

            if (dt.Rows.Count == 0)
                return "";
            else
                return dt.Rows[0][0].ToString();
        }
    }
}
