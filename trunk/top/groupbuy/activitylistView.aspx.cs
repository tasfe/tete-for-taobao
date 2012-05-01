using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_groupbuy_activitylistView : System.Web.UI.Page
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
    public string imgStr = "";
    public string shopgroupbuyEnddate2 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        Rijndael_ encode = new Rijndael_("tetesoft");
        string sql23 = "select enddate from TopTaobaoShop where nick='" + nick + "'";
        DataTable dt32 = utils.ExecuteDataTable(sql23);

        if (dt32 != null && dt32.Rows.Count > 0)
        {
            shopgroupbuyEnddate2 = dt32.Rows[0]["enddate"].ToString();
            teteendDate = dt32.Rows[0]["enddate"].ToString();
        }
        nick = encode.Decrypt(taobaoNick);
        if (nick == "")
        {
            //Response.Write("top签名验证不通过，请不要非法注入");
            //Response.End();
            //return;
        }
        if (Request.QueryString["act"] == "post")
        {
            string activityID = Request.QueryString["activityID"].ToString();
            string ID = Request.QueryString["ID"].ToString();

            startDate = Request.Form["startDate"].ToString();
            endDate = Request.Form["endDate"].ToString();
            itemType = Request.Form["itemType"].ToString();
            discountType = Request.Form["discountType"].ToString();
            zhe = Request.Form["zhe"].ToString();
            yuan = Request.Form["yuan"].ToString();
            decreaseNum = Request.Form["decreaseNum"].ToString();
            rcount = Request.Form["Rcount"].ToString();
            tagId = "1";
            status = "1";//进行中 
            #region  数据格式验证
            if (DateTime.Parse(startDate) > DateTime.Now)
            {
                status = "0";//未开始
            }

            if (DateTime.Parse(shopgroupbuyEnddate2) < DateTime.Now)
            {
                Response.Write("<script>alert('活动结束时间不能大于服务使用结束时间！')</script>");
                return;
            }
 
            if (DateTime.Parse(endDate) < DateTime.Now)
            {
                Response.Write("<script>alert('活动结束时间不能小于当前时间！')</script>");
                return;
            }
            //每个参加活动的宝贝设置相同促销力度
            if (Request.Form["itemType"].ToString() == "same")
            {
                //促销方式
                if (Request.Form["discountType"].ToString() == "DISCOUNT")
                {
                    if (!isNumber(Request.Form["zhe"].ToString()))
                    {
                        Response.Write("<script>alert('折扣格式不正确！')</script>");
                        return;
                    }
                    discountValue = zhe;
                }
                else
                {
                    if (!isNumber(Request.Form["yuan"].ToString()))
                    {
                        Response.Write("<script>alert('金额格式不正确！')</script>");
                        return;
                    }
                    discountValue = yuan;
                }
            }
            if (!isNumber(rcount))
            {
                rcount = "0";
            }

            if (!isDate(Request.Form["startDate"].ToString()))
            {
                Response.Write("<script>alert('团购时间格式不正确！')</script>");
                return;
            }
            if (!isDate(Request.Form["endDate"].ToString()))
            {
                Response.Write("<script>alert('团购时间格式不正确！')</script>");
                return;
            }
            #endregion
            string sql = "update tete_activity set  startDate='" + startDate + "',endDate='" + endDate + "',itemType='" + itemType + "',discountType='" + discountType + "',discountValue='" + discountValue + "',tagId='" + tagId + "',Rcount=" + rcount + ",nick='" + nick + "', decreaseNum='" + decreaseNum + "'  where id=" + activityID; //更新活动
            utils.ExecuteNonQuery(sql);
            sql = "select * from tete_activitylist where  ID=" + ID;
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                sql = "update tete_activitylist set startDate='" + startDate + "',endDate='" + endDate + "',itemType='" + itemType + "',discountType='" + discountType + "',discountValue='" + discountValue + "',tagId='" + tagId + "',Rcount=" + rcount + ",Status=1,decreaseNum='" + decreaseNum + "',isok=0 where ID=" + ID;
                utils.ExecuteNonQuery(sql);//修改活动商品  '延长修改活动 Status=1 和 isok=0 '
            }
            Response.Write("<script>alert('修改成功！')</script>");
            Response.Redirect("activitygetitem.aspx");
        }
        if (!IsPostBack)
        {
            string sql = "select enddate from TopTaobaoShop where nick='" + nick + "'";
            DataTable dt = utils.ExecuteDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                shopgroupbuyEnddate.Value = dt.Rows[0]["enddate"].ToString();//特特结束时间
            }
            sql = "select * from [tete_activitylist] where ID=" + Request.QueryString["ID"].ToString();

            dt = utils.ExecuteDataTable(sql);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    imags.Src = dt.Rows[i]["ProductImg"].ToString();
                  
                    startDate = dt.Rows[i]["startDate"].ToString();
                    endDate = dt.Rows[i]["endDate"].ToString();
                    itemType = dt.Rows[i]["itemType"].ToString();
                    if (itemType != "same")
                    {
                        itemType = "";
                        itemTypeStr = "checked";
                        Detailtype.Value = "2";
                    }
                    else
                    {
                        itemType = "checked";
                        itemTypeStr = "";
                        Detailtype.Value = "1";
                    }
                    discountType = dt.Rows[i]["discountType"].ToString();
                    if (discountType != "DISCOUNT")
                    {
                        discountType = "";
                        discountTypeStr = "checked";
                    }
                    else
                    {
                        discountType = "checked";
                        discountTypeStr = "";
                    }
                    zhe = dt.Rows[i]["discountValue"].ToString();
                    yuan = dt.Rows[i]["discountValue"].ToString();
                    decreaseNum = dt.Rows[i]["decreaseNum"].ToString();
                    if (decreaseNum == "0")
                    {
                        decreaseNum = "selected";
                        decreaseNumStr = "";
                    }
                    else
                    {
                        decreaseNum = "";
                        decreaseNumStr = "selected";
                    }
                    rcount = dt.Rows[i]["Rcount"].ToString();
                }
            }
        }
    }

        
    /// <summary>
    /// 验证是否数字类型
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool isNumber(string obj)
    {

        try
        {
            Convert.ToInt32(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 验证是否时间数据
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool isDate(string obj)
    {
        try
        {
            Convert.ToDateTime(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }
}