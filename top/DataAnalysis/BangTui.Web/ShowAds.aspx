<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowAds.aspx.cs" Inherits="ShowAds" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>帮推广</title>
    <link href="css/common.css" rel="stylesheet" />
      <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        dd,dl,dt{margin:0px;padding:0px;}
    </style>
</head>
<body style="padding:0px; margin:0px;">

<div class="navigation" style="height:600px;">

    <form id="form1" runat="server">



  <div class="crumbs"><a href="#" class="nolink">帮推广</a> 推广展示的位置 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">
    
    <input type=button onclick="window.location.href='useradslist.aspx?istou=1'" value="投放中的广告" />
    <input type=button onclick="window.location.href='useradslist.aspx'" value="等待投放的广告" />
    <input type=button onclick="window.location.href='UserAddAds.aspx'" value="投放新广告" />

    <hr />
   <b style="font-size:14px;"> 您的广告投在“<asp:Label runat="server" ID="LB_SiteName" />”(<asp:Label runat="server" ID="LB_SiteUrl" />)上，位置在“<asp:Label runat="server" ID="LB_AdsTitleSize" />”,请按如下操作查看您的广告投放情况：</b>
   <br />
第一步 打开“<asp:Label runat="server" ID="LB_SiteName1" />”网站，网址：<a href='<%=SiteUrl %>' target="_blank"><asp:Label runat="server" ID="LB_SiteUrl1" /></a>
<br />
第二步 您的广告目前投放的位置在“<asp:Label runat="server" ID="LB_AdsTitleSize1" />”，见下图所示：
<br />
<img src='<%=ImgUrl %>'>
<br />
如果您看到不广告牌，很可能是您的浏览器或安全软件(如360安全卫士)设置了广告过滤规则，点击查看如何解决 
<br />
第三步 点击广告牌，在弹出的页面，即可<a href="UserAdsList.aspx?istou=1">查看您的广告投放</a> 
<br />
（友情提示：我们还提供VIP推广服务，即点击广告牌后，能直接进入您的店铺或宝贝页面，不经过中间的展示页面，详请请查看这里:<a href="">VIP推广介绍及案例</a>） 
    
    
    </div>

    </div>

    </form>
</body>
</html>
