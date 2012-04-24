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
       <SeparatorTemplate>
           <tr>
             <td colspan="3"><hr /></td>
           </tr>
       </SeparatorTemplate>
       </asp:Repeater>
      </table>
      <div style="background-color: #dedede; margin-top: 15px">
                    <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                    <asp:HyperLink ID="lnkFrist" runat="server">首页</asp:HyperLink>
                    <asp:HyperLink ID="lnkPrev" runat="server">上一页</asp:HyperLink>
                    <asp:HyperLink ID="lnkNext" runat="server">下一页</asp:HyperLink>
                    <asp:HyperLink ID="lnkEnd" runat="server">尾页</asp:HyperLink>
     </div>
    </div>
    </form>
</body>
</html>
