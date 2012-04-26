﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customlist.aspx.cs" Inherits="top_crm_customlist" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特特CRM_客户营销</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 客户列表 </div>
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
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="搜索" />
    <input type="button" value="优惠券赠送" onclick="window.location.href='msgsend.aspx?typ=<%=typ %>'" />
        <asp:Button ID="Button1" runat="server" Text="导出全部会员数据" 
            onclick="Button1_Click" />
        <hr />

        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>客户昵称</b></td>
                <td width="50"><b>省</b></td>
                <!--<td width="50"><b>市</b></td>
                <td width="50"><b>区</b></td>-->
                <td width="85"><b>手机</b></td>
                <td width="40"><b>性别</b></td>
                <td width="50"><b>等级</b></td>
                <td width="50"><b>交易量</b></td>
                <td width="50"><b>交易额</b></td>
                <td width="55"><b>最后交易</b></td>
                <td width="55"><b>生日</b></td>
                <td width="60"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("buynick") %> <img src='level/<%#Eval("buyerlevel") %>.gif' valign="middle" /></td>
                <td><%#Eval("sheng")%></td>
                <!--<td><%#Eval("shi")%></td>
                <td><%#Eval("qu")%></td>-->
                <td><%#Eval("mobile")%></td>
                <td><%#getsex(Eval("sex").ToString())%></td>
                <td><%#getgrade(Eval("grade").ToString())%></td>
                <td><%#Eval("tradecount")%></td>
                <td><%#Eval("tradeamount")%></td>
                <td><%#DateTime.Parse(Eval("lastorderdate").ToString()).ToString("yyyy-MM-dd")%></td>
                <td><%#getdateshort(Eval("birthday").ToString())%></td>
                <td> <a href='custommodify.aspx?id=<%#Eval("guid")%>' target="_blank">编辑</a> </td>
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
