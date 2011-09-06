<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ajax.aspx.cs" Inherits="top_market_ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="test" runat="server">
            <ItemTemplate>
                <img src="<%#Eval("PicUrl") %>" width="100" height="100" /><br />
                <%#Eval("name")%>  <br />
                价格：<%#Eval("price") %> <br /> <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
