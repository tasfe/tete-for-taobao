<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateKeyWord.aspx.cs" Inherits="CreateKeyWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       请输入关键字:<asp:TextBox ID="Tb_txt" runat="server"></asp:TextBox>&nbsp;<asp:Button 
            ID="Btn_Add" runat="server" Text="添 加" onclick="Btn_Add_Click" />
       <p />
       
       <table>
           <tr><td>已有关键字列表</td></tr>
           <asp:Repeater ID="Rpt_Keys" runat="server">
             <ItemTemplate>
                <tr>
                   <td><%# Eval("KeyWord") %></td>
                </tr>
             </ItemTemplate>
             <SeparatorTemplate>
               <tr><td><hr /></td></tr>
             </SeparatorTemplate>
           </asp:Repeater>
       </table>
    </div>
    </form>
</body>
</html>
