﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customlist.aspx.cs" Inherits="top_reviewnew_customlist" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> CRM客户列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <input type="button" value="优惠券批量赠送" onclick="window.location.href='msgsend.aspx?typ=<%=typ %>'" />
        <input type="button" value="短信营销" onclick="window.location.href='msgsend.aspx?typ=<%=typ %>'" />
        <input type="button" value="导出会员数据" onclick="window.location.href='msgsend.aspx?typ=<%=typ %>'" />
        <hr />

        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>客户昵称</b></td>
                <td width="50"><b>省</b></td>
                <td width="50"><b>市</b></td>
                <td width="50"><b>区</b></td>
                <td width="85"><b>手机</b></td>
                <td width="40"><b>性别</b></td>
                <td width="50"><b>等级</b></td>
                <td width="50"><b>交易量</b></td>
                <td width="50"><b>交易额</b></td>
                <td width="60"><b>最后交易</b></td>
                <td width="60"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("buynick") %> <img src='level/<%#Eval("buyerlevel") %>.gif' valign="middle" /></td>
                <td><%#Eval("sheng")%></td>
                <td><%#Eval("shi")%></td>
                <td><%#Eval("qu")%></td>
                <td><%#Eval("mobile")%></td>
                <td><%#getsex(Eval("sex").ToString())%></td>
                <td><%#getgrade(Eval("grade").ToString())%></td>
                <td><%#Eval("tradecount")%></td>
                <td><%#Eval("tradeamount")%></td>
                <td><%#Eval("lastorderdate").ToString().Replace(" 0:00:00", "")%></td>
                <td> <a href='custommodify.aspx?id=<%#Eval("guid")%>' target="_blank">编辑会员资料</a> </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>

    </div>
</div>
    </div>
    </form>
</body>
</html>
