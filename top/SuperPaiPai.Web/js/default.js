$(document).ready(function() {
	// 左边一级菜单绑定
	$("#left_menu dl > a").each(function(i) {

		$(this).click(function(event) {

			$("#left_menu dl").removeClass("current");
			$(this).parents("dl").addClass("current");

			var status = $(this).parents("dl").find("dt").css("display");
			$("#left_menu dl dt:not(this)").hide();

			if (status == "none") {
				$(this).parents("dl").find("dt").show();
			} else {
				$(this).parents("dl").find("dt").hide();
			}
			
			
			event.stopPropagation();
		});
	});

	// 左边二级菜单绑定
	$("#left_menu dt > a").each(function(i) {

		$(this).click(function(event) {

			$("#left_menu dt").removeClass("current");
			$(this).parents("dt").addClass("current");

			var status = $(this).parents("dt").find("ul").css("display");
			$("#left_menu dt ul").hide();
			if (status == "none") {
				$(this).parents("dt").find("ul").show();
			} else {
				$(this).parents("dt").find("ul").hide();
			}

			event.stopPropagation();
		});

	});
	
	$("#left_menu li a").each(function(i){
		$(this).click(function(event){
			event.stopPropagation();
		});
	});

	// 左边三级菜单绑定
	$("#left_menu li").each(function(i) {

		$(this).click(function(event) {
			var _a = $(this).find("a");
			var _u = $(_a).attr("href");
			window.location.href=_u;
			// $("#left_menu li").removeClass("current");
			// $(this).addClass("current");
			event.stopPropagation();
		});
	});

	//主动提示会员营销
	var bd_li_imgs = $(".bd ul li img");
	var crm_li_img = bd_li_imgs.get(6);
	var crm_li_value = $(crm_li_img).attr("data");
	var crm_li = ljtips(crm_li_img);
	var is_show_crm = false;

	// 鼠标触发提示语绑定
	$(".bd ul li img").each(function(i) {

		var tiptip = ljtips(this);

		var value = $(this).attr("data");

		if (value) {
			$(this).attr("data", "");

			$(this).mouseover(function() {
				if(is_show_crm) {
					$(crm_li_img).trigger("mouseout");
				} 
				tiptip.show({
					content : value,
					p : 'top'
				});
				is_show_crm = true;
				//crm_li.hide();
			});
			$(this).mouseout(function() {
				tiptip.hide();
				$(".lj-tipsWrap").hide();
				//crm_li.hide();
			});
		}
	});


	//crm_li.hide();
	$(crm_li_img).trigger("mouseover");

	// 表格色调交叉表示
	$(".cycle tr:even").addClass("even");

	// TAB绑定
	if (document.getElementById("tab_nav")) {
		//$("#tab_nav ul li a").attr("href", "javascript:void(0);");

		var tab_nav = $("#tab_nav");

		$("#tab_nav #nav li a").each(function(i) {

			$(this).click(function() {
				tab_nav.find("ul li a").removeClass("current");
				tab_nav.find(".tab_text").hide();
				tab_nav.find(".tab_text").eq(i).show();
				$(this).addClass("current");
			});

		});
	}

	try {
		selectTopNav();
	} catch (e) {
	}

});

function selectTopNav() {
	// 用来选中nav中的索引
	var globeNavObj = document.getElementById("navjs");
	if(globeNavObj) {
		var globeNavJsSrc = globeNavObj.src;
		if (globeNavJsSrc) {
			var globeNavIndex = globeNavJsSrc.indexOf("?");
			var globeNavParams = globeNavJsSrc.substring(globeNavIndex + 1);
			var navVersionIndex = globeNavParams.indexOf("&v=");
			if (navVersionIndex > -1) {
				globeNavParams = globeNavParams.substring(0, navVersionIndex);
			}

			var topNavIndex = globeNavParams.indexOf("index=");
			var topNavIndexStr;
			if (topNavIndex > -1) {
				topNavIndexStr = globeNavParams.substring(topNavIndex);
				var navIndex = topNavIndexStr.split("=")[1];
				var topNavLiArr = $("#nav_href ul li");
				$.each(topNavLiArr, function(i, n) {
					$(n).removeClass("current");
				});
				$(topNavLiArr.get(navIndex)).addClass("current");
			}
		}
	}
}
