<%@ Page Language="C#" AutoEventWireup="true" CodeFile="resultshow.aspx.cs" Inherits="top_market_resultshow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:600px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 广告浏览次数查看 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content">
    
 <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30"><strong>展示网站</strong></td>
        <td><strong>访问次数</strong></td>
      </tr>
      
      <asp:Repeater ID="rptLogList" runat="server">
        <ItemTemplate>
        
      <tr>
        <td height="30"><a href='<%#Eval("url") %>' target="_blank"><%#Eval("url") %></a></td>
        <td><%#Eval("viewcount") %></td>
      </tr>
        
        </ItemTemplate>
      </asp:Repeater>
      
      
      <tr>
        <td colspan="2">
            <input type="button" onclick="history.go(-1)" value="返 回" />
        </td>
      </tr>
      
    </table>

  </div>
</div>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>
