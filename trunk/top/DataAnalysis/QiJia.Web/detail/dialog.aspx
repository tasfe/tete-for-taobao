<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dialog.aspx.cs" Inherits="Web_detail_dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://qijia.7fshop.com/detail/global.css" rel="stylesheet" />
</head>
<body style="margin:12px; padding:0px;">
    <form id="form1" runat="server">
    <div>
    

    <%=html %>

    <!--<br />
    <a href='jiaocheng1/jiaocheng1.html' target="_blank">查看使用教程</a>-->

<script>
    function selectTemplate(obj, page) {
        var url = page + "?nick=<%=nick %>&id=<%=id %>&tplid=" + obj.id;
        window.location.href = url;
    }
</script>
    </div>
    </form>
</body>
</html>
