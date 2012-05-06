using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;

public partial class top_groupbuy_activitysettemp1 : System.Web.UI.Page
{
    public string html = string.Empty;
    public string sql = string.Empty;
    string idstr = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form["selstr"].ToString() != "" && Request.Form["selstr"].ToString() != "0")
        {
            sql = "select * from tete_activitylist where ActivityID=" + Request.Form["selstr"].ToString();
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<div id='div" + dt.Rows[i]["ProductID"].ToString() + "' width=\"700px\"><table width=\"700px\"><tr ><td width=\"200px\"><a href=\"http://item.taobao.com/item.htm?id=" + dt.Rows[i]["ProductID"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["Productname"].ToString() + "</a></td>";
                    
                    html += "<td width=\"100px\"> " + dt.Rows[i]["Productprice"].ToString() + "元 <input   type=\"hidden\" id=\"productid" + dt.Rows[i]["ProductID"].ToString() + "\" name=\"productid\"  value=\"" + dt.Rows[i]["ProductID"].ToString() + "\"><input type=\"hidden\" id=\"price" + dt.Rows[i]["ProductID"].ToString() + "\" name=\"price\" value=\"" + dt.Rows[i]["Productprice"].ToString() + "\"></td>";

                    html += "<td   width=\"100px\">  <input type=\"text\" id=\"zhekou" + dt.Rows[i]["ProductID"].ToString() + "\" size=\"20\" name=\"zhekou\" /> 元 </td>";
                    html += " <td   width=\"100px\">  <input type=\"text\"  size=\"8\"  name=\"rcount\" value=\"300\" /> </td><td><a onclick=\"deleteDIV('del1" + dt.Rows[i]["ProductID"].ToString() + "')\"  style=\"cursor:hand;\">删除</a></td></tr></table><input id=\"del1" + dt.Rows[i]["ProductID"].ToString() + "\" name=\"del\" value='' type=\"hidden\" ></div>";
                    idstr = "," + i.ToString();
                }
                html += "<input type=\"hidden\" id=\"idss\" name=\"idss\" value=\"" + idstr + "\">";
            }
        }
    }
}