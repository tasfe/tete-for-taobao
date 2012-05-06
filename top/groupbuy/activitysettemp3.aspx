<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitysettemp3.aspx.cs" Inherits="top_groupbuy_activitysettemp1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改促销活动</title>
    <link href="../css/common.css" rel="stylesheet" />
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 

    <script language="javascript" type="text/javascript" src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/cal.js"></script>
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        th{text-align:left; height:40px;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
<form id="form1" action="activitysettemp3.aspx" runat="server">

    <div class="navigation">
        <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 4.生成广告代码 </div>
    
        <div id="main-content">
            <asp:TextBox ID="TextBox1" runat="server" Height="178px" TextMode="MultiLine" 
                Width="602px"></asp:TextBox>
            <br />
            <input type="button" value="复制代码" />
        </div>
    </div>
    </form>
</body>
</html>
