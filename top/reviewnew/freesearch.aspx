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
  </div>
</div>
<div class="w950">

  <div style="clear:both;margin:10px 0px;">111</div>
      <div class="h40 f4f p5">
      
      
    <form id="form1" runat="server">
      
      
    <div>
        
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="140"><b>名称</b></td>
                <td width="140"><b>领取时间</b></td>
                <td width="60"><b>免邮时间</b></td>
                <td width="60"><b>使用次数</b></td>
                <td width="60"><b>满金额</b></td>
                <td width="80"><b>地区</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="20"><%#Eval("name")%></td>
                <td><%#Eval("startdate")%></td>
                <td><%#Eval("carddate")%>个月</td>
                <td><%#Eval("usecount")%>/<%#Eval("usecountlimit")%></td>
                <td><%#Eval("price")%>元</td>
                <td><%#Eval("arealist")%></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    </div>
      
      
    </form>
       <br clear=all />
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
