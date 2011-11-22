<%@ Page Language="C#" AutoEventWireup="true" CodeFile="itemsend.aspx.cs" Inherits="top_review_itemsend" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 礼品赠送记录 </div>
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
    
    <hr />


    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>买家</b></td>
                <td width="60"><b>赠送日期 </b></td>
                <td width="60"><b>结果 </b></td>
                <td width="120"><b>礼品名称</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr><td height="30"><%#Eval("sendto") %></td>
                <td><%#Eval("adddate") %></td>
                <td>该买家获得1分钱购买此商品的优惠</td>
                <td><a href="http://item.tmall.com/item.htm?id=<%#Eval("itemid") %>" target="_blank">查看宝贝</a></td>
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