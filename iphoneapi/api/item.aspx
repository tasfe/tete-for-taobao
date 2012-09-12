﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="item.aspx.cs" Inherits="iphoneapi_api_cate" %>

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
                <input name="id" type="hidden" value="<%#Eval("itemid") %>" />
                <%#Eval("itemname") %>---<%#Eval("price") %>---
                <input name="isnew" type="checkbox" value="<%#Eval("itemid") %>" <%#check(Eval("isnew").ToString()) %> />-新品
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
