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
using System.Collections.Generic;
using Search.DAL;
using CusServiceAchievements.DAL;

public partial class SeeTop : BasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KeyWordService kyDal = new KeyWordService();
            GoodsService goodsDal = new GoodsService();
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
            IList<KeyWordInfo> list = kyDal.GetKeyWords(nick);

            IList<TaoBaoAPIHelper.GoodsInfo> goodsList = goodsDal.GetAllGoods(nick);
            Dictionary<string, List<TaoBaoAPIHelper.GoodsInfo>> dic = new Dictionary<string, List<TaoBaoAPIHelper.GoodsInfo>>();
            foreach (KeyWordInfo info in list)
            {
                dic.Add(info.KeyWord, new List<TaoBaoAPIHelper.GoodsInfo>());

                List<TaoBaoAPIHelper.GoodsInfo> sgoods = TaoBaoAPIHelper.TaoBaoAPI.SearchGoods(info.KeyWord);

                foreach (TaoBaoAPIHelper.GoodsInfo ginfo in new List<TaoBaoAPIHelper.GoodsInfo>(goodsList))
                {

                    for (int i = 0; i < sgoods.Count; i++)
                    {
                        if (nick == sgoods[i].nick && ginfo.num_iid == sgoods[i].num_iid)
                        {
                            ginfo.Collection = i + 1;//此处当作排名使用
                            dic[info.KeyWord].Add(ginfo);
                        }
                    }
                }
            }

        }
    }
}
