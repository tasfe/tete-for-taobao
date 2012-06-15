<%@ Page Language="C#" AutoEventWireup="true" CodeFile="callback.aspx.cs" Inherits="top_callback" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div style="border:solid 1px red;">
        <table>
            <tr>
                <td>
                <img src='images/frametop.jpg' />
        您好，亲爱的<%=buynick %> <br />
        优惠券数量： <%=coupon %>张<br />
        支付宝红包： <%=alipay %>张<br />
        包邮卡： <%=freecard %>张<br />
        <hr />
        最新获奖：<br />
        <marquee direction="up">
            <asp:Repeater ID="rptTradeRate" runat="server">
                <ItemTemplate>
                    <%#Eval("buynick") %> 于 <%#Eval("senddate") %>获得优惠券1张，编号<%#Eval("taobaonumber")%><br />
                </ItemTemplate>
            </asp:Repeater>
        </marquee>

                </td>
                <td>
                
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                <table>
                    <tr>
                        <td><a href='http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>'><img border=0 src='http://img01.taobaocdn.com/bao/uploaded/i1/T1NXS7XdRrXXaY4Bo0_035941.jpg_160x160.jpg' height=120 /></a></td>
                        <td><%#Eval("itemid") %><br /> <%#Eval("content") %> <br /> 分享到： ----- <%#Eval("buynick") %></td>
                    </tr>
                </table>
                </ItemTemplate>
            </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
