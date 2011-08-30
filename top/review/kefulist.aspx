﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="kefulist.aspx.cs" Inherits="top_review_kefulist" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 买家好评审核 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
    此处取得的评价结果可能跟实际有1个小时左右的延迟，请您耐心等待~<br />
    如果您开启了基本设置中的“是否开启评价审核”，则所有的评价都将由您自己审核是否赠送礼品~
</div>

    请输入买家昵称：<asp:TextBox ID="search" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="搜索" />

    <input type="button" value="查看历史处理结果" onclick="window.location.href='oldkefulist.aspx'" />
    
    <hr />
        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>买家</b></td>
                <td width="120"><b>订单号</b></td>
                <td width="60"><b>下单日期 </b></td>
                <td width="60"><b>评价日期 </b></td>
                <td width="60"><b>评价等级 </b></td>
                <td width="60"><b>评价内容 </b></td>
                <td width="160"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("buynick")%></td>
                <td><%#Eval("orderid")%></td>
                <td><%#Eval("addtime")%></td>
                <td><%#Eval("reviewtime")%></td>
                <td><img src='<%#getimg(Eval("result").ToString())%>' /></td>
                <td><%#left(Eval("content").ToString())%></td>
                <td>
                    <a href='kefulist.aspx?id=<%#Eval("orderid")%>&act=send&send=1'>赠送</a> | <a href='kefulist.aspx?id=<%#Eval("orderid")%>&act=send&send=2'>不赠送</a>
                </td>
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