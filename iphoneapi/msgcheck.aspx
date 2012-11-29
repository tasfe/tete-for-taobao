<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgcheck.aspx.cs" Inherits="iphoneapi_msgcheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:Button ID="Button1" runat="server" 
            onclick="Button1_Click" Text="审核通过" />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="审核不通过" />
        
        <asp:Button ID="Button3" runat="server" OnClientClick="window.location.href='msgcheck.aspx?ispass=0'" Text="查看历史审核结果" />

        
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <input type="checkbox" name="ids" value="<%#Eval("guid") %>" /> 
                <%#Eval("content") %>
                <%#Eval("adddate") %>
                <%#Eval("ispass") %> <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>
</body>
</html>
