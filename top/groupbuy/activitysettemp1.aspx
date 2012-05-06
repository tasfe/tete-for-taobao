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
<form id="form1" action="activitysettemp2.aspx"  method="post"  >

    <div class="navigation">
        <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 2.选择展示宝贝 </div>
    
        <div id="main-content">
            <table width="700">
            <tr>
                <td align="left" height="30">选择展示宝贝：</td>
                <td>
                           <select id="Select1" name=selstr>
        	                    <option value="0">请选择</option>
                    	        
        	                    <%=  strhtml %>
                                <option value="">手动选择</option>
        	                </select>
                   
                </td>
            </tr>
            <tr>
                <td align="left" height="30"></td>
                <td>


                    <input type="button" value="上一步" onclick="history.go(-1)" />
 
                    <input type=submit value="下一步：设置宝贝顺序和价格" onclick="return isshow()" />
 
                </td>
            </tr>
            </table>
            <input type=hidden name="name"  value="<%= name %>" />
            <input type=hidden name="templetid"  value="<%= templetid %>" />
            <input type=hidden name="bt"  value="<%= bt %>" />
            <input type=hidden name="mall"  value="<%= mall %>" />
            <input type=hidden name="liang"  value="<%= liang %>" />
            <input type=hidden name="baoy"  value="<%= baoy %>" />
 
        </div>
    </div>
 <script type="text/javascript">

     function isshow() {
         var s = document.getElementById('Select1').value;
         if (s == "0") {
             alert("请选择展示宝贝");
             return false;
         }
         if (s == "") {
             alert("手动选择宝贝");
         }
         return true;
     }
 </script>
    </form>
</body>
</html>
