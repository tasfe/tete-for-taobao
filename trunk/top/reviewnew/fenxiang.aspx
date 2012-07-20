<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fenxiang.aspx.cs" Inherits="top_reviewnew_fenxiang" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
</style>

</head>
<body style="padding:0px; margin:0px;" onload="show()">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 分享服务获短信 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <iframe src='fen.html' width="600" height="400" id="test" frameborder="0" scrolling="no"></iframe>
    </div>
    </form>

<script language="javascript">
    function show() {
        str = document.getElementById("test").contentWindow.document.body.innerHTML;
        if (str.indexOf("分享成功！购物齐分享，生活好滋味") != -1) {
            SetOk();
            return;
        }
        setTimeout('show()', 100);
    }

    function SetOk() {
        window.location.href = 'fenxiang.aspx?act=suc&verify=7FDSFE9';
    }
</script>
</body>
</html>
