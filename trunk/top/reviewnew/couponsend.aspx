<%@ Page Language="C#" AutoEventWireup="true" CodeFile="couponsend.aspx.cs" Inherits="top_review_couponsend" %>
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
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 优惠券赠送列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    
    请输入买家昵称：<asp:TextBox ID="search" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="搜索" />
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="导出优惠券赠送记录" />
    
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>名称</b></td>
                <td width="100"><b>优惠券编号</b></td>
                <td width="120"><b>买家</b></td>
                <td width="70"><b>优惠金额</b></td>
                <td width="120"><b>订单号</b></td>
                <td width="70"><b>订单金额</b></td>
                <td width="120"><b>赠送日期 </b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="30"><%#Eval("name") %></td>
                <td height="30"><%#Eval("taobaonumber") %></td>
                <td><%#Eval("buynick") %></td>
                <td>满<%#Eval("condition") %>元减<%#Eval("num") %>元</td>
                <td><%#Eval("orderid") %></td>
                <td><%#Eval("totalprice")%></td>
                <td><%#Eval("senddate") %></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>
    </div>
</div>
</form>

</body>
</html>