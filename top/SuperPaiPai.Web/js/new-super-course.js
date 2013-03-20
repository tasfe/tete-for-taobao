var _window_url = window.location.href;
var _app_url_key = {
	/*ntzh:{keys:["ntzh1"],kind:"32"}  //自动上下架
	*/
	nck:{keys:["nck1"],kind:"104"}  //仓库上架
	,npj:{keys:["npj1"],kind:"29"}  //自动评价
	,nxg:{keys:["nxg1"],kind:"30"}  //批量修改
	,nchch:{keys:["nchch1"],kind:"31"} //橱窗橱窗
	,crm:{keys:["crm1"],kind:"108"}  //会员管理
	,nac:{keys:["nac1"],kind:"105"}  //活动工具
	,nm:{keys:["nm1"],kind:"119"}  //套餐搭配
	,nt:{keys:["nt1"],kind:"113"}  //宝贝推荐
	,nk:{keys:["nk1"],kind:"118"}  //评价推荐
	,w:{keys:["w2"],kind:"121"}   //站外营销
	,ntg:{keys:["ntg1"],kind:"117"}  //团购推荐
	,nhb:{keys:["nhb1"],kind:"126"}  //海报推广
	/*,ncm:{keys:["ncm1"],kind:"127"}  //尺寸模板
	*/
	,nr:{keys:["nr1"],kind:"110"}  //日历模板
	,ncanmou:{keys:["ncanmou1"],kind:"109"} //行情分析
	,ntj:{keys:["ntj1"],kind:"116"}  //店铺体检
	,nbf:{keys:["nbf1"],kind:"125"} //宝贝备份
	};
var _super_is_show_course = true;
if ( $.browser.msie ){
	if( Number($.browser.version) < 7){
		_super_is_show_course = false;
	}
}
if(_super_is_show_course)
		for(var key in _app_url_key){
			var _url = null;
			var _keys = _app_url_key[key].keys;
			for(var i=0;i<_keys.length;i++){
				_url = _keys[i]+".superboss.cc";
				if(_window_url.indexOf(_url)!=-1){
					if($.cookie("_final_course_"+key)==null && $.cookie("_course_"+key)==null){
						if(key=="nck"&&_window_url.indexOf("index.do")!=-1){
							if($(".main_right .kuang:eq(1) table tr").length > 1){
								$.cookie("_final_course_"+key,1,365*40,"superboss.cc","/");
							}
						}else if(key=="npj"&&_window_url.indexOf("index.do")!=-1){
							if($("#commentSwitch").hasClass("middle-blue")){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="nxg"&&_window_url.indexOf("index.do")!=-1){
							if($(".main_right .kuang:eq(1) table tr").length > 1){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}	
						}else if(key=="ntzh"&&_window_url.indexOf("index.do")!=-1){
							if($(".main_right .kuang:eq(1) table tr").length > 1){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="nac"&&_window_url.indexOf("item_queryIndexItem.do")!=-1){
							if($("#create_active_discount").length < 1){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="nm"&&_window_url.indexOf("index.do")!=-1){
							if(Number ($(".temp_basic_info ul li:eq(1) span").text())>0){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="nt"&&_window_url.indexOf("index.do")!=-1){
							if(Number ($(".temp_basic_info ul li:eq(1) span").text())>0){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="nk"&&_window_url.indexOf("index.do")!=-1){
							if(Number ($(".temp_basic_info ul li:eq(1) span").text())>0){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="ntg"&&_window_url.indexOf("index.do")!=-1){
							if(Number ($(".temp_basic_info ul li:eq(1) span").text())>0){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="nhb"&&_window_url.indexOf("index.do")!=-1){
							if(Number ($(".temp_basic_info ul li:eq(1) span").text())>0){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="ntj"&&_window_url.indexOf("index.do")!=-1){
							if($(".main_right .kuang:eq(1) table tr").length > 1){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="nbf"&&_window_url.indexOf("bakItem_getUserBakVersion.action")!=-1){
							if($("#bakList-tb").length > 0 && $("#bakList-tb tr").length > 1){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}else if(key=="w"&&_window_url.indexOf("forward_toIndex.do")!=-1){
							if($("#noBindChannelsDiv").length>0){
								$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
							}
						}
						if(($.cookie("_final_course_"+key)==null && $.cookie("_course_"+key)==null)||getCookie("nick")=="zqshanxi"){
							if(getCookie("nick")!="zqshanxi"){
								$.ajax({
									dataType:'jsonp',
									url:"http://www.superboss.cc/send20121122.php", 
									data:{"nick":$.cookie("nick"),"count":0,"kind":_app_url_key[key].kind},
									jsonp:"callback",
									success:function (data){
										if($.isPlainObject(data)){
											if(data[_app_url_key[key].kind]>10){
												$.cookie("_final_course_"+key,1,1,365*40,"superboss.cc","/");
												return;
											}
										}
										$("body").append($("<link>", {"type": "text/css","rel":"stylesheet","href": "http://static.superboss.cc/kissy/course.css"}));
										$.getScript("http://a.tbcdn.cn/s/kissy/1.2.0/??kissy-min.js?",function(){
											$.getScript("http://static.superboss.cc/kissy/course.js",function(){
												$.getScript("http://static.superboss.cc/kissy/course/"+key+"-course.js");
											});
										});
									}
								});
							}else{
								$.cookie("_final_course_"+key,null);
								$("body").append($("<link>", {"type": "text/css","rel":"stylesheet","href": "http://static.superboss.cc/kissy/course.css"}));
								$.getScript("http://a.tbcdn.cn/s/kissy/1.2.0/??kissy-min.js?",function(){
									$.getScript("http://static.superboss.cc/kissy/course.js",function(){
										$.getScript("http://static.superboss.cc/kissy/course/"+key+"-course.js");
									});
								});
							}
						}
					}
					break;
				}else{
					_url = null;
				}
			}
			if(_url!=null)break;
		}


