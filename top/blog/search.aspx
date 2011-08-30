<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="top_blog_search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%=radio %>

        <asp:TextBox ID="tbKey" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="搜索" onclick="btnSearch_Click" />
        <asp:Button ID="Button1" runat="server" Text="获取内容" onclick="btnSearch1_Click" />
        <asp:Button ID="Button2" runat="server" Text="发布到博客" onclick="btnSearch2_Click" />
        <asp:Button ID="Button3" runat="server" Text="登录博客" onclick="btnSearch3_Click" />
    </div>
    </form>
</body>
</html>
