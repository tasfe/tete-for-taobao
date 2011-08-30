<%@ Page Language="C#" AutoEventWireup="true" CodeFile="accountaddqq.aspx.cs" Inherits="top_blog_accountaddqq" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
<script language="javascript" src="encode.js" type="text/javascript"></script>
    <base Target="_self" /> 

</head>
<body style="padding:0px; margin:0px;" onload="InitVerifyLoad()">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 添加QQ空间帐号 </div>
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
    使用本软件进行推广可能会造成博客被封现象，建议用户使用的新注册的帐号进行推广！
</div>
    
        <input type="hidden" id="truePass" name="truePass" value="" />
        <span style="color:#ccc">为了您的账户安全，特特不会记录客户真实密码，如果您不放心可以<a href='http://reg.qq.com/' style="color:#aaa" target="_blank">重新注册一个帐号</a>用于推广：）</span><br />
        帐　号：<asp:TextBox ID="tbUserName" runat="server" onblur="initVerify(this)"></asp:TextBox><br />
        密　码：<asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox><br />
        <span id="verifyArea" style="display:none">验证码：<input name="tbVerify" id="tbVerify" size="4" /><br />
  	    <img id="verifyImg" /> <a href='#' onclick="freshVerify()">看不清，换一张</a><br /></span>
        <a href='http://reg.qq.com/' target="_blank">没QQ帐号，现在就去注册一个</a><br />
        <input type="button" value="后退" onclick="history.go(-1)" />
        <asp:Button ID="btnSearch" runat="server" Text="添加帐号" onclick="btnSearch_Click" OnClientClick="return checkForm()" />
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    var url = '';
    var isShow = 1;

    function checkForm() {
        var p = document.getElementById("tbPassword").value;
        document.getElementById("truePass").value = p;
        var f = document.getElementById("tbVerify").value.toUpperCase();
        p = md5(md5_3(p) + f);
        document.getElementById("tbPassword").value = p;
        return true;
    }

    function initVerify(obj) {
        url = "qqverify.aspx?q=" + obj.value;
        document.getElementById("verifyImg").src = url + "&d=" + Date();
        //updateCat(obj.value);
        document.getElementById("verifyArea").style.display = "block";
    }

    function freshVerify() {
        document.getElementById("verifyImg").src = url + "&d=" + Date();
        //updateCat(obj.value);
    }

    function InitVerifyLoad() {
        var uid = document.getElementById("tbUserName").value;
        if (uid != "") {
            //updateCat(uid);
            url = "qqverify.aspx?q=" + uid;
            document.getElementById("verifyImg").src = url + "&d=" + Date();
            document.getElementById("verifyArea").style.display = "block";
        }
    }
    
    
    var xmlHttp;
    var itemsStr;
    var itemsStrTxt;
    function createxmlHttpRequest()
    {
        if(window.ActiveXObject)
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        else if(window.XMLHttpRequest)
            xmlHttp = new XMLHttpRequest();
    }
    
    function updateCat(uid) {
        createxmlHttpRequest();
        var queryString = "qqcheck.aspx?uid=" + uid + "&r=" + new Date().getTime();
        //alert(queryString);
        xmlHttp.open("GET", queryString);
        xmlHttp.onreadystatechange = handleStateChangeCat;
        xmlHttp.send(null);
    }

    function handleStateChangeCat() {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
            document.getElementById("tbVerify").value = xmlHttp.responseText.replace("ptui_checkVC('0','", "").replace("');", "");
            //alert(xmlHttp.responseText);
            //alert(document.getElementById("tbVerify").value);
        }
    }
</script>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>