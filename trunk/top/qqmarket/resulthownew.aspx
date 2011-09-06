<%@ Page Language="C#" AutoEventWireup="true" CodeFile="resulthownew.aspx.cs" Inherits="top_market_resulthownew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
        <script type="text/javascript" src="js/swfobject.js"></script>

    <script type="text/javascript">
        swfobject.embedSWF("open-flash-chart.swf", "my_chart", "800", "600",
  "9.0.0", "expressInstall.swf",
  { "data-file": "datafile/Pie.aspx?id=<%=id %>" }
  );
    </script>
</head>
<body>
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 广告浏览次数查看 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

    <form id="form1" runat="server">
        <div id="my_chart">
        </div>
    </form>

    </div>
</div>
</body>
</html>
