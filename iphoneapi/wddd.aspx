<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wddd.aspx.cs" Inherits="iphoneapi_wddd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<meta name="viewport" content="width=device-width,
 initial-scale=0.5, minimum-scale=0.1, 
maximum-scale=2, user-scalable=no" />
</head>
<body  style="background:url(wddd33.png) repeat-y; width:640px; margin:0px; padding:0px;">
    <form id="form1" runat="server">
    <asp:Repeater ID="rptOrder" runat="server">
        <ItemTemplate>
            <div style="height:232px; width:640px;">
                <div style="width:640px; padding:30px 0 0 148px;">
                    <div style="font-size:28px; float:left; width:350px;">[<%#Eval("orderid") %>]</div>
                    <div style="font-size:28px; float:left; width:180px;">[<%#Eval("status") %>]</div>
                </div>
                <div style="padding:84px 0 0 120px;font-size:28px;">￥<%#Eval("price") %></div>
                <div style="padding:24px 0 0 180px;font-size:28px;"><%#Eval("adddate") %></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    </form>
</body>
</html>

