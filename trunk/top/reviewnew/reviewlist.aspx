<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reviewlist.aspx.cs" Inherits="top_review_reviewlist" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 评价列表 </div>
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
    新增加前台店铺展示模块，<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=30642&tracelog=other_serv' target="_blank">点击免费订购好评秀秀</a>
      <a href='jiaocheng1.html'>好评秀秀使用教程</a>
</div>

    
    
    请输入买家昵称：<asp:TextBox ID="search" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="搜索" />
<input type="button" value="展示中评价" onclick="window.location.href='reviewindex.aspx'" />
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Width="160px" Text="导出符合赠送条件的评价" OnClientClick="return confirm('如果您的评价比较多的话，可能需要较长时间，您确定要导出吗？')" />
    
    <hr />
    <div style="margin-bottom:10px;">
        <input type="checkbox" onclick="selectAll()" />
        <input type="button" value="批量展示选中评价" onclick="setOK()" />
    </div>
    <table width="720" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>评价人</b></td>
                <td width="60"><b>评分 </b></td>
                <td width="260"><b>内容 </b></td>
                <td width="100"><b>时间</b></td>
                <td width="70"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="30">
                <input type="checkbox" name="id" id="id" value='<%#Eval("orderid") %>' />
                <%#Eval("buynick") %></td>
                <td><img src='<%#getimg(Eval("result").ToString())%>' /></td>
                <td><%#left(Eval("content").ToString())%></td>
                <td><%#Eval("reviewdate") %></td>
                <td><a href="http://item.tmall.com/item.htm?id=<%#Eval("itemid") %>" title='<%#Eval("sendresult") %>.......<%#Eval("sendresultalipay") %>......<%#Eval("sendresultFree") %>' target="_blank">宝贝</a>|
                <a href='http://trade.taobao.com/trade/detail/trade_item_detail.htm?bizOrderId=<%#Eval("orderid")%>' target="_blank">订单</a>|
                <a href='reviewindex.aspx?act=add&id=<%#Eval("orderid") %>'>展示评价</a></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    
    <div style="margin-bottom:10px;">
        <input type="checkbox" onclick="selectAll()" />
        <input type="button" value="批量展示选中评价" onclick="setOK()" />
    </div>
    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>
    </div>
</div>


<script language="javascript" type="text/javascript">
    function setOK() {
        document.getElementById("t").value = "ok";
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

</form>

</body>
</html>