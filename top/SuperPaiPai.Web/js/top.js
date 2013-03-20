//对话框全局设置
(function(config) {
	config['extendDrag'] = true; // 注意，此配置参数只能在这里使用全局配置，在调用窗口的传参数使用无效
	config['lock'] = true;
	// config['fixed'] = true;
	config['okVal'] = '确认';
	config['cancelVal'] = '取消';
	config['min'] = false;
	config['max'] = false;
	config['background'] = '#FFF';
	config['opacity'] = 0.5;
})($.dialog.setting);

// jquery ajax全局设置
$.ajaxSetup({
	cache : false,
	type : "POST",
	dataFilter : function(data, type) {
		// 可以在这里做登录失效或其他的权限跳转
		if (isSessionInvalid(data)) {
			alert("操作超时，请重新登录");
			return;
		}
		return data;
	}
});

// 全局domain设置
// document.domain="superboss.cc";

var raycloud;
if (raycloud == null) {
	raycloud = new Object();
}
if (raycloud.dialog == null) {
	raycloud.dialog = new Object();
}

raycloud.dialog.dialogById = function(htmlId, dialogOption) {
	var content = $("#" + htmlId).html();
	dialogOption.content = content;
	var closeFunction = dialogOption.close;
	var newCloseFunction;
	if (closeFunction) {
		newCloseFunction = function() {
			try {
				closeFunction();
			} finally {
				returnContentToDom(htmlId, content);
			}
		}
	} else {
		newCloseFunction = function() {
			returnContentToDom(htmlId, content);
		}
	}

	dialogOption.close = newCloseFunction;
	$("#" + htmlId).empty();
	$.dialog(dialogOption);
}

raycloud.dialog.tips = function() {
	return $.dialog.tips('加载中...', 100, 'loading.gif');
}

raycloud.dialog.successTips = function() {
	return $.dialog.tips('处理成功', 2, 'tips.gif');
}

function returnContentToDom(htmlId, content) {
	$("#" + htmlId).html(content);
}

// 验证session是否过期
function isSessionInvalid(data) {
	return false;
}

// 设置菜单状态
// 使用方法：set_menu_status(1,2,3); 1:一级菜单的排名号 2：二级菜单的排名号 3：三级菜单的排名号
function set_menu_status(one, two, three) {
	one = one - 1;
	two = two - 1;
	three = three - 1;
	$("#left_menu dl").removeClass("current");
	$("#left_menu dt").removeClass("current");
	$("#left_menu li").removeClass("current");

	var one_menu = $("#left_menu dl > a").eq(one).parents("dl");
	$(one_menu).addClass("current");

	var two_menu = $(one_menu).find("dt").eq(two);
	$(two_menu).addClass("current");

	var three_menu = $(two_menu).find("li").eq(three);
	$(three_menu).addClass("current");
}

// 左边导航最小化
function menu_size_min() {

	$(".main_left div").hide();

	var menu_width = $(".main_left").width();

	$(".main_left").css("width", "30px");
	$(".main_right").css("width", "952px");

	var menu_height = $(".main_right").height();
	$(".menu_none").css("height", menu_height);
	$(".menu_none").show();

	$(".menu_none").click(function() {
		$(".main_left").css("width", menu_width + "px");
		$(".main_right").css("width", "812px");
		$(".main_left div").show();
		$(this).hide();
		$(".main_left").css("background", "");
	});
}

function set_tab_status(numberic) {
	numberic = numberic - 1;
	$("#tab_nav ul li a").removeClass("current");
	$("#tab_nav .tab_text").hide();
	$("#tab_nav .tab_text").eq(numberic).show();
	$("#tab_nav ul li a").eq(numberic).addClass("current");
}

function set_pingbi(idname) {
	// $("#"+idname).colorbox({speed:1,transition:"none",open:true,html:$("#"+idname).html(),fixed:true,right:1,modal:false});
}

// 与分页相关
function pageGo(formId, pageNo) {
	$("#" + formId + " input[name='pageNo']").get(0).value = pageNo;
	$("#" + formId).submit();
}