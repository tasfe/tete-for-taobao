<%@ Page Language="C#" AutoEventWireup="true" CodeFile="groupbuydetail.aspx.cs" Inherits="top_groupbuy_groupbuydetail" %>

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

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">特特团购</a> 团购购买详情 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

    <input type="button" value="返回列表" onclick="window.location.href='grouplist.aspx'" />
    
    <hr />

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
                <td width="150"><b>活动名称</b></td>
                <td width="100"><b>宝贝</b></td>
                <td width="60"><b>原价</b></td>
                <td width="60"><b>团购价</b></td>
                <td width="40"><b>限购</b></td>
                <td width="60"><b>已参团</b></td>
                <td width="80"><b>开始时间</b></td>
                <td width="80"><b>结束时间</b></td>
                <td width="120"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
            <tr>
                <td height="90"><%#Eval("name").ToString()%></td>
                <td>
                    <a href='http://item.taobao.com/item.htm?id=<%#Eval("productid").ToString() %>' target="_blank"><img width="80" height="80" src='<%#Eval("productimg").ToString() %>_80x80.jpg' alt='<%#Eval("productname").ToString() %>' border="0" /></a> 
                </td>
                <td><s><%#Eval("productprice").ToString() %></s></td>
                <td style="color:Red; font-weight:bold; font-size:14px"><%#Eval("groupbuyprice").ToString() %></td>
                <td><%#Eval("maxcount").ToString() %></td>
                <td><%#Eval("buycount").ToString() %></td>
                <td><%#Eval("starttime")%></td>
                <td><%#Eval("endtime").ToString() %></td>
                <td>
                    <a href='groupbuyadd.aspx?id=<%#Eval("id").ToString()%>'>修改</a> | 
                    <a href='groupbuydetail.aspx?id=<%#Eval("id").ToString()%>&act=del' onclick="return confirm('您确认要取消团购活动，该操作不可恢复？')">取消团购</a>
                </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <hr />

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>会员名称</b></td>
                <td width="60"><b>购买数量</b></td>
                <td width="140"><b>购买时间</b></td>
                <td width="100"><b>订单编号</b></td>
                <td width="180"><b>状态</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="40"><%#Eval("buynick").ToString()%></td>
                <td><%#Eval("count").ToString() %></td>
                <td><%#Eval("adddate") %></td>
                <td><%#splitstr(Eval("ordernumber").ToString())%></td>
                <td><%#splitstrnew(Eval("payStatus").ToString())%></td>
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
