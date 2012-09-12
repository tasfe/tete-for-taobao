<%@ Page Language="C#" AutoEventWireup="true" CodeFile="itemorder.aspx.cs" Inherits="iphoneapi_api_cate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    <asp:DropDownList ID="ddl1" runat="server" AutoPostBack="true"
             OnSelectedIndexChanged="ddl1_SelectedIndexChanged">
    </asp:DropDownList>

    <br />
    <div style="height:300px; overflow:scroll;">
        <asp:Repeater ID="rpt1" runat="server">
            <ItemTemplate>
                <input name="ids" type="hidden" value="<%#Eval("itemid") %>" />
                <input name="orderid_<%#Eval("itemid") %>" value="<%#Eval("orderid") %>" size=4 /> 
                <%#Eval("itemname") %>---<%#Eval("price") %>---
                <img src='<%#Eval("picurl") %>' width=20 height=20 />
                <br />
            </ItemTemplate>
        </asp:Repeater>
        </div>

        <br /><br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存" />
    </div>
    </form>
</body>
</html>
