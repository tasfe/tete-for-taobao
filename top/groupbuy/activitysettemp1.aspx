<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitysettemp1.aspx.cs" Inherits="top_groupbuy_activitysettemp1" %>

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
<form id="form1"  runat="server">

    <div class="navigation">
        <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 2.选择展示宝贝 </div>
    
        <div id="main-content">
            <table width="700">
            <tr>
                <td align="left" height="30">选择展示宝贝：</td>
                <td>
                    <select runat="server" id="select1">
                        <option  value="0">请选择</option>
        	                    <asp:Repeater ID="Repeater1" runat="server">
        	                        <ItemTemplate>
        	                            <option value='<%#Eval("ID") %>'><%#Eval("name") %></option>
        	                        </ItemTemplate>
        	                    </asp:Repeater>
     
                        <option>手动选择</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="left" height="30"></td>
                <td>
                    <input type="button" value="上一步" onclick="history.go(-1)" />
 
                    <asp:Button ID="Button1" runat="server" Text="下一步：设置宝贝顺序和价格" 
                        onclick="Button1_Click" />
                </td>
            </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
