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
                <td align="left" width="180" height="30">请选择发送的会员组：</td>
                <td>
                    <select name="typ">
                        <option value="all">所有会员</option>
                        <optgroup label="按购买次数区别">
                            <option value="0">未成功购买的会员</option>
                            <option value="1">购买过一次的会员</option>
                            <option value="2">购买过多次的会员</option>
                        </optgroup>
                        <optgroup label="按用户组区别">
                            <option value="a">未购买</option>
                            <option value="b">普通会员</option>
                            <option value="c">高级会员</option>
                            <option value="d">VIP会员</option>
                            <option value="e">至尊VIP会员</option>
                        </optgroup>
                    </select>
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