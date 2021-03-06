﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reviewlistnew.aspx.cs" Inherits="top_review_reviewlist" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 评价列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

    测试人员如果店铺内没有评价数据，请联系客户人员索取测试数据，非常感谢！<br />
    
    请输入买家昵称：<asp:TextBox ID="search" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="搜索" />
    
    <hr />

    <table width="720" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>评价人</b></td>
                <td width="60"><b>评分 </b></td>
                <td width="280"><b>内容 </b></td>
                <td width="100"><b>时间</b></td>
                <td width="70"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="30"><%#Eval("buynick") %></td>
                <td><img src='<%#getimg(Eval("result").ToString())%>' /></td>
                <td><%#left(Eval("content").ToString())%></td>
                <td><%#Eval("reviewdate") %></td>
                <td><a href="http://item.tmall.com/item.htm?id=<%#Eval("itemid") %>" title='<%#Eval("sendresult") %>' target="_blank">查看宝贝</a> |
                <a href='reviewindexnew.aspx?act=add&id=<%#Eval("orderid") %>'>展示该评价</a>
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