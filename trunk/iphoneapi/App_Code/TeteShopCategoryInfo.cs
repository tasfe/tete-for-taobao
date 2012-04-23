using System;

/// <summary>
/// Summary description for TeteShopCategoryInfo
/// </summary>
public class TeteShopCategoryInfo
{
    private Int32 _Id;
    public Int32 Id
    {
        get { return _Id; }
        set { _Id = value; }
    }
    private String _Cateid;
    public String Cateid
    {
        get { return _Cateid; }
        set { _Cateid = value; }
    }
    private String _Catename;
    public String Catename
    {
        get { return _Catename; }
        set { _Catename = value; }
    }
    private Int32 _Catecount;
    public Int32 Catecount
    {
        get { return _Catecount; }
        set { _Catecount = value; }
    }
    private String _Parentid;
    public String Parentid
    {
        get { return _Parentid; }
        set { _Parentid = value; }
    }
    private String _Nick;
    public String Nick
    {
        get { return _Nick; }
        set { _Nick = value; }
    }
    private String _Catepicurl;
    public String Catepicurl
    {
        get { return _Catepicurl; }
        set { _Catepicurl = value; }
    }
}
