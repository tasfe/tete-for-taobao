using System.Reflection;
using System;

/// <summary>
/// Summary description for GoodsInfo
/// </summary>
public class GoodsInfo
{
    public GoodsInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string num_iid { set; get; }

    public string title { set; get; }

    public string nick { set; get; }

    public decimal price { set; get; }

    public int Count { set; get; }

    public string pic_url { set; get; }

    public override bool Equals(object obj)
    {
        if (obj is GoodsInfo)
        {
            if ((obj as GoodsInfo).num_iid == num_iid)
                return true;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        if (num_iid == string.Empty) return base.GetHashCode();
        string stringRepresentation = MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" + num_iid;
        return stringRepresentation.GetHashCode();
    }
}

[Serializable]
public class GoodsClassInfo
{
    public string cid { set; get; }

    public string name { set; get; }

    public string pic_url { set; get; }

    public int sort_order { set; get; }

    /// <summary>
    /// 父类目ID=0时，代表的是一级的类目
    /// </summary>
    public string parent_cid { set; get; }

    public string status { set; get; }

    public string type { set; get; }
}