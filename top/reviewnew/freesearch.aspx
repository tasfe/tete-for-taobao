<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freesearch.aspx.cs" Inherits="top_reviewnew_freesearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>好评有礼</title>
    <link rel="stylesheet" href="images/show.css" />
    <script src="js/jquery-1.5.2.min.js"></script>
    <style>
    .other_coupon {padding-left:10px;}
    .other_coupon img{border:0;}
    </style>  
</head>
<body>
     <div class="top-authbtn-container"></div>
    <!--
  <div class="content">
  <div style="	width:950px;
	margin:6px auto;
	font-family:微软雅黑;
	color:#fff;
	font-weight:bold;">-->
        <!--<div style="width:200; float:left; padding:10px;">
            <b style="color:white"><%=buynick %>，您好！</b>  <br /><br />
            <a href='http://ecrm.taobao.com/mallcoupon/got_bonus.htm' style="color:white" target="_blank">查看我的优惠券</a>
        </div>-->
        <!--<div style="width:950px; float:left;"><img src='images/clientbanner.jpg' width="950" /></div>
        <div style="clear:both"></div>
  </div>
</div>
-->
<asp:Panel ID="p1" runat="server" Visible="false">
<div class="w950">

        <div style="clear:both;margin:10px 0px;"></div>

        <!--
        <div class="h40 f4f p5">

<div style="width:900px;">
        <div style="clear:both;margin:10px 0px;"></div>
<div>
            <h1 style="ling-height:50px;"><img src='images/gift01.jpg' /></h1>
            <%=con %><br /> 
</div>
</div>
</div>


 <div class="h40 f4f p5" style="margin-top:5px">

<div style="width:900px; height:380px">
            <h1 style="ling-height:50px;"><img src='images/gift02.jpg'/></h1>
            <%=gift %><br clear="all" /> 
</div>
<div style="clear:both"></div>
</div>
-->


            <img src="images/gift04.jpg" />
      <asp:Panel ID="Panel1" runat="server">
         <div class="h40 f4f p5" style="margin-top:5px; padding:10px;">
            <span style="font-weight:bold; font-size:14px; color:#FF00FF;">亲爱的买家，感谢您的选择与支持，以下礼品是本店对亲及时优质好评的一点心意，祝亲生活愉快~</span>
    <br /><br />
            <img src="images/gift03.jpg" />
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="180" height=30><b>名称</b></td>
                <td width="140"><b>领取时间</b></td>
                <td width="90"><b>免邮时间</b></td>
                <td width="90"><b>使用次数</b></td>
                <td width="90"><b>满金额可用</b></td>
                <td width="180"><b>地区</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="24"><%#Eval("name")%></td>
                <td><%#Eval("startdate")%></td>
                <td><%#Eval("carddate")%>个月</td>
                <td><%#Eval("usecount")%>/<%#Eval("usecountlimit")%></td>
                <td><%#Eval("price")%>元</td>
                <td><%#show(Eval("areaisfree").ToString(), Eval("arealist").ToString())%></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
       <br clear=all />
  </div>
      </asp:Panel>
      <br />
      在线客服： <a target="_blank" href="http://amos.im.alisoft.com/msg.aw?v=2&uid=<%=nickencode %>&site=cntaobao&s=1&charset=utf-8" ><img border="0" 
align="absmiddle" src="http://amos.im.alisoft.com/online.aw?v=2&uid=<%=nickencode %>&site=cntaobao&s=1&charset=utf-8" alt="有问题请点这里" /></a>
    <br />
</div>   
</asp:Panel>
<!--
<div class="content">
  <div class="footer" id="foot"> Copyright © 2010-2012 <a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&from=client' target="_blank">好评有礼</a> 版权所有 </div>
</div>  -->
<script src="http://a.tbcdn.cn/apps/top/x/sdk.js?appkey=12690739"></script>
<script type="text/javascript" language="javascript">

TOP.ui("authbtn", {
    container: '.top-authbtn-container',
    name: '点击查询包邮卡情况',
    type: 'mini',
    callback: function (data) { alert(JSON.stringify(data)); }
}); 

</script>
</body>
</html>
