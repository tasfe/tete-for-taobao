<%@ Page Language="C#" AutoEventWireup="true" CodeFile="callback.aspx.cs" Inherits="testflash_callback" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" /> 
</head>

<body>
<object id="FlashID" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="750" height="450">
  <param name="movie" value="Main.swf?id=<%= url %>" />
  <param name="FlashID" value="id=<%= url %> " />
  <param name="flashvars" value="name=true&amp;gacode=UA-7483693-4&amp;config=config/loader.xml">
  <!-- Next object tag is for non-IE browsers. So hide it from IE using IECC. -->
  <!--[if !IE]>-->
  <object type="application/x-shockwave-flash" data="Main.swf?id=<%= url %>" width="750" height="450">
    <!--<![endif]-->
    <param name="quality" value="high" />
    <param name="wmode" value="opaque" />
    <param name="swfversion" value="6.0.65.0" />
    <!-- This param tag prompts users with Flash Player 6.0 r65 and higher to download the latest version of Flash Player. Delete it if you don’t want users to see the prompt. -->
    <param name="expressinstall" value="Scripts/expressInstall.swf" />
       <param name="flashvars" value="name=true&amp;gacode=UA-7483693-4&amp;config=config/loader.xml">
    <!-- The browser displays the following alternative content for users with Flash Player 6.0 and older. -->
    <div>
      <h4>Content on this page requires a newer version of Adobe Flash Player.</h4>
      <p><a href="http://www.adobe.com/go/getflashplayer"><img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" /></a></p>
    </div>
    <!--[if !IE]>-->
  </object>
  <!--<![endif]-->
</object>
<script type="text/javascript">
<!--
    //SWFFix.registerObject("FlashID");
//-->
</script>

<%--
<object id="Object1" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="750" height="450">
  <param name="movie" value="http://groupbuy.7fshop.com/testflash/Main.swf?id=<%= url %>" />
  <param name="quality" value="high" />
  <param name="wmode" value="opaque" />
  <param name="swfversion" value="6.0.65.0" />
  <param name="flashvars" value="name=true&amp;gacode=UA-7483693-4&amp;config=config/loader.xml">
  <!-- 此 param 标签提示使用 Flash Player 6.0 r65 和更高版本的用户下载最新版本的 Flash Player。如果您不想让用户看到该提示，请将其删除。 -->
  <param name="expressinstall" value="Scripts/expressInstall.swf" />
  <!-- 下一个对象标签用于非 IE 浏览器。所以使用 IECC 将其从 IE 隐藏。 -->
  <!--[if !IE]>-->
  <object type="application/x-shockwave-flash" data="http://groupbuy.7fshop.com/testflash/Main.swf?id=<%= url %>" width="750" height="450">
    <!--<![endif]-->
    <param name="quality" value="high" />
    <param name="wmode" value="opaque" />
    <param name="swfversion" value="6.0.65.0" />
    <param name="expressinstall" value="Scripts/expressInstall.swf" />
	<param name="flashvars" value="name=true&amp;gacode=UA-7483693-4&amp;config=config/loader.xml">
    <!-- 浏览器将以下替代内容显示给使用 Flash Player 6.0 和更低版本的用户。 -->
    <div>
      <h4>此页面上的内容需要较新版本的 Adobe Flash Player。</h4>
      <p><a href="http://www.adobe.com/go/getflashplayer"><img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="获取 Adobe Flash Player" width="750" height="450" /></a></p>
    </div>
    <!--[if !IE]>-->
  </object>
  <!--<![endif]-->
</object>
<script type="text/javascript">
    swfobject.registerObject("FlashID");
</script>--%>
</body>
</html>
