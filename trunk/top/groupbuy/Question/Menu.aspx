<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
       <div>
        <div >后台问题管理</div>
        <ul>
            <li><a href="Admin/TT_Question_Manage.aspx" target="mainFrame">客户问题管理</a></li>
        </ul>
        <div >前台问题管理</div>
        <ul>
            <li><a href="TT_Question_Manage.aspx" target="mainFrame">客户问题管理</a></li>
             <li><a href="TT_Question.aspx" target="mainFrame">添加新问题</a></li>
        </ul>
    
    </div>
    </form>
</body>
</html>
