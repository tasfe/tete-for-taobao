<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="top_blog_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>测试账号</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        用户ID：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        密码：
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><br />
        title:&nbsp;
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><br />
        conent:
        <asp:TextBox ID="TextBox4" runat="server" Height="96px" TextMode="MultiLine" Width="190px"></asp:TextBox><br />
        &nbsp; &nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="测试登录" /></div>
    </form>
</body>
</html>
