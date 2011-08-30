using System;
using System.Collections.Generic;
using OpenFlashChart;
using System.Data;
using Common;

public partial class Pie : System.Web.UI.Page
{
    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //判断是否删除
        string act = utils.NewRequest("act", utils.RequestType.Form);
        id = utils.NewRequest("id", utils.RequestType.QueryString);

        showResult();
    }

    private void showResult()
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT id,name,viewcount,hitcount FROM TopIdea WHERE nick = '" + taobaoNick + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        OpenFlashChart.OpenFlashChart chart = new OpenFlashChart.OpenFlashChart();
        chart.Title = new Title("广告浏览次数查看");

        OpenFlashChart.Pie pie = new OpenFlashChart.Pie();
        Random random = new Random();

        List<PieValue> values = new List<PieValue>();
        List<string> labels = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            values.Add(new PieValue(double.Parse(dt.Rows[i]["viewcount"].ToString()), dt.Rows[i]["name"].ToString()));
            labels.Add(i.ToString());
        }
        //values.Add(0.2);
        pie.Values = values;
        pie.FontSize = 20;
        pie.Alpha = .5;
        PieAnimationSeries pieAnimationSeries = new PieAnimationSeries();
        pieAnimationSeries.Add(new PieAnimation("bounce", 5));
        pie.Animate = pieAnimationSeries;
        //pie.GradientFillMode = false;

        //pie.FillAlpha = 10;

        //pie.Colour = "#ffffff";
        pie.Colours = new string[] { "#04f", "#1ff", "#6ef", "#f30" };
        pie.Tooltip = "#label#, #percent# of 100%";
        chart.AddElement(pie);
        chart.Bgcolor = "#eeeeee";
        string s = chart.ToPrettyString();
        Response.Clear();
        Response.CacheControl = "no-cache";
        Response.Write(s);
        Response.End();
    }
}
