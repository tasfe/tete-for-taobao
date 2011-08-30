<%@ Page Language="C#" AutoEventWireup="true" CodeFile="promotionlist.aspx.cs" Inherits="top_market_promotionlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <asp:TextBox ID="TextBox3" runat="server" Height="99px" TextMode="MultiLine" 
            Width="229px"></asp:TextBox>
        <br />
        <br />
        <br />
        获取商品对应活动ID<br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
        <br />
        <br />
        删除该活动<br />
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
        
    </div>
    </form>
</body>
</html>
