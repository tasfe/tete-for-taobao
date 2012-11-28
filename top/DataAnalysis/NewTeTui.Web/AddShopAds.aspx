<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddShopAds.aspx.cs" Inherits="AddShopAds" Title="推广店铺"  ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <div class="right01">
                    <img src="images/04.gif" />
                    广告投放 &gt; <span>投放店铺</span></div>
                    
                    <input type=button onclick="window.location.href='useradslist.aspx?istou=1'" value="投放中的广告" />
    <input type=button onclick="window.location.href='useradslist.aspx'" value="等待投放的广告" />

    <hr />
    
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
          <td class="twid">店铺名称</td>
          <td><asp:TextBox ID="TB_ShppName" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>店铺介绍</td>
          <td><textarea ID="TB_Description" runat="server" rows="20" cols="50"/></td>
        </tr>
         <tr>
          <td>客服旺旺</td>
          <td><asp:TextBox ID="TB_AliWang" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>店铺图标</td>
          <td><img src='<%=ShopImg %>' /><asp:FileUpload ID="FUD_Img" runat="server" />(仅支持gif,jpg,png) </td>
        </tr>
         <tr>
          <td>店铺网址</td>
          <td><asp:TextBox ID="TB_ShowUrl" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td colspan="2" align="center"><asp:button ID="BTN_Tui" Text="推广店铺" runat="server" 
                  onclick="BTN_Tui_Click" /></td>
        </tr>
      </table>

</asp:Content>

