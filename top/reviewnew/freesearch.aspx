<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freesearch.aspx.cs" Inherits="top_reviewnew_freesearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>好评有礼</title>
    <link rel="stylesheet" href="images/show.css" />
</head>
<body>
    <form id="form1" runat="server">
    
  <div class="content">
  <div style="	width:950px;
	margin:0 auto;
	font-family:"微软雅黑";
	color:#fff;
	font-weight:bold;">
  <div style="background:#f2f2f2;margin:10px 0px;height:120px;background:url(/market/front/images/header.png) 0px 20px  no-repeat #ff6600">
  
    <div style="float:left;width:400px;display:block;">
      <div style="color:#fff;padding:10px;">&nbsp;</div>
      <div style="height:60px;padding:10px;">&nbsp;</div>
    </div>
    
    <div style="float:left;width:130px;display:block;margin:0px 10px;">
      <div style="color:#fff;padding:10px;"><a target="_blank" href="http://amos.im.alisoft.com/msg.aw?v=2&uid=shenglongdedian&site=cntaobao&s=2&charset=utf-8" ><img border="0" src="http://amos.im.alisoft.com/online.aw?v=2&uid=shenglongdedian&site=cntaobao&s=1&charset=utf-8" alt="点击这里联系客服"/></a></div>
      <div style="height:60px;padding:10px;color:#fff;font-size:14px;">

      <br />
      <a href="http://shop34248669.taobao.com" target="_blank"><font color="#ffffff">浏览店铺</font></a><br />
      <a href="http://shuo.taobao.com/shop34248669" target="_blank"><font color="#ffffff">店铺动态</font></a>
      
      </div>
    </div>
    
    <div style="float:right;width:400px;display:block;">
      <div style="color:#fff;padding:10px;font-size:14px">店铺公告</div>
      <div style="color:#fff;height:60px;padding:10px;"><span> 本店暂无公告</span></div>
    </div>
    
  </div>  
  </div>
</div>
<div class="w950">

  <div style="clear:both;margin:10px 0px;"></div>
      <div class="h40 f4f p5">
      
      
      
      
    <div>
        
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="140"><b>领取时间</b></td>
                <td width="60"><b>免邮时间</b></td>
                <td width="60"><b>使用次数</b></td>
                <td width="60"><b>满金额</b></td>
                <td width="80"><b>地区</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("startdate")%></td>
                <td height="35"><%#Eval("carddate")%>个月</td>
                <td><%#Eval("usecount")%>/<%#Eval("usecountlimit")%></td>
                <td><%#Eval("price")%>元</td>
                <td><%#Eval("arealist")%></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    </div>
      
      
       
  </div>
<style>
.other_coupon {padding-left:10px;}
.other_coupon img{border:0;}
</style>  
</div>
<!--
-->
<div class="content">
  <div class="footer" id="foot"> </div>
</div>  
    
    </form>
</body>
</html>
