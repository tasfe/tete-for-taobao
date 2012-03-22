<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="top_reviewnew_Default" %>

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
    <form id="form2" runat="server">
        <div class="navigation" style="height:600px;">
            <div class="user_box2"
			style="padding-top: 0px; padding-left: 0px; width: 998px">
  <table width="100%" border="0" align="center" cellpadding="0"
				cellspacing="1" style="line-height: 25px;">
    <tr>
      <td style="font: 22px 微软雅黑; border-right: 1px solid #C5DCE7; vertical-align: middle; color: #000000; padding-left: 25px" width="50px"> 好 <br />
        评 <br />
        有 <br />
        礼 </td>
      <td style="vertical-align: top;" valign="top"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td><div class="item_list item_list_left_bottom">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_1.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="couponadd.aspx">店铺优惠券</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><a
												href="#">好评就送店铺优惠券，提升评价又带来2次销售</a><s class="pop-r"></s></span>
                  <p><img src="img/item_3.png" title="该功能只有高级版用户可以使用！"/>&nbsp;好评就送店铺优惠券，提升评价又带来2次销售</p>
                </div>
                <div class="item_list_btn"><a href="couponadd.aspx" target="_blank" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
            <td><div class="item_list item_list_right_bottom">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_2.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="alipay.aspx">支付宝红包</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 好评就送支付宝红包，直达客户手机，到达率100% </p>
                </div>
                <div class="item_list_btn"><a href="alipay.aspx" target="_blank" class="btn-middle middle-blue">好评就送支付宝红包，直达客户手机，到达率100%</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
          </tr>
          <tr>
            <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_3.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="keyword.aspx">内容自动判定</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 还在担心莫名其妙的给用户送了优惠券嘛，好评判定条件自定义，让您的每一分钱都花的有价值</p>
                </div>
                <div class="item_list_btn"><a href="keyword.aspx" target="_blank" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
            <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_4.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="msg.aspx">短信提醒</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 订单发货、物流到货、订单催评、礼品赠送4个时点短信提醒，让您的优惠信息直达客户手机</p>
                </div>
                <div class="item_list_btn"><a href="msg.aspx" target="_blank" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
          </tr>
          <tr>
            <td><div class="item_list item_list_left_bottom">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_5.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="kefulist.aspx">客服手动审核</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p><img src="img/item_3.png" title="该功能只有高级版用户可以使用！"/>&nbsp;自行根据客户评价决定是否赠送优惠券，让真正的好评得到更大的优惠</p>
                </div>
                <div class="item_list_btn"><a href="kefulist.aspx" target="_blank" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
            <td><div class="item_list item_list_right_bottom">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_6.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="#">二次销售统计</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 查看你的评价周期是否缩短，好评是否有上升，给你带来了多少2次消费，一切尽在掌握</p>
                </div>
                <div class="item_list_btn"><a href="#" target="_blank" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
          </tr>
        </table></td>
    </tr>
  </table>
</div>

        </div>
    </form>
</body>
</html>
