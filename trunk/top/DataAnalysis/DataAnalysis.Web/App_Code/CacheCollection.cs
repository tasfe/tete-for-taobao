using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Keede.Caching;
using System.Collections;
using Model;
using CusServiceAchievements.DAL;

public class CacheCollection
{
    /// <summary>
    /// 所有订购商户信息
    /// </summary>
    public const string KEY_ALLNICKSESSIONINFO = "CacheKey_NickSessionList";

    /// <summary>
    /// 省市信息
    /// </summary>
    public const string KEY_ALLPROVINCECITY = "CacheKey_ProvinceCity";

    /// <summary>
    /// 快递信息
    /// </summary>
    public const string KEY_ALLEXPRESS = "CacheKey_Express";

    /// <summary>
    ///  所有订购用户信息缓存(12小时更新一次)
    /// </summary>
    /// <returns></returns>
    public static IList<TopNickSessionInfo> GetNickSessionList()
    {
        return new CacheUtility<IList<TopNickSessionInfo>>().Get(KEY_ALLNICKSESSIONINFO, delegate()
        {
            NickSessionService nickDal = new NickSessionService();
            return nickDal.GetAllNickSession();
        }, 60 * 12);
    }

    public static IList<ProvinceInfo> GetAllProvinceInfo()
    {
        return new CacheUtility<IList<ProvinceInfo>>().Get(KEY_ALLNICKSESSIONINFO, delegate()
        {
            ProvinceService proviDal = new ProvinceService();
            return proviDal.GetAllProvince();
        }, 60 * 12);
    }

    public static IList<ExpressInfo> GetAllExpressInfo()
    {
        return new CacheUtility<IList<ExpressInfo>>().Get(KEY_ALLNICKSESSIONINFO, delegate()
        {
            ExpressService exprDal = new ExpressService();
            return exprDal.GetAllExpressInfo("");
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
