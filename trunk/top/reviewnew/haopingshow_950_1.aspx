<%@ Page Language="C#" AutoEventWireup="true" CodeFile="haopingshow_950_1.aspx.cs" Inherits="top_reviewnew_haopingshow_190_1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<style>
.ptitle {
	height:16px;
	overflow:hidden;
}
.ptitle a:link, .ptitle a:visited {
	font-size:12px;
	color: #09C;
	text-decoration: none;
}
.ptitle a:hover {
	color: #09C;
	text-decoration: underline;
}
.ptitle a:active {
	color: #09C;
	text-decoration: none;
}
a:link, a:visited {
	font-size:12px;
	color:#666;
	text-decoration: none;
}
a:hover {
	text-decoration: underline;
	color: #666666;
}
a:active {
	text-decoration: none;
	color: #666666;
}
.hp_list {
	padding:10px 0px 10px 0px;
	font-size:12px;
	color:#666;
	line-height:20px;
	border-bottom: dotted 1px #E1E1E1;
}
.vip_text {
	color:#F00;
	margin:0px 1px 0px 1px;
	font-weight:bold
}
.text2 {
	margin:0 2 0 2px;
	color:#C90;
	font-size:12px;
	font-weight:bold;
}
.text3 {
	margin:0 2 0 2px;
	color:#F60;
	font-size:12px;
	font-weight:bold;
}
.rate_ico {
	float:left;
	background:url(http://img04.taobaocdn.com/tps/i4/T1jQaaXdhgXXXXXXXX-676-800.png);
	background-repeat:no-repeat;
	background-position:-294px -178px;
	height:18px;
	width:18px;
	margin:2px 0px 0px 1px;
}
.bottomcover {
	width:950px;
	height:5px;
	position:absolute;
	bottom:0;
	background-color:#FFF;
	z-index:5;
	overflow:hidden;
}
.buyer_box {
	float:right;
	margin:5px 10px 0px 0px;
	color:#999;
}
.rate_box {
	word-break:break-all;
	width:500px;
	overflow:hidden;
}
.share_btn_box {
	float:left;
	margin:0px 0px 0px 0px;
	padding:0px;
}
.share_btn {
	float:left;
	background:url(http://img04.taobaocdn.com/imgextra/i4/26024301/T2Ej4QXkRaXXXXXXXX_!!26024301.gif);
	background-repeat:no-repeat;
	height:16px;
	cursor:pointer;
	margin:0px 2px 0px 0px;
}
.s_title {
	cursor:auto;
	width:36px;
	background-position:0px 0;
}
.s_sina {
	width:16px;
	background-position:-37px 0;
}
.s_qqweibo {
	width:16px;
	background-position:-88px 0;
}
.s_qqspace {
	width:16px;
	background-position:-71px 0;
}
.s_jianghu {
	width:16px;
	background-position:-54px 0;
}
</style>
<body style="background-color:#FFF;">
<div style="position:absolute; width:100%; z-index:10;background-image: url(images/title_bar.png); margin:0px; line-height:30px; height:30px; font-size:12px; padding:0px 0px 0px 10px; font-weight:bold;"><a href="http://fuwu.taobao.com/serv/detail.htm?service_id=4545&from=shopindex" target="_blank"><%=title %></a></div>
<div style="overflow:hidden;position:relative;border:solid 1px #E6E6E6;height:498px;">
  <div id="hp_box" style="position:absolute; z-index:2; top:20px; padding:0px 8px 0px 8px;">
    
<asp:Repeater ID="rptTradeRate" runat="server">
    <ItemTemplate>
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="hp_list" id="rate<%#Eval("itemid") %>">
  <tr>
    <td valign="top"><div class="ptitle"><a class="rate_url" href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" target="_blank">
        <div class="rate_title"><%#Eval("itemname") %></div>
        </a></div>
      <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
          <td width="1" valign="top"><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" target="_blank">
            <div style="margin:5px 2px 5px 0px;"><img src="<%#Eval("itemsrc") %>_100x100.jpg" border="0" /></div>
            </a></td>
          <td align="left" valign="top"><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" target="_blank">
            <div style="font-size:12px; color:#666; margin:6px 0px 0px 0px; line-height:20px;">已售<span class="text2"><%#Eval("sale") %></span>件<br />
              <span class="text3">￥<%#Eval("price") %></span></div>
            </a>
            <div class="share_btn_box" rid="<%#Eval("itemid") %>"></div></td>
          <td>
            <div class="rate_box"><span class="rate_ico"></span><span class="vip_text">[好评]</span><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" target="_blank"><span class="rate_content"><%#Eval("content") %></span><span class="params" num_iid="<%#Eval("itemid") %>" pic_url="<%#Eval("itemsrc") %>"></span></a></div>
            <div style="color:Red; font-weight:bold; font-size:12px;"><%#Eval("showcontent")%></div>
          </td>
        </tr>
      </table>
      
      </td>
  </tr>
  <tr>
    <td valign="bottom"><div class="buyer_box"><%# hidden(Eval("buynick").ToString()) %> <img src='http://haoping.7fshop.com/top/crm/level/<%#Eval("userlevel") %>.gif' /><br />
      </div></td>
  </tr>
</table>
    </ItemTemplate>
</asp:Repeater>

  </div>
  <div class="bottomcover"></div>
</div>

<script type="text/javascript" src="js/jquery-1.5.2.min.js"></script>
<script>

    /* $.ajax({ url: '/front/rates/' + width + '/' + seller_name + '/' + page_no + '/',
    type: 'GET',
    dataType: 'html',
    timeout: 200000,
    success: function (datas) {
    if (is_first) {
    $('#hp_box').html(datas); is_first = false;
    } else {
    $('#hp_box').append(datas);
    } loop_index += 1;
    }
    }); */

    var pages = parseInt('3');
    var total_num = parseInt('26');
    var seller_name = encodeURI('etanliuyang');
    var page_no = Math.floor(Math.random() * pages) + 1;
    var width = '950';
    var c_record = 0;
    var loop_index = 0;
    var up_switch;
    var is_first = true;
    if (pages > 1 && page_no == pages) {
        page_no = 1 
    }; 

    function get_rates() {
        if (page_no > pages) 
            page_no = 1;
            page_no += 1;
    }
    get_rates();
    var i = 0;
    function go_up() {
        i += 1;
        if (i == (2+<%=time %>)) {
            c_record += 1;
            if (c_record == 5) {
                c_record = 0
            }; 
            if ((loop_index < pages) && c_record == 0) {
                get_rates()
            };
            var init_top = ($('#hp_box').offset().top);
            var temp_row = $('.hp_list').first();
            $("#hp_box").animate(
            { "top": -temp_row.height() - 2 },
            2000, 
            function () {
                $('#hp_box').append(temp_row.detach()).offset({ top: init_top })
            });
            i = 0;
        }
    }

    function timer() {
        up_switch = setInterval('go_up()', 1000);
        if (total_num < 3 && $("#hp_box").height() < 480)
            clearInterval(up_switch);
    }

    function mouse_switch() {
        $('#hp_box').mouseover(function () {
            clearInterval(up_switch);
        }).mouseout(function () {
            timer() 
        });
    }
    timer(); 
    mouse_switch();
    </script>
<script>    function make_btns(rid) { var html = '<span class="share_btn s_title"></span><span class="share_btn s_sina" onclick="share_sina(' + rid + ')"></span><span class="share_btn s_qqweibo" onclick="share_qqweibo(' + rid + ')"></span><span class="share_btn s_qqspace" onclick="share_qqspace(' + rid + ')"></span><span class="share_btn s_jianghu" onclick="share_jianghu(' + rid + ')"></span>'; return html; } $('.share_btn_box').each(function (e) { $(this).html(make_btns($(this).attr('rid'))); });</script>
<script>    var win_params = 'resizable=no,location=no,status=no'; var rate_content; var rate_title; var rate_img; var rate_url; var num_iid; function make_btns(rid) { var html = '<span class="share_btn s_title"></span><span class="share_btn s_sina" onclick="share_sina(' + rid + ')"></span><span class="share_btn s_qqweibo" onclick="share_qqweibo(' + rid + ')"></span><span class="share_btn s_qqspace" onclick="share_qqspace(' + rid + ')"></span><span class="share_btn s_jianghu" onclick="share_jianghu(' + rid + ')"></span>'; return html; } function get_params(rid) { rate_content = encodeURI('大家都觉得不错，而且还说：' + $('#rate' + rid).find('.rate_content').text()); rate_title = encodeURI($('#rate' + rid + ' .rate_title').text()); rate_url = encodeURIComponent($('#rate' + rid + ' .rate_url').attr('href')); rate_img = encodeURIComponent($('#rate' + rid + ' .params').attr('pic_url')); num_iid = encodeURIComponent($('#rate' + rid + ' .params').attr('num_iid')); } function share_sina(rid) { get_params(rid); var params = 'url=' + rate_url + '&title=' + rate_content + '&pic=' + rate_img; window.open('http://service.weibo.com/share/share.php?' + params, 'share_sina', win_params); } function share_jianghu(rid) { get_params(rid); var params = 'url=' + rate_url + '&type=9&id=' + num_iid; window.open('http://share.jianghu.taobao.com/share/addShare.htm?' + params, 'share_jianghu', win_params); } function share_qqspace(rid) { get_params(rid); var params = 'url=' + rate_url + '&title=' + rate_title + '&summary=' + rate_content + '&pics=' + rate_img; window.open('http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?' + params, 'share_qqspace', win_params); } function share_qqweibo(rid) { get_params(rid); var params = 'url=' + rate_url + '&title=' + rate_content + '&summary=' + rate_content + '&pic=' + rate_img; window.open('http://v.t.qq.com/share/share.php?' + params, 'share_qqweibo', win_params); }</script>

</body>
</html>

