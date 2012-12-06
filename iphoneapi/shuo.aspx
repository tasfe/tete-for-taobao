<%@ Page Language="C#" AutoEventWireup="true" CodeFile="shuo.aspx.cs" Inherits="iphoneapi_shuoshuo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <textarea name="content" cols="40" rows="3"></textarea>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="提交说说" />
        
        <hr />

        <asp:Repeater ID="rptShuo" runat="server">
            <ItemTemplate>
                <%#Eval("content") %> - <%#Eval("adddate") %> <br /><br />
            </ItemTemplate>
        </asp:Repeater>

    </div>
    </form>
</body>
</html>
