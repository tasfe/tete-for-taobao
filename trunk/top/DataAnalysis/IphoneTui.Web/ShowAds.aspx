<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowAds.aspx.cs" Inherits="ShowAds" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特推广</title>
    <link href="css/common.css" rel="stylesheet" />

    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <style>
        td
        {
            font-size: 12px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
        dd, dl, dt
        {
            margin: 0px;
            padding: 0px;
        }
    </style>
</head>
<body style="padding: 0px; margin: 0px;">
    <div class="navigation" style="height: 600px;">
        <form id="form1" runat="server">
        <div class="crumbs">
            <a href="#" class="nolink">特推广</a> 查看您的广告
        </div>
        <div class="absright">
            <ul>
                <li>
                    <div class="msg">
                    </div>
                </li>
            </ul>
        </div>
        <div id="main-content">
            <input type="button" onclick="window.location.href='useradslist.aspx?istou=1'" value="投放中的广告" />
            <input type="button" onclick="window.location.href='useradslist.aspx'" value="等待投放的广告" />
            <input type="button" onclick="window.location.href='UserAddAds.aspx'" value="投放新广告" />
            <hr />
            <div style="font-size:20px">
            1、在苹果应用中找到应用名为<asp:Label ID="Lbl_AppName" ForeColor="Red" runat="server"></asp:Label>安装到手机上
            <br />
            2、安装后打开应用:
            <br />
             <asp:Label ID="Lbl_show" runat="server"></asp:Label> <asp:Label ID="Lbl_AdsPosition" ForeColor="Red" runat="server"></asp:Label>
            <br />
           就能<a href="UserAdsList.aspx?istou=1">查看您的广告投放</a>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
