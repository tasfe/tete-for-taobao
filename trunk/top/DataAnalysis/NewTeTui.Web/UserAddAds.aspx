<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserAddAds.aspx.cs" Inherits="UserAddAds" Title="推广宝贝" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <div class="right01">
                    <img src="images/04.gif" />
                    广告投放 &gt; <span>投放宝贝</span></div>
      <div>
        宝贝名：<asp:TextBox ID="TB_GoodsName" runat="server" Width="90px"></asp:TextBox>宝贝分类：<asp:DropDownList ID="DDL_GoodsClass" runat="server"></asp:DropDownList>
        修改时间：<asp:TextBox runat="server" ID="TB_StartTime" Width="70px" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />至
        <asp:TextBox ID="TB_EndTime" runat="server" Width="70px" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />
        <asp:Button ID="BTN_SELECT" runat="server" Text="查询" onclick="BTN_SELECT_Click" />
      </div>
      <hr>
    <table width="700" cellpadding="0" cellspacing="0">
         
         <tr>
           <td colspan="5" align="left"><asp:Button Text="开始推广选中的宝贝" ID="Button1" 
                   runat="server" onclick="BTN_ShowGoods_Click" />&nbsp;
                   <asp:Button ID="Btn_ShowAddGoods" runat="server" Text="添加商品" 
                   onclick="Btn_ShowAddGoods_Click" Visible="false" /></td>
         </tr>
         <tr>
           <td height="35"><b style=font-size:14px>选择</b></td>
           <td><b style=font-size:14px>宝贝图片</b></td>
           <td><b style=font-size:14px>宝贝名称</b></td>
           <td><b style=font-size:14px>价格(元)</b></td>
           <td><b style=font-size:14px>数量</b></td>
         </tr>
         
         <asp:Repeater ID="Rpt_GoodsList" runat="server">
            
            <ItemTemplate>
                <tr>
                   <td>
                       <asp:CheckBox ID="CBOX_Goods" runat="server" />
                       <asp:Label ID="LB_GoodsId" runat="server" Text='<%# Eval("GoodsId") %>' Visible="false" />
                       <asp:Label ID="LB_CateId" runat="server" Text='<%# Eval("CateId") %>' Visible="false" />
                       <asp:Label ID="LB_TaoBaoCId" runat="server" Text='<%# Eval("TaoBaoCId") %>' Visible="false" />
                       
                       <asp:Label ID="LB_Img" runat="server" Text='<%# Eval("GoodsPic") %>' Visible="false" />
                   </td>
                   <td height="85"><img src='<%# Eval("GoodsPic") %>_80x80.jpg' /></td>
                   <td><asp:Label ID="LB_GoodsName" Text='<%# Eval("GoodsName") %>' runat="server" /></td>
                   <td><asp:Label ID="LB_Price" Text='<%# Eval("GoodsPrice")%>' runat="server" /></td>
                   <td><%# Eval("GoodsCount")%></td>
                </tr>
            </ItemTemplate>
         
         </asp:Repeater>
         
         <tr>
           <td colspan="5" align="left"><asp:Button Text="开始推广选中的宝贝" ID="BTN_ShowGoods" 
                   runat="server" onclick="BTN_ShowGoods_Click" /></td>
         </tr>
         
       </table>
    
       <div style="background-color:#dedede; margin-top:15px">
            <asp:label ID="lblCurrentPage" runat="server"></asp:label>
            <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
            <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
            <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
            <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
        </div>
        
</asp:Content>

