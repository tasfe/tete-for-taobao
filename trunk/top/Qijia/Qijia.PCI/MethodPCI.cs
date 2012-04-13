using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using Qijia.DAL;
using Qijia.Model;

namespace Qijia.PCI
{
    public class MethodPCI
    {
        private static MethodGroup mg = new MethodGroup();

        private static Jia_ApiSucLogService slogDal = new Jia_ApiSucLogService();
        private static Jia_ApiFailLogService flogDal = new Jia_ApiFailLogService();

        /// <summary>
        ///  对外发布的服务方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public object GetYouWant(string str)
        {
            Type type = mg.GetType();

            PasswordParam pp = new PasswordParam();
            string decrptStr = pp.Decrypt3DES(str);

            //最后会多出一个\0
            decrptStr = decrptStr.Replace("\0", "");

            //判断合法
            if (!decrptStr.Contains("sign="))
            {
                return "未传入sign";
            }

            string sign = decrptStr.Substring(decrptStr.IndexOf("sign=") + 5, decrptStr.Length - (decrptStr.IndexOf("sign=") + 5));

            if (pp.Encrypt3DES(decrptStr.Substring(0, decrptStr.IndexOf("sign=")-1)) != sign)
            {
                return "不合法";
            }

            decrptStr = decrptStr.Replace("&sign=" + sign, "");

            string[] paramsNameValue = decrptStr.Split('&');
            if (paramsNameValue.Length == 0 || paramsNameValue[0] == "")
                return "";

            Dictionary<string, string> paramDic = new Dictionary<string, string>();
            
            string method = "";

            string datatype = "";

            foreach (string p in paramsNameValue)
            {
                string[] pnamevalue = p.Split('=');
                if (pnamevalue.Length != 2)
                    return "";

                if (pnamevalue[0].ToLower() == "method")
                {
                    method = pnamevalue[1].ToLower();
                    continue;
                }

                if(pnamevalue[0].ToLower() == "datatype")
                {
                    datatype = pnamevalue[1];
                }

                paramDic.Add(pnamevalue[0], pnamevalue[1]);
            }

            if (method == "")
            {
                return "未传入方法名";
            }

            MethodInfo methodinfo = type.GetMethod(method);
            ParameterInfo[] pinfos = methodinfo.GetParameters();

            List<string> paramsList = new List<string>();
            foreach (ParameterInfo pinfo in pinfos)
            {
                foreach (KeyValuePair<string, string> kvp in paramDic)
                {
                    if (pinfo.Name.ToLower() == kvp.Key.ToLower())
                    {
                        paramsList.Add(kvp.Value);
                        break;
                    }
                }
            }
            bool isdoit = false;
            string errinfo = "";
            try
            {
                object obj = methodinfo.Invoke(mg, paramsList.ToArray());
                isdoit = true;
                return obj;
            }
            catch (Exception ex)
            {
                errinfo = ex.Message;
                if (datatype.ToLower() == "json")
                {
                    return GetJsonErr(ex.Message);
                }

                return GetXmlErr(ex.Message);
            }

            finally
            {
                if (isdoit)
                {
                    Jia_ApiSucLog slog = new Jia_ApiSucLog { ActDate = DateTime.Now, ApiName = method, Data = str, Guid = Guid.NewGuid().ToString() };
                    slogDal.AddJia_ApiSucLog(slog);
                }
                else
                {
                    Jia_ApiFailLog flog = new Jia_ApiFailLog { ActDate = DateTime.Now, ApiName = method, Data = str, Guid = Guid.NewGuid().ToString(), ErrInfo = errinfo };
                    flogDal.AddJia_ApiFailLog(flog);
                }
            }
        }

        public string GetJsonErr(string errMsg)
        {
            ResponseMsg pmsg = new ResponseMsg();
            msg msg = new msg();
            pmsg.result = "fail";
            msg.content = errMsg;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<msg> list = new List<msg>();
            list.Add(msg);
            dic.Add("msg", list);
            pmsg.msgs = dic;
            string json = JSONHelper.ObjectToJSON(pmsg);
            json = "{\"qijia_response\":" + json + "}";
            return json;
        }

        public string GetXmlErr(string errMsg)
        {
            ResponseXMLMsg xmlmsg = new ResponseXMLMsg();
            xmlmsg.result = "fail";
            msg msg = new msg();
            msg.content = errMsg;
            List<msg> list = new List<msg>();
            list.Add(msg);
            xmlmsg.msgs = list;

            XmlSerializer xs = new XmlSerializer(typeof(ResponseXMLMsg));

            StringBuilder sb = new StringBuilder();
            TextWriter stringWriter = new StringWriter(sb);

            xs.Serialize(stringWriter, xmlmsg);
            string xmltext = Regex.Replace(sb.ToString(), @"ResponseXMLMsg[^\>]*>", "qijia_response>");
            xmltext = xmltext.Replace("utf-16", "utf-8");

            return xmltext;

        }
    }
}
 