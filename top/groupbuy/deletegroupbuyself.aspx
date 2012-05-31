<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deletegroupbuyself.aspx.cs" Inherits="top_market_deletegroupbuy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        本功能用于团购用户关闭服务后无法删除宝贝描述中的团购代码，非此问题请勿使用... <br /><br />
        <br />
        nick:<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        session:<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="一键删除团购" />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="一键删除好评" />
        <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="一键删除老团购" />
        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="Label2" runat="server" Visible="false"></asp:Label>
    </div>
    </form>
</body>
</html>
