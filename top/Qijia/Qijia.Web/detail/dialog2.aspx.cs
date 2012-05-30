using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Qijia.PCI;
using Qijia.DAL;
using Qijia.Model;
using DBHelp;
using System.Data;
using System.Text.RegularExpressions;

public partial class Web_detail_dialog1 : System.Web.UI.Page
{
    Jia_ShopService ssDal = new Jia_ShopService();
    Jia_ItemService itemDal = new Jia_ItemService();
    Jia_TemplateService tempDal = new Jia_TemplateService();
    Jia_ImgCustomerService icDal = new Jia_ImgCustomerService();

    public string id = string.Empty;
    public string nick = string.Empty;
    public string tplid = string.Empty;

    public string item1 = string.Empty;
    public string item2 = string.Empty;
    public string item3 = string.Empty;
    public string item4 = string.Empty;
    public string item5 = string.Empty;
    public string item6 = string.Empty;
    public string item7 = string.Empty;
    public string property = string.Empty;
    public string text = string.Empty;
    public string text1 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = Request.QueryString["id"] == null ? "0" : Request.QueryString["id"].ToString();
        nick = Request.QueryString["nick"] == null ? "0" : Request.QueryString["nick"].ToString();
        tplid = Request.QueryString["tplid"] == null ? "0" : Request.QueryString["tplid"].ToString();

        BindData();
    }

    /// <summary>
    /// 数据绑定
    /// </summary>
    private void BindData()
    {
        //获取图片地址，如果已经传过的
        item1 = GetImgByTag(id, "{item1}");
        item2 = GetImgByTag(id, "{item2}");
        item3 = GetImgByTag(id, "{item3}");
        item4 = GetImgByTag(id, "{item4}");
        item5 = GetImgByTag(id, "{item5}");
        item6 = GetImgByTag(id, "{item6}");
        item7 = GetImgByTag(id, "{item7}");

        //商品属性绑定
        string sql = "SELECT * FROM Jia_Item WHERE ItemId = '" + id + "'";
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            property = CreateProperty(dt.Rows[0]["propertyText"].ToString());
            text = CreateProperty(dt.Rows[0]["charText"].ToString());
        }
    }

    private string CreateProperty(string str)
    {
        if (str.Length == 0)
        {
            return "";
        }

        string newStr = string.Empty;
        string propertyText = str.Substring(1, str.Length - 2); //剔除{}
        string[] chars = Regex.Split(propertyText, "{,}");
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == 0)
            {
                newStr = GetRight(chars[i]);
            }
            else
            {
                newStr += "{||}" + GetRight(chars[i]);
            }
        }
        return newStr;
    }

    private string GetRight(string s)
    {
        if (s.IndexOf(":")!=-1)
        {
            return s.Substring(s.IndexOf(':') + 1, s.Length - s.IndexOf(':') - 1).Replace("'", "\\\'");
        }
        else
        {
            return "";
        }
    }

    private string GetImgByTag(string id, string tag)
    {
        string sql = "SELECT * FROM Jia_ImgCustomer WHERE ItemId = '" + id + "' AND tag = '" + tag + "'";
        DataTable dt = DBHelper.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            return dt.Rows[0]["JiaImg"].ToString();
        }
        else
        {
            return "";
        }
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

        string fileName = string.Empty;
        //判断日期文件夹是否存在
        string dateName = "pic/" + DateTime.Now.ToString("yyyy-MM-dd");
        if (!Directory.Exists(Server.MapPath(dateName)))
        {
            Directory.CreateDirectory(Server.MapPath(dateName));
        }

        UploadFileCommon(FileUpload1, "{item1}", dateName, "707*481");
        UploadFileCommon(FileUpload2, "{item2}", dateName, "463*383");
        UploadFileCommon(FileUpload3, "{item3}", dateName, "233*204");
        UploadFileCommon(FileUpload4, "{item4}", dateName, "233*204");
        UploadFileCommon(FileUpload5, "{item5}", dateName, "463*385");
        UploadFileCommon(FileUpload6, "{item6}", dateName, "467*384");
        UploadFileCommon(FileUpload7, "{item7}", dateName, "235*204");
    }

    private void UploadFileCommon(FileUpload fileUpload1, string tag, string dateName, string wihe)
    {
        string url = "http://qijia.7fshop.com/detail/";

        if (CheckFileIsSave(fileUpload1))
        {
            Jia_ImgCustomer imgCus = new Jia_ImgCustomer();
            Guid imgId = Guid.NewGuid();
            string picName = dateName + "/" + imgId + ".jpg";
            //保存原图
            fileUpload1.PostedFile.SaveAs(Server.MapPath(picName));
            string picsName = "~/temp/" + imgId + "_s.jpg";

            //生成合理尺寸图(对照模板)
            string[] widhei = wihe.Split('*');
            HttpUtil.MakeThumbnail(picName, Server.MapPath(picsName), int.Parse(widhei[0]), int.Parse(widhei[1]), "Cut");
            imgCus.ItemId = id;
            imgCus.Tag = tag;
            imgCus.Guid = Guid.NewGuid().ToString();
            imgCus.MyImg = url + picName;

            imgCus.JiaImg = "http://qijia.7fshop.com/temp/" + imgId + "_s.jpg";

            //发送图片到齐家网站
            List<Parameter> list = new List<Parameter>();
            Parameter paramter = new Parameter("image", Server.MapPath(picsName));
            list.Add(paramter);
            try
            {
                string realUrl = UploadFile.HttpPostWithFile("http://mall.jia.com/site/upload_describe_image", "", list);
                string[] chars = Regex.Split(realUrl, "=>");

                imgCus.JiaImg = chars[2].Replace(")", "").Trim();
            }
            catch (Exception ex)
            {
                LogHelper.LogInfo.Add("用户上传图片错误", ex.Message);
            }

            //可做删除生成的图片操作(暂未做)

            string sql = "SELECT * FROM Jia_ImgCustomer WHERE ItemId = '" + id + "' AND tag = '" + tag + "'";
            DataTable dt = DBHelper.ExecuteDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                icDal.AddJia_ImgCustomer(imgCus);
            }
            else
            {
                imgCus.Guid = dt.Rows[0]["guid"].ToString();
                icDal.ModifyJia_ImgCustomer(imgCus);
            }
        }
    }

    /// <summary>
    /// 判断上传的文件是否合法
    /// </summary>
    /// <param name="FileUpload1"></param>
    /// <returns></returns>
    private bool CheckFileIsSave(FileUpload FileUpload1)
    {
        if (FileUpload1.PostedFile.ContentType.IndexOf("jpeg") != -1 || FileUpload1.PostedFile.ContentType.IndexOf("jpg") != -1 || FileUpload1.PostedFile.ContentType.IndexOf("png") != -1 || FileUpload1.PostedFile.ContentType.IndexOf("gif") != -1 || FileUpload1.PostedFile.ContentType.IndexOf("bmp") != -1)
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

        Response.Write("<iframe src=\"http://shop.jia.com/item/get_item_template?id=" + id + "\" width=0 height=0 frameborder=0></iframe>");
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
        string sql = "SELECT COUNT(*) FROM Jia_Item WHERE itemid = '" + id + "'";
        string count = DBHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
        Jia_Item item = CreateItemInfo();
        if (count == "0")
        {
            Jia_ItemService jiaService = new Jia_ItemService();
            jiaService.AddJia_Item(item);
        }
        else
        {
            Jia_ItemService jiaService = new Jia_ItemService();
            jiaService.ModifyJia_Item(item);
        }

        //创建宝贝图片

        //获取模板信息
        Jia_Template temp = tempDal.GetJia_TemplateById(item.TplId);

        string content = MethodGroup.GetRealItemInfo(item, temp, "1");

        return content;
    }

    private Jia_Item CreateItemInfo()
    {
        Jia_Item item = new Jia_Item();

        if (id == "0")
        {
            id = nick;
        }

        item.ItemId = id;
        item.Nick = nick;
        item.PropertyText = CreateProperty();
        item.CharText = CreateChar();
        item.TplId = tplid;
        item.UpdateDate = DateTime.Now;

        return item;
    }
}