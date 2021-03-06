﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="alipaydetail.aspx.cs" Inherits="top_reviewnew_alipay" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>二次销售魔方</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">二次销售魔方</a> 支付宝红包赠送详情 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

 <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
    此处的赠送成功是指客户已经接受到包含您支付红包卡号密码的短信
</div>

                <input type="button" value="返回列表" onclick="window.location.href='alipay.aspx'" />
    
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="140"><b>卡号</b></td>
                <td width="80"><b>密码</b></td>
                <td width="140"><b>领取人</b></td>
                <td width="120"><b>赠送日期</b> </td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("card")%></td>
                <td><%#Eval("pass")%></td>
                <td><%#Eval("buynick")%></td>
                <td><%#Eval("senddate")%></td>
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
