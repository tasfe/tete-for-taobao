using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;

public partial class CustomerList : BasePage
{

    TeteUserTokenService tutDal = new TeteUserTokenService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

        IList<TeteUserTokenInfo> list = tutDal.GetAllTeteUserToken(nick);

        Rpt_CustomerList.DataSource = list;
        Rpt_CustomerList.DataBind();
    }
}
