﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="buy.aspx.cs" Inherits="top_groupbuy_buy" validateRequest="false" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 版本升级提示 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <img src="http://a.tbcdn.cn/sys/wangwang/smiley/48x48/68.gif" /> 
        <span style="font-size:16px;"><%=msg %></span><br /><br />
        <span style="font-size:20px; font-weight:bold; color:red">3月特惠全部短信8折，现在升级版本更赠送短信，具体情况请联系客服人员...</span>
        <br> 
        <br> 
        <a href='qubie.html'>查看版本区别</a>
        <a href='javascript:history.go(-1)'>返回</a>
    </div>
</div>
</form>

</body>
</html>