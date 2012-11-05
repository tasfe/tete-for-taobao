<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UpdateGoods.aspx.cs" Inherits="UpdateGoods" Title="更新宝贝信息" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="right01">
                    <img src="images/04.gif" />
                    广告投放 &gt; <span>更新宝贝信息</span></div>
                     <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
         如果您重新编辑过商品，请点击更新商品； 如果您还修改过分类，请点击更新分类。
         </div>
            <asp:Button ID="Btn_UpdateGoods" Text="更新商品" runat="server" OnClick="Btn_UpdateGoods_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="Btn_UpdateClass" Text="更新分类" runat="server" OnClick="Btn_UpdateClass_Click" />  
</asp:Content>

