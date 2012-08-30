<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tuiguang.aspx.cs" Inherits="top_reviewnew_tuiguang" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
    <tr>
        <td>卖家</td>
        <td>浏览时间</td>
        <td>浏览次数</td>
        <td>IP地址</td>
        <td>来源</td>
    </tr>
        <asp:Repeater ID="rpt" runat="server">
            <ItemTemplate>
              <td> <%#Eval("nick") %> </td>
              <td> <%#Eval("adddate") %> </td>
              <td> <%#Eval("count") %> </td>
              <td> <%#Eval("ip") %> </td>
              <td> <%#Eval("laiyuan") %> </td>
            </ItemTemplate>
        </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
