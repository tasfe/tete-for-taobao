<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="top_reviewnew_Default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 引导教程 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
   <!-- <div id="main-content" style="padding:0px; margin:0px;">
    <a href='http://fuwu.taobao.com/service/service.htm?service_code=ts-30642&from=haoping' target="_blank"><img src='http://haoping.7fshop.com/top/reviewnew/images/xiuxiu2.jpg' border=0 /></a>
    <br />
            <div class="user_box2"
			style="padding-top: 0px; padding-left: 0px; width: 778px">
  <table width="100%" style="border:1px solid #ABCCDC;" align="center" cellpadding="0"
				cellspacing="1" style="line-height: 25px;">
    <tr>
      <td style="font: 22px 微软雅黑; border-right: 1px solid #C5DCE7; vertical-align: middle; color: #000000; padding-left: 18px" width="40px"> 好 <br />
        评 <br />
        有 <br />
        礼 <br />
        十 <br />
        大 <br />
        功 <br />
        能 </td>
      <td style="vertical-align: top;" valign="top">
      <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_1.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="couponadd.aspx">店铺优惠券</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p>好评就送店铺优惠券，系统自动赠送，提升评价又带来2次销售。</p>
                </div>
                <div class="item_list_btn"><a href="couponadd.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

                      <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_6.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="html.aspx">一键同步到店铺</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p>将您的优惠活动一键同步到店铺左侧分类或者宝贝描述里，让更多客户知道您的好评活动。</p>
                </div>
                <div class="item_list_btn"><a href="html.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

          </tr>

          <tr>
           <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_6.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="saletotal.aspx">二次销售统计</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 查看你赠送的优惠券给你带来了多少2次消费，一切尽在掌握</p>
                </div>
                <div class="item_list_btn"><a href="saletotal.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
            <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_3.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="msgsend.aspx">手动赠送优惠券</a> </h4>
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
													style="vertical-align: middle; margin-right: 0px" /><a href="alipay.aspx">支付宝红包</a></h4> 
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 好评就送支付宝红包，真金白银的优惠，直达客户手机，到达率100%。 <br /> <span style="color:Red; font-weight:bold">（短信需单独收费，最低8分1条）</span></p>
                </div>
                <div class="item_list_btn"><a href="alipay.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

            <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_4.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="msg.aspx">短信提醒</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 发货、物流到货、订单催评、礼品赠送4个时点短信提醒，让您的优惠信息直达客户手机。 <br /> <span style="color:Red; font-weight:bold">（短信需单独收费，最低8分1条）</span></p>
                </div>
                <div class="item_list_btn"><a href="msg.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
          </tr>
          <tr>

           <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_2.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="../freecard/freecardadd.aspx">包邮卡</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 下单免邮费，可设置包邮时间和包邮次数，手动赠送给买家，让你的2次销售猛增。</p>
                </div>
                <div class="item_list_btn"><a href="../freecard/freecardadd.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

            <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_4.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="../crm/missionadd.aspx">未付款订单催单</a> <a class="qubie" href='qubie.html'>(VIP)</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 对未付款订单进行短信催付款，让您的未付款订单不再流失</p>
                </div>
                <div class="item_list_btn"><a href="../crm/missionadd.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
          </tr>
          <tr>
            <td><div class="item_list item_list_left_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_5.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="kefulist.aspx">客服手动审核</a> <a class="qubie" href='qubie.html'>(VIP)</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p>自行根据客户评价决定是否赠送优惠券，让真正的好评得到更大的优惠。</p>
                </div>
                <div class="item_list_btn"><a href="kefulist.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>
            
            <td><div class="item_list item_list_right_top">
                <div class="title_contents">
                  <h4 style="padding-left: 0px"><img src="images/icon_3.gif" width="32" height="32"
													style="vertical-align: middle; margin-right: 0px" /><a href="keyword.aspx">内容自动判定</a> <a href='qubie.html' class="qubie">(VIP)</a></h4>
                  <span class="category-pop"><s class="pop-l"></s><s class="pop-r"></s></span>
                  <p> 不用担心莫名其妙的给用户送优惠券，好评判定条件自定义，让您每一分钱都花的有价值。</p>
                </div>
                <div class="item_list_btn"><a href="keyword.aspx" class="btn-middle middle-blue">立即使用</a></div>
                <p style="clear: both; height: 1px;"></p>
              </div></td>

          </tr>
        </table></td>
    </tr>
  </table>

  <br />
  <div style="margin:10px;">
    <b style="font-size:14px; font-weight:bold">热门软件：</b><br />

    <iframe src="http://isvu.haodianpu.com/isvu.php?fid=service-0-22904" frameborder="0" height="120" width="750" scrolling="no" marginheight="0" marginwidth="0"  ></iframe>


    <a href= 'http://fuwu.taobao.com/ser/detail.htm?service_id=16226&from=haoping'><img alt='数据罗盘' border=0 src='http://img01.taobaocdn.com/top/i1/T162DjXf4aXXaCwpjX.png' /></a>

<a href= 'http://fuwu.taobao.com/ser/detail.htm?service_id=19444&from=haoping' target="_blank"><img alt='凹凸曼网盾卫士' border=0 src='http://img01.taobaocdn.com/top/i1/T1BM9SXhhhXXb1upjX.jpg' /></a>

<a href= 'http://fuwu.taobao.com/ser/detail.htm?service_code=ts-12067&from=haoping' target="_blank"><img alt='宝贝团' border=0 src='http://img01.taobaocdn.com/top/i1/T1htKnXaRNXXaCwpjX.png' /></a>

<a href= 'http://fuwu.taobao.com/serv/detail.htm?service_id=14849&from=haoping' target="_blank"><img alt='包邮卡' border=0 src='http://img03.taobaocdn.com/imgextra/i3/62192401/T25Q8xXlNMXXXXXXXX_!!62192401.jpg' /></a>

<a href= 'http://fuwu.taobao.com/serv/detail.htm?service_id=12745&from=haoping' target="_blank"><img alt='魔法营销' border=0 src='http://img02.taobaocdn.com/imgextra/i2/62192401/T2L7pgXXBOXXXXXXXX_!!62192401.png' /></a>

<a href= 'http://fuwu.taobao.com/serv/detail.htm?service_id=7223&from=haoping' target="_blank"><img alt='批量打印' border=0 src='http://img01.taobaocdn.com/top/i1/T1o7l9XcFBXXaCwpjX.png' /></a>

<a href= 'http://fuwu.taobao.com/ser/detail.htm?service_code=ts-28184&from=haoping' target="_blank"><img alt='掌柜帮' border=0 src='http://img04.taobaocdn.com/imgextra/i4/62192401/T29NXwXXBNXXXXXXXX_!!62192401.png' /></a>
<br />

 <a href= 'http://fuwu.taobao.com/ser/detail.htm?service_id=17458&from=haoping' target="_blank"><img alt='淘名片' border=0 src='http://img03.taobaocdn.com/imgextra/i3/749864544/T2C09gXXpbXXXXXXXX_!!749864544.png' /></a>

 <a href= 'http://fuwu.taobao.com/ser/detail.htm?service_code=appstore-21767&from=haoping' target="_blank"><img alt='天天旺' border=0 src='http://img.taobaocdn.com/sns_logo/i1/T1bfqWXiddXXaCwpjX.png_80x80.jpg' /></a>
 
 <a href= 'http://fuwu.taobao.com/ser/detail.htm?service_code=ts-19158&tracelog=haopingyouli' target="_blank"><img alt='订单透视' border=0 src='http://img02.taobaocdn.com/imgextra/i2/380087440/T2TBV3Xd8cXXXXXXXX_!!380087440.png' /></a>

  </div>
</div>

    </div> -->

    
    <div id="main-content" style="padding:0px; margin:0px;">
        <img src='images/yindao.jpg' /> <br /><br />
        <img src='images/yindao1.jpg' /> <br />
    </div>
</div>

</body>
</html>
