using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Common;

public partial class QuestionAjax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "list")
            {
                #region  获取问题html
                string divHtml = string.Empty;
                string id = "0";
                string date = string.Empty;
                string returnDate = string.Empty;
                string Family=string.Empty;
                if (Request.QueryString["wd"] != null)
                {
                    id = Request.QueryString["wd"].ToString();

                    
                }
                DataTable dt = utils.ExecuteDataTable(" SELECT * FROM TT_Question WHERE Family like '%," + id + ",%' ORDER BY id");
                if (dt != null && dt.Rows.Count > 0)
                {
                    date = dt.Rows[0]["date"].ToString();
                    returnDate = dt.Rows[dt.Rows.Count - 1]["ReturnDate"].ToString();
                 
                }



                divHtml = " <div> <br /><input id=FamilyID type=hidden name=FamilyID value=\"" + id + "\" /> 提问时间：<span id=\"Label1\">" + date + "</span>" +
                        "最后回复时间：<span id=\"Label2\">" + returnDate + "</span> <table width=\"758px;\">" +
                        "<tr style=\" height:35px;  background-color:#E1D897;  text-align:center\">" +
                        "<td  >提问,回复</td></tr>";


                if (dt != null)
                {
                    for (int index = 0; index < dt.Rows.Count; index++)
                    {
                        divHtml += "<tr style=\"height:30px ; color:Green;\" > <td>";
                        divHtml += "客户提问：" + dt.Rows[index]["title"].ToString() + " </td></tr> <tr style=\"height:30px ; color:Blue;\"> <td> ";
                        divHtml += " 系统回复：" + dt.Rows[index]["Answer"].ToString() + " </td> </tr>";
                    }
                }


                divHtml += "<tr> <td align=center> <textarea name=\"TextBox1\" rows=\"2\" cols=\"20\"" +
                    "id=\"TextBox1\" style=\"height:149px;width:372px;\" >我还有疑问如下</textarea><br />" +
                    " <br /> <img onclick=\"addQuestsub()\" name=\"ImageButton1\" id=\"ImageButton1\"" +
                    " src=\"images/img-12.gif\" style=\"cursor:pointer;border-width:0px;\" /></td>" +
                    " </tr>";

                divHtml += "<tr><td align=center ><span style=\"display: none;\" id=\"loading_span\">&nbsp" +
                "<img height=\"16\" align=\"absmiddle\" width=\"16\" src=\"images/loading_16x16.gif\">"+
                "正在提交，请稍候...</span></td> </tr>";

                divHtml += "<tr><td  ><hr  style=\"color:Green;\"/></td> </tr></table> </div> ";
                Response.Write(divHtml);
                #endregion
            }
            else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "andQA")
            {
                #region  继续问题提交
                string sql = string.Empty;
                string title = Request.QueryString["wd"].ToString();
                string FamilyID = Request.QueryString["FamilyID"].ToString();
                DataTable dt2 = utils.ExecuteDataTable("SELECT * FROM TT_Question WHERE ID="+FamilyID);
                if (dt2 != null)
                {

                    //添加
                    sql = "INSERT INTO  TT_Question ([Title] ,[Details] ,[Phone] ,[Date],[ReturnDate]," +
                        "[State] ,[UserID],LV,Family) VALUES ('" + (title.Length > 40 ? title.Substring(40) : title) + "','" + title + "','" + dt2.Rows[0]["Phone"].ToString() + "' ,'" + DateTime.Now.ToString() + "',null,0,'" + dt2.Rows[0]["UserID"].ToString() + "',2,'" + dt2.Rows[0]["Family"].ToString() + "')";


                    utils.ExecuteNonQuery(sql);
                    //查询
                    DataTable dt = utils.ExecuteDataTable("SELECT  TOP 1 ID  FROM TT_Question WHERE LV=2 AND USERID='" + dt2.Rows[0]["UserID"].ToString() + "' ORDER BY ID DESC ");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //更新
                        utils.ExecuteNonQuery("UPDATE TT_Question SET Family=Family+'," + dt.Rows[0]["id"] + ",' WHERE ID=" + dt.Rows[0]["id"]);

                        Response.Write("1");
                    }
                    else
                    {
                        Response.Write("");
                    }
                }
                else
                {
                    Response.Write("");
                }
                #endregion
            }
            else
            {
                Response.Write("");

            }
        }
    }
}
