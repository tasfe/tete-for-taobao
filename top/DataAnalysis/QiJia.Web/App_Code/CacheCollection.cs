using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Qijia.Model;
using System.Collections.Generic;
using Keede.Caching;
using System.Collections;

/// <summary>
///CacheCollection 的摘要说明
/// </summary>
public class CacheCollection
{
    public const string KEY_ALLShopId = "CacheKey_ShopId";

    public static IList<string> GetShopIdList()
    {
        return new CacheUtility<IList<string>>().Get(KEY_ALLShopId, delegate()
        {
            ShopTotalService shopTotalDal = new ShopTotalService();
            return shopTotalDal.GetAllShopIds();
        }, 60 * 12);
    }

    #region [清除含有关键字的缓存]

    /// <summary>
    /// 清除含有关键字的缓存
    /// </summary>
    public static void RemoveCacheByKey(string key)
    {
        //如果直接找到对应KEY，则清除该KEY的缓存，否则搜索所有的缓存
        if (HttpRuntime.Cache.Remove(key) == null)
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            ArrayList allKeys = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                allKeys.Add(CacheEnum.Key);
            }
            foreach (string keys in allKeys)
            {
                if (keys.IndexOf(key) != -1)
                    HttpRuntime.Cache.Remove(keys);
            }
        }
    }

    #endregion
}
