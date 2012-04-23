<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateAds.aspx.cs" Inherits="UpdateAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <table width="100%">
         <tr>
           <td>网站Logo：</td>
           <td><asp:TextBox ID="Tb_Logo" runat="server" Width="570px"></asp:TextBox></td>
         </tr>
          <tr>
           <td>网站Ads：</td>
           <td><asp:TextBox ID="Tb_Ads" runat="server" Width="570px"></asp:TextBox></td>
         </tr>
         <tr>
           <td colspan="2"><asp:Button ID="Btn_Up" runat="server" Text="确 定" 
                   onclick="Btn_Up_Click" /></td>
         </tr>
       </table>
    </div>
    </form>
</body>
</html>
