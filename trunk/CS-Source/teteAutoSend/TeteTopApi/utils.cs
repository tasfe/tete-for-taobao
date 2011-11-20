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
