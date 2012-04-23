using System;

public class TeteUserMsgInfo
{
    private Int32 _Id;
    public Int32 Id
    {
        get { return _Id; }
        set { _Id = value; }
    }
    private String _Title;
    public String Title
    {
        get { return _Title; }
        set { _Title = value; }
    }
    private String _Html;
    public String Html
    {
        get { return _Html; }
        set { _Html = value; }
    }
    private DateTime _Adddate;
    public DateTime Adddate
    {
        get { return _Adddate; }
        set { _Adddate = value; }
    }
    private String _Nick;
    public String Nick
    {
        get { return _Nick; }
        set { _Nick = value; }
    }
    private Int32 _Isread;
    public Int32 Isread
    {
        get { return _Isread; }
        set { _Isread = value; }
    }
    private String _Token;
    public String Token
    {
        get { return _Token; }
        set { _Token = value; }
    }
}
