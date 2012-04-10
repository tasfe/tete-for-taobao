<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeeTop.aspx.cs" Inherits="SeeTop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <table>
         <tr>
           <td>关键字</td><td>商品及排名</td>
         </tr>
         
         <asp:Repeater ID="Rpt_KeyWords" runat="server">
         
           <ItemTemplate>
             <tr>
                <td><%# Eval("Key") %></td> 
                
                <td>
                    <asp:Repeater DataSource='<%# Eval("Value") %>' runat="server">
                        <ItemTemplate>
                            <tr>
                              <td></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
             </tr>
           </ItemTemplate>
         
           <SeparatorTemplate>
             <tr>
               <td colspan="2"><hr /></td>
             </tr>
           </SeparatorTemplate>
         </asp:Repeater>
         
       </table>
    </div>
    </form>
</body>
</html>
