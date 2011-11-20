using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;

namespace TestApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Url = "http://gw.api.taobao.com/router/rest";
            string AppKey = "12159997";
            string Secret = "614e40bfdb96e9063031d1a9e56fbed5";
            string method = "taobao.trade.fullinfo.get";
            string session = "610192900fa27231672293aa11839e534747b1c9c40c096159209052";

            string start = "2011-11-17 11:12:41.000";
            string end = "2011-11-17 11:15:19.000";

            this.textBox3.Text = (DateTime.Parse(end) - DateTime.Parse(start)).Seconds.ToString();

            //IDictionary<string, string> param = new Dictionary<string, string>();
            ////param.Add("fields", "delivery_start,delivery_end,status");
            //////param.Add("tid", "111497637831694");
            ////param.Add("seller_nick", "yimengqifei");
            ////param.Add("tid", "111497637831694");
            //param.Add("fields", "receiver_mobile, orders.num_iid, created, consign_time");
            //param.Add("tid", "111514079838859");

            //string result = CommonPost(Url, AppKey, Secret, method, session, param);

            //this.textBox3.Text = result;
        }

        /// <summary> 
        /// TOP API POST 请求 
        /// </summary> 
        /// <param name="url">请求容器URL</param> 
        /// <param name="appkey">AppKey</param> 
        /// <param name="appSecret">AppSecret</param> 
        /// <param name="method">API接口方法名</param> 
        /// <param name="session">调用私有的sessionkey</param> 
        /// <param name="param">请求参数</param> 
        /// <returns>返回字符串</returns> 
        public static string CommonPost(string url, string appkey, string appSecret, string method, string session, IDictionary<string, string> param)
        {
            try
            {
                #region -----API系统参数----
                param.Add("app_key", appkey);
                param.Add("method", method);
                param.Add("session", session);
                param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                param.Add("format", "json");
                param.Add("v", "2.0");
                param.Add("sign_method", "md5");
                param.Add("sign", CreateSign(param, appSecret));
                #endregion
                string result = string.Empty;
                #region ---- 完成 HTTP POST 请求----
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.KeepAlive = true;
                req.Timeout = 300000;
                req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
                byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                Stream stream = null;
                StreamReader reader = null;
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                result = reader.ReadToEnd();
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
                #endregion
                return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
            }
            catch (Exception e)
            {
                return e.StackTrace;
            }
        }


        protected static string CreateSign(IDictionary<string, string> parameters, string secret)
        {
            parameters.Remove("sign");
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            query.Append(secret);
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }
            return result.ToString();
        }
        /// <summary> 
        /// 组装普通文本请求参数。 
        /// </summary> 
        /// <param name="parameters">Key-Value形式请求参数字典</param> 
        /// <returns>URL编码后的请求数据</returns> 
        protected static string PostData(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数 
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
    }
}
