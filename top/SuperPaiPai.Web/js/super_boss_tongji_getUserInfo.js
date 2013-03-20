var super_boss_tongji_domain = "http://tongji.superboss.cc";
//var super_boss_tongji_domain = "http://test.superboss.cc";
function super_boss_tongji_setUserInfos(infoArray) {
	var flag = "browserType=" + getBrowser("v") + "&browser=" + getBrowser("n")
			+ "&taobaoNick=" + infoArray[0] + "&top_parameters="
			+ getQueryString("top_parameters") + "&resolution="
			+ getBrowserResolution() + "&typeId=" + infoArray[1] + "&source="
			+ encodeURIComponent(document.referrer);
	document.write("<img src='" + super_boss_tongji_domain
			+ "/getClientInfo.jsp?" + flag + "' style='display:none;' /> ");
}
function getBrowser(n) {
	// 获取浏览器信息
	// getBrowser("n")所获得的就是浏览器名称
	// getBrowser("v")所获得的就是浏览器内核的版本号
	// getBrowser()所获得的就是浏览器内核加版本号
	var ua = navigator.userAgent.toLowerCase(), s, name = '', ver = 0;
	// 探测浏览器
	if (ua.indexOf('se 2.x') != -1) {
		(s = ua.match(/msie ([\d.]+)/)) ? _set("sogou", _toFixedVersion(s[1]))
				: 0;
	} else if (ua.indexOf('360se') != -1) {
		(s = ua.match(/msie ([\d.]+)/)) ? _set("360", _toFixedVersion(s[1]))
				: 0;
	} else {
		(s = ua.match(/msie ([\d.]+)/)) ? _set("ie", _toFixedVersion(s[1]))
				: (s = ua.match(/firefox\/([\d.]+)/)) ? _set("firefox",
						_toFixedVersion(s[1])) : (s = ua
						.match(/chrome\/([\d.]+)/)) ? _set("chrome",
						_toFixedVersion(s[1])) : (s = ua
						.match(/opera.([\d.]+)/)) ? _set("opera",
						_toFixedVersion(s[1])) : (s = ua
						.match(/version\/([\d.]+).*safari/)) ? _set("safari",
						_toFixedVersion(s[1])) : 0;
	}
	function _toFixedVersion(ver, floatLength) {
		ver = ('' + ver).replace(/_/g, '.');
		floatLength = floatLength || 1;
		ver = String(ver).split('.');
		ver = ver[0] + '.' + (ver[1] || '0');
		ver = Number(ver).toFixed(floatLength);
		return ver;
	}
	function _set(bname, bver) {
		name = bname;
		ver = bver;
	}
	return (n == 'n' ? name : (n == 'v' ? ver : name + ver));
};
function getBrowserResolution() {
	// 获取显示器分辨率
	var width = window.screen.width;
	var hight = window.screen.height;
	return width + "*" + hight;
}
function getQueryString(name) {
	// 获取url指定的url参数的值
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
	var r = window.location.search.substr(1).match(reg);
	if (r != null)
		return unescape(r[2]);
	return null;
}
super_boss_tongji_setUserInfos(globe_user_info_arr);
