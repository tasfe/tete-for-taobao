<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
      <tr><td>商品名</td><td>价格</td></tr>
     <asp:Repeater ID="Rpt_Goods" runat="server">
        <ItemTemplate>
           <tr>
              <td>
                 <%# Eval("title") %>
              </td>
              <td>
                <%# Eval("price") %>
              </td>
           </tr>
          
        </ItemTemplate>
     
     </asp:Repeater>
    </table>
        
    </div>
    </form>
</body>
</html>
