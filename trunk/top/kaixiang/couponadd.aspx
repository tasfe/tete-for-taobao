<%@ Page Language="C#" AutoEventWireup="true" CodeFile="couponadd.aspx.cs" Inherits="top_review_couponadd" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>开箱有礼</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">开箱有礼</a> 添加优惠券 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
    需要购买淘宝的“店铺优惠券功能”才能使用优惠券功能 <a href='http://seller.taobao.com/fuwu/service.htm?service_id=6831' target="_blank">马上去购买</a>
</div>

        <table width="100%">
            <tr>
                <td align="left" width="120">优惠券名称：</td>
                <td>
                    <input name="coupon_name" type="text" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">优惠券金额：</td>
                <td>
                    <select name="price">
                        <option value="3">3</option>
                        <option value="5">5</option>
                        <option value="10">10</option>
                        <option value="20">20</option>
                        <option value="50">50</option>
                        <option value="100">100</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">优惠券截至日期：</td>
                <td>
                    <input name="end_time" type="text" value="<%=enddate %>" /> 日期格式：2011-01-01
                </td>
            </tr>
            <tr>
                <td align="left" height="30">优惠券使用条件：</td>
                <td>
                    购物满<input name="condition" type="text" value="0" size="2" />元可以使用，写0则为不限制
                </td>
            </tr>
            <tr>
                <td align="left" height="30">总数量：</td>
                <td>
                    <input name="total" type="text" value="2000" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">每人限领数量：</td>
                <td>
                    <select name="per">
                        <option value="5">5</option>
                        <option value="4">4</option>
                        <option value="3">3</option>
                        <option value="2">2</option>
                        <option value="1">1</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="left" height="30" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建优惠券" />
                </td>
            </tr>
        </table>
    </div>
</div>
</form>

</body>
</html>