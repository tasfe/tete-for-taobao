using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;

public partial class top_groupbuy_activitytaobaoItem : System.Web.UI.Page
{
    public string teteendDate = string.Empty;
    public string nick = string.Empty;
    public string name = "";
    public string memo = "";
    public string startDate = "";
    public string endDate = "";
    public string itemType = "";
    public string discountType = "";
    public string zhe = "";
    public string yuan = "";
    public string decreaseNum = "";
    public string rcount = "";
    public string tagId = "";
    public string status = "";
    public string itemTypeStr = "";
    public string discountTypeStr = "";
    public string decreaseNumStr = "";
    public string discountValue = "";
    public string activityenddatestr = string.Empty;
    public string activitystatrdatestr = string.Empty;
    public string activitystatusstr = string.Empty;
    public string activityitemTypestr = string.Empty;
    public string activitydiscountTypestr = string.Empty;
    public string activitydiscountValuestr = string.Empty;
    public string activitydecreaseNumstr = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");

        if (Request.QueryString["activityID"] == null)
        {
            Response.Redirect("activityList.aspx");
            return;
        }
        string activityID = Request.QueryString["activityID"].ToString();
        string sql = "SELECT [ID],[Name] ,[Description] ,[Remark] ,[startDate] ,[endDate] ,[itemType] ,[discountType] ,[discountValue] ,[tagId] ,[Rcount] ,[Nick] ,[Status] ,[decreaseNum] ,[isOK]  FROM  [tete_activity] where ID=" + activityID;
        DataTable dt3 = utils.ExecuteDataTable(sql);
        if (dt3 != null && dt3.Rows.Count>0)
        {
            activityenddatestr = dt3.Rows[0]["endDate"].ToString();
            activitystatrdatestr = dt3.Rows[0]["startDate"].ToString();
            activitystatusstr = dt3.Rows[0]["Status"].ToString();
            if (activitystatusstr == "0")
            {
                activitystatusstr = "活动未开始";
            }
            else if (activitystatusstr == "1")
            {
                activitystatusstr = "活动进行中";
            }
            else if (activitystatusstr == "2")
            {
                activitystatusstr = "活动已结束";
            }
            else if (activitystatusstr == "3")
            {
                activitystatusstr = "活动已暂停";
            }
            else
            {
                activitystatusstr = "活动已删除";
            }
            activityitemTypestr = dt3.Rows[0]["itemType"].ToString();
            if (activityitemTypestr == "same")
            {
                activitydiscountTypestr = dt3.Rows[0]["discountType"].ToString();
                activitydiscountValuestr = dt3.Rows[0]["discountValue"].ToString();
                activitydecreaseNumstr = dt3.Rows[0]["decreaseNum"].ToString();

                if (activitydiscountTypestr.Trim() == "DISCOUNT")
                {
                    activitydiscountTypestr = "打折";
                    activitydiscountValuestr += "折";
                    activitydecreaseNumstr = "买家拍下的多件商品均享受优惠";
                }
                else
                {
                    activitydiscountTypestr = "减价";
                    activitydiscountValuestr += "元";
                    if (activitydecreaseNumstr == "0")
                    {
                        activitydecreaseNumstr = "买家拍下的多件商品均享受优惠";
                    }
                    else
                    {
                        activitydecreaseNumstr = "只对买家拍下的第一件商品优惠";
                    }
                }

            }
            else
            {
 
                activitydiscountTypestr = "每个参加活动的宝贝设置不同促销力度 ";
                activitydiscountValuestr = "每个参加活动的宝贝设置不同促销力度 ";
                activitydecreaseNumstr = "每个参加活动的宝贝设置不同促销力度 ";
            }
           
        }
        //Status 活动状态(0:未开始，1:进行中，2：已结束，3已暂停，4已删除)

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12159997'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

          sql = "SELECT * FROM TopTaobaoShopCat WHERE nick = '" + taobaoNick + "'";

        DataTable dt = utils.ExecuteDataTable(sql);

        Repeater1.DataSource = dt;
        Repeater1.DataBind();
    }
}