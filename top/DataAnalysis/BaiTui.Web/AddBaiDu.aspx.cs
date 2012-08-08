﻿using System;
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

public partial class AddBaiDu : System.Web.UI.Page
{

    GoodsService goodsDal = new GoodsService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string nick = "nick";
            if (Request.Cookies["nick"] != null)
                nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
            else
            {
                if (Session["snick"] != null)
                    nick = Session["snick"].ToString();
            }

            Rpt_GoodsList.DataSource = goodsDal.SelectAllGoodsByNick(nick);
            Rpt_GoodsList.DataBind();
        }
    }

    protected void Btn_Search_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_Next_Click(object sender, EventArgs e)
    {
        string keys = CheckVali();
        string goodsId = "";
        string goodsname="";
        string goodspic = "";
        if (keys != "")
        {
            foreach (RepeaterItem item in Rpt_GoodsList.Items)
            {
                RadioButton rb = item.FindControl("RBtn_Goods") as RadioButton;
                if (rb.Checked)
                {
                    Label lbl = item.FindControl("Lbl_GoodsId") as Label;
                    goodsId = lbl.Text;

                    Label lblname = item.FindControl("Lbl_GoodsName") as Label;
                    goodsname = lblname.Text;

                    Label lblpic = item.FindControl("Lbl_GoodsPic") as Label;
                    goodspic = lblpic.Text;

                    break;
                }
            }
            if (goodsId != "")
            {
                string nick = "nick";
                if (Request.Cookies["nick"] != null)
                    nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value); //"nick"; 
                else
                {
                    if (Session["snick"] != null)
                        nick = Session["snick"].ToString();
                }
                TuiGoodsInfo info = new TuiGoodsInfo();
                info.GoodsId = goodsId;
                info.Keywords = keys;
                info.TuiId = Guid.NewGuid();
                info.Nick = nick;
                info.GoodsName = goodsname;
                info.GoodsPic = goodspic;
                info.Type = 1;

                new TuiGoodsService().InsertTuiGoods(info);
                Response.Redirect("BaiShare.aspx?id=" + info.TuiId);
            }
        }
    }

    private string CheckVali()
    {
        int count = 0;
        string reval = "";
        if (!string.IsNullOrEmpty(TB_Key1.Text.Trim()))
        {
            reval += TB_Key1.Text.Trim() + "[ksp]";
            count++;
        }
        if (!string.IsNullOrEmpty(TB_Key2.Text.Trim()))
        {
            reval += TB_Key2.Text.Trim() + "[ksp]";
            count++;
        }
        if (!string.IsNullOrEmpty(TB_Key3.Text.Trim()))
        {
            reval += TB_Key3.Text.Trim() + "[ksp]";
            count++;
        }
        if (!string.IsNullOrEmpty(TB_Key4.Text.Trim()))
        {
            reval += TB_Key4.Text.Trim() + "[ksp]";
            count++;
        }
        if (!string.IsNullOrEmpty(TB_Key5.Text.Trim()))
        {
            reval += TB_Key5.Text.Trim() + "[ksp]";
            count++;
        }

        if (count >= 3)
            return reval.Substring(0, reval.Length - 5);
        return "";
    }
}