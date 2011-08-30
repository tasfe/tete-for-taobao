<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getcode.aspx.cs" Inherits="top_groupbuy_getcode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
<link href="images/tab.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="javascript:;" class="nolink">特特团购</a> 获取团购推广代码 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
          <!--<p class="help"><a href="http://service.taobao.com/support/help.htm" target="_blank">查看帮助</a></p>-->
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content">
    
    <br />
    <img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/68.gif' />
    <span style="font-size:20px; font-weight:bold">恭喜您，您已经成功创建了一个属于您自己的团购！<br />特特推荐您使用以下几种站外推广方式：</span><br />
    

<!--tab ads start-->
<div id="main" style="overflow:hidden; margin-left:6px;">
<div id="tabCot_product" class="tab" style="margin-left:30px;">
<div class="tabContainer">
<ul class="tabHead" id="tabCot_product-li-currentBtn-">
   <li class="currentBtn"><a href="javascript:void(0)" title="淘帮派推广代码" rel="1">淘帮派推广</a></li>
   <li><a href="javascript:void(0)" title="网站HTML代码" rel="2">网站HTML代码</a></li>
   <li><a href="javascript:void(0)" title="图片推广" rel="3">图片推广</a></li>
</ul>
</div>

<div id="tabCot_product_2" class="tabCot" style="display: none;">
<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">2、可以以内嵌HTML形式放在自己论坛或者网站页面里面</span>
        </td>
      </tr>
      <tr>
        <td>
        	<textarea cols="80" rows="2" id="Textarea61"><%=html2%></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea61')" />
        	<br />
    <br />
        </td>
      </tr>
</table>
<div class="clear"></div>
</div>
<div id="tabCot_product_3" class="tabCot" style="display: none;">

<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">3、您可以直接将下面的图片用鼠标括起来右键点复制，然后粘贴到自己的博客日志里面，也可以将此图片右键下载<br />到本地发到您的微博里面</span>  <a href='http://blog.jianghu.taobao.com/u/MjA0MjAwODU2/blog/blog_detail.htm?aid=46415639' target="_blank" style="font-weight:bold; color:Red">客户案例</a>
        </td>
      </tr>
      <tr>
        <td>
        	<img src='http://groupbuy.7fshop.com/top/groupbuy/getImage.aspx?id=<%=id %>' border=0 /><br />
        </td>
      </tr>
</table>

<div class="clear"></div>
</div>
<div id="tabCot_product_1" class="tabCot">

<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">1、这个是HTML形式的推广代码，适于用淘帮派等只能使用HTML发布不能使用脚本的论坛</span>
        </td>
      </tr>
      <tr>
        <td>
        	<textarea cols="80" rows="3" id="Textarea4"><%=html1 %></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea4')" />
        	<br />
    <br />
        </td>
      </tr>
</table>

<div class="clear"></div>
</div>

<div class="modBottom">
    <span class="modABL"></span>
    <span class="modABR"></span>
</div>
</div>
</div>
<div class="noprint"> 
<script type="text/javascript" language="jscript">
    function tab(o, s, cb, ev) {//tab切换类
        var $ = function (o) { return document.getElementById(o) };
        var css = o.split((s || '_'));
        if (css.length != 4) return;
        this.event = ev || 'onclick';
        o = $(o);
        if (o) {
            this.ITEM = [];
            o.id = css[0];
            var item = o.getElementsByTagName(css[1]);
            var j = 1;
            for (var i = 0; i < item.length; i++) {
                if (item[i].className.indexOf(css[2]) >= 0 || item[i].className.indexOf(css[3]) >= 0) {
                    if (item[i].className == css[2]) o['cur'] = item[i];
                    item[i].callBack = cb || function () { };
                    item[i]['css'] = css;
                    item[i]['link'] = o;
                    this.ITEM[j] = item[i];
                    item[i]['Index'] = j++;
                    item[i][this.event] = this.ACTIVE;
                }
            }
            return o;
        }
    }
    tab.prototype = {
        ACTIVE: function () {
            var $ = function (o) { return document.getElementById(o) };
            this['link']['cur'].className = this['css'][3];
            this.className = this['css'][2];
            try {
                $(this['link']['id'] + '_' + this['link']['cur']['Index']).style.display = 'none';
                $(this['link']['id'] + '_' + this['Index']).style.display = 'block';
            } catch (e) { }
            this.callBack.call(this);
            this['link']['cur'] = this;
        }
    }
    /*
    tab 使用方法:
    new tab(标签ID, id分隔符, 单击事触发函数, 什么事件触发TAB切换);
    标签ID:        ID命名格式为: 前缀+分隔符+TAB标签的HTML标签名+激活状态下标签样式+分隔符+非激活状态下标签样式(必须)
    id分隔符:       分隔符(必须)
    TAB切换时触发函数: TAB切换时触发函数(可选)
    什么事件触发TAB切换:可选(默认为onclick)
    注: 标签ID命名时的前缀将做为 该标签的新ID值,所以前缀不要与现在任何元素的ID值相同.
    返回值为: 标签ID所对象的对象.
    切换标签时对应的 项目名称命名规则:
    前缀+_+顺序值
    具体理解,可以看上面的代码,比如
    ID为 "test3_li_now_" 的对象代表的意思是:
    前缀为 test3
    li 为 id为 "test3_li_now_" 标签下面的 li 标签 做为TAB项.
    now 为 标签激活时的样式
    "" 最后的空为 非激活状态下的样式
    每个标签项 激活 状态下对应的元素的ID应该命名为:
    test3_1   第一个标签项对应项目
    test3_2   第二个标签项对应项目
    test3_3   第三个标签项对应项目
    等等
    */
    new tab('tabCot_product-li-currentBtn-', '-');
</script>
</div>


<!--tab ads end-->

  <br />   

 <!--<a href='javascript:GoToHelp();' style="color:Red; font-weight:bold; font-size:16px;">【如果您还是不会操作，请点此查看更加详细的教程】</a> <br />-->
<input type="button" value="返回列表" onclick="window.location.href='grouplist.aspx'" />
  </div>
</div>

<script language="javascript" type="text/javascript">
    function del(id) {
        if (confirm('你确定要删除吗，该操作不可恢复?')) {
            window.location.href = 'idealist.aspx?act=del&id=' + id;
        }
    }

    function copyToClipBoard(id) {
        var clipBoardContent = document.getElementById(id).value;
        if (window.clipboardData) {
            window.clipboardData.setData("Text", clipBoardContent);
            alert("复制成功");
        } else {
            alert("FireFox浏览器不支持此功能");
        }
    }
</script>
</body>
</html>
