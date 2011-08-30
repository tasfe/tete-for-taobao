<%@ Page Language="C#" AutoEventWireup="true" CodeFile="accountadd.aspx.cs" Inherits="top_blog_accountadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 添加新浪博客帐号 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <span style="color:#ccc">为了您的账户安全，特特不会记录客户真实密码，如果您不放心可以<a href='http://login.sina.com.cn/signup/signupmail.php?entry=blog&r=&srcuid=&src=blog' style="color:#aaa" target="_blank">重新注册一个帐号</a>用于推广：）</span><br />
        帐 号：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
        密 码：<asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox><br />
        <a href='http://login.sina.com.cn/signup/signupmail.php?entry=blog&r=&srcuid=&src=blog' target="_blank">没博客帐号，现在就去注册一个</a><br />
        <input type="button" value="后退" onclick="history.go(-1)" />
        <asp:Button ID="btnSearch" runat="server" Text="添加帐号" onclick="btnSearch_Click" />
    </div>
</div>
</form>


<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>