﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mshop.aspx.cs" Inherits="top_market_mshop" %>

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
    <b style="font-size:24px; color:Red;">他们都有自己的APP了，你还在犹豫什么...</b><br />
    <a href='http://iphone.tetesoft.com/down/sfznhbx/mshop.apk' target="_blank"><img src='images/mshop/mshop1.jpg' style='border:solid 1px black;' /></a>
    <a href='http://haoping.7fshop.com/top/test/Tacera.apk' target="_blank"><img src='images/mshop/mshop2.jpg' style='border:solid 1px black;' /></a>
    <a href='http://iphone.tetesoft.com/down/xnttd/mshop.apk' target="_blank"><img src='images/mshop/mshop3.jpg' style='border:solid 1px black;' /></a>
    <br />
    <b style="font-size:24px; color:Red;">点击以上图标下载安装，查看他们的应用效果</b>
    <p />
     <b style="font-size:24px; color:blue;">选择自己的图片，立即生成属于自己店铺的手机安装文件</b>
        <div id="ggArea" style="  z-index: 10003; background-color:white; padding:5px">
<%--<span style="font-size:18px; font-weight:bold; color:red;">6月特价，店铺APP最低只要9元，让您的网店直接入驻买家手机</span>
<img alt="" src="http://a.tbcdn.cn/sys/wangwang/smiley/48x48/12.gif" />--%>
<a name="chooseimg"></a>
<br />

<font color="blue"> 选择加载图：</font><input id="user_load" type=file onchange="InitImg(this,'bg1')" size=12 />
<font color="blue"> 选择头部广告：</font><input id="user_head" type=file onchange="InitImg(this,'bg2')" size=10 />
<font color="blue"> 选择LOGO：</font><input id="user_logo" type=file onchange="InitImg(this,'bg3')" size=10 />
<style type="text/css">    
#bg1    
{    
filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);    
}   
#bg2   
{    
filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);    
}   
#bg3    
{    
filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);    
}    
</style> 
<script>
    function InitImg(obj, id) {
        document.getElementById(id).filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = getPath(obj);
        document.getElementById(id).style.display = '';  
    }


    function getPath(obj) {
        if (obj) {
            if (window.navigator.userAgent.indexOf("MSIE") >= 1) {
                obj.select();
                return document.selection.createRange().text;
            }
            else if (window.navigator.userAgent.indexOf("Firefox") >= 1) {
                if (obj.files) {
                    return obj.files.item(0).getAsDataURL();
                }
                return obj.value;
            }
            return obj.value;
        }
    }

    function CheckNull() {
        if (document.getElementById("user_load").value == "" || document.getElementById("user_head").value == "" || document.getElementById("").value == "user_logo") {
            alert("请选择属于您自己的店铺图片");
            location.hash = "chooseimg";
            return false;
        }
        return true;
    }
</script>

<table><tr>
<td id="testbg" style="background:url('图片2.png'); width:240px; height:400px;" valign=top>
<div id="bg1" style="margin:28px 0 0 18px; width:223px; height:330px; display:none" ></div>
</td>
<td style="background:url('图片3.png'); width:240px; height:400px;" valign=top>
<div id="bg2" style="margin:28px 0 0 10px; width:228px; height:150px; display:none" ></div>
</td>
<td style="background:url('tupian5.jpg') repeat-x; width:220px;" valign=top>
<div id="bg3" style="margin:21px 0 0 7px; width:34px; height:34px; display:none" ></div>
</td>
</tr></table>
<br />



<asp:ImageButton ImageUrl="buy.jpg" width="180" BorderWidth="0" runat="server" ID="Img_Buy" OnClientClick="return CheckNull()" OnClick="CheckNull" />


</div>
    </div>
</div>
</form>
<script src="http://s84.cnzz.com/stat.php?id=4211299&web_id=4211299&show=pic" language="JavaScript"></script>

</body>
</html>
