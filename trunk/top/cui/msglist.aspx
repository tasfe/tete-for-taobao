<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msglist.aspx.cs" Inherits="top_review_msglist" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>催单有礼</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">催单有礼</a> 催单短信发送清单 </div>
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
    
    <hr />
        <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>买家</b></td>
                <td width="90"><b>手机号码</b></td>
                <td width="60"><b>发送日期 </b></td>
                <td width="60"><b>发送内容 </b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="40"><%#Eval("buynick")%></td>
                <td><%#Eval("mobile")%></td>
                <td><%#Eval("adddate")%></td>
                <td><%#Eval("content")%></td>
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