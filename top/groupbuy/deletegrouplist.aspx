﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deletegrouplist.aspx.cs" Inherits="top_groupbuy_deletegrouplist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特团购</a> 已取消团购列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>


    <div id="main-content">

        <input type="button" value="添加团购" onclick="window.location.href='groupbuyadd.aspx'" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <b style=" color:Red"> 清除关联描述商品有漏掉的没清除时候,请点击强力清除描述</b>
    <hr />

       <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
                <td width="150"><b>活动名称</b></td>
                <td width="100"><b>宝贝</b></td>
                <td width="60"><b>原价</b></td>
                <td width="60"><b>团购价</b></td>
                <td width="60"><b>已参团</b></td>
                <td width="80"><b>开始时间</b></td>
                <td width="80"><b>结束时间</b></td>
                <td width="120"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="90"><%#Eval("name").ToString()%></td>
                <td>
                    <a href='http://item.taobao.com/item.htm?id=<%#Eval("productid").ToString() %>' target="_blank"><img width="80" height="80" src='<%#Eval("productimg").ToString() %>_80x80.jpg' alt='<%#Eval("productname").ToString() %>' border="0" /></a> 
                </td>
                <td><s><%#Eval("productprice").ToString() %></s></td>
                <td style="color:Red; font-weight:bold; font-size:14px"><%#Eval("groupbuyprice").ToString() %></td>
                <td><%#Eval("buycount").ToString() %></td>
                <td><%#Eval("starttime")%></td>
                <td><%#Eval("endtime").ToString() %></td>
                <td>
                    活动已取消 | <a href='grouplist.aspx?id=<%#Eval("id").ToString()%>&act=del' onclick="return confirm('有的时候取消活动因为网络问题可能失败，点此再次发送取消请求')">取消</a> <br />
                    <a href='groupbuydetail.aspx?id=<%#Eval("id").ToString()%>'>查看团购订单</a>  |  <a href='deletetaobao.aspx?id=<%#Eval("id").ToString()%>' onclick="return confirm('您确认要清除关联描述，该操作不可恢复？')">清除关联描述</a>
                    |  <a href='deletetaobao.aspx?type=dle&id=<%#Eval("id").ToString()%>' onclick="return confirm('您确认要清除关联描述，该操作不可恢复？')">强力清除描述</a>
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
