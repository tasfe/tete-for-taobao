<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddAds.aspx.cs" Inherits="AddAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <div>
         网站： <asp:DropDownList ID="DDL_SiteList" runat="server" AutoPostBack="True" 
               ontextchanged="DDL_SiteList_TextChanged"></asp:DropDownList>
       </div>
    <table>
      <tr>
      
         <td>广告名称</td>
         <td>广告大小</td>
         <td>归属网站</td>
         <td>广告类型</td>
         <td></td>
      </tr>
      <asp:Repeater ID="RPT_AdsList" runat="server" onitemcommand="RPT_AdsList_ItemCommand">
        <ItemTemplate>
           <tr>
              <td>
                <%# Eval("AdsName")%>
              </td>
              <td>
                <%# Eval("AdsSize")%>
              </td>
              <td>
                <%# GetSiteName(Eval("SiteId").ToString()) %>
              </td>
              <td>
                 <%# GetAdsType(Eval("AdsType").ToString()) %>
              </td>
              <td>
                <asp:Button ID="BTN_Update" runat="server" Text="修改" CommandArgument='<%# Eval("AdsId") %>' CommandName="Up" />
                <asp:Button ID="BTN_Delete" runat="server" Text="删除" CommandArgument='<%# Eval("AdsId") %>' CommandName="De"  /> 
              </td>
           </tr>
        </ItemTemplate>
      </asp:Repeater>
    </table>
        <table>
           <tr>
             <td>广告名称：</td>
             <td><asp:TextBox ID="TB_AdsName" runat="server" Width="370px" /></td>
           </tr>
           <tr>
             <td>广告位大小：</td> 
             <td><asp:TextBox ID="TB_AdsSize" runat="server" Width="370px" /></td>
           </tr>
           <tr>
              <td>广告类型：</td>
              <td>
                 <asp:DropDownList ID="DDL_AdsType" runat="server">
                   <asp:ListItem Text="不限个数" Value="1"></asp:ListItem>
                   <asp:ListItem Text="单个" Value="5"></asp:ListItem>
                 </asp:DropDownList>
              </td>
           </tr>
           <tr>
             <td>广告位置图片：</td>
             <td>
                <asp:FileUpload ID="FUP_Up" runat="server" />
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
    </div>
    <script type="text/javascript">
        document.getElementById("TB_AdsName").focus();
    </script>
    </form>
</body>
</html>
