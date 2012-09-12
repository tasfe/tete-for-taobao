<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cate.aspx.cs" Inherits="iphoneapi_api_cate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="rpt1" runat="server">
            <ItemTemplate>
                <input name="id" type="hidden" value="<%#Eval("cateid") %>" />
                <input name="cate_<%#Eval("cateid") %>" value="<%#Eval("catename") %>" />
                <input name="orderid_<%#Eval("cateid") %>" value="<%#Eval("orderid") %>" /> <br />
            </ItemTemplate>
        </asp:Repeater>

        <br /><br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存" />
    </div>
    </form>
</body>
</html>
