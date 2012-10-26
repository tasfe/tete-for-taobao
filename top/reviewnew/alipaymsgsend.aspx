<%@ Page Language="C#" AutoEventWireup="true" CodeFile="alipaymsgsend.aspx.cs" Inherits="top_groupbuy_alipaymsgsend" validateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;font-size:14px;}
</style>


</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 手动赠送支付宝红包 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

        <table width="700">
            <tr>
                <td align="left" width="180" height="30">请输入您要赠送的买家昵称：</td>
                <td>
                    <asp:TextBox ID="txtBuyerNick" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">选择赠送的支付宝红包：</td>
                <td>
                    <%=couponstr%>
                    <a href="alipayadd.aspx">导入支付宝红包</a>
                    <a href="alipay.aspx">查看赠送记录</a>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">是否强行赠送：</td>
                <td>
                    <input type="checkbox" name="issend" value = "1" /> 强行赠送是指支付宝红包赠送时不受红包设定数量限制
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="赠送支付宝红包给买家" />
                </td>
            </tr>
        </table>

    </div>
</div>
</form>

</body>
</html>