using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_groupbuy_activityadd : System.Web.UI.Page
{
    public string teteendDate = string.Empty;//团购结束时间
    public string nick = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        teteendDate = "2012-12-12";
        teteendDateID.Value = teteendDate;
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

      

       
        if (nick == "")
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        if (Request.Form["act"] == "post")
        {
            string name= Request.Form["name"].ToString();
            string memo= Request.Form["memo"].ToString();
            string startDate=Request.Form["startDate"].ToString();
            string endDate=Request.Form["endDate"].ToString();
            string itemType=Request.Form["itemType"].ToString();
            string discountType=Request.Form["discountType"].ToString();
            string zhe= Request.Form["zhe"].ToString();
            string yuan=Request.Form["yuan"].ToString();
            string decreaseNum=Request.Form["decreaseNum"].ToString();
            string rcount=Request.Form["Rcount"].ToString();
            string tagId = "1";
            string status = "1";//进行中
            string discountValue = "";
            string shopgroupbuyEnddate = "";
           
#region  数据格式验证
            if (DateTime.Parse(startDate) > DateTime.Now)
            {
                status = "0";//未开始
            }

            string sql23 = "select enddate from TopTaobaoShop where nick='" + nick + "'";
            DataTable  dt = utils.ExecuteDataTable(sql23);

            if (dt != null && dt.Rows.Count > 0)
            {
                shopgroupbuyEnddate  = dt.Rows[0]["enddate"].ToString();
            }
            if (DateTime.Parse(shopgroupbuyEnddate) < DateTime.Now)
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
                Response.Write("<script>alert('活动时间格式不正确！')</script>");
                return;
            }
            if (!isDate(Request.Form["endDate"].ToString()))
            {
                Response.Write("<script>alert('活动时间格式不正确！')</script>");
                return;
            }
#endregion 
            string sql = "INSERT INTO [tete_activity] ([Name]   ,[Description] ,[Remark] ,[startDate] ,[endDate],[itemType] ,[discountType] ,[discountValue] ,[tagId] ,[Rcount],[Nick] ,[Status],decreaseNum,isok) VALUES('" + name + "','','" + memo + "','" + startDate + "','" + endDate + "','" + itemType + "','" + discountType + "','" + discountValue + "','" + tagId + "'," + rcount + ",'" + nick + "'," + status + "," + decreaseNum + ",0)";
            utils.ExecuteNonQuery(sql);
            Response.Redirect("activityAddsuccess.aspx");
            //Response.Write("<script>alert('添加成功！')</script>");//添加活动商品时 要把活动状态修改为1 进行中
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
        catch {
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