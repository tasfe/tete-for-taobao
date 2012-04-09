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
using Model;

public partial class CreateKeyWord : System.Web.UI.Page
{
    Search.DAL.KeyWordService kyDal = new Search.DAL.KeyWordService(); 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = Request.Cookies["nick"].Value;

            Rpt_Keys.DataSource = kyDal.GetKeyWords(nick);
            Rpt_Keys.DataBind();
        }
    }
    protected void Btn_Add_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Tb_txt.Text.Trim()))
        {
            return;
        }

        KeyWordInfo info = new KeyWordInfo();
        info.Nick = Request.Cookies["nick"].Value;
        info.KeyWord = Tb_txt.Text.Trim();

        kyDal.Insert(info);

        Rpt_Keys.DataSource = kyDal.GetKeyWords(info.Nick);
        Rpt_Keys.DataBind();

    }
}
