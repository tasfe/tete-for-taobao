using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TopNickSessionInfo
{
    public string Nick { get; set; }

    public string Session { set; get; }

    public DateTime LastGetOrderTime { set; get; }

    public bool NickState { set; get; }

    public DateTime JoinDate { set; get; }
}
