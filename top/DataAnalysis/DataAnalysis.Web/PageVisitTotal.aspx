<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PageVisitTotal.aspx.cs" Inherits="PageVisitTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看具体页面访问</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     
        <table width="720" cellpadding="0" cellspacing="0">
            <tr>
                <td width="480" align="center">
                    <b>受访页面</b>
                </td>
                <td width="120" align="center">
                    <b>浏览次数</b>
                </td>
                <td width="120" align="center">
                    <b>人均浏览次数 </b>
                </td>
            </tr>
            <asp:Repeater ID="Rpt_PageVisit" runat="server">
                <ItemTemplate>
                    <tr>
                        <td height="35">
                            <%#Eval("VisitURL")%>
                        </td>
                        <td align="center">
                            <%#Eval("VisitCount")%>
                        </td>
                        <td align="center">
                            <%#Eval("VisitAvg")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div>
            <asp:Label ID="lbPage" runat="server"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
