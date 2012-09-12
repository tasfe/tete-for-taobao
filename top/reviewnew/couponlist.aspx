<%@ Page Language="C#" AutoEventWireup="true" CodeFile="couponlist.aspx.cs" Inherits="top_review_couponlist" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 优惠券列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
                <input type="button" value="添加优惠券" onclick="window.location.href='couponadd.aspx'" />
    
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="90"><b>优惠券名称</b></td>
                <td width="40"><b>金额</b></td>
                <td width="70"><b>领取数量</b></td>
                <td width="100"><b>自动赠送截至日期</b></td>
                <td width="100"><b>有效日期</b></td>
                <td width="60"><b>使用条件</b> </td>
                <td width="90"><b>总量/已领用</b> </td>
                <td width="40"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("name")%></td>
                <td><%#Eval("num")%></td>
                <td><%#Eval("per")%></td>
                <td><%#Eval("enddate").ToString().Replace("0:00:00", "")%></td>
                <td><%#Eval("endsenddate").ToString().Replace("0:00:00", "")%></td>
                <td><%#Eval("condition")%></td>
                <td><%#Eval("count")%> / <%#Eval("used")%></td>
                <td><a href='couponmodify.aspx?id=<%#Eval("guid")%>'>修改</a> | <a href='couponlist.aspx?act=del&id=<%#Eval("guid")%>' onclick="return confirm('您确定要删除吗，该操作不可恢复？')">删除</a></td>
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
