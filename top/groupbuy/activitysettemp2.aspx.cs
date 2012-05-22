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
    string newprice = string.Empty;
    public string name = string.Empty;
    public string templetid = string.Empty;
    public string bt = string.Empty;
    public string mall = string.Empty;
    public string liang = string.Empty;
    public string baoy = string.Empty;
    public string hdID = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        name = Request.Form["name"].ToString();
        templetid = Request.Form["templetid"].ToString();
        bt = Request.Form["bt"].ToString();
        mall = Request.Form["mall"].ToString();
        liang = Request.Form["liang"].ToString();

        if (Request.Form["baoy"] != null && Request.Form["baoy"] != "")
        {
            baoy = Request.Form["baoy"].ToString();
        }

        if (Request.Form["selstr"].ToString() != "" && Request.Form["selstr"].ToString() != "0")
        {
            hdID = Request.Form["selstr"].ToString();
            sql = "select * from tete_shoptemplet where ActivityID=" + hdID + " and templetID=" + templetid + " and Isdelete=0 ";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                Response.Write("这个活动已经创建了这套模板.请从新选择模板！  <a href=\"activitysettemp1.aspx\">返回</a>");
                Response.End();
            }
            sql = "select * from tete_activitylist where status<>4 and ActivityID=" + Request.Form["selstr"].ToString();
            dt = utils.ExecuteDataTable(sql);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<div id='div" + dt.Rows[i]["ProductID"].ToString() + "' width=\"700px\"><table width=\"700px\"><tr ><td width=\"200px\"><a href=\"http://item.taobao.com/item.htm?id=" + dt.Rows[i]["ProductID"].ToString() + "\" target=\"_blank\">" + dt.Rows[i]["Productname"].ToString() + "</a><input type=\"hidden\" id=\"pname" + dt.Rows[i]["Productname"].ToString() + "\" name=\"pname\" value=\"" + dt.Rows[i]["Productname"].ToString() + "\"></td>";

                    html += "<td width=\"100px\"> " + dt.Rows[i]["Productprice"].ToString() + "元 <input   type=\"hidden\" id=\"productid" + dt.Rows[i]["ProductID"].ToString() + "\" name=\"productid\"  value=\"" + dt.Rows[i]["ProductID"].ToString() + "\"><input type=\"hidden\" id=\"price" + dt.Rows[i]["ProductID"].ToString() + "\" name=\"price\" value=\"" + dt.Rows[i]["Productprice"].ToString() + "\"><input type=\"hidden\" id=\"pimg" + dt.Rows[i]["ProductID"].ToString() + "\" name=\"pimg\" value=\"" + dt.Rows[i]["ProductImg"].ToString() + "\"></td>";
                    try
                    {
                        if (dt.Rows[i]["discountType"].ToString() == "DISCOUNT") //discountValue
                        {
                            newprice = decimal.Round((decimal.Parse(dt.Rows[i]["Productprice"].ToString()) * decimal.Parse(dt.Rows[i]["discountValue"].ToString()) * 0.1m), 2).ToString();
                        }
                        else if (dt.Rows[i]["discountType"].ToString() == "PRICE")
                        {
                            newprice = decimal.Round((decimal.Parse(dt.Rows[i]["Productprice"].ToString()) - decimal.Parse(dt.Rows[i]["discountValue"].ToString())), 2).ToString();
                        }
                    }
                    catch { }
                    html += "<td   width=\"100px\">  <input type=\"text\" id=\"zhekou" + dt.Rows[i]["ProductID"].ToString() + "\" size=\"10\" name=\"zhekou\" value=" + newprice + " /> 元 </td>";
                    html += "<td   width=\"80px\">  <input type=\"text\" id=\"sort" + dt.Rows[i]["ProductID"].ToString() + "\" size=\"10\" name=\"sort\" value=" + i.ToString() + " />  </td>";
                    html += " <td   width=\"80px\">  <input type=\"text\"  size=\"8\"  name=\"rcount\" value=\"300\" /> </td><td><a onclick=\"deleteDIV('del1" + dt.Rows[i]["ProductID"].ToString() + "')\"  style=\"cursor:hand;\">删除</a></td></tr></table><input id=\"del1" + dt.Rows[i]["ProductID"].ToString() + "\" name=\"del\" value='' type=\"hidden\" ></div>";
                    idstr = "," + i.ToString();
                }
                html += "<input type=\"hidden\" id=\"idss\" name=\"idss\" value=\"" + idstr + "\">";
            }
        }
    }
}