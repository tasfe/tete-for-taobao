using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class UpdatePrice : BasePage
{
    TaoBaoGoodsServive tbgDal = new TaoBaoGoodsServive();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string page = Request.QueryString["Page"];
            if (string.IsNullOrEmpty(page))
                Bind("", 1, 9);
            else
            {
                int p = 1;
                try
                {
                    p = int.Parse(page);
                }
                catch { }
                Bind("", p, 9);
            }
        }
    }

    private void Bind(string goodsName,int page,int count)
    {
       
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

        int totalCount = tbgDal.GetAllGoodsCount(nick, goodsName);
        int TotalPage = totalCount % count != 0 ? (totalCount / count) + 1 : totalCount / count; //总页数

        IList<GoodsInfo> list = tbgDal.GetAllGoods(nick, goodsName, page, count);

        lblCurrentPage.Text = "共" + totalCount.ToString() + "条记录 当前页：" + page + "/" + (TotalPage == 0 ? 1 : TotalPage);

        lnkFrist.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=1&";
        if (page > 1)
            lnkPrev.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page - 1);

        if (page != TotalPage && TotalPage != 0)
            lnkNext.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + Convert.ToString(page + 1);
        lnkEnd.NavigateUrl = Request.CurrentExecutionFilePath + "?Page=" + (TotalPage == 0 ? 1 : TotalPage);

        Rpt_GoodsList.DataSource = list;
        Rpt_GoodsList.DataBind();
    }

    protected void Btn_Update_Click(object sender, EventArgs e)
    {
        IList<GoodsInfo> list = new List<GoodsInfo>();
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        foreach (RepeaterItem item in Rpt_GoodsList.Items)
        {
            TextBox tb = (TextBox)item.FindControl("TB_PurchasePrice");
            Label lb = (Label)item.FindControl("Lbl_GoodsId");
            Label lbp = (Label)item.FindControl("Lbl_PurchasePrice");
            GoodsInfo info = new GoodsInfo();
            info.nick = nick;
            info.num_iid = lb.Text;
            try
            {
                info.PurchasePrice = decimal.Parse(tb.Text);
            }
            catch(Exception ex)
            {
                info.PurchasePrice = decimal.Parse(lbp.Text);
            }
            list.Add(info);
        }

        foreach (GoodsInfo info in list)
        {
            tbgDal.UpdatePrice(info);
        }
    }
    protected void Btn_Search_Click(object sender, EventArgs e)
    {
        Bind(Tb_GoodsName.Text.Trim(), 1, 9);
    }
}
