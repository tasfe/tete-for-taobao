var _cs='';
_cs += "<scr"+"ipt src=\"js/tooltip.js\" type=\"text/javascript\"></scr"+"ipt>";
_cs += "<div class=\"clear_height\"></div>";
_cs += "<div class=\"partner\">";
_cs += "	<div id=\"adv_fooler\"></div>";
_cs += "	<div class=\"foot\">";
_cs += "		<p class=\"about_left\">Copyright©2008-2013; 特特软件 All Rights Reserved";
_cs += "			沪ICP备10023864号</p>";
_cs += "		<p class=\"about_right\">";
//_cs += "			<a href=\"http://raycloud.com/about.html\" target=\"_blank\">关于我们</a>&nbsp;|&nbsp;<a href=\"http://raycloud.com/contact.html\" target=\"_blank\">联系我们</a>&nbsp;|&nbsp;<a";
//_cs += "				href=\"http://bangpai.taobao.com/group/548716.htm\" target=\"_blank\">在线留言</a>";
_cs += "		</p>";
_cs += "		<p style=\"clear: both;\"></p>";
_cs += "	</div>";
_cs += "</div>";
_cs += "";

function getCookie(name)    
{
    var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
    if(arr != null) return unescape(arr[2]); return null;
}
var _visitorId = getCookie("visitorId");
var _nick = getCookie("nick");
//if(getCookie("nick")=="yukilovesummer"){

	//_cs += "<scr"+"ipt type=\"text/javascript\" src=\"js/new-super-course.js?t="+(new Date().getTime())+"\"></scr"+"ipt>";
	
//}
_cs += "<scr"+"ipt type=\"text/javascript\" src=\"adv.aspx?uid="+_visitorId+"&nick="+_nick+"\"></scr"+"ipt>";


document.write(_cs);
