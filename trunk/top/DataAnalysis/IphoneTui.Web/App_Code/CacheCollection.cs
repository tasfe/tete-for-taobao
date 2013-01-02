using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Keede.Caching;
using System.Collections;

public class CacheCollection
{
    /// <summary>
    /// 所有网站信息
    /// </summary>
    public const string KEY_ALLSITEINFO = "CacheKey_SiteList";

    /// <summary>
    /// 所有收费信息
    /// </summary>
    public const string KEY_ALLFEENINFO = "CacheKey_FeeList";

    /// <summary>
    /// 所有购买信息
    /// </summary>
    public const string KEY_ALLBUYNINFO = "CacheKey_BuyList";

    /// <summary>
    /// 所有广告信息
    /// </summary>
    public const string KEY_ALLADSINFO = "CacheKey_AdsList";

    public const string KEY_ALLSITEADS = "CacheKey_SiteAds";

    /// <summary>
    ///  所有网站信息缓存(12小时更新一次)
    /// </summary>
    /// <returns></returns>
    public static IList<SiteInfo> GetAllSiteList()
    {
        return new CacheUtility<IList<SiteInfo>>().Get(KEY_ALLSITEINFO, delegate()
        {
            AdsSiteService adsSiteDal = new AdsSiteService();
            return adsSiteDal.SelectAllSite();
        }, 60 * 12);
    }

    /// <summary>
    /// 收费分类集合
    /// </summary>
    /// <returns></returns>
    public static IList<FeeInfo> GetAllFeeInfo()
    {
        return new CacheUtility<IList<FeeInfo>>().Get(KEY_ALLFEENINFO, delegate()
        {
            FeeService feeDal = new FeeService();
            return feeDal.SelectAllFee();
        }, 60 * 12);
    }

    /// <summary>
    /// 所有购买信息
    /// </summary>
    /// <returns></returns>
    public static IList<BuyInfo> GetAllBuyInfo()
    {
        return new CacheUtility<IList<BuyInfo>>().Get(KEY_ALLBUYNINFO, delegate()
        {
            FeeService feeDal = new FeeService();
            return feeDal.SelectAllBuy();
        }, 60 * 12);
    }

    /// <summary>
    /// 所有广告位
    /// </summary>
    /// <returns></returns>
    public static IList<AdsInfo> GetAllAdsInfo()
    {
        return new CacheUtility<IList<AdsInfo>>().Get(KEY_ALLADSINFO, delegate()
        {
            AdsSiteService asDal = new AdsSiteService();
            return asDal.SelectAllAds();
        }, 60 * 12);
    }

    public static IList<SiteAdsInfo> GetAllSiteAdsInfo()
    {
        return new CacheUtility<IList<SiteAdsInfo>>().Get(KEY_ALLSITEADS, delegate()
        {
            return new SiteAdsService().SelectAllSiteAds();
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
