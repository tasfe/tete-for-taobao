<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freecardlog.aspx.cs" Inherits="top_freecard_freecardlog" %>

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

  <div class="crumbs"><a href="../reviewnew/default.aspx" class="nolink">好评有礼</a> 包邮卡使用记录 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
                <input type="button" value="返回" onclick="window.location.href='freecardcustomer.aspx'" />
    
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="140"><b>订单号</b></td>
                <td width="80"><b>买家昵称</b></td>
                <td width="100"><b>使用时间</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("orderid")%></td>
                <td><%#Eval("buynick")%></td>
                <td><%#Eval("adddate")%></td>
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