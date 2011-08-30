<%@ Page Language="C#" AutoEventWireup="true" CodeFile="microblogadd2.aspx.cs" Inherits="top_microblog_microblogadd2" ValidateRequest="false" EnableViewStateMac="false"  enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
<script language="javascript" src="encode.js" type="text/javascript"></script>

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">微博营销</a> 我要推广 (3.填写QQ帐号密码) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <input type="hidden" value="<%=topic %>" name="topic" />
        <asp:TextBox ID="tbContent" runat="server" Visible="false"></asp:TextBox>
        <asp:Label ID="lbErrMessage" runat="server"></asp:Label>
        您可以在此修改您QQ帐号密码：<br />
        <span style="color:#ccc">为了您的账户安全，特特不会记录客户真实密码，如果您不放心可以<a href='http://reg.qq.com/' style="color:#aaa" target="_blank">重新注册一个帐号</a>用于推广：）</span><br />
        帐　号：<asp:TextBox ID="tbUserName" runat="server" onblur="initVerify(this)"></asp:TextBox><br />
        密　码：<asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox><br />
        验证码：<input name="tbVerify" id="tbVerify" size="4" /><br />
  	    <img id="verifyImg" /> <a href='#' onclick="freshVerify()">看不清，换一张</a><br />
        <a href='http://reg.qq.com/' target="_blank">没QQ帐号，现在就去注册一个</a><br />
        <input type="button" value="后退" onclick="history.go(-1)" />
        <asp:Button ID="btnSearch" runat="server" Text="自动发送微博" onclick="btnSearch_Click" OnClientClick="return checkForm()" />
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    var url = '';

    function checkForm() {
        var p = document.getElementById("tbPassword").value;
        var f = document.getElementById("tbVerify").value.toUpperCase();
        p = md5(md5_3(p) + f);
        document.getElementById("tbPassword").value = p;
        return true;
    }

    function initVerify(obj) {
        url = "qqverify.aspx?q=" + obj.value;
        document.getElementById("verifyImg").src = url + "&d=" + Date();
    }

    function freshVerify() {
        document.getElementById("verifyImg").src = url + "&d=" + Date();
    }
</script>

</body>
</html>