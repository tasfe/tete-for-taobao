<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tmall.aspx.cs" Inherits="taobaoke_tmall" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>粉色生活-女性时尚资讯、潮流、搭配</title>
<meta name="description" content="粉色生活是一个服饰搭配 鞋包 美容护肤 减肥瘦身 魅力彩妆 发型潮流，及时尚资讯、潮流、搭配的资讯站点。" />
<meta name="keywords" content="服饰搭配 鞋包 美容护肤 减肥瘦身 化妆 香水 魅力彩妆 发型潮流" />
<link href="/templets/taobao1/images/css.css" rel="stylesheet" type="text/css" />
<link href="/templets/taobao1/images/dedecms.css" rel="stylesheet" type="text/css" />
<SCRIPT language=javascript src="/templets/taobao1/js/changimages.js"></SCRIPT>
</head>
<body>
﻿<div id="top">
  <div id="logo"><img src="/templets/taobao1/images/logo.jpg" /></div>
  <div id="toplink">
<a href="http://www.chihuotianxia.com" target="_blank">吃货天下</a>|
<a href="http://www.mengmeng8.com" target="_blank">萌萌吧</a></div>
 <ul class="topmenu">
  <li class="topmenu1"><a href="/index.html">首页</a></li>
  <li class="topmenu2"><a href="/tmall.aspx" target="_blank">天猫精选</a></li>
  <li class="topmenu2"><a href="http://www.go12go.com" target="_blank">导购网</a></li>
  <li class="topmenu2"><a href="http://www.niubibang.com" target="_blank">男人邦</a></li>
  <li class="topmenu2"><a href="http://www.hi859.com" target="_blank">房产</a></li>
  <li class="topmenu2"><a href="http://www.5izhuangxiu.com" target="_blank">家居</a></li>
  <li class="topmenu2"><a href="http://www.ianglebaby.com" target="_blank">亲子</a></li>
  </ul>
  <div id="topnotice"><marquee direction="left" scrollamount="2"></marquee></div>
</div>

<div id="topsearch">
<div id="topsearch1">新闻搜索：</div>
<form action="/plus/search.php" method="get">
<input type="hidden" id="searchtype" value="titlekeyword" />
<div class="topsearch2"><input name="keyword" type="text" style="width:300px; border:0px; height:22px" /></div>
<div class="topsearch2"><input type="submit" id="btnsearch" value="搜索" /></div>
</form>
<div id="topsearch3">热搜：<a href="/plus/search.php?keyword=%E5%A4%8F%E5%AD%A3%E6%90%AD%E9%85%8D&searchtype=titlekeyword" target="_blank">搭配</a> 
<a href="/plus/search.php?keyword=%E7%89%9B%E4%BB%94%E8%A3%A4&searchtype=titlekeyword" target="_blank">牛仔裤</a> 
<a href="/plus/search.php?keyword=%E7%9C%BC%E5%A6%86&searchtype=titlekeyword" target="_blank">画眼妆</a> 
<a href="/plus/search.php?keyword=%E6%98%8E%E6%98%9F%E6%90%AD%E9%85%8D&searchtype=titlekeyword" target="_blank">明星搭配</a>
<a href="/plus/search.php?keyword=%E5%87%89%E9%9E%8B&searchtype=titlekeyword" target="_blank">凉鞋</a>
<a href="/plus/search.php?keyword=%E6%B3%B3%E8%A3%85&searchtype=titlekeyword" target="_blank">泳装</a></div>
</div>

<div id="topclass">

    <div>
        <h4><a href='/fushipindao/'><span>服饰频道</span></a></h4>
        <ul>
            
            <li><a href='/shishangpindao/fushidapei/'>服饰搭配</a></li>
            
            <li><a href='/shishangpindao/mingxingdapei/'>明星搭配</a></li>
            
            <li><a href='/shishangpindao/hanrifushi/'>韩日服饰</a></li>
            
            <li><a href='/shishangpindao/oumeifushi/'>欧美服饰</a></li>
            
            <li><a href='/shishangpindao/jietousuipai/'>街头随拍</a></li>
            
            <li><a href='/shishangpindao/nayidaren/'>内衣达人</a></li>
            
            <li><a href='/shishangpindao/chaonanfushi/'>潮男服饰</a></li>
            
            <li><a href='/shishangpindao/xiebaoliuxing/'>鞋包流行</a></li>
            
        </ul>
    </div>
    
    
    <div>
        <h4><a href='/meirongmeifa/'><span>美容美发</span></a></h4>
        <ul>
            
            <li><a href='/meitisushen/jifubaoyang/'>肌肤保养</a></li>
            
            <li><a href='/meitisushen/qianyanmeizhuang/'>前沿美妆</a></li>
            
            <li><a href='/meitisushen/huazhuangjiqiao/'>化妆技巧</a></li>
            
            <li><a href='/meitisushen/faxingchaoliu/'>发型潮流</a></li>
            
            <li><a href='/meitisushen/meifabaoyang/'>美发保养</a></li>
            
            <li><a href='/meitisushen/xiangshuiwuyu/'>香水物语</a></li>
            
        </ul>
    </div>
    
    
    <div>
        <h4><a href='/meitisushen/'><span>美体塑身</span></a></h4>
        <ul>
            
            <li><a href='/meitisushen/jianfeishoushen/'>减肥瘦身</a></li>
            
            <li><a href='/meitisushen/xiangshoumeishi/'>享瘦美食</a></li>
            
            <li><a href='/meitisushen/sushenyoudao/'>塑身有道</a></li>
            
            <li><a href='/meitisushen/mingxingmeiti/'>明星美体</a></li>
            
        </ul>
    </div>
    
    
    <div>
        <h4><a href='/shishangpindao/'><span>时尚频道</span></a></h4>
        <ul>
            
            <li><a href='/fushipindao/yulezixun/'>娱乐资讯</a></li>
            
            <li><a href='/fushipindao/jiaodianxinwen/'>焦点新闻</a></li>
            
            <li><a href='/fushipindao/qingganyinsi/'>情感隐私</a></li>
            
            <li><a href='/fushipindao/feizhuliu/'>非主流</a></li>
            
        </ul>
    </div>

</div>

<script language="javascript">
    objList = document.getElementById("topclass").childNodes;
    for (i = 0; i < objList.length; i++) {
        objList[i].id = "topclass" + (i + 1);
        objListChild = objList[i].childNodes[1].childNodes;
        for (j = 0; j < objListChild.length; j++) {
            if (j % 2 == 0) {
                objListChild[j].childNodes[0].className = 'color';
            }
        }
    }
</script>



<div id="main">
    
    <%=html %>

</div>

﻿<div id="link">
<div id="linktitle">友情链接</div>
<div id="linkbody">
<ul>

<li><a href='http://www.mengmeng8.com' target='_blank'>萌萌吧</a> </li><li><a href='http://www.go12go.com/' target='_blank'>导购网</a> </li><li><a href='http://www.chihuotianxia.com' target='_blank'>吃货天下</a> </li>

</ul>
</div>
</div>
<div id="foot">
Copyright &copy; 2010-2012 www.fenseshenghuo.com 版权所有 ICP证：<A href="http://www.miibeian.gov.cn/" target="_blank">闽ICP备09004062号-1</A>
</div>

<script language="javascript" type="text/javascript" src="http://js.users.51.la/14663083.js"></script>

</body>
</html>
<script type="text/javascript">
    function showtime() {
        var time = new Date();
        var hour = time.getHours();
        var minute = time.getMinutes();
        if (minute < 10) minute = "0" + minute;
        var second = time.getSeconds();
        if (second < 10) second = "0" + second;
        document.getElementById("time").innerHTML = hour + ":" + minute + ":" + second;
        setTimeout("showtime()", "1000");
    }
    showtime(); 
</script>


