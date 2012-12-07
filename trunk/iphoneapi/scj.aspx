<%@ Page Language="C#" AutoEventWireup="true" CodeFile="scj.aspx.cs" Inherits="iphoneapi_scj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<meta name="viewport" content="width=device-width,
 initial-scale=0.5, minimum-scale=0.1, 
maximum-scale=2, user-scalable=no" />
</head>
<body style="margin:0px; padding:0px;">
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="rptCoupon" runat="server">
            <ItemTemplate>
                <img src='<%#Eval("imgurl") %>' /> <br /><br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
