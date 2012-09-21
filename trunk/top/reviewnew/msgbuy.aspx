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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a>  短信购买  </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
    因为找到了新的短信供应商，现在短信价格全面下调，短信最低75折，祝大家生意兴隆！
</div>

    <input type="button" value="返回短信设置页面" onclick="window.location.href='msg.aspx'" />
    
    <hr />
                <div style="width:700px; font-size:14px;">
            购买短信服务：<br />
            短信包100条短信，剩余可累积  10元  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-4:1;' target="_blank">点击购买</a><br />
            短信包500条短信，剩余可累积(9折)  45元  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-5:1;' target="_blank">点击购买</a> (现在购买立即赠送10条短信) <br />
            短信包1000条短信，剩余可累积(85折)  85元  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-6:1;' target="_blank">点击购买</a> (现在购买立即赠送30条短信) <br />
            短信包5000条短信，剩余可累积(8折)  400元  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-7:1;' target="_blank">点击购买</a>  (现在购买立即赠送200条短信) <br />
            短信包10000条短信，剩余可累积(75折)  750元  <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-8:1;' target="_blank">点击购买</a>  (现在购买立即赠送500条短信) <br /><br />
            <b style="color:Red; font-size:28px">购买完毕后请重新进入好评有礼服务，否则短信充值不上去，非常感谢！</b>

             <br> 
        <br> 
        <br> 
            </div>
    </div>
</div>
</form>

</body>
</html>