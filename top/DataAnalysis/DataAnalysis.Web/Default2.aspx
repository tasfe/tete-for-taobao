<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .all
        {
            width: 900px;
        }
        .detail
        {
            width: 60px;
            float: left;
        }
        .detail div
        {
            font-size: 12px;
            height: 20px;
        }
        .detail1
        {
            width: 90px;
            float: left;
        }
        .detail1 div
        {
            font-size: 12px;
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="all">
            <div class="detail1">
                <div>
                </div>
                <div>
                    PV
                </div>
                <div>
                    UV
                </div>
                <div>
                    直通车
                </div>
                <div>
                    CPC
                </div>
                <div>
                    询单数
                </div>
                <div>
                    丢单率
                </div>
                <div>
                    订单数
                </div>
                <div>
                    订单总价
                </div>
                <div>
                    每单均价
                </div>
                <div>
                    客单价
                </div>
                <div>
                    转化率
                </div>
                <div>
                    二次浏览用户数
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
                <div>
                    浏览排行
                </div>
                <div>
                    销售排行
                </div>
                <div>
                    收藏量
                </div>
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
                            <%# Eval("CPC")%>
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
                            <%# Eval("OneOrderPrice")%>
                        </div>
                        <div>
                            <%# Eval("OneCustomerPrice")%>
                        </div>
                        <div>
                            <%# Eval("CreateAVG")%>
                        </div>
                        <div>
                            <%# Eval("SiteUVBack")%>
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
                        <div>
                           <img  src="" alt='<%# Eval("SeeTop")%>' width="1" height="1"/>
                            <%# GetGoodsName(Eval("SeeTop").ToString())%>
                        </div>
                        <div> <img  src="" alt='<%# Eval("SellTop")%>' width="1" height="1"/>
                            <%# GetGoodsName(Eval("SellTop").ToString())%>
                        </div>
                        <div>
                            <%# Eval("Collection")%>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        
    </div>
    </form>
</body>
</html>
