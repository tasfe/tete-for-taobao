<%@ Page Language="C#" AutoEventWireup="true" CodeFile="logout.aspx.cs" Inherits="top_logout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>退出中...</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        退出中，请您稍等...
    </div>
    
    <script language="javascript" type="text/javascript">
        function go(){
            window.location.href='http://www.taobao.com/';
        }
        
        setTimeout('go()', 2000);
    </script>
    
    </form>
</body>
</html>
