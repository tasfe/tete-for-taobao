<%@ Page Language="C#" AutoEventWireup="true" CodeFile="result.aspx.cs" Inherits="top_market_result" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:600px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 投放结果分析 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
         <!--<p class="help"><a href="http://service.taobao.com/support/help.htm" target="_blank">查看帮助</a></p>-->
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content" style="overflow:scroll;">
    
    <b>访问结果统计如下：</b>
    
    <br />
    <hr />
 
 <asp:Repeater ID="rptResult" runat="server">
    <ItemTemplate>
    
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="130">广告名称</td>
        <td><%#Eval("name") %></td>
      </tr>
      <tr>
        <td>展示次数</td>
        <td><%#Eval("viewcount") %>
        </td>
      </tr>
      <tr>
        <td>商品点击次数</td>
        <td><%#Eval("hitcount") %>
        </td>
      </tr>
    </table>
    <hr />
    
    </ItemTemplate>
 </asp:Repeater>   

  </div>
</div>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>
