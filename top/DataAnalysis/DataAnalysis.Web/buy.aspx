<%@ Page Language="C#" AutoEventWireup="true" CodeFile="buy.aspx.cs" Inherits="top_groupbuy_buy" validateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>营销决策</title>
<link href="css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;font-size:14px;}
</style>


</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">营销决策</a> 版本升级提示 </div>
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
        <span style="font-size:16px;"><%=msg %></span>
        <br> 
        <br> 
        <a href='javascript:history.go(-1)'>返回</a>
    </div>
</div>
</form>

</body>
</html>