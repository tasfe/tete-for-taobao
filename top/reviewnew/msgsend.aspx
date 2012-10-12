<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgsend.aspx.cs" Inherits="top_groupbuy_msgsend" validateRequest="false" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 手动赠送优惠券 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
    本功能用于某些买家延期购买后的优惠券赠送等特殊赠送，正常评价的优惠券会自动赠送，不需要手动操作，买家获得的优惠券不会超过您设定的每人获取上限..
</div>

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
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="赠送优惠券给买家" OnClick="return hidden(this)" />
                </td>
            </tr>
        </table>

    </div>
</div>

<script language="javascript">
    var isclick = false;

    function hidden(obj) {
        if (isclick) {
            return false;
        } else {
            this.value = '赠送中，请不要重复点击！';
            isclick = true;
        }
    }
</script>
</form>

</body>
</html>