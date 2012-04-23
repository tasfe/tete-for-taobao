using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Keede.Caching;
using System.Collections;

public class CacheCollection
{
    /// <summary>
    /// 所有订购商户信息
    /// </summary>
    public const string KEY_ALLNICKSESSIONINFO = "CacheKey_NickSessionList";

    /// <summary>
    ///  所有订购用户信息缓存(12小时更新一次)
    /// </summary>
    /// <returns></returns>
    public static IList<TeteShopInfo> GetNickSessionList()
    {
        return new CacheUtility<IList<TeteShopInfo>>().Get(KEY_ALLNICKSESSIONINFO, delegate()
        {
            TeteShopService nickDal = new TeteShopService();
            return nickDal.GetAllShopInfo();
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
