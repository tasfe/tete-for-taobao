<%@ Page Language="C#" AutoEventWireup="true" CodeFile="grsz.aspx.cs" Inherits="iphoneapi_grsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body  style="background:url(gerenziliao.png); width:640px; height:616px; margin:0px; padding:0px;">
    <form id="form1" runat="server">
    <div>
        <input type="checkbox" value="1" name="isqq" onclick="showQQverify()" />
    </div>
    <div>
        <input type="checkbox" value="1" name="issina" onclick="showSinaverify()" />
    </div>
    </form>

    <script>
        function showQQverify() {
            alert(1);
        }

        function showSinaverify() {
            alert(2);
        }
    </script>

</body>
</html>
