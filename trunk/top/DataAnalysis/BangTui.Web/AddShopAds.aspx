<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddShopAds.aspx.cs" Inherits="AddShopAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
     .twid{width:100px}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table>
        <tr>
          <td class="twid">店铺名称</td>
          <td><asp:TextBox ID="TB_ShppName" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>店铺介绍</td>
          <td><asp:TextBox ID="TB_Description" runat="server" Height="207px" Width="418px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>客服旺旺</td>
          <td><asp:TextBox ID="TB_AliWang" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>店铺图标</td>
          <td><img src='' /><asp:FileUpload ID="FUD_Img" runat="server" />(仅支持gif,jpg,png) </td>
        </tr>
         <tr>
          <td>店铺网址</td>
          <td><asp:TextBox ID="TB_ShowUrl" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td colspan="2" align="center"><asp:button ID="BTN_Tui" Text="推广店铺" runat="server" 
                  onclick="BTN_Tui_Click" /></td>
        </tr>
      </table>
    </div>
    </form>
</body>
</html>
