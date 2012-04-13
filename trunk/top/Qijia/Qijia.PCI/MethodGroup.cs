using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using Qijia.DAL;
using Qijia.Model;

namespace Qijia.PCI
{
    public class MethodGroup
    {

        Jia_ShopService ssDal = new Jia_ShopService();

        Jia_ItemService itemDal = new Jia_ItemService();
        Jia_TemplateService tempDal = new Jia_TemplateService();

        public string Test(string name, string password, string datatype)
        {
            return name + password;
        }

        public string checkexpire(string uId, string dataType)
        {
            Jia_Shop shop = ssDal.GetJia_ShopByNick(uId);

            ResponseMsg pmsg = new ResponseMsg();
            msg msg = new msg();

            if (shop == null)
            {
                pmsg.result = "fail";
                msg.content = "该用户尚未订购模版";
            }
            else
            {
                pmsg.result = "success";
                //if (shop.IsExpired == 1)
                if (shop.ExpireDate <= DateTime.Now)
                    msg.content = "0";
                else
                    msg.content = "1";
            }
           
            List<msg> list = new List<msg>();
            list.Add(msg);

            if (dataType == "json")
            {
                return GetJsonStr(list, pmsg);
            }

            ResponseXMLMsg xmlmsg = new ResponseXMLMsg();
            xmlmsg.result = pmsg.result;

            return GetXMLStr(list, xmlmsg);

        }

        public string updategoods(string goodsId, string tempType, string dataType)
        {
            Jia_Item item = itemDal.GetJia_ItemById(goodsId);

            ResponseMsg pmsg = new ResponseMsg();
            msg msg = new msg();

            if (item == null)
            {
                pmsg.result = "fail";
                msg.content = "该商品未使用模板";
            }
            else
            {
                pmsg.result = "success";
                Jia_Template temp = tempDal.GetJia_TemplateById(item.TplId);
                if (temp == null)
                {
                    pmsg.result = "fail";
                    msg.content = "未找到模板";
                }
                else
                {
                    pmsg.result = "success";
                    msg.content = GetRealItemInfo(item, temp, tempType);
                }
            }
            List<msg> list = new List<msg>();
            list.Add(msg);
            if (dataType == "json")
            {
                return GetJsonStr(list, pmsg);
            }

            ResponseXMLMsg xmlmsg = new ResponseXMLMsg();
            xmlmsg.result = pmsg.result;

            return GetXMLStr(list, xmlmsg);
        }

        private static string GetRealItemInfo(Jia_Item item, Jia_Template temp, string type)
        {
            Jia_ImgService imgDal = new Jia_ImgService();
            Jia_ImgCustomerService cimgDal = new Jia_ImgCustomerService();
            string tempHtml = "";
            if (type == "1")
                tempHtml = temp.TplHtml;
            if (type == "0")
                tempHtml = temp.UglyTplHtml;
            IList<Jia_Img> imgList = imgDal.GetAllJia_Img(temp.TplId);
            IList<Jia_ImgCustomer> cimgList = cimgDal.GetAllJia_ImgCustomer(item.ItemId);

            //替换图片
            foreach (Jia_Img jimg in imgList)
            {
                tempHtml = tempHtml.Replace(jimg.Tag, jimg.JiaImg);
            }

            foreach (Jia_ImgCustomer jcimg in cimgList)
            {
                tempHtml = tempHtml.Replace(jcimg.Tag, jcimg.JiaImg);
            }

            //替换chartext
            string chartext = item.CharText.Substring(1, item.CharText.Length - 2); //剔除{}
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string[] chars = Regex.Split(chartext, "{,}");
            foreach (string s in chars)
            {
                dic.Add(s.Substring(0, s.IndexOf(':')), s.Substring(s.IndexOf(':') + 1, s.Length - s.IndexOf(':') - 1));
            }

            foreach (KeyValuePair<string, string> kvp in dic)
            {
                tempHtml = tempHtml.Replace("{" + kvp.Key + "}", kvp.Value);
            }

            //替换PropertyText
            string propertyText = item.PropertyText.Substring(1, item.PropertyText.Length - 2); //剔除{}
            string loop = tempHtml.Substring(tempHtml.IndexOf("{loop}") + 6, tempHtml.IndexOf("{/loop}") - tempHtml.IndexOf("{loop}") - 6);

            chars = Regex.Split(propertyText, "{,}");
            string realpropertyText = "";

            foreach (string s in chars)
            {
                realpropertyText += loop.Replace("{left}", s.Substring(0, s.IndexOf(':'))).Replace("{right}", s.Substring(s.IndexOf(':') + 1, s.Length - s.IndexOf(':') - 1));
            }

            Regex regex = new Regex("{loop}.*?{/loop}");

            tempHtml = regex.Replace(tempHtml, realpropertyText);

            return tempHtml;
        }

        private static string GetJsonStr(List<msg> list, ResponseMsg pmsg)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("msg", list);
            pmsg.msgs = dic;
            string json = JSONHelper.ObjectToJSON(pmsg);
            json = json.Replace("\\u003c", "<");
            json = json.Replace("\\u003e", ">");
            json = json.Replace("\\t", "");
            json = "{\"qijia_response\":" + json + "}";
            return json;
        }

        private static string GetXMLStr(List<msg> list, ResponseXMLMsg xmlmsg)
        {
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
