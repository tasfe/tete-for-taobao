<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fuwuLog.aspx.cs" Inherits="detail_fuwuLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b>商品描述模板管理</b><br /><br />
        订购成功，您的服务到期时间是<asp:Label ID="lbEndDate" runat="server"></asp:Label>   
        <a href='fuwuLoglist.aspx?nick=<%=nickurl %>'>查看订购日志</a><br /><br />
        <asp:Button ID="Button1" runat="server" Text="立即使用" Height="45px" 
            onclick="Button1_Click" Width="135px" /> <!--<a href='jiaocheng1/jiaocheng1.html' target="_blank">查看使用教程</a>--> <br /><br />
            <asp:Label ID="Label1" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
