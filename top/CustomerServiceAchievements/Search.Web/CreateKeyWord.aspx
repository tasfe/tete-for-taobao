<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateKeyWord.aspx.cs" Inherits="CreateKeyWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       请输入关键字：<asp:TextBox ID="Tb_txt" runat="server"></asp:TextBox>&nbsp;<asp:Button 
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
        <div style="background-color:#dedede; margin-top:15px">
            <asp:label ID="lblCurrentPage" runat="server"></asp:label>
            <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
            <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
            <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
            <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
        </div>
    </div>
    </form>
</body>
</html>
