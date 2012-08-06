<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>主页</title>
    <link href="css/common.css" rel="stylesheet" />
    <style>
        td
        {
            font-size: 12px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
    </style>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height: 600px;">
            <div class="crumbs">
                <a href="javascript:;" class="nolink">销售分析王</a> 店铺统计总览
            </div>
            <div class="absright">
                <ul>
                    <li>
                        <div class="msg">
                        </div>
                    </li>
                </ul>
            </div>
            <div id="main-content">
                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <asp:Button ID="Btn_AddCId" runat="server" Text="一键添加统计代码" Font-Size="14" Height="50"
                        Width="400" OnClick="Btn_AddCId_Click" />
                </asp:Panel>
                <div>
                    <div style="font-size: 18px; font-weight: bold;">
                        销售数据统计 - <span style="font-size: 14px; font-weight: normal; color: #333333">今日
                            <asp:Repeater ID="Rpt_IpPV" runat="server">
                                <ItemTemplate>
                                    【<%#Eval("Key")%>-<font color='green'><%#Eval("Value")%></font>】
                                </ItemTemplate>
                            </asp:Repeater>
                            </span></div>
                    <hr />
                    <div>
                        <table width="740" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="120" align="center">
                                </td>
                                <td width="120" align="center">
                                    <b>销售额</b>
                                </td>
                                <td width="120" align="center">
                                    <b>订单数</b>
                                </td>
                                <td width="120" align="center">
                                    <b>回头订单数</b>
                                </td>
                                <td width="100" align="center">
                                    <b>客单价</b>
                                </td>
                                <td width="100" align="center">
                                    <b>销售单价</b>
                                </td>
                                <td width="100" align="center">
                                    <b>销售关联数</b>
                                </td>
                            </tr>
                            <asp:Repeater ID="Rpt_OrderTotal" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td height="25" style="font-size: 14px; font-weight: bold;" align="center">
                                            <%# Eval("SiteNick")%>
                                        </td>
                                        <td align="center">
                                           <%# Eval("SiteOrderPay")%>
                                        </td>
                                        <td align="center">
                                            <%# Eval("SiteOrderCount") %>
                                        </td>
                                        <td align="center">
                                           <%# Eval("SiteSecondBuy")%>
                                        </td>
                                        <td align="center">
                                            <%# GetCusOne(Eval("SiteOrderPay").ToString(), Eval("SiteBuyCustomTotal").ToString()) %>
                                        </td>
                                        <td align="center">
                                            <%# GetSellOne(Eval("SiteOrderPay").ToString(), Eval("GoodsCount").ToString())%>
                                        </td>
                                        <td align="center">
                                            <%# GetEval(Eval("SiteOrderPay").ToString(), Eval("SiteBuyCustomTotal").ToString(), Eval("GoodsCount").ToString())%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                        <td height="25" style="font-size: 14px; font-weight: bold;" align="center">
                                            最近7天走势
                                        </td>
                                        <td align="center">
                                            <%=SevenSitePay %>
                                        </td>
                                        <td align="center">
                                            <%=SevenSiteOrderCount()%>
                                        </td>
                                        <td align="center">
                                           <%=SevenSiteBackOrder()%>
                                        </td>
                                        <td align="center">
                                           <%=SevenSiteOnePay()%>
                                        </td>
                                        <td align="center">
                                           <%=SevenSiteSellOnePay()%>
                                        </td>
                                        <td align="center">
                                           <%=SevenSiteSellWith()%>
                                        </td>
                                    </tr>
                        </table>
                    </div>
                </div>
                <div style="padding-top: 40px;">
                    <div style="font-size: 18px; font-weight: bold;">
                        爆款宝贝排行</div>
                    <hr />
                    <div>
                        <table width="740" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="70">
                                </td>
                                <td width="120" align="center">
                                    <b>图片</b>
                                </td>
                                <td width="220" align="center">
                                    <b>宝贝名称</b>
                                </td>
                                <td width="120" align="center">
                                    <b>价格</b>
                                </td>
                                <td width="120" align="center">
                                    <b>销售数量</b>
                                </td>
                            </tr>
                            <asp:Repeater runat="server" ID="Rpt_GoodsSellTop">
                                <ItemTemplate>
                                    <tr>
                                        <td align="center">
                                            NO.<%# Container.ItemIndex + 1%>
                                        </td>
                                        <td align="center">
                                            <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' style="color: Black"
                                                target="_blank">
                                                <img src='<%# Eval("pic_url") %>_80x80.jpg' border="0" alt='<%#Eval("title")%>' />
                                            </a>
                                        </td>
                                        <td>
                                            <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' style="color: Black"
                                                target="_blank">
                                                <%#Eval("title")%>
                                            </a>
                                        </td>
                                        <td align="center">
                                            ￥<%#Eval("price")%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("Count")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
                <div style="padding-top: 40px;">
                    <div style="font-size: 18px; font-weight: bold;">
                        店铺浏览统计</div>
                    <hr />
                    <div>
                        <table width="740" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="120" align="center">
                                    <b>来访者IP</b>
                                </td>
                                <td width="120" align="center">
                                    <b>来访者时间</b>
                                </td>
                                <td width="120" align="center">
                                    <b>来访者所在地区</b>
                                </td>
                                <td width="120" align="center">
                                    <b>来访者网络提供商</b>
                                </td>
                                <td width="120" align="center">
                                    <b>查看访问轨迹</b>
                                </td>
                            </tr>
                            <asp:Repeater ID="Rpt_OnlineCustomer" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td align="center">
                                            <%#Eval("VisitIP")%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("VisitTime")%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("IPLocation")%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("NetWork")%>
                                        </td>
                                        <td align="center">
                                            <a href='UVisitPage.aspx?visitip=<%#Eval("VisitIP")%>' style="color: Black">查看 </a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
