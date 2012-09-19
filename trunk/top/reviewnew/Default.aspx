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

    
    <div id="main-content" style="padding:0px; margin:0px;">
        <img src='images/yindao.jpg' border="0" usemap="#Map" />
<map name="Map">
<area shape="rect" coords="151,75,222,149" href="couponadd.aspx" alt="创建优惠券">
<area shape="rect" coords="144,156,221,239" href="../freecard/freecardadd.aspx" alt="包邮卡">
<area shape="rect" coords="144,246,220,329" href="alipayadd.aspx" alt="支付宝红包">
<area shape="rect" coords="297,132,385,266" href="setting.aspx" alt="设置赠送条件">
<area shape="rect" coords="451,146,551,263" href="msg.aspx" alt="短信提醒设置">
<area shape="rect" coords="604,61,703,184" href="html.aspx" alt="描述图片展示">
<area shape="rect" coords="604,206,717,375" href="reviewindex.aspx" alt="首页动态展示">
</map> <hr />
        <img src='images/yindao1.jpg' border="0" usemap="#Map2" />
<map name="Map2">
<area shape="rect" coords="147,78,226,160" href="couponadd.aspx" alt="优惠券">
<area shape="rect" coords="143,165,223,248" href="../freecard/freecardadd.aspx" alt="包邮卡">
<area shape="rect" coords="141,250,229,345" href="alipayadd.aspx" alt="支付宝红包">
<area shape="rect" coords="142,386,234,473" href="../crm/msgsend.aspx" alt="礼品批量赠送">
<area shape="rect" coords="285,341,361,426" href="../crm/missionadd.aspx" alt="未付款催单">
<area shape="rect" coords="285,431,369,511" href="../crm/missionadd.aspx" alt="会员活动营销">
<area shape="rect" coords="412,377,507,479" href="../crm/customlist.aspx" alt="客户管理">
<area shape="rect" coords="275,268,358,337" href="keyword.aspx" alt="内容自动判定">
<area shape="rect" coords="372,269,442,342" href="kefulist.aspx" alt="客服审核">
<area shape="rect" coords="293,104,381,232" href="setting.aspx" alt="设置赠送条件">
<area shape="rect" coords="442,130,544,248" href="msg.aspx" alt="短信提醒设置">
<area shape="rect" coords="595,59,705,192" href="html.aspx" alt="描述图片展示">
<area shape="rect" coords="601,199,721,368" href="reviewindex.aspx" alt="首页动态展示">
</map>
  </div>
</div>

</body>
</html>
