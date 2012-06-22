<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowGoods.aspx.cs" Inherits="ShowGoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
    
     <asp:Repeater ID="RPT_GOODSCLASS" runat="server">
       <ItemTemplate>
         <tr>
           <td><%# Eval("name") %></td>
         </tr>
         
       </ItemTemplate>
     </asp:Repeater>
     
    </table>
    
    <table>
    
    </table>
    </div>
    </form>
</body>
</html>
