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

        public string UpdateGoods(string goodsId, int tempType, string dataType)
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

        private static string GetJsonStr(List<msg> list, ResponseMsg pmsg)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("msg", list);
            pmsg.msgs = dic;
            string json = JSONHelper.ObjectToJSON(pmsg);
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
