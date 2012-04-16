<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgaddlist.aspx.cs" Inherits="top_review_msgaddlist" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 短信充值记录 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    
    <input type="button" value="返回短信设置页面" onclick="window.location.href='msg.aspx'" />
    
    <hr />
        <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>充值条数</b></td>
                <td width="160"><b>充值编号</b></td>
                <td width="60"><b>充值时间 </b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="40"><%#Eval("count")%></td>
                <td><%#Eval("typ")%></td>
                <td><%#Eval("adddate")%></td>
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