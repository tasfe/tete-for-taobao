using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

public partial class verifyimg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CreateVerifyImg();
    }

    /// <summary>
    /// 生成验证码图片
    /// </summary>
    private void CreateVerifyImg()
    {
        string vMapPath = string.Empty;                     //验证码路径
        Random rnd = new Random();                          //随机发生器
        Bitmap imgTemp = null;                              //验证码图片
        Graphics g = null;                                  //图形库函数
        SolidBrush blueBrush = null;                        //缓冲区

        //暂时注销
        //string rndstr = rnd.Next(1111, 9999).ToString();    //验证码

        string rndstr = string.Empty;    //验证码
        //测试时，将验证码统一设为0000
        rndstr = rnd.Next(1111, 9999).ToString();

        int totalwidth = 0;                                 //验证码图片总宽度
        int totalheight = 0;                                //验证码图片总高度

        //给缓存添加验证码
        SetVerifyCookie(rndstr);

        //生成验证码图片文件
        System.Drawing.Image ReducedImage1 = System.Drawing.Image.FromFile(Server.MapPath("images/yzm/" + rndstr.Substring(0, 1) + ".gif"));
        System.Drawing.Image ReducedImage2 = System.Drawing.Image.FromFile(Server.MapPath("images/yzm/" + rndstr.Substring(1, 1) + ".gif"));
        System.Drawing.Image ReducedImage3 = System.Drawing.Image.FromFile(Server.MapPath("images/yzm/" + rndstr.Substring(2, 1) + ".gif"));
        System.Drawing.Image ReducedImage4 = System.Drawing.Image.FromFile(Server.MapPath("images/yzm/" + rndstr.Substring(3, 1) + ".gif"));

        totalwidth = 52;
        totalheight = 17;
        imgTemp = new Bitmap(totalwidth, totalheight);
        g = Graphics.FromImage(imgTemp);
        blueBrush = new SolidBrush(Color.Black);
        g.FillRectangle(blueBrush, 0, 0, totalwidth, totalheight);
        g.DrawImage(ReducedImage1, 0, 0);
        g.DrawImage(ReducedImage2, ReducedImage1.Width, 0);
        g.DrawImage(ReducedImage3, ReducedImage1.Width + ReducedImage2.Width, 0);
        g.DrawImage(ReducedImage4, ReducedImage1.Width + ReducedImage2.Width + ReducedImage3.Width, 0);

        vMapPath = Server.MapPath("images/yzm/verify.gif");
        try
        {
            imgTemp.Save(vMapPath);
        }
        catch
        {
            //utils.JsAlert("创建验证验错误!");
        }

        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        imgTemp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        Response.ClearContent();
        Response.ContentType = "image/Gif";
        Response.BinaryWrite(ms.ToArray());

        blueBrush.Dispose();
        g.Dispose();
        imgTemp.Dispose();
    }

    /// <summary>
    /// 设置验证码到缓存
    /// </summary>
    /// <param name="rndstr">序列号</param>
    private void SetVerifyCookie(string rndstr)
    {
        HttpCookie VerifyCode = null;       //登录的密码(MD5后)

        //生成Cookie(用户名, 密码)
        VerifyCode = new HttpCookie("VerifyCode", rndstr.Trim());

        //保存Cookie(用户名, 密码)
        HttpContext.Current.Response.Cookies.Add(VerifyCode);
    }

}
