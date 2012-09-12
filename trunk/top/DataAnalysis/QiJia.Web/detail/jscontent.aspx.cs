using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Qijia.PCI;
using Qijia.DAL;
using Qijia.Model;

public partial class detail_ajaxjs : System.Web.UI.Page
{
    public string id = string.Empty;
    public string nick = string.Empty;
    public string tplid = string.Empty;
    public string newhtml = string.Empty;

    Jia_ShopService ssDal = new Jia_ShopService();
    Jia_ItemService itemDal = new Jia_ItemService();
    Jia_TemplateService tempDal = new Jia_TemplateService();
    Jia_ImgCustomerService icDal = new Jia_ImgCustomerService();

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"] == null ? "0" : Request.QueryString["id"].ToString();
        nick = Request.QueryString["nick"] == null ? "0" : Request.QueryString["nick"].ToString();
        tplid = Request.QueryString["tplid"] == null ? "0" : Request.QueryString["tplid"].ToString();

        //创建宝贝
        Jia_ItemService jiaService = new Jia_ItemService();
        Jia_Item item = jiaService.GetJia_ItemById(id);

        //获取模板信息
        Jia_Template temp = tempDal.GetJia_TemplateById(item.TplId);

        newhtml = MethodGroup.GetRealItemInfo(item, temp, "1");
        newhtml = newhtml.Replace("\"", "\\\"");
    }
}