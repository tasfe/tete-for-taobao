using System;

public class TeteUserTokenInfo
{
    private Int32 _Id;
    public Int32 Id
    {
        get { return _Id; }
        set { _Id = value; }
    }
    private String _Nick;
    public String Nick
    {
        get { return _Nick; }
        set { _Nick = value; }
    }
    private String _Token;
    public String Token
    {
        get { return _Token; }
        set { _Token = value; }
    }
    private DateTime _Adddate;
    public DateTime Adddate
    {
        get { return _Adddate; }
        set { _Adddate = value; }
    }
    private String _Mobile;
    public String Mobile
    {
        get { return _Mobile; }
        set { _Mobile = value; }
    }
    private DateTime _Updatedate;
    public DateTime Updatedate
    {
        get { return _Updatedate; }
        set { _Updatedate = value; }
    }
    private Int32 _Logintimes;
    public Int32 Logintimes
    {
        get { return _Logintimes; }
        set { _Logintimes = value; }
    }
}