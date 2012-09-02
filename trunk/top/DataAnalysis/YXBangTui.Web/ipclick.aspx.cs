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

            //AdsInfo info = adsList[rand.Next(adsList.Count)];
            //IList<UserAdsInfo> list = uasDal.SelectAllUserAdsByAdsId(info.AdsId, 1);

            IList<UserAdsInfo> list = uasDal.SelectAllUserAdsClick(DateTime.Now);

            //获取访问IP
            string ip = Request.ServerVariables["REMOTE_ADDR"];
            //if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 9)
            //{
            //    IList<ClickInfo> clickList = clickDal.SelectAllClickCount(DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"));

            //    //获取当天已经访问来了的IP
            //    IList<ClickIPInfo> ipList = clickDal.SelectAllClickIPByDate(DateTime.Now.ToString("yyyyMMdd"));
            //    //和当前IP相同的
            //    ipList = ipList.Where(o => o.VisitIP == ip).ToList();

            //    for (int i = 0; i < ipList.Count; i++)
            //    {
            //        //已经访问了该广告
            //        IList<UserAdsInfo> hadlist = list.Where(o => o.Id == ipList[i].UserAdsId).ToList();
            //        if (hadlist.Count > 0)
            //        {
            //            list.Remove(hadlist[0]);
            //        }
            //    }
            //}

            IList<Guid> clickIPlist = uasDal.SelectAllClickIP(DateTime.Now, ip);
            foreach (Guid gid in clickIPlist)
            {
                IList<UserAdsInfo> hadlist = list.Where(o => o.Id == gid).ToList();
                if (hadlist.Count > 0)
                {
                    list.Remove(hadlist[0]);
                }
            }

            if (list.Count == 0)
                return;

            int suiCount = 0;

            if ((int)DateTime.Now.DayOfWeek == 1)
                suiCount = rand.Next(3, 6);

            if ((int)DateTime.Now.DayOfWeek == 2)
                suiCount = rand.Next(6, 8);

            if ((int)DateTime.Now.DayOfWeek == 3)
                suiCount = rand.Next(8, 10);

            if ((int)DateTime.Now.DayOfWeek == 4)
                suiCount = rand.Next(10, 12);

            if ((int)DateTime.Now.DayOfWeek == 5)
                suiCount = rand.Next(8, 10);

            if ((int)DateTime.Now.DayOfWeek == 6)
                suiCount = rand.Next(6, 8);

            if ((int)DateTime.Now.DayOfWeek == 0)
                suiCount = rand.Next(1, 3);

            if (list.Where(o => o.IsSend < (10 + suiCount)).ToList().Count > 0)
                list = list.Where(o => o.IsSend < (10 + suiCount)).ToList();

            //for (int i = 0; i < clickList.Count; i++)
            //{

            //    if (clickList[i].ClickCount >= num1)//10)
            //    {
            //        IList<UserAdsInfo> hadlist = list.Where(o => o.Id == clickList[i].UserAdsId).ToList();

            //        if (hadlist.Count > 0)
            //        {
            //            FeeInfo finfo = CacheCollection.GetAllFeeInfo().Where(o => o.FeeId == hadlist[0].FeeId).ToList()[0];
            //            if (finfo.AdsType == 1)
            //            {
            //                if (finfo.AdsCount == 1)
            //                    list.Remove(hadlist[0]);
            //                else
            //                {
            //                    if (clickList[i].ClickCount >= num2)//18)
            //                        list.Remove(hadlist[0]);
            //                }
            //            }
            //            if (finfo.AdsType == 5)
            //            {
            //                if (clickList[i].ClickCount >= num3) //27)
            //                    list.Remove(hadlist[0]);
            //            }
            //        }
            //    }
            //}

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
