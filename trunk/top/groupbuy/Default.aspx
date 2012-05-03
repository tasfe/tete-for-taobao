<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="top_reviewnew_Default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特特团购</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        .middle-blue {
    background-position: 0 -148px;
    text-shadow: 1px 1px 1px #227AA7;
}

.btn-middle {
    background: url("images/item-botton.png") no-repeat scroll -68px 0 transparent;
    border: medium none;
    color: #FFFFFF;
    cursor: pointer;
    display: inline-block;
    font-size: 14px;
    height: 29px;
    line-height: 29px;
    position: relative;
    text-align: center;
    text-decoration: none;
    width: 87px;
}

.item_list_btn {
    float: right;
    padding-top: 30px;
    width: 92px;
}

.item_list .title_contents, .item_list_adv .title_contents {
    float: left;
    position: relative;
    vertical-align: top;
    width: 250px;
}

.item_list_left_top {
    border-bottom: 1px solid #ABCCDC;
    border-right: 1px solid #C5DCE7;
    margin-bottom: 5px;
}

.item_list_right_top {
    border-bottom: 1px solid #ABCCDC;
    border-left: 1px solid #C5DCE7;
    margin-bottom: 5px;
    margin-left: 10px;
}

.item_list {
    background: none repeat scroll 0 0 #FEFEFE;
    padding-left: 5px;
    vertical-align: top;
}

h4 {
    font-family: "微软雅黑","宋体";
    font-size: 18px;
    padding-left: 5px;
    padding-top: 5px;
}

h4 a {
    color: #004499;
    text-decoration: none;
}

.item_list p, .item_list_adv p {
    color: #555555;
}

.qubie{color:Black; font-size:14px;}

    </style>
</head>
<body style="padding:0px; margin:0px;">

<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特团购</a> 功能介绍 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content" style="padding:0px; margin:0px;">
            <div class="user_box2"
			style="padding-top: 0px; padding-left: 0px; width: 778px">
  <table width="100%" style="border:1px solid #ABCCDC;" align="center" cellpadding="0"
				cellspacing="1" style="line-height: 25px;">
    <tr>
      <td style="font: 22px 微软雅黑; border-right: 1px solid #C5DCE7; vertical-align: middle; color: #000000; padding-left: 18px" width="40px"> 特 <br />
        特 <br />
        团 <br />
        购 <br />
        六 <br />
        大 <br />
        功 <br />
        能 </td>
      <td style="vertical-align: top;" valign="top">
      <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_1.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="activityadd.aspx">创建促销活动</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p>好评就送店铺优惠券，系统自动赠送，提升评价又带来2次销售。</p>
                </div>
                <div class="item_list_btn"><a href="activityadd.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

                      <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_6.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="activityList.aspx">促销商品设置</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p>将您的优惠活动一键同步到店铺左侧分类或者宝贝描述里，让更多客户知道您的好评活动。</p>
                </div>
                <div class="item_list_btn"><a href="activityList.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

          </tr>

          <tr>
           <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_6.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="saletotal.aspx">团购模板设置</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 查看你赠送的优惠券给你带来了多少2次消费，一切尽在掌握</p>
                </div>
                <div class="item_list_btn"><a href="saletotal.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
            <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_3.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="msgsend.aspx">店铺首页展示</a> </h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 手动给您的客户赠送优惠券，不同的客户赠送不同的优惠券</p>
                </div>
                <div class="item_list_btn"><a href="msgsend.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
          </tr>

          <tr>
              
      
            <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_2.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="alipay.aspx">活动效果统计</a> </h4> 
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 好评就送支付宝红包，真金白银的优惠，直达客户手机，到达率100%。 <br /> <span style="color:Red; font-weight:bold"></span></p>
                </div>
                <div class="item_list_btn"><a href="alipay.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

            <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_4.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="msg.aspx">一键安装模板</a> </h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 发货、物流到货、订单催评、礼品赠送4个时点短信提醒，让您的优惠信息直达客户手机。 <br /> <span style="color:Red; font-weight:bold"></span></p>
                </div>
                <div class="item_list_btn"><a href="msg.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
          </tr>
        </table></td>
    </tr>
  </table>

</div>

    </div>
</div>

</body>
</html>
