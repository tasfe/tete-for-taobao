using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TopNickSessionInfo
{
    /// <summary>
    /// 服务ID
    /// </summary>
    public Enum.TopTaoBaoService ServiceId { set; get; }

    public string Nick { get; set; }

    public string Session { set; get; }

    public DateTime LastGetOrderTime { set; get; }

    public bool NickState { set; get; }

    public DateTime JoinDate { set; get; }

    /// <summary>
    /// 店铺ID
    /// </summary>
    public string ShopId { set; get; }

    /// <summary>
    /// 更新token
    /// </summary>
    public string RefreshToken { set; get; }
}
