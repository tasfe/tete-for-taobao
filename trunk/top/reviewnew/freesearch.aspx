<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freesearch.aspx.cs" Inherits="top_reviewnew_freesearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>好评有礼</title>
    <link rel="stylesheet" href="images/show.css" />
</head>
<body>
    
  <div class="content">
  <div style="	width:950px;
	margin:0 auto;
	font-family:"微软雅黑";
	color:#fff;
	font-weight:bold;">
  <div style="background:#f2f2f2;margin:10px 0px;height:228px;background:url(images/shopbanner2.gif) 0px 20px  no-repeat #ff6600">
  
    
    
  </div>  
  </div>
</div>
<div class="w950">

  <div style="clear:both;margin:10px 0px;"></div>
      <div class="h40 f4f p5">
      
      
    <form id="form1" runat="server">
      
      
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
      
      
    </form>
       
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
    
</body>
</html>
