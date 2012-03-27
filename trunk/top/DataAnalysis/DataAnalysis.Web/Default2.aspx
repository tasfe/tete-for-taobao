<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .all
        {
            width: 1200px;
        }
        .detail
        {
            width: 60px;
            float: left;
        }
        .detail div
        {
            font-size: 12px;
            height: 30px;
        }
        .detail1
        {
            width: 90px;
            float: left;
        }
        .detail1 div
        {
            font-size: 12px;
            height: 30px;
        }
    </style>
    
    <style type="text/css">  /*Tooltips*/
.tooltips{  position:relative; /*这个是关键*/
z-index:2;  }  .tooltips:hover{  z-index:3;  background:none; /*没有这个在IE中不可用*/
}  .tooltips span{  display: none;  }  .tooltips:hover span{ /*span 标签仅在 :hover 状态时显示*/
display:block;  position:absolute;  top:21px;  left:9px;  width:15em;  border:1px solid black;  background-color:#ccFFFF;  padding: 3px;  color:black;  }
</style> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="Btn_AddCookie"  Text="点击查看客户真实数据" runat="server" 
            onclick="Btn_AddCookie_Click" /><font size="2">此处只是为了让审核人员能够更直观的看到软件真实效果</font>
        <div class="all">
            <div class="detail1">
                <div>
                </div>
                <div>
                    <a href="VisitTotal.aspx" class="tooltips">PV
                     <span>店铺浏览页面量</span>
                     </a>
                </div>
                <div>
                    UV
                </div>
                <div>
                    直通车
                </div>
                <div>
                    钻展流量
                </div>
                <div>
                    询单数
                </div>
                <div>
                     <a href="AskOrderTotal.aspx" class="tooltips">丢单率
                     <span>丢单率等于询问客服人中成功下订单的人数除以询问客服的总人数</span>
                     </a>
                </div>
                <div>
                    订单数
                </div>
                <div>
                    订单总价
                </div>
                <div>
                    销售单价
                </div>
                <div>
                    客单价
                </div>
                <div>
                    转化率
                </div>
                <div>
                    回头客
                </div>
                <div>
                    浏览回头率
                </div>
                <div>
                    退款率
                </div>
                <div>
                    平均访问深度
                </div>
                <div style="height:60px">
                    浏览排行
                </div>
                <div style="height:60px">
                    销售排行
                </div>
                <%--<div>
                    收藏量
                </div>--%>
            </div>
            <asp:Repeater runat="server" ID="Rpt_TotalList">
                <ItemTemplate>
                    <div class="detail">
                        <div>
                            <%# GetMonthDay(Eval("SiteTotalDate").ToString()) %>
                        </div>
                        <div>
                            <%# Eval("SitePVCount")%>
                        </div>
                        <div>
                            <%# Eval("SiteUVCount")%>
                        </div>
                        <div>
                            <%# Eval("ZhiTongFlow")%>
                        </div>
                        <div>
                            <%# Eval("SiteZuanZhan")%>
                        </div>
                        <div>
                            <%# Eval("AskOrder")%>
                        </div>
                        <div>
                            <%# Eval("LostOrder")%>
                        </div>
                        <div>
                            <%# Eval("SiteOrderCount")%>
                        </div>
                        <div>
                            <%# Eval("SiteOrderPay")%>
                        </div>
                        <div>
                            <%# Eval("SellAvg")%>
                        </div>
                        <div>
                            <%# Eval("OneCustomerPrice")%>
                        </div>
                        <div>
                            <%# Eval("CreateAVG")%>
                        </div>
                        <div>
                            <%# Eval("SiteSecondBuy")%>
                        </div>
                        <div>
                            <%# Eval("BackSee") %>
                        </div>
                        <div>
                            <%# Eval("Refund")%>
                        </div>
                        <div>
                            <%# Eval("SeeDeepAVG")%>
                        </div>
                        <div style="height:60px">
                           <img src='<%# Eval("SeeTop.pic_url")%>_60x60.jpg' alt='<%# Eval("SeeTop.title")%>' height="50px" />
                        </div>
                        <div style="height:60px">
                           <img src='<%# Eval("SellTop.pic_url")%>_60x60.jpg' alt='<%# Eval("SellTop.title")%>' height="50px" />
                        </div>
                       <%--     <div>
                                <%# Eval("Collection")%>
                            </div>--%>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        
    </div>
    </form>
</body>
</html>
