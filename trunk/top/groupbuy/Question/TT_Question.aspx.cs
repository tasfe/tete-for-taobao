using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Data;

public partial class TT_Question : System.Web.UI.Page
{

    /// <summary>
    /// 加载事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
        }
    }

    /// <summary>
    /// 提交事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.txtv.Text != "")
        {
           if (HttpContext.Current.Request.Cookies["VerifyCode"].Value != this.txtv.Text.Trim())
           {
               Response.Write("<script>alert(\"验证码错误！\");</script>");
 
           }
           else {
//获取用户信息
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
               //查询该用户是否发布
               string sql = string.Empty;
               DataTable dtisE = utils.ExecuteDataTable("SELECT * FROM TT_Question WHERE lv=1 AND state=0  AND userID='"+taobaoNick+"'");
               if (dtisE != null && dtisE.Rows.Count > 0)
               {
                   Response.Write("<script>alert(\"你提交问题还没关闭！\");</script>");
               }
               else
               {
                   //添加
                   sql = "INSERT INTO  TT_Question ([Title] ,[Details] ,[Phone] ,[Date],[ReturnDate]," +
                       "[State] ,[UserID],LV) VALUES ('" + txttitle.Text.Trim() + "','" + txtxxi.Text + "','" + txtph.Text
                       + "' ,'" + DateTime.Now.ToString() + "',null,0,'"+taobaoNick+"',1)";

                   utils.ExecuteNonQuery(sql);
                   //查询
                   DataTable dt = utils.ExecuteDataTable("SELECT  TOP 1 ID  FROM TT_Question WHERE LV=1 AND USERID='"+taobaoNick+"' ORDER BY ID DESC ");
                   if (dt != null && dt.Rows.Count > 0)
                   {
                       //更新
                       utils.ExecuteNonQuery("UPDATE TT_Question SET Family='," + dt.Rows[0]["id"] + ",' WHERE ID=" + dt.Rows[0]["id"]);
                   }

               }
            Response.Write("<script>alert(\"提交成功！\");</script>");
           }
        }
        else
        {
            Response.Write("<script>alert(\"验证码不能为空！\");</script>");
            
        }
       
    }
}