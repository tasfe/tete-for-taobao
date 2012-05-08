<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dialog.aspx.cs" Inherits="Web_detail_dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <img src="template1.gif" /><br />
<input type="button" value="选择此模板" id="tpl1" onclick="selectTemplate(this)">

<script>
    function selectTemplate(obj) {
        var url = "dialog1.aspx?id=" + obj.id;
        window.location.href = url;
    }
</script>
    </div>
    </form>
</body>
</html>
