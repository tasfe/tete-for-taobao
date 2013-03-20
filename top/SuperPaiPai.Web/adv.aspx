<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adv.aspx.cs" Inherits="adv" %>
var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");document.write("<div style=\"display:none\">"+unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F6a46176ff83d6f29a3390a8ae6352d0a' type='text/javascript'%3E%3C/script%3E")+"</div>");var isIE=!!window.ActiveXObject;  
var isIE6=isIE&&!window.XMLHttpRequest;  
var isIE8=isIE&&!!document.documentMode;  
var isIE7=isIE&&!isIE6&&!isIE8;  

function SetCookie(sName, sValue) 
{ 
	var expireDate = new Date();
	expireDate.setTime(expireDate.getTime() + 24 * 3600 * 1000);
	document.cookie = sName + "=" + escape(sValue) + ";expires=" + expireDate.toGMTString();
}

function GetCookie(sName) 
{
	var aCookie = document.cookie.split("; "); 
	for (var i=0; i < aCookie.length; i++) 
	{ 
		var aCrumb = aCookie[i].split("="); 
		  if (sName == aCrumb[0]) {
			return unescape(aCrumb[1]); 
		}
	}
}

function DelCookie (sName) 
{ 
		document.cookie = sName + "=" + escape(0) + "; expires=Fri, 31 Dec 1999 23:59:59 GMT;"; 
}
var timerID = null;
var timerRunning = false;

var End_Year = 0;
var End_Month = 0;
var End_Date = 0;
var End_Hour = 0;
var End_Minute = 0;
var End_Second = 0;

function showtime() {
	Today = new Date();
	var NowHour = Today.getHours();
	var NowMinute = Today.getMinutes();
	var NowMonth = Today.getMonth();
	var NowDate = Today.getDate();
	var NowYear = Today.getYear();
	var NowSecond = Today.getSeconds();

	Hourleft = End_Hour - NowHour;
	Minuteleft = End_Minute - NowMinute;
	Secondleft = End_Second - NowSecond;
	Yearleft = End_Year - NowYear;
	Monthleft = End_Month - NowMonth - 1;
	Dateleft = End_Date - NowDate;

	if (Secondleft<0)
	{
	Secondleft=60+Secondleft;
	Minuteleft=Minuteleft-1;
	}
	if (Minuteleft<0)
	{
	Minuteleft=60+Minuteleft;
	Hourleft=Hourleft-1;
	}
	if (Hourleft<0)
	{
	Hourleft=24+Hourleft;
	Dateleft=Dateleft-1;
	}
	if (Dateleft<0)
	{
	Dateleft=31+Dateleft;
	Monthleft=Monthleft-1;
	}
	if (Monthleft<0)
	{
	Monthleft=12+Monthleft;
	Yearleft=Yearleft-1;
	}

	var dddss = new Date(); 
	var ssss = String(dddss.getMilliseconds());
	ssss = ssss.substring(0,2);
	var Temp = '';
	if(Dateleft > 0)
	{
		Temp += Dateleft+'天';
	}
	if(Hourleft > 0)
	{
		Temp += Hourleft+'小时';
	}
	if(Minuteleft > 0)
	{
		Temp += Minuteleft+'分';
	}
	
	Temp += Secondleft+'秒'+ssss+'后';
	if (document.getElementById('timediv') != null){
		document.getElementById('timediv').innerHTML = Temp;
	}

	timerID = setTimeout("showtime()",100);
	timerRunning = true;
}

function stopclock () {
if(timerRunning)
clearTimeout(timerID);
timerRunning = false;
}
function startclock () {
stopclock();
showtime();
}

$("div[id^=adv_]").hide();
$("div[id^=adv_]").html("");


/**  文本 **/



var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_left_order');
if (adv_diy_id != null){
	document.getElementById('adv_left_order').innerHTML = document.getElementById('adv_left_order').innerHTML + '<div class="clear_height"></div><a href="http://www.paipai.com" target="_blank"><img style="display:none" src="http://img03.taobaocdn.com/imgextra/i3/266681563/T2ecOdXmxcXXXXXXXX_!!266681563.jpg" border="0" width="170px" height="100px"/></a><div class="clear_height"></div>    <div class="ask" style="display:none;"><h4>最新订购</h4>  	<div id="adv_order_demo" style="overflow:hidden;height:262px;width:100%;margin-top:10px;line-height:20px;">	      <div class="list01" id="adv_order_demo1" valign="top">	 			<%=gundong%>	 		</div>		<div id="adv_order_demo2" valign="top"></div>	</div></div>';
		$("#adv_left_order").show();
	}





var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_left_qa');
if (adv_diy_id != null){
	document.getElementById('adv_left_qa').innerHTML = document.getElementById('adv_left_qa').innerHTML + '   <div class="ask" style="display:none">   <h4>有问必答</h4>        <div class="list01">        <ul>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=11">宝贝推荐</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=2">促销助手</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=4">仓库上架</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=20">店铺统计</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=9">绩效管理</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=19">日历模板</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=16">店铺模板</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=22">店铺体检</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=7">发货管理</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=10">限时折扣</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=8">会员管理</a></li>          <li><a target="_blank" href="http://adv.superboss.cc/question.php?product_id=1&function_id=21">行情参考</a></li>        </ul>      </div><div class="clear_height"></div></div>';
		$("#adv_left_qa").show();
	}





var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_right_notice');
if (adv_diy_id != null){
	//document.getElementById('adv_right_notice').innerHTML = document.getElementById('adv_right_notice').innerHTML + '<div class="notice"><span>通知：</span>您当前进入的是新版拍拍店长，同时你也可以切换到旧版界面！[<a href="http://f.superboss.cc/default.jsp" target="_blank">返回旧版</a>] 免费领取店铺运营报告啦！<a href="http://f.superboss.cc/ProductUsingServlet?kind=35" target="_blank">立即领取>></a></div><div class="clear_height"></div>';
		//$("#adv_right_notice").show();
	}





var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_fooler');
if (adv_diy_id != null){
	document.getElementById('adv_fooler').innerHTML = document.getElementById('adv_fooler').innerHTML + '<div class="title" style="display:none">   <p><a href="#" style="font-size:16px">更多>></a></p><h1 style="font-size:16px">合作伙伴</h1> </div> <div class="ppbox" style="display:none">    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=%E6%B0%B8%E6%88%90" target="_blank"><img src="http://logo.taobao.com/shop-logo/8a/2d/T1QBXOXnXgXXartXjX.gif" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_4.gif" /><br/>永成</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=ray_14" target="_blank"><img src="http://a.tbcdn.cn/app/sns/img/default/avatar-120.png" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_3.gif" /><br/>ray_14</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=%E5%86%B0%E6%B4%8B" target="_blank"><img src="http://logo.taobao.com/shop-logo/86/a8/T1M1NAXlBdXXb6czI._082732.jpg" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_blue_1.gif" /><br/>冰洋</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=%E5%B0%8F%E9%99%B5" target="_blank"><img src="http://logo.taobao.com/shop-logo/49/f5/T1ZxXrXclfXXaCwpjX.png" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_blue_1.gif" /><br/>小陵</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=wushenju" target="_blank"><img src="http://logo.taobao.com/shop-logo/8e/85/T1_U4uXaNpXXb1upjX.jpg" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_4.gif" /><br/>wushenju</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=%E9%97%B2%E8%80%85%E6%82%A0%E5%93%89" target="_blank"><img src="http://logo.taobao.com/shop-logo/cf/1a/T1YWRBXXXrXXb1upjX.jpg" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_4.gif" /><br/>闲者悠哉</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=cao1007" target="_blank"><img src="http://logo.taobao.com/shop-logo/84/55/T122tpXaXiXXb1upjX.jpg" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_3.gif" /><br/>cao1007</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=%E5%B0%8F%E4%B9%88%E5%AD%901204" target="_blank"><img src="http://logo.taobao.com/shop-logo/d5/42/T14wdDXbNqXXartXjX.gif" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_2.gif" /><br/>小么子1204</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=jerry5735" target="_blank"><img src="http://logo.taobao.com/shop-logo/63/d0/T1mt4ZXc4HXXb1upjX" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_2.gif" /><br/>jerry5735</a>   </div>    <div class="photo" style="border: 1px solid #EEEEEE;text-align: center;margin: 0 5px;padding: 3px;overflow:hidden;height:120px">     <a href="http://adv.superboss.cc/shop.php?nick=huiyi1222" target="_blank"><img src="http://logo.taobao.com/shop-logo/98/03/9803a1c87b8ef66e5bd5c531f1b32edd_1152668591667.gif" width="80px" height="80px" /><br/><img src="http://pics.taobao.com/newrank/s_red_2.gif" /><br/>huiyi1222</a>   </div>  <p style="clear:both; height:5px;"></p> </div><br/>';
		$("#adv_fooler").show();
	}





var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_right_user');
if (adv_diy_id != null){
	document.getElementById('adv_right_user').innerHTML = document.getElementById('adv_right_user').innerHTML + '<div class="kuang">	<div class="title_list">		<div class="title_left">店铺信息</div>		<div class="title_right"  id="adv_right_title0"><table border="0"><tr><td>客服咨询：</td><td><a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=2265075069&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:2265075069:41 &r=0.34079961851239204" alt="给我留言哦，我们会尽快回复您" title="给我留言哦，我们会尽快回复您"></a> <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=2265075069&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:2265075069:41 &r=0.34079961851239204" alt="给我留言哦，我们会尽快回复您" title="给我留言哦，我们会尽快回复您"></a> <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=2265075069&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:2265075069:41 &r=0.34079961851239204" alt="给我留言哦，我们会尽快回复您" title="给我留言哦，我们会尽快回复您"></a> <a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=2265075069&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:2265075069:41 &r=0.34079961851239204" alt="给我留言哦，我们会尽快回复您" title="给我留言哦，我们会尽快回复您"></a> 投诉或建议：<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=2265075069&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:2265075069:41 &r=0.34079961851239204" alt="给我留言哦，我们会尽快回复您" title="给我留言哦，我们会尽快回复您"></a> </td></tr></table></div>		<p style="clear:both;"></p>	</div>		<div class="contents">			<table width="100%" border="0" cellspacing="0" cellpadding="0">			 <tr>  <td>	<p style="line-height:22px;height:22px">掌柜：<a href="#"><b><%=Nick %></b></a></p></td>  <td>  <p style="line-height:24px;">当前版本:<b><%=Copyright %></b></p></td>  <td><p style="line-height:24px;">到期时间：<span class="fred fb"><%=ExpiredTime %></span></p></td>  <td><!--<p style="line-height:24px;">剩余积分：<span class="fred fb" style="color:green">0</span> 点</p>--></td>  <td><p style="text-align:center; padding-top:6px;"><a class="btn-middle middle-red" href="http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=234454">续费/升级</a></p></td></tr>			</table>		</div>  </div><div class="clear_height"></div>';
		$("#adv_right_user").show();
	}





var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_left_user');
if (adv_diy_id != null){
	document.getElementById('adv_left_user').innerHTML = document.getElementById('adv_left_user').innerHTML + '<div class="clear_height"></div> <div class="ask" > <h4>系统公告</h4>  	<br/>      <div class="list01">      	<p style="line-height:24px;">拍拍店长新版将更加稳定、完善、强大，但是价格还是一如既往的便宜，在拍拍开店怎么能没有一个顺手的强大工具在手呢？值的拥有！</p><div style="height:14px"/></div>        <p width="150px;" align="right" id="weiboTd"></p>      </div></div>';
		$("#adv_left_user").show();
	}






var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_left_about');
if (adv_diy_id != null){
	document.getElementById('adv_left_about').innerHTML = document.getElementById('adv_left_about').innerHTML + ' <div class="ask"><h4>客户服务</h4>        <p style="padding:5px 0 0 1px; line-height:19px;">服务：9:00-18:00 <br/>	  客服：<a target="_blank" href="http://wpa.qq.com/msgrd?v=3&uin=2265075069&site=qq&menu=yes"><img border="0" src="http://wpa.qq.com/pa?p=2:2265075069:41 &r=0.34079961851239204" alt="给我留言哦，我们会尽快回复您" title="给我留言哦，我们会尽快回复您"></a>	  	  </p>  <div class="clear_height"></div>      <h2>QQ讨论群</h2>      <p style="padding:5px; line-height:19px;">949074926</p>  </div>';
		$("#adv_left_about").show();
	}





var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_center_item');
if (adv_diy_id != null){
	document.getElementById('adv_center_item').innerHTML = document.getElementById('adv_center_item').innerHTML + '<div class="clear_height"></div>  <div class="kuang">	<div class="title_list">		<div class="title_left">更多工具</div>		<div class="title_right" > <a style="color:#8C8C8C; display:none" href="http://bbs.taobao.com/catalog/thread/154503-251970535-1.htm" target="_blank">中小卖家如何在短时间内让流量翻倍</a>							</div>		<p style="clear:both;"></p>	</div>		<div class="contents">       <p style="color:#8c8c8c; line-height:18px; padding-top:5px;">利用工具事半功倍，在当今拍拍店铺竞争越来越激烈、营销成本越来高的情况下，利用好各种工具为店铺省时、省力、省广告费！ </p>        <div class="bd">		<ul>		<li>         <a href="http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=231314" target="_blank"><img src="http://img.paipaiimg.com/item-0136BBE9-00000000000000000000000000231314.1.jpg" style="width:80px;height:80px" data="一键在多款ios苹果app上推广您的店铺和宝贝，让您的宝贝走到无数买家的苹果手机里！！"/></a></li>		 <li><a  href="http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=229509" target="_blank"><img src="http://img.paipaiimg.com/item-012EED23-00000000000000000000000000229509.1.jpg" style="width:80px;height:80px" data="推广店铺，推广宝贝，提升流量。"/></a> </li> <li><a  href="http://fuwu.paipai.com/appstore/ui/my/app/appdetail.xhtml?appId=49485" target="_blank"><img src="http://img.paipaiimg.com/item-0E658898-00000000000000000000000000049485.1.jpg" style="width:80px;height:80px" data="内置博客营销、微博营销和社区营销等数十种营销方式，迅速覆盖网络。"/></a></li>		 		 </ul>       </div>    </div></div>';
		$("#adv_center_item").show();
	}





var float_type_center = 0;
if(GetCookie("float_type_center"))
{
	float_type_center = GetCookie("float_type_center");
}

var float_type_right_down = 0;
if(GetCookie("float_type_right_down"))
{
	float_type_right_down = GetCookie("float_type_right_down");
}

var float_type_left_down = 0;
if(GetCookie("float_type_left_down"))
{
	float_type_left_down = GetCookie("float_type_left_down");
}

var adv_diy_id = document.getElementById('adv_center_biz');
if (adv_diy_id != null){
	//document.getElementById('adv_center_biz').innerHTML = document.getElementById('adv_center_biz').innerHTML + '<div class="clear_height"></div><div id="tab_nav">	<ul id="nav">	<li style="margin-left:0px;"><a href="javascript:void(0);" class="current"  >增加店铺流量</a></li>		<li><a href="javascript:void(0);">促进购买转化</a></li>		<li><a href="javascript:void(0);">提高管理效率</a></li>		<li><a href="javascript:void(0);">优化店铺排名</a></li>		<li><a href="javascript:void(0);">开展促销活动</a></li>	</ul>	<div class="tab_contents">		<div class="tab_text" style="display:block">								<h4 style="line-height: 35px;">									拍拍开店流量是基础，采用下面方式帮助您快速提高店铺流量：								</h4>								<table style="margin-top: 10px;margin-bottom: 10px;" width="100%">									<tbody><tr>										<td height="25" width="20"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">1</span></td>										<td><p class="desc" style="margin-bottom: 0px;">开启<a href="/ProductUsingServlet?kind=31" style="color:#33CC00;">【自动橱窗】</a>，并设置好需要推荐的宝贝</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">2</span></td>										<td><p class="desc" style="margin-bottom: 0px;">使用<a href="/ProductUsingServlet?kind=121" style="color:#0033FF;">【站外营销】</a>功能，将宝贝、活动的分享放在宝贝详情中刺激买家帮助您推广</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">3</span></td>										<td><p class="desc" style="margin-bottom: 0px;">同一时间不要上架太多宝贝，利用<a href="/ProductUsingServlet?kind=32" style="color:#FF33FF;">【自动上下架】</a>将宝贝的上架时间错开</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">4</span></td>										<td><p class="desc" style="margin-bottom: 0px;">使用<a href="/ProductUsingServlet?kind=108" style="color:#660033;" target="_blank">【会员营销】</a>功能定时维护好老用户，每次活动千万不要忘记告诉他们</p>										</td>									</tr>								</tbody></table>									</div>		<div class="tab_text">										<h4 style="line-height: 35px;">									留住每一个进店客户，提升流量价值，帮助您刺激用户快速下单付款：								</h4>								<table style="margin-top: 10px;margin-bottom: 10px;" width="100%">									<tbody><tr>										<td height="25" width="20"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">1</span></td>										<td><p class="desc" style="margin-bottom: 0px;">首先利用<a href="/ProductUsingServlet?kind=113" style="color:#FF33FF;">【宝贝推荐】</a>功能将店铺中的所有宝贝进行网状关联，这可以让他看到更多的宝贝</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">2</span></td>										<td><p class="desc" style="margin-bottom: 0px;">再利用<a href="/ProductUsingServlet?kind=118" style="color:#0033FF;">【评价推荐】</a>将一些好评的宝贝放在宝贝详情页的上面或下面，看到有人好评购买的机会更大</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">3</span></td>										<td><p class="desc" style="margin-bottom: 0px;">利用<a href="/ProductUsingServlet?kind=3" style="color:#33CC00;" target="_blank">【店铺模板】</a>将店铺的样子装修的更加美观、简洁，要突显出宝贝，不要弄的太花俏</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">4</span></td>										<td><p class="desc" style="margin-bottom: 0px;">使用<a href="/ProductUsingServlet?kind=119" style="color:#660033;">【搭配套餐】</a>将一些用户最喜欢一起购买的宝贝打包一起出售，可以提高宝贝连带销售转化</p>										</td>									</tr>								</tbody></table>									</div>		<div class="tab_text">								<h4 style="line-height: 35px;">									帮助您降低人员成本、减少工作时间！打单、水印、批量修改一步到位，轻松自如：								</h4>								<table style="margin-top: 10px;margin-bottom: 10px;" width="100%">									<tbody><tr>										<td height="25" width="20"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">1</span></td>										<td><p class="desc" style="margin-bottom: 0px;">使用<a href="/ProductUsingServlet?kind=114" style="color:#FF33FF;" target="_blank">【促销水印】</a>可以批量给指定或所有宝贝增加各个活动促销水印，新品、包邮、打折应有尺有</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">2</span></td>										<td><p class="desc" style="margin-bottom: 0px;">每天使用<a href="/ProductUsingServlet?kind=106" style="color:#0033FF;">【发货管理】</a>，可以轻松打印快递单、包裹清单，一人每天发货几百、上千件不再是梦</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">3</span></td>										<td><p class="desc" style="margin-bottom: 0px;">采用<a href="/ProductUsingServlet?kind=30" style="color:#33CC00;">【批量修改】</a>宝贝让原来要花几小时的工作几分钟完成，并且准确无误，大大降低出错机率</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">4</span></td>										<td><p class="desc" style="margin-bottom: 0px;">查看<a href="/ProductUsingServlet?kind=15" style="color:#660033;" target="_blank">【客服绩效】</a>功能就可以对客服各方面的工作情况了如指掌，接待、转化、压力谁好谁差一目了然</p>										</td>									</tr>								</tbody></table>		</div>		<div class="tab_text">										<h4 style="line-height: 35px;">									关注店铺健康状态、优化标题、解析流量，及时修复店铺短板：								</h4>								<table style="margin-top: 10px;margin-bottom: 10px;" width="100%">									<tbody><tr>										<td height="25" width="20"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">1</span></td>										<td><p class="desc" style="margin-bottom: 0px;">每隔几天或一星期就使用<a href="/ProductUsingServlet?kind=116" style="color:#33CC00;">【店铺体检】</a>功能对店铺做一个健康程度分析，及时发现有问题的宝贝</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">2</span></td>										<td><p class="desc" style="margin-bottom: 0px;">使用<a href="/ProductUsingServlet?kind=109" style="color:#0033FF;">【行情分析】</a>了解所属行业关键字的情况与自己宝贝进行对比，再对标题进行不断的优化</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">3</span></td>										<td><p class="desc" style="margin-bottom: 0px;">每天查看<a href="/ProductUsingServlet?kind=1" style="color:#FF33FF;" target="_blank">【店铺统计】</a>功能，清楚每天的流量走势，知道直通车、淘客、店内搜索的各种情况</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">4</span></td>										<td><p class="desc" style="margin-bottom: 0px;">每个月初查看上个月的<a href="#" style="color:#660033;">【运营报告】</a>，及时发现上个月的不足与优势，适当进行调整与优化</p>										</td>									</tr>								</tbody></table>									</div>		<div class="tab_text">										<h4 style="line-height: 35px;">									活动是当今拍拍店铺的营销灵魂，只有不断开展活动诱导客户下单才能快速提高销量：								</h4>								<table style="margin-top: 10px;margin-bottom: 10px;" width="100%">									<tbody><tr>										<td height="25" width="20"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">1</span></td>										<td><p class="desc" style="margin-bottom: 0px;">利用<a href="/ProductUsingServlet?kind=117" style="color:#660066;">【团购推荐】</a>功能将一些相对火爆的商品进行团购促销，类似拍拍上的聚划算，吸人气又提转化</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">2</span></td>										<td><p class="desc" style="margin-bottom: 0px;">再将新上宝贝进一段时间内进行<a href="/ProductUsingServlet?kind=105" style="color:#0033FF;">【活动工具】</a>，快速拉升订购人数，这样后面才更好吸引其他人订购</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">3</span></td>										<td><p class="desc" style="margin-bottom: 0px;"><a href="/ProductUsingServlet?kind=119" style="color:#FF33FF;">【搭配套餐】</a>对于活动促销来说也不错，将二款宝贝打包降价一起卖，在成交记录中还是显示原价</p>										</td>									</tr>									<tr>										<td height="25"><span style="color:#ff0000;font-size:20px;font-family:\'微软雅黑\',\'黑体\'">4</span></td>										<td><p class="desc" style="margin-bottom: 0px;">在节日里还可以使用<a href="http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22806" style="color:#33CC00;" target="_blank">【幸运砸蛋】</a>、<a href="http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22918" style="color:#990000;" target="_blank">【淘翁富】</a>、<a href="http://fuwu.taobao.com/ser/detail.htm?service_code=ts-11806" style="color:#FF3300;" target="_blank">【超级营销】</a>，活动放在店铺首页效果更明显</p>										</td>									</tr>								</tbody></table>		</div>	</div>	</div><script type="text/javascript">	                                                                                                                                                               	             	                                                                                            		                                                 		                                                 		                                                 		                                                 	     	                          		                                            								                               									                            								     								                                                                  									           										                                                                                                               										                                                                                                                                          										     									     									    										                                                                                                    										                                                                                                                                                           										     									     									    										                                                                                                    										                                                                                                                                                      										     									     									    										                                                                                                    										                                                                                                                                                                        										     									     								                									      		                      										                               									                               								     								                                                                  									           										                                                                                                               										                                                                                                                                                               										     									     									    										                                                                                                    										                                                                                                                                                                   										     									     									    										                                                                                                    										                                                                                                                                                                            										     									     									    										                                                                                                    										                                                                                                                                                                 										     									     								                									      		                      								                               									                                     								     								                                                                  									           										                                                                                                               										                                                                                                                                                                                  										     									     									    										                                                                                                    										                                                                                                                                                                 										     									     									    										                                                                                                    										                                                                                                                                                                										     									     									    										                                                                                                    										                                                                                                                                                                                   										     									     								                		      		                      										                               									                            								     								                                                                  									           										                                                                                                               										                                                                                                                                                                 										     									     									    										                                                                                                    										                                                                                                                                                                										     									     									    										                                                                                                    										                                                                                                                                                                              										     									     									    										                                                                                                    										                                                                                                                                   										     									     								                									      		                      										                               									                                      								     								                                                                  									           										                                                                                                               										                                                                                                                                                                   										     									     									    										                                                                                                    										                                                                                                                                                                   										     									     									    										                                                                                                    										                                                                                                                                                                  										     									     									    										                                                                                                    										                                                                                                                                                                                                                                                                                                                                                                                                                                                               										     									     								                		      	      	          //设置TAG页选中状态set_tab_status(1);</script>';
	//	$("#adv_center_biz").show();
	}



//滚动信息
if (document.getElementById('adv_order_demo') != null){
		var speed = 1500;
	    var adv_order_demo = document.getElementById("adv_order_demo");
	    var adv_order_demo1 = document.getElementById("adv_order_demo1");
	    var adv_order_demo2 = document.getElementById("adv_order_demo2");
	    adv_order_demo2.innerHTML=adv_order_demo1.innerHTML//克隆adv_order_demo1为adv_order_demo2
	    function Marquee(){
		if(adv_order_demo2.offsetHeight-adv_order_demo.scrollTop<=0)//当滚动至adv_order_demo1与adv_order_demo2交界时
		adv_order_demo.scrollTop-=adv_order_demo1.offsetHeight//adv_order_demo跳到最顶
		else{
		    adv_order_demo.scrollTop  = adv_order_demo.scrollTop + 30 //如果是横向的 将 所有的 height top 改成 width left
		}
	    }
	    var MyMar=setInterval(Marquee,speed)//设置定时
	    adv_order_demo.onmouseover=function() {clearInterval(MyMar)}//鼠标移上时清除定时器达到滚动停止的目的
	    adv_order_demo.onmouseout=function() {MyMar=setInterval(Marquee,speed)}//鼠标移开时重设定时
}

/**  获取当前页面的来源地址 **/
var ref = '';
 if (document.referrer.length > 0) {
  ref = document.referrer;
 }
 try {
  if (ref.length == 0 && opener.location.href.length > 0) {
   ref = opener.location.href;
  }
 } catch (e) {}

if (document.getElementById('weiboTd') != null){
	document.getElementById("weiboTd").innerHTML="<iframe width='136' scrolling='no' height='24' frameborder='0' src='http://widget.weibo.com/relationship/followbutton.php?width=136&height=24&uid=1900547182&style=2&btn=red&dpc=1' border='0' marginheight='0' marginwidth='0' allowtransparency='true'></iframe>";
}
var globe_user_info_arr = new Array();
globe_user_info_arr[0]='<%=nick %>';
globe_user_info_arr[1]= 0;	
document.write('<scr'+'ipt src="js/super_boss_tongji_getUserInfo.js" type=text/javascript></scr'+'ipt>'); 



 var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-37160050-1']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? ' https://ssl' : ' http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();