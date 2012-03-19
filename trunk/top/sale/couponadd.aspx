<%@ Page Language="C#" AutoEventWireup="true" CodeFile="couponadd.aspx.cs" Inherits="top_review_couponadd" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>二次销售魔方</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">二次销售魔方</a> 添加优惠券 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    
                <input type="button" value="优惠券列表" onclick="window.location.href='couponlist.aspx'" />
    
    <hr />
<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
    需要购买淘宝的“店铺优惠券功能”才能使用优惠券功能 <a href='http://seller.taobao.com/fuwu/service.htm?service_id=6831' target="_blank">马上去购买</a>
    您可以在后台创建优惠券，然后就可以在列表页面查看。。。
</div>


    </div>
</div>
</form>

</body>
</html>