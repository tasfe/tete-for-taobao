<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dialog.aspx.cs" Inherits="Web_detail_dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://qijia.7fshop.com/detail/global.css" rel="stylesheet" />
</head>
<body style="margin:0px; padding:0px;">
    <form id="form1" runat="server">
    <div>
    <img src="template1.gif" /><br />
<input type="button" style="background:url(btn_01.jpg) no-repeat; width:116px; height:29px; border:0;margin-left:40px; margin-top:10px; cursor:pointer" id="a3b6283d-ac07-4bc4-9d68-c64ab09a4903" onclick="selectTemplate(this)">

<script>
    function selectTemplate(obj) {
        var url = "dialog1.aspx?nick=<%=nick %>&id=<%=id %>&tplid=" + obj.id;
        window.location.href = url;
    }
</script>
    </div>
    </form>
</body>
</html>
