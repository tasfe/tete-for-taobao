<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fuwuBuy.aspx.cs" Inherits="detail_fuwuBuy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b>商品描述模板管理</b><br /><br />
        您可以使用此功能来快速创建漂亮的商品描述<br /><br />
        <asp:Button ID="Button1" runat="server" Text="免费试用" Height="45px" 
            onclick="Button1_Click" Width="135px" /> <br />
            <br /><br />
        <b>(注：本功能共有3次免费体验的机会，每次免费体验15天，您需要在到期前3天来继续续订，否则将失去免费试用资格，谢谢！)</b>
    </div>
    </form>
</body>
</html>
