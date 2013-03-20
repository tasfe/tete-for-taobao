var _cs='';
_cs += "<div class=\"main_left\">";
_cs += "	<div class=\"menu_none\">";
_cs += "		<img src=\"images/forward-ltr.png\" /> 点<br /> <br />击<br /> <br />显<br />";
_cs += "		<br />示<br /> <br />左<br /> <br />边<br /> <br />菜<br /> <br />单";
_cs += "	</div>";
_cs += "	<div id=\"left_menu\"></div>";
_cs += "";
_cs += "	<div id=\"adv_left_user\">";
_cs += "		<div style=\"height: 200px;\" class=\"ask\">";
_cs += "			<table style=\"width: 100%;height: 100%;\">";
_cs += "				<tr>";
_cs += "					<td valign=\"middle\" align=\"center\">";
_cs += "						<img alt=\"\" src=\"images/large-loading.gif\" />";
_cs += "					</td>";
_cs += "				</tr>";
_cs += "			</table>";
_cs += "		</div>";
_cs += "		<div class=\"clear_height\"></div>";
_cs += "	</div>";
_cs += "	<div id=\"adv_left_order\">";
_cs += "		<div style=\"height: 200px;\" class=\"ask\">";
_cs += "			<table style=\"width: 100%;height: 100%;\">";
_cs += "				<tr>";
_cs += "					<td valign=\"middle\" align=\"center\">";
_cs += "						<img alt=\"\" src=\"images/large-loading.gif\" />";
_cs += "					</td>";
_cs += "				</tr>";
_cs += "			</table>";
_cs += "		</div>";
_cs += "	</div>";
_cs += "	<div id=\"adv_left_qa\"></div>";
_cs += "	<div id=\"adv_left_about\"></div>";
_cs += "</div>";
document.write(_cs)

function getSecondChildNodeHTML(arr){
	var h = "";
	if(arr && arr.length>0){
		h+= '<dt><ul>';
		for(var i=0;i<arr.length;i++){
			var o = arr[i];
			h+='<li id="'+o.key+'"><a href="'+o.href+'">'+o.name+'</a></li>';
		}
		h+='</ul></dt>';
		return h;
	}
	return h;
}

function getThridChildNodeHTML(obj,arr){
	var h = "";
	if(obj != null){
		h+='<dt id="'+obj.key+'"><a href="javascript:void(0)">'+obj.name+'</a><ul>';
		for(var o in arr){
			var to = arr[o];
			h+='<li id="'+to.key+'"><a href="'+to.href+'">'+to.name+'</a></li>';
		}
		h+='</ul></dt>';
	}
	return h;
}

function getSecondNode(id, secondNodes){
	for(var i=0;i<secondNodes.length;i++) {
		var node = secondNodes[i];
		if(node.id == id) {
			return node;
		}
	}
	return null;
}

function selectNavNode(appType) {
	var topNavLiArr = $("#nav_href ul li");
	$.each(topNavLiArr,function(i,n){
		$(n).removeClass("current");
	});
	try {
		$(topNavLiArr.get(appType)).addClass("current");
	} catch (e) {
	}
}

//var globeMenuJson = '[{"appType":1,"href":"","id":1,"key":"dianputijian","kindId":116,"name":"店铺体检","parentId":0,"rank":1,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":2,"key":"tjshouye","kindId":0,"name":"首页","parentId":1,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/plan/stratCheck.do","id":3,"key":"tjkaishi","kindId":0,"name":"开始体检","parentId":1,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/report/report.do","id":5,"key":"tj_report","kindId":0,"name":"体检报告","parentId":1,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":3,"href":"","id":4,"key":"canmou","kindId":109,"name":"行情参谋","parentId":0,"rank":2,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":6,"key":"cm-home","kindId":0,"name":"行情分析","parentId":4,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"","id":7,"key":"cm-shop-index","kindId":0,"name":"店铺分析","parentId":4,"rank":2,"showInLeft":false,"subMenus":[{"appType":0,"href":"/shop_index.do","id":10,"key":"cm_shop","kindId":0,"name":"店铺分析","parentId":7,"rank":0,"showInLeft":false,"type":"2"}],"type":"1"},{"appType":0,"href":"","id":8,"key":"cm-keyword","kindId":0,"name":"关键字","parentId":4,"rank":3,"showInLeft":false,"subMenus":[{"appType":0,"href":"/keyword_index.do","id":33,"key":"cm-keyword-index","kindId":0,"name":"关键字首页","parentId":8,"rank":0,"showInLeft":false,"type":"2"},{"appType":0,"href":"/keyword_search.do","id":34,"key":"cm-keyword-search","kindId":0,"name":"关键字查询","parentId":8,"rank":1,"showInLeft":false,"type":"2"}],"type":"1"},{"appType":0,"href":"","id":9,"key":"cm-item","kindId":0,"name":"宝贝分析","parentId":4,"rank":4,"showInLeft":false,"subMenus":[{"appType":0,"href":"/item_index.do","id":27,"key":"cm-item-index","kindId":0,"name":"宝贝排行","parentId":9,"rank":0,"showInLeft":false,"type":"2"},{"appType":0,"href":"/item_search.do","id":16,"key":"cm-item-search","kindId":0,"name":"宝贝查询","parentId":9,"rank":1,"showInLeft":false,"type":"2"},{"appType":0,"href":"/item_list.do","id":32,"key":"cm-item-list","kindId":0,"name":"我的宝贝","parentId":9,"rank":2,"showInLeft":false,"type":"2"},{"appType":0,"href":"/itemTitleAnalyse_queryAllItems.do","id":35,"key":"cm-item-analyse","kindId":0,"name":"宝贝优化","parentId":9,"rank":3,"showInLeft":false,"type":"2"}],"type":"1"}],"type":"1"},{"appType":2,"href":"","id":12,"key":"bbtj","kindId":113,"name":"宝贝推荐","parentId":0,"rank":3,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":13,"key":"bb_index","kindId":0,"name":"首页","parentId":12,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/temp_temps.do","id":22,"key":"bb_temps","kindId":0,"name":"创建活动","parentId":12,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/activity_list.do","id":23,"key":"bb_activity","kindId":0,"name":"查看活动","parentId":12,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/analyse_list.do","id":36,"key":"bb_analyse","kindId":0,"name":"活动效果","parentId":12,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/log_logs.do","id":24,"key":"bb_log","kindId":0,"name":"操作日志","parentId":12,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":1,"href":"","id":18,"key":"instocklisting","kindId":104,"name":"仓库上架","parentId":0,"rank":4,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":19,"key":"in_index","kindId":0,"name":"首页","parentId":18,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/setting.do","id":20,"key":"in_cp","kindId":0,"name":"创建计划","parentId":18,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/planlist.do","id":21,"key":"in_pls","kindId":0,"name":"计划列表","parentId":18,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/report.do","id":25,"key":"in_report","kindId":0,"name":"计划报告","parentId":18,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":5,"href":"","id":28,"key":"s_crm","kindId":108,"name":"会员营销","parentId":0,"rank":5,"showInLeft":false,"subMenus":[{"appType":0,"href":"/index.do","id":29,"key":"crm_index","kindId":0,"name":"首页","parentId":28,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/customer/customer_query.do","id":30,"key":"crm_hygl","kindId":0,"name":"会员管理","parentId":28,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/customer/grade_promotion_view.do","id":59,"key":"crm_grade","kindId":0,"name":"会员等级","parentId":28,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/marketing/marketing_index.do","id":31,"key":"crm_hyfu","kindId":0,"name":"短信服务","parentId":28,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/report/msgsend_report_view.do","id":68,"key":"crm_msgsend_report","kindId":0,"name":"发送日志","parentId":28,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/setting/view.do","id":86,"key":"xxsz","kindId":0,"name":"信息设置","parentId":28,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":1,"href":"","id":37,"key":"comment","kindId":29,"name":"自动评价","parentId":0,"rank":6,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":41,"key":"index","kindId":0,"name":"评价首页","parentId":37,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/set.do","id":39,"key":"com_set","kindId":0,"name":"评价设置","parentId":37,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/report.do","id":42,"key":"report","kindId":0,"name":"评价报告","parentId":37,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/warning.do","id":156,"key":"warning","kindId":0,"name":"中差评报告","parentId":37,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":44,"key":"discount","kindId":105,"name":"限时折扣","parentId":0,"rank":7,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":45,"key":"ds_index","kindId":0,"name":"首页","parentId":44,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/createActive.do","id":46,"key":"ds_create","kindId":0,"name":"创建活动","parentId":44,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/active_query_result.do","id":47,"key":"ds_query","kindId":0,"name":"活动列表","parentId":44,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/item_queryAllItem.do","id":141,"key":"ds_query_item","kindId":0,"name":"宝贝查询","parentId":44,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":48,"key":"modify","kindId":30,"name":"批量修改","parentId":0,"rank":8,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":49,"key":"modify_index","kindId":0,"name":"批量修改首页","parentId":48,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/title.do","id":51,"key":"modify_title1","kindId":0,"name":"修改商品标题","parentId":48,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/price.do","id":52,"key":"modify_price","kindId":0,"name":"修改商品价格","parentId":48,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/stock.do","id":54,"key":"modify_stock","kindId":0,"name":"修改商品库存","parentId":48,"rank":6,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/desc.do","id":55,"key":"modify_desc1","kindId":0,"name":"修改商品描述","parentId":48,"rank":7,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/post.do","id":56,"key":"modify_post","kindId":0,"name":"修改商品邮费","parentId":48,"rank":8,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/other.do","id":57,"key":"modify_other","kindId":0,"name":"修改其它信息","parentId":48,"rank":9,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/report.do","id":58,"key":"modify_report","kindId":0,"name":"修改日志报表","parentId":48,"rank":10,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":60,"key":"tgtj","kindId":117,"name":"团购推荐","parentId":0,"rank":9,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":63,"key":"group_index","kindId":0,"name":"首页","parentId":60,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/temp_temps.do","id":65,"key":"group_create","kindId":0,"name":"创建活动","parentId":60,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/activity_list.do","id":64,"key":"group_activity","kindId":0,"name":"查看活动","parentId":60,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/analyse_list.do","id":66,"key":"group_analyse","kindId":0,"name":"活动效果","parentId":60,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/log_logs.do","id":67,"key":"group_log","kindId":0,"name":"活动日志","parentId":60,"rank":6,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":69,"key":"fahuo","kindId":106,"name":"发货管理","parentId":0,"rank":10,"showInLeft":true,"subMenus":[{"appType":0,"href":"/count_index.do","id":83,"key":"shouye","kindId":0,"name":"首页","parentId":69,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/queryOrders_kuaidi.do","id":70,"key":"kuaidi","kindId":0,"name":"打印快递单","parentId":69,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/queryOrders_fahuodan.do","id":81,"key":"fahuodan","kindId":0,"name":"打印发货单","parentId":69,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/queryOrders_fahuo.do","id":80,"key":"fhgl","kindId":0,"name":"批量发货","parentId":69,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/count_list.do","id":85,"key":"fhglbb","kindId":0,"name":"发货报表","parentId":69,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/printInfo_view.do","id":78,"key":"qxpz","kindId":0,"name":"发货地址配置","parentId":69,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/noticeLog_query.do","id":82,"key":"xxtz","kindId":0,"name":"消息通知","parentId":69,"rank":6,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/designKuaidi.do","id":84,"key":"kddsj","kindId":0,"name":"快递单设计","parentId":69,"rank":7,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":71,"key":"weibo","kindId":121,"name":"站外营销","parentId":0,"rank":11,"showInLeft":true,"subMenus":[{"appType":0,"href":"/forward_toIndex.do","id":72,"key":"weibo_index_li","kindId":0,"name":"首页","parentId":71,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/forward_toBindWeiBo.do","id":73,"key":"weibo_bind_li","kindId":0,"name":"微博绑定","parentId":71,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/forward_toAutoSend.do","id":74,"key":"weibo_auto_li","kindId":0,"name":"自动群发","parentId":71,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/forward_toManualSend.do","id":75,"key":"weibo_handed_li","kindId":0,"name":"手动群发","parentId":71,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/forward_toAttractFan.do","id":76,"key":"sc_attract_li","kindId":0,"name":"自动吸粉","parentId":71,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/forward_toShareAlliance.do","id":77,"key":"weibo_share_li","kindId":0,"name":"分享联盟","parentId":71,"rank":6,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/forward_toAnalysis.do","id":79,"key":"weibo_statistics_li","kindId":0,"name":"效果分析","parentId":71,"rank":7,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/jsp/toOldVersion.jsp","id":170,"key":"weibo_old_version","kindId":0,"name":"返回老版","parentId":71,"rank":8,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"/index.do","id":87,"key":"promotions","kindId":0,"name":"促销助手","parentId":0,"rank":12,"showInLeft":false,"subMenus":[{"appType":0,"href":"/index.do","id":88,"key":"","kindId":0,"name":"首页","parentId":87,"rank":1,"showInLeft":false,"subMenus":[{"appType":0,"href":"","id":89,"key":"pro_index","kindId":0,"name":"首页1","parentId":88,"rank":0,"showInLeft":false,"type":"2"}],"type":"1"}],"type":"1"},{"appType":0,"href":"/index.do","id":90,"key":"promotion","kindId":0,"name":"促销助手","parentId":0,"rank":13,"showInLeft":false,"subMenus":[{"appType":0,"href":"55","id":92,"key":"pro_create","kindId":0,"name":"55","parentId":90,"rank":0,"showInLeft":false,"subMenus":[],"type":"1"},{"appType":0,"href":"/active_queryList.do","id":93,"key":"pro_active_list","kindId":0,"name":"投放列表","parentId":90,"rank":1,"showInLeft":false,"subMenus":[],"type":"1"},{"appType":0,"href":"/index.do","id":91,"key":"pro","kindId":0,"name":"首页","parentId":90,"rank":2,"showInLeft":false,"subMenus":[],"type":"1"},{"appType":0,"href":"/log_queryLog.do","id":94,"key":"pro_log","kindId":0,"name":"日志报告","parentId":90,"rank":3,"showInLeft":false,"subMenus":[],"type":"1"}],"type":"1"},{"appType":0,"href":"","id":95,"key":"autoshowcase","kindId":0,"name":"自动橱窗","parentId":0,"rank":14,"showInLeft":false,"subMenus":[{"appType":0,"href":"/showcaseItem.do","id":178,"key":"showcaseItem","kindId":0,"name":"橱窗商品","parentId":95,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/onsaleItems.do","id":179,"key":"onsaleItem","kindId":0,"name":"出售中商品","parentId":95,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/index.do","id":96,"key":"showcase_index","kindId":0,"name":"橱窗推荐","parentId":95,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/setting.do","id":97,"key":"showcase_setting","kindId":0,"name":"设置规则","parentId":95,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/detail.do","id":98,"key":"showcase_detail","kindId":0,"name":"推荐详情","parentId":95,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/report.do","id":99,"key":"showcase_report","kindId":0,"name":"橱窗分析","parentId":95,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":100,"key":"comment_recommend","kindId":118,"name":"评价推荐","parentId":0,"rank":15,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":101,"key":"cmtRecommend_index","kindId":0,"name":"首页","parentId":100,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/temp_temps.do","id":102,"key":"cmtRecommend_create","kindId":0,"name":"创建活动","parentId":100,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/activity_list.do","id":104,"key":"cmtRecommend_activitys","kindId":0,"name":"查看活动","parentId":100,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/analyse_list.do","id":105,"key":"cmtRecommend_analyse","kindId":0,"name":"活动效果","parentId":100,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/log_logs.do","id":106,"key":"cmtRecommend_logs","kindId":0,"name":"活动日志","parentId":100,"rank":6,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"","id":107,"key":"cmtRecommend_show","kindId":0,"name":"宝贝好评","parentId":100,"rank":7,"showInLeft":false,"subMenus":[{"appType":0,"href":"/comment_temps.do","id":108,"key":"cmtRecommend_addShow","kindId":0,"name":"添加好评","parentId":107,"rank":1,"showInLeft":false,"type":"2"},{"appType":0,"href":"/comment_quit.do","id":109,"key":"cmtRecommend_quitShow","kindId":0,"name":"批量删除","parentId":107,"rank":2,"showInLeft":false,"type":"2"},{"appType":0,"href":"/comment_babys.do","id":110,"key":"cmtRecommend_babysShow","kindId":0,"name":"好评宝贝","parentId":107,"rank":3,"showInLeft":false,"type":"2"},{"appType":0,"href":"/comment_logs.do","id":132,"key":"cmtRecommend_gcLogs","kindId":0,"name":"好评日志","parentId":107,"rank":4,"showInLeft":false,"type":"2"}],"type":"1"}],"type":"1"},{"appType":0,"href":"/index.do","id":111,"key":"cx_promotion","kindId":0,"name":"促销助手","parentId":0,"rank":16,"showInLeft":false,"subMenus":[{"appType":0,"href":"/index.do","id":112,"key":"cx_index","kindId":0,"name":"首页","parentId":111,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/active_createWaterMark.do","id":113,"key":"cx_create","kindId":0,"name":"添加水印","parentId":111,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/active_queryList.do","id":114,"key":"cx_list","kindId":0,"name":"投放列表","parentId":111,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/activeItem_queryActiedItemResult.do","id":115,"key":"cx_queryItem","kindId":0,"name":"活动中宝贝","parentId":111,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"http://f.superboss.cc/ProductUsingServlet?kind=119","id":116,"key":"dptc","kindId":119,"name":"搭配套餐","parentId":0,"rank":17,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":119,"key":"dptc_shouye","kindId":0,"name":"首页","parentId":116,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/temp_temps.do","id":118,"key":"dptc_cjhd","kindId":0,"name":"创建活动","parentId":116,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/activity_list.do","id":120,"key":"dptc_list","kindId":0,"name":"查看活动","parentId":116,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/analyse_list.do","id":121,"key":"dptc_analyse","kindId":0,"name":"活动效果","parentId":116,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/log_logs.do","id":122,"key":"dptc_logs","kindId":0,"name":"活动日志","parentId":116,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"/","id":123,"key":"sizetemplate","kindId":127,"name":"尺寸模板","parentId":0,"rank":18,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":125,"key":"size_index","kindId":0,"name":"首页","parentId":123,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"size_table","id":126,"key":"size_add","kindId":0,"name":"尺寸对照表","parentId":123,"rank":2,"showInLeft":false,"subMenus":[{"appType":0,"href":"/sizetable/sizeView.do","id":135,"key":"size_view","kindId":0,"name":"查看尺寸对照表","parentId":126,"rank":0,"showInLeft":false,"type":"2"},{"appType":0,"href":"/sizetable/sizeAddStart.do","id":127,"key":"size_add1","kindId":0,"name":"添加尺寸对照表","parentId":126,"rank":1,"showInLeft":false,"type":"2"},{"appType":0,"href":"/sizetable/sizeUpdate.do","id":128,"key":"size_update","kindId":0,"name":"修改尺寸对照表","parentId":126,"rank":2,"showInLeft":false,"type":"2"},{"appType":0,"href":"/sizetable/sizeDelStart.do","id":142,"key":"size_del","kindId":0,"name":"删除尺寸信息","parentId":126,"rank":3,"showInLeft":false,"type":"2"}],"type":"1"},{"appType":0,"href":"/sizebaby","id":136,"key":"size_baby","kindId":0,"name":"宝贝尺寸信息管理","parentId":123,"rank":3,"showInLeft":false,"subMenus":[{"appType":0,"href":"/sizebaby/sizebaby.do","id":137,"key":"sizebabystart","kindId":0,"name":"我的尺寸模板","parentId":136,"rank":1,"showInLeft":false,"type":"2"}],"type":"1"},{"appType":0,"href":"/sizelog/loglist.do","id":139,"key":"operate_log","kindId":0,"name":"操作日志","parentId":123,"rank":5,"showInLeft":false,"subMenus":[{"appType":0,"href":"/sizelog/loglist.do","id":140,"key":"size_logs","kindId":0,"name":"日志列表","parentId":139,"rank":1,"showInLeft":false,"type":"2"}],"type":"1"}],"type":"1"},{"appType":0,"href":"","id":129,"key":"hbtuiguang","kindId":126,"name":"海报推广","parentId":0,"rank":19,"showInLeft":true,"subMenus":[{"appType":0,"href":"/index.do","id":130,"key":"hb_index","kindId":0,"name":"首页","parentId":129,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/temp_temps.do","id":131,"key":"hb_temp","kindId":0,"name":"创建海报","parentId":129,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/myTemp_myTemps.do","id":180,"key":"hb_myTemp","kindId":0,"name":"我的模板","parentId":129,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/active_list.do","id":133,"key":"hb_activity","kindId":0,"name":"活动列表","parentId":129,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/log_logs.do","id":134,"key":"hb_log","kindId":0,"name":"操作日志","parentId":129,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":143,"key":"bbbf","kindId":125,"name":"宝贝备份","parentId":0,"rank":20,"showInLeft":true,"subMenus":[{"appType":0,"href":"/bakItem_getUserBakVersion.action","id":144,"key":"sdbf","kindId":0,"name":"手动备份","parentId":143,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/bakItem_getOldBak.action?menuId=1","id":145,"key":"lsbf","kindId":0,"name":"历史备份","parentId":143,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/bakItem_getRestoreLog.action?menuId=2","id":146,"key":"hfrz","kindId":0,"name":"恢复日志","parentId":143,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"},{"appType":0,"href":"","id":147,"key":"rilimuban","kindId":110,"name":"日历模板","parentId":0,"rank":21,"showInLeft":true,"subMenus":[{"appType":0,"href":"/jsp/super_calendar_manage.jsp","id":148,"key":"riliguanlishouye","kindId":0,"name":"首页","parentId":147,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/jsp/super_calendar_code.jsp","id":149,"key":"huoqurilidaima","kindId":0,"name":"获取代码","parentId":147,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/jsp/super_calendar_collect.jsp","id":150,"key":"woshoucangderiliyangshi","kindId":0,"name":"我的收藏","parentId":147,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/jsp/super_calendar_lovely.jsp","id":153,"key":"genghuanrili","kindId":0,"name":"更换日历","parentId":147,"rank":6,"showInLeft":false,"subMenus":[{"appType":0,"href":"/jsp/super_calendar_lovely.jsp","id":154,"key":"lovely","kindId":0,"name":"可爱美观系列","parentId":153,"rank":1,"showInLeft":false,"type":"2"},{"appType":0,"href":"/jsp/super_calendar.jsp","id":155,"key":"common","kindId":0,"name":"简洁大方系列","parentId":153,"rank":2,"showInLeft":false,"type":"2"}],"type":"1"}],"type":"1"},{"appType":0,"href":"/item_queryIndexItem.do","id":157,"key":"active_promotion","kindId":0,"name":"活动工具","parentId":0,"rank":22,"showInLeft":false,"subMenus":[{"appType":0,"href":"/item_queryIndexItem.do","id":164,"key":"index_active","kindId":0,"name":"首页","parentId":157,"rank":0,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"","id":158,"key":"active_discount","kindId":0,"name":"活动促销","parentId":157,"rank":2,"showInLeft":false,"subMenus":[{"appType":0,"href":"/forward_create.do?type=discount","id":160,"key":"discount_create","kindId":0,"name":"限时折扣","parentId":158,"rank":0,"showInLeft":false,"type":"2"},{"appType":0,"href":"/forward_create.do?type=one","id":165,"key":"active_create","kindId":0,"name":"一件优惠","parentId":158,"rank":2,"showInLeft":false,"type":"2"},{"appType":0,"href":"/forward_create.do?type=purchase","id":168,"key":"active_purchase","kindId":0,"name":"限购","parentId":158,"rank":4,"showInLeft":false,"type":"2"},{"appType":0,"href":"/forward_create.do?type=post","id":176,"key":"active_post","kindId":0,"name":"包邮","parentId":158,"rank":5,"showInLeft":false,"type":"2"},{"appType":0,"href":"/forward_create.do?type=ladder","id":177,"key":"ladder","kindId":0,"name":"阶梯价","parentId":158,"rank":6,"showInLeft":false,"type":"2"},{"appType":0,"href":"/forward_create.do?type=reward","id":181,"key":"active_reward","kindId":0,"name":"满就减","parentId":158,"rank":7,"showInLeft":false,"type":"2"},{"appType":0,"href":"/forward_create.do?type=order","id":183,"key":"active_order","kindId":0,"name":"会员打折","parentId":158,"rank":8,"showInLeft":false,"type":"2"}],"type":"1"},{"appType":0,"href":"/active_query_result.do","id":159,"key":"active_one","kindId":0,"name":"活动列表","parentId":157,"rank":3,"showInLeft":false,"subMenus":[{"appType":0,"href":"/active_query_result.do","id":161,"key":"active_list","kindId":0,"name":"活动列表","parentId":159,"rank":1,"showInLeft":false,"type":"2"},{"appType":0,"href":"/item_queryAllItem.do","id":162,"key":"item_query","kindId":0,"name":"宝贝查询","parentId":159,"rank":2,"showInLeft":false,"type":"2"}],"type":"1"}],"type":"1"},{"appType":0,"href":"","id":171,"key":"autoadjust","kindId":0,"name":"自动上下架","parentId":0,"rank":23,"showInLeft":false,"subMenus":[{"appType":0,"href":"/index.do","id":172,"key":"adjust_index","kindId":0,"name":"首页","parentId":171,"rank":1,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/setting.do","id":173,"key":"adjust_arrange","kindId":0,"name":"创建计划","parentId":171,"rank":2,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/detail.do","id":174,"key":"adjust_detail","kindId":0,"name":"调整详情","parentId":171,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"/analyse.do","id":175,"key":"adjust_analyse","kindId":0,"name":"调整分析","parentId":171,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"}]'
var globeMenuJson = '[{"appType":1,"href":"","id":28,"key":"s_crm","kindId":108,"name":"会员营销","parentId":0,"rank":1,"showInLeft":false,"subMenus":[{"appType":0,"href":"huiyuan.aspx","id":31,"key":"huiyuan","kindId":0,"name":"短信服务","parentId":28,"rank":3,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"msglog.aspx","id":68,"key":"msglog","kindId":0,"name":"发送日志","parentId":28,"rank":4,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"msgbuy.aspx","id":86,"key":"msgbuy","kindId":0,"name":"购买短信","parentId":28,"rank":5,"showInLeft":false,"subMenus":[],"type":"2"},{"appType":0,"href":"msgsetname.aspx","id":87,"key":"msgsetname","kindId":0,"name":"信息设置","parentId":28,"rank":6,"showInLeft":false,"subMenus":[],"type":"2"}],"type":"1"}]';
var globeMenuObj = eval('(' + globeMenuJson + ')');
var globeMenuJsSrc = document.getElementById("menujs").src;
var globeMenuIndex = globeMenuJsSrc.indexOf("?");
var globeMenuParams = globeMenuJsSrc.substring(globeMenuIndex + 1);
var versionIndex = globeMenuParams.indexOf("&v=");
if(versionIndex > -1) {
	globeMenuParams = globeMenuParams.substring(0, versionIndex);
}

//检测是否显示menu
var isShow = true;
var showMenuIndex = globeMenuParams.indexOf("&show=false");
var countLeftNode = true;
if(showMenuIndex > -1) {
	isShow = false;
	globeMenuParams = globeMenuParams.replace(/&show=false/,"");
	$("#left_menu").hide();
	//找到指定显示头部的索引
	var selectNavIndex = globeMenuParams.indexOf("index=");
	var selectNavIndexStr; 
	if(selectNavIndex > -1) {
		var andMarkIndex =  globeMenuParams.indexOf("&");
		if(andMarkIndex > -1) {
			selectNavIndexStr = globeMenuParams.substring(0, andMarkIndex);
		} else {
			selectNavIndexStr = globeMenuParams;
		}
		var navIndex = selectNavIndexStr.split("=")[1];
		var topNavLiArr = $("#nav_href ul li");
		$.each(topNavLiArr,function(i,n){
			$(n).removeClass("current");
		});
		$(topNavLiArr.get(navIndex)).addClass("current");
		countLeftNode = false;
	}
}
var globeMenuParamArr = globeMenuParams.split("&");
if(globeMenuParamArr.length == 0){
	alert(":menuJS:参数至少有1个");
}
var globeMemuId = globeMenuParamArr[0].split("=")[1];
var globeSecondKey = null;
if(globeMenuParamArr.length >= 2) {
	globeSecondKey = globeMenuParamArr[1].split("=")[1];
}
var globeThridKey = (globeMenuParamArr.length == 3) ? globeMenuParamArr[2].split("=")[1] : null;
var secondObj;
var $leftMenu = $("#left_menu");
var secondNodeArr = new Array();
var allHTML = '';
var selectedSecondNode;
var thridSecondNode;
if(countLeftNode) {
	secondObj = getSecondNode(globeMemuId, globeMenuObj);
}
if(secondObj){
	//选中头部的菜单
	selectNavNode(secondObj.appType);
	
	var headHTML = '<dl class="current"><a href="javascript:void(0)">'+secondObj.name+'</a>';
	for(var i=0; i<secondObj.subMenus.length;i++){
		var subMenu = secondObj.subMenus[i];
		if(subMenu.key == globeSecondKey) {
			selectedSecondNode = subMenu;
		}
		if(subMenu.type == "2"){//is a child
			secondNodeArr.push(subMenu);
		}else{
			var thridNodes = subMenu.subMenus;
			if(thridNodes){
				var tHTML = getThridChildNodeHTML(subMenu, thridNodes);
				allHTML += tHTML;
			}
		}
	}
	allHTML = getSecondChildNodeHTML(secondNodeArr) + allHTML;
	allHTML = headHTML + allHTML + "</dl>";
}

var rootHrefObj = [{"has_new":true,"key":"dianputijian","new_v":116,"old_v":116},{"has_new":true,"key":"canmou","new_v":109,"old_v":109},{"has_new":true,"key":"instocklisting","new_v":104,"old_v":104},{"has_new":true,"key":"bbtj","new_v":113,"old_v":113},{"has_new":true,"key":"comment","new_v":29,"old_v":29},{"has_new":true,"key":"tgtj","new_v":117,"old_v":117},{"has_new":true,"key":"discount","new_v":105,"old_v":105},{"has_new":true,"key":"modify","new_v":30,"old_v":30},{"has_new":true,"key":"fahuo","new_v":106,"old_v":106},{"has_new":true,"key":"weibo","new_v":121,"old_v":121},{"has_new":true,"key":"comment_recommend","new_v":118,"old_v":118},{"has_new":true,"key":"dptc","new_v":119,"old_v":119},{"has_new":true,"key":"sizetemplate","new_v":127,"old_v":127},{"has_new":true,"key":"hbtuiguang","new_v":126,"old_v":126},{"has_new":true,"key":"bbbf","new_v":125,"old_v":125},{"has_new":true,"key":"rilimuban","new_v":110,"old_v":110}];
//var rootHrefObj = [{key:"dianputijian",old_v:16,new_v:116,has_new:true},{key:"canmou",old_v:9,new_v:109,has_new:true},{key:"instocklisting",old_v:4,new_v:104,has_new:true},{key:"bbtj",old_v:13,new_v:113,has_new:true},{key:"s_crm",old_v:13,new_v:113,has_new:false},{key:"comment",old_v:13,new_v:113,has_new:true},{key:"discount",old_v:5,new_v:105,has_new:true},];
//var rootHrefObj = [{key:"dianputijian",old_v:16,new_v:116,has_new:true},{key:"canmou",old_v:9,new_v:109,has_new:true},{key:"instocklisting",old_v:4,new_v:104,has_new:true},{key:"bbtj",old_v:13,new_v:113,has_new:true},{key:"s_crm",old_v:13,new_v:113,has_new:false},{key:"comment",old_v:13,new_v:113,has_new:false},{key:"sizetemplate",old_v:27,new_v:127,has_new:true},{key:"discount",old_v:5,new_v:105,has_new:true},{key:"tgtj",old_v:17,new_v:117,has_new:true},{key:"comment_recommend",old_v:18,new_v:118,has_new:true},{key:"weibo",old_v:21,new_v:121,has_new:true},{key:"dptc",old_v:19,new_v:119,has_new:true},{key:"comment",old_v:29,new_v:29,has_new:true},{key:"modify",old_v:30,new_v:30,has_new:true}];

for(var i=0;i < globeMenuObj.length;i++) {
	var secondKey = globeMenuObj[i];
	if(secondKey.id != globeMemuId){
		var selectRootHrefE;
		var findFlag = false;
		for(var j=0;j < rootHrefObj.length;j++) {
			var rootHrefE = rootHrefObj[j];
			if(rootHrefE.key == secondKey.key) {
				selectRootHrefE = rootHrefE;
				findFlag = true;
				
				if(findFlag && selectRootHrefE.has_new) {
					var hrefId = selectRootHrefE.new_v;
					var hrefLocation = "ProductUsingServlet?kind="+hrefId+"&rrid=" + (Math.random()*1000000+1);
					allHTML += '<dl><a href="'+hrefLocation+'">'+secondKey.name+'</a></dl>';
				}
				
				break;
			}
		}
	}
}

$leftMenu.html(allHTML);
if(selectedSecondNode){
	$("#" + selectedSecondNode.key).addClass("current");
	if(selectedSecondNode.type = "1") {
		for(var i=0;i<selectedSecondNode.subMenus.length;i++){
			var tNode = selectedSecondNode.subMenus[i];
			if(globeThridKey && tNode.key == globeThridKey){
				$("#" + globeThridKey).addClass("current");
			}
		}
	}
}

//alert(document.getElementById('main_left'));
//$("#main_left").css("visibility","visible");