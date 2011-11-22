<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xufei.aspx.cs" Inherits="top_review_xufei" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;font-size:14px;}
</style>


</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 版本续费 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <img src="http://a.tbcdn.cn/sys/wangwang/smiley/48x48/68.gif" /> 尊敬的客户您好，您的版本已经到期，您可以选择以下版本进行续费：<br /><br />
        请选择您要续费的版本：
        <br> 
        <select name="typ">
            <option value="service-0-22904-1">标准版</option>
            <option value="service-0-22904-2">专业版</option>
            <option value="service-0-22904-3">VIP版</option>
        </select>
        <br> 

        请选择时间：
        <br> 
        <select name="num">
            <option value="1">1个月</option>
            <option value="3">3个月</option>
            <option value="6">6个月</option>
            <option value="12">12个月</option>
        </select>
        <br> 
        <br> 

        <input type="button" value="马上去续费" onclick="redirect()" />

    </div>
</div>

<script>
    function redirect() {
        var typ = document.getElementById("typ").value;
        var num = document.getElementById("num").value;

        var url = "http://fuwu.taobao.com/item/subsc.htm?items=" + typ + ":" + num + ";";
        parent.location.href = url;
    }
</script>

</form>

</body>
</html>