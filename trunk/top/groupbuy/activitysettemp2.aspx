<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitysettemp2.aspx.cs" Inherits="top_groupbuy_activitysettemp1" %>

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
    <script type="text/javascript">

        function deleteDIV(obj) {
            var ob = document.getElementById(obj);

            $(ob).parent().remove();
        }
    </script>
</head>
<body style="padding:0px; margin:0px;">
<form id="form1" action="activitysettemp3.aspx" method="post">

    <div class="navigation">
        <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 3.设置宝贝顺序和价格 </div>
    
        <div id="main-content">
            <table width="700">
            <tr>
                <td width="200px">宝贝名称</td>
                <td  width="100px">原价 </td>
                <td  width="100px">促销价 </td>
                <td  width="80px">排序号</td>
                <td  width="80px">参团人数</td>
                <td  >操作</td>
            </tr>
 
            </table>
            <div id="listhtml" style=" width:700px">
            <%=html%>
            </div>
           <table width="700">
            <tr>
                <td align="left" height="30" width="200px"></td>
                <td>
                    <input type="button" value="上一步" onclick="history.go(-1)" />
                    <input type="submit" value="下一步：生成促销代码" />
                </td>
            </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
