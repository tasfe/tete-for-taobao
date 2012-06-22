<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddSite.aspx.cs" Inherits="AddSite" %>

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
         <td>网站名称</td>
         <td>网站地址</td>
         <td></td>
      </tr>
      <asp:Repeater ID="RPT_SiteList" runat="server" onitemcommand="RPT_SiteList_ItemCommand">
        <ItemTemplate>
           <tr>
              <td>
                <%# Eval("SiteName")%>
              </td>
              <td>
                <%# Eval("SiteUrl")%>
              </td>
              <td>
                <asp:Button ID="BTN_Update" runat="server" Text="修改" CommandArgument='<%# Eval("SiteId") %>' CommandName="Up" />
                <asp:Button ID="BTN_Delete" runat="server" Text="删除" CommandArgument='<%# Eval("SiteId") %>' CommandName="De"  /> 
                <asp:Button ID="BTN_ManageAds" runat="server" Text="广告位管理"  CommandArgument='<%# Eval("SiteId") %>' CommandName="Manage"  />       
              </td>
           </tr>
        </ItemTemplate>
      </asp:Repeater>
    </table>
    
    <table>
           <tr>
             <td>网站名称：</td>
             <td><asp:TextBox ID="TB_SiteName" runat="server" Width="370px" /></td>
           </tr>
           <tr>
             <td>网站地址：</td>
             <td><asp:TextBox ID="TB_SiteUrl" runat="server" Width="370px" /></td>
           </tr>
           <tr>
             <td colspan="2" align="center">
             <asp:Button ID="BTN_Add" runat="server" Text="确定添加" 
                     onclick="BTN_Add_Click" />
             <asp:Button ID="BTN_Up" runat="server" Text="确定修改" 
                     onclick="BTN_Up_Click" Visible="false" />
            </td>
           </tr>
    </table>
     <script type="text/javascript">
         document.getElementById("TB_SiteName").focus();
    </script>
    </div>
    </form>
</body>
</html>
