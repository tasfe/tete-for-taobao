<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerList.aspx.cs" Inherits="CustomerList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table width="100%">
        <tr>
           <td>Token</td>
           <td>安装日期</td>
           <td>登录次数</td>
        </tr>
       <asp:Repeater ID="Rpt_CustomerList" runat="server">
         <ItemTemplate>
           <tr>
             <td><%# Eval("Token")%></td>
             <td><%# Eval("Adddate")%></td>
             <td><%# Eval("Logintimes")%></td>
           </tr>
         </ItemTemplate>
       
       </asp:Repeater>
      </table>
    </div>
    </form>
</body>
</html>
