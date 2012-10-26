<%@ Page Language="C#" AutoEventWireup="true" CodeFile="kefulist.aspx.cs" Inherits="top_review_kefulist" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 买家好评审核 </div>
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
    此处取得的评价结果可能跟实际有1个小时左右的延迟，请您耐心等待~<br />
    如果您开启了基本设置中的“是否开启评价审核”，则所有的评价都将由您自己审核是否赠送礼品~
</div>

    请输入买家昵称：<asp:TextBox ID="search" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="搜索" />

    <input type="button" value="查看历史处理结果" onclick="window.location.href='oldkefulist.aspx'" />
    
    <hr />
    <div style="margin-bottom:10px;">
        <input type="checkbox" onclick="selectAll()" />
        <input type="button" value="审核通过并赠送礼品" onclick="setOK()" />
        <input type="button" value="审核不通过" onclick="setNotOK()" />
        <input type="button" value="一键过滤不满足内容判断的评价" onclick="passContent()" />
    </div>
        <table width="780" cellpadding="0" cellspacing="0">
        <tr>
                <td width="30"><input type="checkbox" onclick="selectAll()" /></td>
                <td width="90"><b>买家</b></td>
                <td width="70"><b>回头客</b></td>
                <td width="120"><b>订单号</b></td>
                <td width="50"><b>金额</b></td>
                <td width="60"><b>下单日期 </b></td>
                <td width="60"><b>评价日期 </b></td>
                <td width="60"><b>评价等级 </b></td>
                <td width="120"><b>评价内容 </b></td>
                <td width="100"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><input name="id" type="checkbox" value="<%#Eval("orderid")%>" /></td>
                <td><a href='http://trade.taobao.com/trade/itemlist/list_sold_items.htm?buyerNick=<%#UrlEncode(Eval("buynick").ToString())%>&event_submit_do_query=1&user_type=1&pageNum=0&action=itemlist%2FQueryAction' target="_blank" title="点击查看订单详情"><%#Eval("buynick")%></a></td>
                <td><a href='couponsend.aspx?buynick=<%#HttpUtility.UrlEncode(Eval("buynick").ToString())%>' target="_blank">赠送：<%#Eval("giftcount")%></a> <br /> <a href='salelist.aspx?buynick=<%#HttpUtility.UrlEncode(Eval("buynick").ToString())%>' target="_blank">下单：<%#Eval("couponcount")%></a></td>
                <td><a href='http://trade.taobao.com/trade/itemlist/list_sold_items.htm?bizOrderId=<%#Eval("orderid")%>&event_submit_do_query=1&user_type=1&pageNum=0&action=itemlist%2FQueryAction' target="_blank" title="点击查看订单详情"><%#Eval("orderid")%></a></td>
                <td><%#Eval("totalprice")%></td>
                <td><%#Eval("adddate")%></td>
                <td><%#Eval("reviewdate")%></td>
                <td><img src='<%#getimg(Eval("result").ToString())%>' /></td>
                <td><%#left(Eval("content").ToString())%></td>
                <td>
                    <a href='kefulist.aspx?id=<%#Eval("orderid")%>&act=send&send=1'>赠送</a> | <a href='kefulist.aspx?id=<%#Eval("orderid")%>&act=send&send=2'>不赠送</a>
                </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>

    <div>
        <input type="checkbox" onclick="selectAll()" />
        <input type="button" value="审核通过并赠送礼品" onclick="setOK()" />
        <input type="button" value="审核不通过" onclick="setNotOK()" />
    </div>
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function setOK() {
        document.getElementById("t").value = "ok";
        document.getElementById("form1").submit();
    }

    function setNotOK() {
        document.getElementById("t").value = "no";
        document.getElementById("form1").submit();
    }

    function passContent() {
        document.getElementById("t").value = "pass";
        document.getElementById("form1").submit();
    }

    function selectAll() { 
        var ids = document.getElementsByName("id");
        for (i = 0; i < ids.length; i++) {
            if (ids[i].checked == true) {
                ids[i].checked = false;
            } else {
                ids[i].checked = true;
            }
        }
    }
</script>

</body>
</html>