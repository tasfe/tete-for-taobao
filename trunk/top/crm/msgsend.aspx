<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgsend.aspx.cs" Inherits="top_groupbuy_msgsend" validateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特特CRM_客户关系营销</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 手动赠送优惠券 </div>
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
                <td align="left" height="30">选择赠送的优惠券：</td>
                <td>
                    <%=couponstr%>
                    <a href="couponadd.aspx">创建优惠券</a>
                    <a href="couponsend.aspx">查看优惠券赠送记录</a>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="赠送优惠券给买家" />
                </td>
            </tr>
        </table>

    </div>
</div>
</form>

</body>
</html>