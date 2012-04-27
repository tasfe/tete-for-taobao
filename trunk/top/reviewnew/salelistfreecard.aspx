<%@ Page Language="C#" AutoEventWireup="true" CodeFile="salelistfreecard.aspx.cs" Inherits="top_reviewnew_salelist" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 2次购买记录 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <table width="720" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>买家昵称</b></td>
                <td width="60"><b>订单号</b></td>
                <td width="80"><b>金额</b></td>
                <td width="90"><b>下单时间</b></td>
                <td width="100"><b>优惠券编号</b></td>
                <td width="70"><b>优惠金额</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="30"><%#Eval("buynick") %></td>
                <td><%#Eval("orderid") %></td>
                <td><%#Eval("totalprice").ToString()%></td>
                <td><%#Eval("adddate") %></td>
                <td><%#Eval("couponnumber") %></td>
                <td><%#Eval("couponprice") %></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

        <div>
            <asp:Label ID="lbPage" runat="server"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
