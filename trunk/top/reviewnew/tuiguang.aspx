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
        <td>进入服务时间</td>
    </tr>
        <asp:Repeater ID="rpt" runat="server">
            <ItemTemplate>
            <tr>
              <td> <%#Eval("nick") %> </td>
              <td> <%#Eval("starttime") %> </td>
              </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>

        <hr />


        <table>
    <tr>
        <td>IP</td>
        <td>PV</td>
        <td>日期</td>
    </tr>
        <asp:Repeater ID="Repeater3" runat="server">
            <ItemTemplate>
            <tr>
              <td> <%#Eval("count1") %> </td>
              <td> <%#Eval("count2")%> </td>
              <td> <%#Eval("count3")%> </td>
              </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>

        
        <hr />

        <table>
        <tr>
            <td valign="top">
            
        <table>
    <tr>
        <td>卖家</td>
        <td>浏览时间</td>
        <td>浏览次数</td>
        <td>IP地址</td>
        <td>来源</td>
    </tr>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
            <tr>
              <td> <%#Eval("nick") %> </td>
              <td> <%#Eval("adddate") %> </td>
              <td> <%#Eval("count") %> </td>
              <td> <%#Eval("ip") %> </td>
              <td> <%#Eval("laiyuan") %> </td>
              </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
            </td>
            <td valign="top">
            
        <table>
    <tr>
        <td>卖家</td>
        <td>浏览时间</td>
        <td>浏览次数</td>
        <td>IP地址</td>
        <td>来源</td>
    </tr>
        <asp:Repeater ID="Repeater2" runat="server">
            <ItemTemplate>
            <tr>
              <td> <%#Eval("nick") %> </td>
              <td> <%#Eval("adddate") %> </td>
              <td> <%#Eval("count") %> </td>
              <td> <%#Eval("ip") %> </td>
              <td> <%#Eval("laiyuan") %> </td>
              </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
            </td>
            <td valign="top">
            
        <table>
    <tr>
        <td>卖家</td>
        <td>浏览时间</td>
        <td>浏览次数</td>
        <td>IP地址</td>
        <td>来源</td>
    </tr>
        <asp:Repeater ID="Repeater4" runat="server">
            <ItemTemplate>
            <tr>
              <td> <%#Eval("nick") %> </td>
              <td> <%#Eval("adddate") %> </td>
              <td> <%#Eval("count") %> </td>
              <td> <%#Eval("ip") %> </td>
              <td> <%#Eval("laiyuan") %> </td>
              </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
            </td>
        </tr>
        </table>
    </div>
    </form>
</body>
</html>
