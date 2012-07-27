<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendUserAds.aspx.cs" Inherits="SendUserAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      店铺ID：<asp:TextBox ID="TB_Nick" runat="server"></asp:TextBox>
      <p />
      添加密码：<asp:TextBox ID="TB_Password" TextMode="Password" runat="server" />
      <p />
      
      <asp:Button ID="Btn_AddAds" runat="server" Text="确定添加" onclick="Btn_AddAds_Click" />
      
    </div>
    </form>
</body>
</html>
