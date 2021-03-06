﻿using System;
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
using System.Collections.Generic;

public partial class ipclick : System.Web.UI.Page
{

    UserAdsService uasDal = new UserAdsService();
    ClickService clickDal = new ClickService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<AdsInfo> adsList = CacheCollection.GetAllAdsInfo();

            Random rand = new Random();
            AdsInfo info = adsList[rand.Next(adsList.Count)];

            IList<UserAdsInfo> list = uasDal.SelectAllUserAdsByAdsId(info.AdsId, 1);

            IList<ClickInfo> clickList = clickDal.SelectAllClickCount(DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"));

            //获取访问IP
            string ip = Request.ServerVariables["REMOTE_ADDR"];
            //获取当天已经访问来了的IP
            IList<ClickIPInfo> ipList = clickDal.SelectAllClickIPByDate(DateTime.Now.ToString("yyyyMMdd"));
            //和当前IP相同的
            ipList = ipList.Where(o => o.VisitIP == ip).ToList();

            foreach (ClickIPInfo ipinfo in ipList)
            {
                //已经访问了该广告
                IList<UserAdsInfo> hadlist = list.Where(o => o.Id == ipinfo.UserAdsId).ToList();
                if (hadlist.Count > 0)
                {
                    list.Remove(hadlist[0]);
                }
            }

            if (list.Count == 0)
                return;

            foreach (ClickInfo cinfo in clickList)
            {

                if (cinfo.ClickCount >= rand.Next(6,15))//10)
                {
                    IList<UserAdsInfo> hadlist = list.Where(o => o.Id == cinfo.UserAdsId).ToList();

                    if (hadlist.Count > 0)
                    {
                        FeeInfo finfo = CacheCollection.GetAllFeeInfo().Where(o => o.FeeId == hadlist[0].FeeId).ToList()[0];
                        if (finfo.AdsType == 1)
                        {
                            if (finfo.AdsCount == 1)
                                list.Remove(hadlist[0]);
                            else
                            {
                                if (cinfo.ClickCount >= rand.Next(14, 23))//18)
                                    list.Remove(hadlist[0]);
                            }
                        }
                        if (finfo.AdsType == 5)
                        {
                            if (cinfo.ClickCount >= rand.Next(22, 33)) //27)
                                list.Remove(hadlist[0]);
                        }
                    }
                }
            }

            if (list.Count == 0)
                return;

            UserAdsInfo uinfo = list[0];
            if (list.Count > 1)
                uinfo = list[rand.Next(list.Count)];

            ClickIPInfo iinfo = new ClickIPInfo();
            iinfo.VisitIP = ip;
            iinfo.ClickId = Guid.NewGuid();
            iinfo.VisitDate = DateTime.Now.ToString("yyyyMMdd");
            iinfo.UserAdsId = uinfo.Id;
            clickDal.InsertClickIP(iinfo);

            string param = "id=" + uinfo.Id + "&url=" + uinfo.AdsUrl;
            Response.Redirect("getclick.aspx?" + HttpUtility.UrlEncode(param));
        }
    }
}
