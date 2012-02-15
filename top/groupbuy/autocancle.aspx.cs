using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_groupbuy_autocancle : System.Web.UI.Page
{
    public string isautocancle = string.Empty;
    public string cancletime = string.Empty;
    public string taobaoNick = string.Empty;
    public string str = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");
   
        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=11807' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }


        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        if (!IsPostBack)
        {
            string sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + taobaoNick + "'";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                isautocancle = dt.Rows[0]["isautocancle"].ToString();
                cancletime = dt.Rows[0]["cancletime"].ToString();

                //判断是否订购了此服务
                string enddate = dt.Rows[0]["pay1"] == DBNull.Value ? DateTime.Now.ToString() : dt.Rows[0]["pay1"].ToString();
                if (DateTime.Parse(enddate) > DateTime.Now)
                {
                    str = "该服务到期时间是" + enddate;
                }
                else
                {
                    Response.Write("未开启<b>订单自动取消</b>服务，<a href='http://fuwu.taobao.com/item/subsc.htm?items=ts-11807-4:3;' target='_blank'>请点击此处订购</a>");
                    Response.End();
                }

                this.mintime.Text = cancletime;
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string isautocancle = utils.NewRequest("type", utils.RequestType.Form);
        string cancletime = this.mintime.Text;

        string sql = "UPDATE TopTaobaoShop SET isautocancle = '" + isautocancle + "',cancletime = '" + cancletime + "' WHERE nick = '" + taobaoNick + "'";
        utils.ExecuteNonQuery(sql);

        if (isautocancle == "1")
        {
            Response.Write("<script>alert('设置成功，程序将自动取消" + cancletime + "分钟内未付款的团购订单！');window.location.href='autocancle.aspx';</script>");
        }
        else
        {
            Response.Write("<script>alert('设置成功，程序将不自动取消未付款的团购订单！');window.location.href='autocancle.aspx';</script>");
        }
        Response.End();
    }
}