<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mshop.aspx.cs" Inherits="top_market_mshop" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特特营销推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特营销推广</a> 店铺APP </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    <img alt="" src="http://a.tbcdn.cn/sys/wangwang/smiley/48x48/17.gif" />
    <b style="font-size:24px; color:Red;">他们都有自己的APP了，你再不拥有就OUT了...</b><br />
    <a href='http://iphone.tetesoft.com/down/sfznhbx/mshop.apk' target="_blank"><img src='images/mshop/mshop1.jpg' style='border:solid 1px black;' /></a>
    <a href='http://haoping.7fshop.com/top/test/Tacera.apk' target="_blank"><img src='images/mshop/mshop2.jpg' style='border:solid 1px black;' /></a>
    <a href='http://iphone.tetesoft.com/down/xnttd/mshop.apk' target="_blank"><img src='images/mshop/mshop3.jpg' style='border:solid 1px black;' /></a>
    <br />
        <div id="ggArea" style="  z-index: 10003; background-color:white; padding:5px">
<span style="font-size:18px; font-weight:bold; color:red;">6月特价，店铺APP最低只要9元，让您的网店直接入驻买家手机</span>
<img alt="" src="http://a.tbcdn.cn/sys/wangwang/smiley/48x48/12.gif" /><br>

选择加载图：<input type=file onchange="InitImg(this,'bg1')" size=12 />
选择头部广告：<input type=file onchange="InitImg(this,'bg2')" size=10 />
选择LOGO：<input type=file onchange="InitImg(this,'bg3')" size=10 />

<script>
    function InitImg(obj, id) {
        document.getElementById(id).style.display = '';
        document.getElementById(id).src = obj.value;
    }
</script>

<table><tr>
<td style="background:url('图片2.png'); width:240px; height:400px;" valign=top>
<img id="bg1" src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/12.gif' width=223 height=330 style="margin:28px 0 0 18px; display:none" />
</td>
<td style="background:url('图片3.png'); width:240px; height:400px;" valign=top>
<img id="bg2" src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/12.gif' width=228 height=150 style="margin:28px 0 0 10px; display:none" />
</td>
<td style="background:url('tupian5.jpg') repeat-x; width:220px;" valign=top>
<img id="bg3" src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/12.gif' width=34 height=34 style="margin:21px 0 0 7px; display:none" />
</td>
</tr></table>
<br />

<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-6:1' target='_blank'>
<img src='buy.jpg' width=180 border=0>
</a>


</div>
    </div>
</div>
</form>
<script src="http://s84.cnzz.com/stat.php?id=4211299&web_id=4211299&show=pic" language="JavaScript"></script>

</body>
</html>
