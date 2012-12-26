<%@ Page Language="C#" AutoEventWireup="true" CodeFile="actlist.aspx.cs" Inherits="top_review_couponlist" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 活动列表 </div>
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
    活动里面的赠送礼品可以跟基本设置里面的条件同时生效，但是每种礼品每订单最多赠送一张。
</div>

                <input type="button" value="创建新活动" onclick="window.location.href='actadd.aspx'" />
    
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="90"><b>活动名称</b></td>
                <td width="80"><b>开始日期</b></td>
                <td width="80"><b>结束日期</b></td>
                <td width="80"><b>满足金额</b></td>
                <td width="80"><b>指定商品</b></td>
                <td width="80"><b>优惠券</b> </td>
                <td width="80"><b>支付宝红包</b> </td>
                <td width="80"><b>包邮卡</b> </td>
                <td width="60"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("name")%></td>
                <td><%#Eval("startdate")%></td>
                <td><%#Eval("enddate")%></td>
                <td><%#Eval("condprice")%></td>
                <td><%#Eval("conditemlist")%></td>

                <td><%#Eval("couponid")%></td>
                <td><%#Eval("alipayid")%></td>
                <td><%#Eval("freeid")%></td>

                <td><a href='actmodify.aspx?id=<%#Eval("guid")%>'>修改</a> | <a href='actlist.aspx?act=del&id=<%#Eval("guid")%>' onclick="return confirm('您确定要删除吗，该操作不可恢复？')">删除</a></td>
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
