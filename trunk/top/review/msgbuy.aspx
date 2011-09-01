<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgbuy.aspx.cs" Inherits="top_groupbuy_build" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
</style>


</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a>  短信购买  </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    
    <input type="button" value="返回短信设置页面" onclick="window.location.href='msg.aspx'" />
    
    <hr />
                <div style="width:700px; font-size:14px;">
            购买短信服务：<br />
            每月100条短信可累积  10元/月  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-4:1;' target="_blank">点击购买</a><br />
            每月500条短信可累积  50元/月  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-5:1;' target="_blank">点击购买</a><br />
            每月1000条短信可累积  100元/月  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-6:1;' target="_blank">点击购买</a><br />
            每月5000条短信可累积  500元/月  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-7:1;' target="_blank">点击购买</a><br /><br />
            <b>购买完毕后请重新从 seller.taobao.com 右上 “我的服务”进入...</b>
            </div>
    </div>
</div>
</form>

</body>
</html>