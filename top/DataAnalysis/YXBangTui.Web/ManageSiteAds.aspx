<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageSiteAds.aspx.cs" Inherits="ManageSiteAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        #TB_AdsCode
        {
            height: 114px;
            width: 367px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>
         网站： <asp:DropDownList ID="DDL_SiteList" runat="server" AutoPostBack="True" 
               ontextchanged="DDL_SiteList_TextChanged"></asp:DropDownList>
       </div>
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
         <td>ID</td>
         <td><b>网站地址</b></td>
         <td><b>广告位置</b></td>
         <td><b>广告代码</b></td>
         <td><b>广告类型</b></td>
         <td></td>
      </tr>
      <asp:Repeater ID="RPT_AdsList" runat="server" onitemcommand="RPT_AdsList_ItemCommand">
        <ItemTemplate>
           <tr>
              <td><%# Eval("Id") %></td>
              <td>
                <%# Eval("SiteUrl")%>
              </td>
              <td>
                <%# Eval("AdsPosition")%>
              </td>
              <td>
                <%# Eval("AdsCode") %>
              </td>
              <td>
                 <%# GetAdsType(Eval("AdsType").ToString()) %>
              </td>
              <td>
<%--                <asp:Button ID="BTN_Update" runat="server" Text="修改" CommandArgument='<%# Eval("AdsId") %>' CommandName="Up" />
--%>                <asp:Button ID="BTN_Delete" runat="server" Text="删除" CommandArgument='<%# Eval("Id") %>' CommandName="De"  /> 
              </td>
           </tr>
        </ItemTemplate>
      </asp:Repeater>
    </table>
        <table>
           <tr>
             <td>广告位置：</td>
             <td><asp:TextBox ID="TB_AdsPosition" runat="server" Width="370px" /></td>
           </tr>
           <tr>
             <td>广告代码：</td> 
             <td><textarea ID="TB_AdsCode" runat="server"></textarea></td>
           </tr>
           <tr>
              <td>广告类型：</td>
              <td>
                 <asp:DropDownList ID="DDL_AdsType" runat="server">
                   <asp:ListItem Text="站内" Value="0"></asp:ListItem>
                   <asp:ListItem Text="站外" Value="5"></asp:ListItem>
                   <asp:ListItem Text="淘宝" Value="10"></asp:ListItem>
                 </asp:DropDownList>
              </td>
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
        document.getElementById("TB_AdsPosition").focus();
    </script>
    </div>
    </form>
</body>
</html>
