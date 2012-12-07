<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zhsz.aspx.cs" Inherits="iphoneapi_zhsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<meta name="viewport" content="width=device-width,
 initial-scale=0.5, minimum-scale=0.1, 
maximum-scale=2, user-scalable=no" />
</head>
<body style="background:url(zhanghaoshezhi.png); width:640px; height:743px; margin:0px; padding:0px;">
    <form id="form1" runat="server">
    <div style="margin:54px 0 0 455px;">
        <input type="checkbox" value="1" <%=check1 %> name="isqq" style="width:40px;height:40px" onclick="showQQverify()" />
    </div>
    <div style="margin:76px 0 0 455px;">
        <input type="checkbox" value="1" <%=check2 %> name="issina" style="width:40px;height:40px" onclick="showSinaverify()" />
    </div>
    </form>

    <script>
        function showQQverify() {
            window.location.href = 'zhsz.aspx?isqq=1';
        }

        function showSinaverify() {
            window.location.href = 'zhsz.aspx?issina=1';
        }
    </script>

</body>
</html>
