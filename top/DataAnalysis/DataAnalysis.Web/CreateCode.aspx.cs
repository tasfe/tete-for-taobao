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
using System.Security.Cryptography;
using Common;
using System.Collections.Generic;
using System.IO;

public partial class CreateCode : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TB_Code.Text = "<img src=\"" + DataHelper.GetAppSetings("hostname") + "GetData.ashx?nick=" + Request.Cookies["nick"].Value + "\" border=\"0\" />";

            if (File.Exists(Server.MapPath("~/Images/nickimgs/" + DataHelper.Encrypt(HttpUtility.UrlDecode(Request.Cookies["nick"].Value + ".jpg")))))
            {
                UserImage = "/Images/nickimgs/" + Request.Cookies["nikc"].Value + ".jpg";
            }
            else
            {
                UserImage = "/Images/nickimgs/newlogo1.jpg";
            }
        }
    }

    protected void Btn_Upload_Click(object sender, EventArgs e)
    {
        //try
        //{
            //获取cookie

            string nickNo = Request.Cookies["nick"].Value;
            string topsession = Request.Cookies["nicksession"].Value;
            if (string.IsNullOrEmpty(nickNo))
            {
                Page.RegisterStartupScript("error", "<script>alert('非法用户!');</script>");
            }
            //Rijndael_ encode = new Rijndael_("tetesoft");
            ////解密得到真实nick
            //string realNickNo = encode.Encrypt(nickNo);
            string name = FUp_Img.FileName;
            string type = name.Substring(name.LastIndexOf(".") + 1).ToLower();

            if (type == "jpg" || type == "png" || type == "bmp" || type == "gif" || type == "jpeg")
            {
                try
                {
                    //图片名称统一用md5加密后的，后缀为jpg格式
                    String ReName = DataHelper.Encrypt(HttpUtility.UrlDecode(nickNo)) + ".jpg";//图片重命名

                    String Ipath = Server.MapPath("~/Images/nickimgs") + "\\" + ReName;//文件实际路径

                    FUp_Img.SaveAs(Ipath);//上传到图片目录

                    TopNickSessionInfo info = new TopNickSessionInfo();
                    info.Nick = nickNo;
                    info.NickState = true;
                    info.JoinDate = DateTime.Now;
                    info.Session = topsession;
                    //new NickSessionService().AddSession(info);
                }
                catch (Exception ex)
                {
                    Page.RegisterStartupScript("error", "<script>alert('图片上传失败,请重试!" + ex.Message.ToString() + "');</script>");
                }

                TB_Code.Text = "<img src=\"" + DataHelper.GetAppSetings("hostname") + "GetData.ashx?nick=" + nickNo + "\" border=\"0\" />";

            }
            else
            {
                Page.RegisterStartupScript("error", "<script>alert('图片格式不对!');</script>");
            }
        //}
        //catch(Exception ex)
        //{
        //    Page.RegisterStartupScript("error", "<script>alert('图片上传失败,请重试"+ex.Message+"!');</script>");
        //}
    }

    protected string UserImage
    {
        get { return ViewState["uimg"].ToString(); }
        set { ViewState["uimg"] = value; }
    }

    public string ParametersName(String top_parameters)
    {
        string nick = null;
        Dictionary<string, string> dic = convertBase64StringtoDic(top_parameters);
        foreach (KeyValuePair<string, string> kvp in dic)
        {
            if (kvp.Key == "visitor_nick")
            {
                nick = kvp.Value;
            }
        }
        return nick;
    }

    private static Dictionary<string, string> convertBase64StringtoDic(string str)
    {
        if (str == null)
            return null;
        String keyvalues = null;
        keyvalues = HttpUtility.HtmlDecode(str);
        String[] keyvalueArray = keyvalues.Split('&');
        Dictionary<string, string> dic = new Dictionary<string, string>();
        foreach (string keyvalue in keyvalueArray)
        {
            string[] s = keyvalue.Split('=');
            if (s == null || s.Length != 2)
                return null;
            dic.Add(s[0], s[1]);
        }
        return dic;
    }
}
