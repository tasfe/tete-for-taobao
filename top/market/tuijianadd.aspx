<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tuijianadd.aspx.cs" Inherits="top_market_tuijianadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
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

        <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold">
            您在这里写上您要推荐好友的淘宝昵称，然后他成功购买特特确认后便会成为你的成功推荐，每成功推荐10个好友赠送价值10元的特特博客推广专家一个月！
            推荐人数最多者将获得价值360元的特特博客推广专家VIP版本一年！！心动不如行动，快去推荐的好友来使用免费的特特吧！！
        </div>
        <br />
        好友的淘宝昵称：<asp:TextBox ID="TextBox1" runat="server" Width="150px"></asp:TextBox><br />
        <br />
        <asp:Button ID="btnSearch" runat="server" Text="添加" onclick="btnSearch_Click" />
        <input type="button" value="返回列表" onclick="window.location.href='tuijianlist.aspx'" />

  </div>
</div>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>

</form>
</body>
</html>
