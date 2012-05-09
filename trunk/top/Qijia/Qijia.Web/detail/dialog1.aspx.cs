using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Qijia.PCI;
using Qijia.DAL;
using Qijia.Model;

public partial class Web_detail_dialog1 : System.Web.UI.Page
{
    Jia_ShopService ssDal = new Jia_ShopService();
    Jia_ItemService itemDal = new Jia_ItemService();
    Jia_TemplateService tempDal = new Jia_TemplateService();
    Jia_ImgCustomerService icDal = new Jia_ImgCustomerService();

    public string id = string.Empty;
    public string nick = string.Empty;
    public string tplid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"] == null ? "0" : Request.QueryString["id"].ToString();
        nick = Request.QueryString["nick"] == null ? "0" : Request.QueryString["nick"].ToString();
        tplid = Request.QueryString["tplid"] == null ? "0" : Request.QueryString["tplid"].ToString();
    }

    /// <summary>
    /// 上传客户图片到自己服务器
    /// </summary>
    private void UploadUserPic()
    {
        //如果是编辑则调用宝贝ID，如果是添加则生成随机数
        if (id == "0")
        {
            id = nick;
        }

        string url = "http://qijia.7fshop.com/detail/";
        string fileName = string.Empty;
        //判断日期文件夹是否存在
        string dateName = "pic/" + DateTime.Now.ToString("yyyy-MM-dd");
        if (!Directory.Exists(Server.MapPath(dateName)))
        {
            Directory.CreateDirectory(Server.MapPath(dateName));
        }

        if (CheckFileIsSave(FileUpload1))
        {
            Jia_ImgCustomer imgCus = new Jia_ImgCustomer();
            string picName = dateName + "/" + Guid.NewGuid() + ".jpg";
            this.FileUpload1.PostedFile.SaveAs(Server.MapPath(picName));
            imgCus.JiaImg = url + picName;
            imgCus.ItemId = id;
            imgCus.Tag = "{item1}";
            imgCus.Guid = Guid.NewGuid().ToString();
            icDal.AddJia_ImgCustomer(imgCus);
        }

        if (CheckFileIsSave(FileUpload2))
        {
            Jia_ImgCustomer imgCus = new Jia_ImgCustomer();
            string picName = dateName + "/" + Guid.NewGuid() + ".jpg";
            this.FileUpload2.PostedFile.SaveAs(Server.MapPath(picName));
            imgCus.JiaImg = url + picName;
            imgCus.ItemId = id;
            imgCus.Tag = "{item2}";
            imgCus.Guid = Guid.NewGuid().ToString();
            icDal.AddJia_ImgCustomer(imgCus);
        }

        if (CheckFileIsSave(FileUpload3))
        {
            Jia_ImgCustomer imgCus = new Jia_ImgCustomer();
            string picName = dateName + "/" + Guid.NewGuid() + ".jpg";
            this.FileUpload3.PostedFile.SaveAs(Server.MapPath(picName));
            imgCus.JiaImg = url + picName;
            imgCus.ItemId = id;
            imgCus.Tag = "{item3}";
            imgCus.Guid = Guid.NewGuid().ToString();
            icDal.AddJia_ImgCustomer(imgCus);
        }

        if (CheckFileIsSave(FileUpload4))
        {
            Jia_ImgCustomer imgCus = new Jia_ImgCustomer();
            string picName = dateName + "/" + Guid.NewGuid() + ".jpg";
            this.FileUpload4.PostedFile.SaveAs(Server.MapPath(picName));
            imgCus.JiaImg = url + picName;
            imgCus.ItemId = id;
            imgCus.Tag = "{item4}";
            imgCus.Guid = Guid.NewGuid().ToString();
            icDal.AddJia_ImgCustomer(imgCus);
        }

        if (CheckFileIsSave(FileUpload5))
        {
            Jia_ImgCustomer imgCus = new Jia_ImgCustomer();
            string picName = dateName + "/" + Guid.NewGuid() + ".jpg";
            this.FileUpload5.PostedFile.SaveAs(Server.MapPath(picName));
            imgCus.JiaImg = url + picName;
            imgCus.ItemId = id;
            imgCus.Tag = "{item5}";
            imgCus.Guid = Guid.NewGuid().ToString();
            icDal.AddJia_ImgCustomer(imgCus);
        }

        if (CheckFileIsSave(FileUpload6))
        {
            Jia_ImgCustomer imgCus = new Jia_ImgCustomer();
            string picName = dateName + "/" + Guid.NewGuid() + ".jpg";
            this.FileUpload6.PostedFile.SaveAs(Server.MapPath(picName));
            imgCus.JiaImg = url + picName;
            imgCus.ItemId = id;
            imgCus.Tag = "{item6}";
            imgCus.Guid = Guid.NewGuid().ToString();
            icDal.AddJia_ImgCustomer(imgCus);
        }
    }

    /// <summary>
    /// 判断上传的文件是否合法
    /// </summary>
    /// <param name="FileUpload1"></param>
    /// <returns></returns>
    private bool CheckFileIsSave(FileUpload FileUpload1)
    {
        if (FileUpload1.PostedFile.ContentType.IndexOf("jpeg") != -1 || FileUpload1.PostedFile.ContentType.IndexOf("jpg") != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        UploadUserPic();

        string content = GetRealItemInfo();

        Response.Write("<iframe src=\"http://shop.jia.com/item/get_item_template?id=" + id + "\"></iframe>");
        Response.End();
    }

    private string CreateProperty()
    {
        string result = string.Empty;
        int i = 0;
        foreach (string p in Request.Form)
        {
            if (p.IndexOf("property") != -1)
            {
                i++;
                if (i % 2 == 1)
                {
                    if (i == 1)
                    {
                        result = Request.Form[p] + ":";
                    }
                    else
                    {
                        result += "{,}" + Request.Form[p] + ":";
                    }
                }
                else
                {
                    result += Request.Form[p];
                }
            }
        }
        result = "{" + result + "}";

        return result;
    }


    private string CreateChar()
    {
        string result = string.Empty;
        int i = 0;
        foreach (string p in Request.Form)
        {
            if (p.IndexOf("text") != -1)
            {
                i++;
                if (i % 2 == 1)
                {
                    if (i == 1)
                    {
                        result = Request.Form[p] + ":";
                    }
                    else
                    {
                        result += "{,}" + Request.Form[p] + ":";
                    }
                }
                else
                {
                    result += Request.Form[p];
                }
            }
        }
        result = "{" + result + "}";

        return result;
    }


    private string GetRealItemInfo()
    {
        //如果是编辑则调用宝贝ID，如果是添加则生成随机数
        if (id == "0")
        {
            id = nick;
        }

        //创建宝贝
        Jia_Item item = CreateItemInfo();
        Jia_ItemService jiaService = new Jia_ItemService();
        jiaService.AddJia_Item(item);

        //创建宝贝图片

        //获取模板信息
        Jia_Template temp = tempDal.GetJia_TemplateById(item.TplId);

        string content = MethodGroup.GetRealItemInfo(item, temp, "1");

        return content;
    }

    private Jia_Item CreateItemInfo()
    {
        Jia_Item item = new Jia_Item();

        item.ItemId = nick;
        item.Nick = nick;
        item.PropertyText = CreateProperty();
        item.CharText = CreateChar();
        item.TplId = tplid;
        item.UpdateDate = DateTime.Now;

        return item;
    }
}