<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestUserSeePage.aspx.cs" Inherits="TestUserSeePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:TextBox ID="Tb_UserNick" runat="server">
      </asp:TextBox>
      <p />
      <asp:TextBox ID="Tb_UserSession" runat="server">
      </asp:TextBox>
      <p /> 
      <asp:Button ID="Btn_AddCookie" runat="server" Text="确 定" onclick="Btn_AddCookie_Click" />
    </div>
    </form>
</body>
</html>
