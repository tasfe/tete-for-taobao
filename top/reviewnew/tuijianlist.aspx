<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tuijianlist.aspx.cs" Inherits="top_market_tuijianlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="default.aspx">好评有礼</a> 推荐好友使用送短信 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; font-weight:bold;  font-size:12px;">
    <span style="font-size:14px; color:Red">您需要把下面的网址发给您的好友，让您的好友购买好评有礼服务！他购买成功进入该应用便会成为您的推荐用户，每成功推荐一个客户赠送80条短信。<br /> 【您已经成功推荐了<%=tuijian%>个好友使用好评有礼】</span><br> 
    http://haoping.7fshop.com/top/reviewnew/tuijianredirect.aspx?id=<%=nickcode %>
    <input type="button" value="复制推广地址" onclick="copyToClipBoard('http://haoping.7fshop.com/top/reviewnew/tuijianredirect.aspx?id=<%=nickcode %>')" />
</div>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30"><strong>推荐朋友</strong></td>
        <td><strong>时间</strong></td>
      </tr>
      
      <asp:Repeater ID="rptIdeaList" runat="server">
        <ItemTemplate>
        
      <tr>
        <td height="30"><%#Eval("nickto") %></td>
        <td><%#Eval("adddate") %></td>
      </tr>
        
        </ItemTemplate>
      </asp:Repeater>
    </table>

  </div>
</div>

<script>
    function copyToClipBoard(str) {
        var clipBoardContent = str;
        if (window.clipboardData) {
            window.clipboardData.setData("Text", clipBoardContent);
            alert("复制成功");
        } else {
            alert("FireFox浏览器不支持此功能,请手动复制");
        }
    }
</script>

</body>
</html>
