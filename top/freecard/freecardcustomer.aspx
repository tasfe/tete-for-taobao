﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freecardcustomer.aspx.cs" Inherits="top_freecard_freecardcustomer" %>


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

  <div class="crumbs"><a href="../reviewnew/default.aspx" class="nolink">好评有礼</a> 包邮卡赠送记录 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
                <input type="button" value="返回列表" onclick="window.location.href='freecardlist.aspx'" />
                <input type="button" value="手动赠送" onclick="window.location.href='freecardsend.aspx'" />
    
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="80"><b>买家昵称</b></td>
                <td width="100"><b>赠送时间</b></td>
                <td width="80"><b>免邮时间</b></td>
                <td width="50"><b>使用次数</b></td>
                <td width="60"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("buynick")%></td>
                <td><%#Eval("startdate")%></td>
                <td><%#Eval("carddate")%></td>
                <td><%#Eval("usecount")%></td>
                <td><a href='freecardlog.aspx?id=<%#Eval("guid")%>'>查看使用记录</a></td>
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