using System;
using System.Web;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;

public partial class TestUserSeePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PostIphoneMsg pmsg = new PostIphoneMsg(Tb_UserNick);
            //pmsg.PostMsg("sddfs");
            //int width = 160;

            //QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

            //qrCodeEncoder.QRCodeScale = 1;
            //qrCodeEncoder.QRCodeVersion = 4;

            //qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //var pbImg = qrCodeEncoder.Encode("{\"coupon\":\"F90B057C-F36A-430F-8030-0E2C995930A1\"}");
            //var dwidth = width * 2;
            //Bitmap bmp = new Bitmap(pbImg.Width + dwidth, pbImg.Height + dwidth);
            //Graphics g = Graphics.FromImage(bmp);
            //var c = System.Drawing.Color.White;
            //g.FillRectangle(new SolidBrush(c), 0, 0, pbImg.Width + dwidth, pbImg.Height + dwidth);
            //g.DrawImage(pbImg, width, width);
            //g.Dispose();

            //bmp.Save(HttpRuntime.AppDomainAppPath + "two.jpg");

        }
    }
    protected void Btn_AddCookie_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlEncode(Tb_UserNick.Text);
        string session = Tb_UserSession.Text;
        HttpCookie cookie = new HttpCookie("nick", nick);
        HttpCookie cooksession = new HttpCookie("nicksession", session);
        cookie.Expires = DateTime.Now.AddDays(1);
        cooksession.Expires = DateTime.Now.AddDays(1);


        HttpCookie istongji = new HttpCookie("istongji", "1");
        istongji.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Add(istongji);

        Response.Cookies.Add(cookie);
        Response.Cookies.Add(cooksession);
        Response.Redirect("CustomerList.aspx");
    }
}
