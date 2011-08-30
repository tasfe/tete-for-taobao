<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dialogPic.aspx.cs" Inherits="top_blog_dialogPic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
   <div>
        请输入您要插入图片的地址：<br />
        <input type="text" id="pic" style="width:400px;" /><br />
        <input type="button" value="确定插入" class="button" onclick="InitData('pic')" />
        <input type="button" value="关闭窗口" class="button" onclick="CloseWindow()" />
    </div>
    </form>

    <script language="javascript">
        function InitData(id) {
            var obj = document.getElementById(id);
            var str = '<img src="' + obj.value + '">';

            if (navigator.appVersion.indexOf("MSIE") == -1) {
                window.opener.returnAction(str);
                window.close();
            } else {
                window.returnValue = str;
                window.close();
            }
        }

        function CloseWindow() {
            if (navigator.appVersion.indexOf("MSIE") == -1) {
                window.close();
            } else {
                window.close();
            }
        }
    </script>

</body>
</html>
