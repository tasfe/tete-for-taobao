using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;

namespace Server
{
    public partial class SendSMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SendSMSMethod();
        }

        private void SendSMSMethod()
        {

            string param = "username=" + this.username.Value + "&password=" + this.password.Value + "&method=" + this.method.Value + "&mobile=" + this.mobile.Value + "&msg=" + this.msg.Value;
            byte[] bs = Encoding.ASCII.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms2.eachwe.com/api.php");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
                {
                    string content = reader.ReadToEnd();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);
                    XmlElement rootElment = doc.DocumentElement;
                    if (rootElment != null)
                    {
                        XmlNode errorXmlNode = rootElment.ChildNodes[0];
                        XmlNode messageXmlNode = rootElment.ChildNodes[1];
                        if (errorXmlNode != null && !string.IsNullOrEmpty(errorXmlNode.InnerText) && errorXmlNode.InnerText == "0") 
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenWindow", "<script type='text/javascript'>alert('发送成功！');</script>"); 
                        }

                        if (errorXmlNode != null && messageXmlNode != null && !string.IsNullOrEmpty(errorXmlNode.InnerText) && errorXmlNode.InnerText !="0")
                        {
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "OpenWindow", "<script type='text/javascript'>alert('"+messageXmlNode.InnerText+"');</script>");
                        } 
                    }
                }
            }
        }
    }
}
