<%@ Page Language="C#" AutoEventWireup="true" CodeFile="oldkefulist.aspx.cs" Inherits="top_review_oldkefulist" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 买家好评审核处理结果 </div>
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
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="搜索" />
                <input type="button" value="返回待审核评价列表" onclick="window.location.href='kefulist.aspx'" />
    
    <hr />
        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>买家</b></td>
                <td width="120"><b>订单号</b></td>
                <td width="60"><b>下单日期 </b></td>
                <td width="60"><b>评价日期 </b></td>
                <td width="60"><b>评价等级 </b></td>
                <td width="60"><b>评价内容 </b></td>
                <td width="60"><b>处理结果</b></td>
                <td width="160"><b>处理时间</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="30"><%#Eval("buynick")%></td>
                <td><%#Eval("orderid")%></td>
                <td><%#Eval("adddate")%></td>
                <td><%#Eval("reviewdate")%></td>
                <td><img src='<%#getimg(Eval("result").ToString())%>' /></td>
                <td><%#left(Eval("content").ToString())%></td>
                <td><%#result(Eval("issend").ToString())%></td>
                <td><%#Eval("checkdate")%></td>
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