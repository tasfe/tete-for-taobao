<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fuwuLoglist.aspx.cs" Inherits="detail_fuwuLoglist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b>服务订购日志</b><br /><br />
        
        <table>
        <tr>
            <td><b>订购项目</b></td>
            <td><b>订购日期</b></td>
            <td><b>订购类型</b></td>
            <td><b>费用</b></td>
        </tr>

        <asp:Repeater ID="rpt" runat="server">
            <ItemTemplate>
        <tr>
            <td>宝贝描述模板</td>
            <td><%#Eval("addDate") %></td>
            <td><%#Eval("isold") %></td>
            <td>0元</td>
        </tr>
            </ItemTemplate>
        </asp:Repeater>

        </table>
        
        <br /><br />
        <input type="button" value="返回" style="height:45px; width:135px;" onclick="history.go(-1)" />
     </div>
    </form>
</body>
</html>
