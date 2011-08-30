using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class click_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string s = string.Empty;
        s = utils.NewRequest("s", utils.RequestType.QueryString);

//Response.Write(s);
//Response.End();
//return;

        s = strDecode(s.Replace(" ", "+"));
        //切割参数数组
        string[] parmArray = s.Split('|');
        //如果参数非法则直接退出
        if (parmArray.Length < 3)
        {
            return;
        }
        //记录广告点击
        string sql = "UPDATE TopIdea SET hitcount = hitcount + 1 WHERE id = " + parmArray[0];
        utils.ExecuteNonQuery(sql);
        //记录点击日志
        sql = "INSERT INTO TopIdeaHitLog (ideaid, itemid, url) VALUES ('" + parmArray[0] + "', '" + parmArray[1] + "', '" + parmArray[2] + "')";
        utils.ExecuteNonQuery(sql);
        //页面跳转
        Response.Redirect(parmArray[2]);
    }

    /// <summary>
    /// 解密函数
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string strDecode(string str)
    {
        string newstr = string.Empty;
        Rijndael_ encode = new Rijndael_("tetesoftstr");
        newstr = encode.Decrypt(str);
        return newstr;
    }
}