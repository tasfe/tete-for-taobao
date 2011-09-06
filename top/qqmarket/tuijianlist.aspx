<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tuijianlist.aspx.cs" Inherits="top_market_tuijianlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 推荐好友使用送大奖 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content">

  <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red;">
    您在这里写上您要推荐好友的淘宝昵称，然后他成功购买特特确认后便会成为你的成功推荐，每成功推荐10个好友赠送价值10元的<a href='http://seller.taobao.com/fuwu/service.htm?service_id=4545' target="_blank">特特博客推广专家</a>标准版一个月！
    推荐人数最多者将获得价值360元的特特博客推广专家VIP版本一年！！心动不如行动，快去推荐的好友来使用免费的特特吧！！
</div>

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:4px 3px; color:black; font-size:20px; font-weight:bold">
    您已经成功推荐了<%=tuijian%>个好友使用特特，再推荐<%=need%>个好友就可以获得价格10元的特特博客推广标准版1个月
</div>

<br />

<!--<input type="button" value="我要推荐好友使用特特" onclick="window.location.href='tuijianadd.aspx'" />-->

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold;  font-size:16px;">
    您需要把下面的网址发给您的好友，让您的好友购买该免费服务！他购买成功进入该应用便会成为您的推荐用户！<br><br> 
    http://www.7fshop.com/top/market/tuijianredirect.aspx?id=<%=id %>
</div>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30"><strong>推荐朋友</strong></td>
        <td><strong>是否成功</strong></td>
        <td><strong>成功时间</strong></td>
      </tr>
      
      <asp:Repeater ID="rptIdeaList" runat="server">
        <ItemTemplate>
        
      <tr>
        <td height="30"><%#Eval("nickto") %></td>
        <td style="color:#333"><%#Eval("isok")%></td>
        <td><%#Eval("okdate") %></td>
      </tr>
        
        </ItemTemplate>
      </asp:Repeater>
    </table>

  </div>
</div>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>

</body>
</html>
