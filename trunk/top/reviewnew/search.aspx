<%@ Page Language="C#" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="top_reviewnew_search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="查询客户详细资料" />
        <hr />
        
        用户名：<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        短信条数：<asp:TextBox ID="TextBox4" runat="server" Text="100"></asp:TextBox>
        内容：<asp:TextBox ID="TextBox5" runat="server" Text="填写联系方式赠送100条短信"></asp:TextBox>
        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="给客户增加短信" />
        <hr />

        <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
        <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="看下此客户是否进过服务" />
        <hr />
        
        卖家昵称：<asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
        评价日期：<asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
        <asp:Button ID="Button4" runat="server" onclick="Button3_Click" Text="获取该客户评价消息" />
    </div>
    </form>
</body>
</html>
