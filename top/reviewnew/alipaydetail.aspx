<%@ Page Language="C#" AutoEventWireup="true" CodeFile="alipaydetail.aspx.cs" Inherits="top_reviewnew_alipay" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 支付宝红包赠送详情 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
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
