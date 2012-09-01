<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="UpdateGoods.aspx.cs" Inherits="UpdateGoods" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="ContentLeft">
    <div id="ContentLeftTop">
      <div class="BarLeft"></div>
      <div id="ContentLeftTopText">更新宝贝信息</div>
      <div class="BarRight"></div>
      <div class="Cal"> </div>
    </div>
    <div id="ContentLeftBox">
      <ul>
        <li><a href="#"> &gt; 更新宝贝信息</a></li>
      </ul>
    </div>
  </div>
<div id="ContentRight">
    <div id="ContentRightTop">
      <div class="BarLeft"></div>
      <div id="ContentRightTopText">更新宝贝信息</div>
      <div class="BarRight"></div>
      <div class="Cal"></div>
    </div>
    <div id="ContentRightBox">
     <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
         如果您重新编辑过商品，请点击更新商品； 如果您还修改过分类，请点击更新分类。
         </div>
            <asp:Button ID="Btn_UpdateGoods" Text="更新商品" runat="server" OnClick="Btn_UpdateGoods_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="Btn_UpdateClass" Text="更新分类" runat="server" OnClick="Btn_UpdateClass_Click" />  
            </div>
            </div>
</asp:Content>

