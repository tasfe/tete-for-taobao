<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freesearch.aspx.cs" Inherits="top_reviewnew_freesearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="140"><b>创建时间</b></td>
                <td width="60"><b>免邮时间</b></td>
                <td width="60"><b>使用次数</b></td>
                <td width="60"><b>满金额</b></td>
                <td width="80"><b>地区</b></td>
                <td width="80"><b>已领用</b> </td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("adddate")%></td>
                <td height="35"><%#Eval("carddate")%>个月</td>
                <td><%#Eval("usecount")%></td>
                <td><%#Eval("price")%>元</td>
                <td><%#Eval("arealist")%></td>
                <td><%#Eval("sendcount")%></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    </div>
    </form>
</body>
</html>
