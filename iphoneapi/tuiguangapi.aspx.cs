using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.IO;
using System.Net;
using System.Web.Security;

public partial class iphoneapi_tuiguangapi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        string guid = utils.NewRequest("guid", utils.RequestType.QueryString);

        if (act == "add")
        {
            AddAdsInfo("");
        }
        else if (act == "edit")
        {
            EditAdsInfo(guid);
        }
        else if (act == "del")
        {
            DeleteAdsInfo(guid);
        }
    }

    private void DeleteAdsInfo(string guid)
    {
        string sql = string.Empty;

        if (guid.IndexOf("ads-") != -1)
        {
            guid = guid.Replace("ads-", "");
            sql = "DELETE FROM TeteShopAds WHERE guid = '" + guid + "'";
        }
        else
        {
            sql = "DELETE FROM TeteShopItem WHERE newid = '" + guid + "'";
        }

        utils.ExecuteNonQuery(sql);
    }

    private void EditAdsInfo(string guid)
    {
        DeleteAdsInfo(guid);

        AddAdsInfo(guid);
    }

    /// <summary>
    /// 添加广告
    /// </summary>
    private void AddAdsInfo(string guid)
    {
        string uid = utils.NewRequest("uid", utils.RequestType.QueryString);
        string area = utils.NewRequest("area", utils.RequestType.QueryString);
        string typ = utils.NewRequest("typ", utils.RequestType.QueryString);
        string imgurl = utils.NewRequest("imgurl", utils.RequestType.QueryString);
        string linkurl = utils.NewRequest("linkurl", utils.RequestType.QueryString);
        string price = utils.NewRequest("price", utils.RequestType.QueryString);
        string name = utils.NewRequest("name", utils.RequestType.QueryString);

        string sql = string.Empty;

        if (guid == "")
            guid = Guid.NewGuid().ToString();

        //大广告
        if (typ == "0")
        {
            string adstyp = string.Empty;

            if (area == "0")
            {
                adstyp = "index";
            }
            else
            {
                adstyp = (int.Parse(area) - 1).ToString();
            }

            sql = "INSERT INTO [TeteShopAds] (guid, nick, logo, url, typ) VALUES ('" + guid + "','" + uid + "','" + imgurl + "','" + linkurl + "','" + adstyp + "')";
            utils.ExecuteNonQuery(sql);

            Response.Write("ads-" + guid);
            Response.End();
        }
        else
        {
            //新品
            string isnew = "0";
            string ishot = "0";
            string isindex = "0";
            string categoryid = string.Empty;

            if (area == "6")
            {
                isnew = "1";
                categoryid = "5";
            }
            else if (area == "7")
            {
                ishot = "1";
                categoryid = "5";
            }
            else
            {
                if (typ == "1")
                {
                    isindex = "1";
                    if (area == "0")
                    {
                        categoryid = "5";
                    }
                    else
                    {
                        categoryid = (int.Parse(area) - 1).ToString();
                    }
                }
                else
                {
                    categoryid = "5";
                }
            }

            string fileName = Server.MapPath("api/tmpimg/" + strMD5(imgurl + "_100x100.jpg"));
            //保存临时图片获取图片尺寸
            if (!File.Exists(fileName))
            {
                WebClient c = new WebClient();
                c.DownloadFile(imgurl + "_100x100.jpg", fileName);
            }
            System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);

            sql = "INSERT INTO TeteShopItem (newid, cateid, picurl, linkurl, isnew, ishot, price, nick, width, height, isindex, itemname) VALUES ('" + guid + "','" + categoryid + "','" + imgurl + "','" + linkurl + "','" + isnew + "','" + ishot + "','" + price + "','" + uid + "','" + img.Width + "','" + img.Height + "','" + isindex + "','" + name + "')";
            utils.ExecuteNonQuery(sql);

            Response.Write(guid);
            Response.End();
        }
    }

    public static string strMD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }
}