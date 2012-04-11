using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Qijia.PCI
{
    public class MethodPCI
    {
        private static MethodGroup mg = new MethodGroup();

        /// <summary>
        ///  对外发布的服务方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public object GetYouWant(string str)
        {
            Type type = mg.GetType();

            PasswordParam pp = new PasswordParam();
            string decrptStr =  pp.Decrypt3DES(str);

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

            foreach (string p in paramsNameValue)
            {
                string[] pnamevalue = p.Split('=');
                if (pnamevalue.Length != 2)
                    return "";

                if (pnamevalue[0].ToLower() == "method")
                {
                    method = pnamevalue[1];
                    continue;
                }

                paramDic.Add(pnamevalue[0], pnamevalue[1]);
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

            object obj = methodinfo.Invoke(mg, paramsList.ToArray());
            return obj;
        }
    }
}
 