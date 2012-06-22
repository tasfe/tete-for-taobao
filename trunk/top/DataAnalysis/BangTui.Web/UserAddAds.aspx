<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAddAds.aspx.cs" Inherits="UserAddAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
      <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <div>
        宝贝名：<asp:TextBox ID="TB_GoodsName" runat="server"></asp:TextBox>宝贝分类：<asp:DropDownList ID="DDL_GoodsClass" runat="server"></asp:DropDownList>
        修改时间：<asp:TextBox runat="server" ID="TB_StartTime" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />至
        <asp:TextBox ID="TB_EndTime" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />
        <asp:Button ID="BTN_SELECT" runat="server" Text="查询" onclick="BTN_SELECT_Click" />
      </div>
       <table>
         <tr>
           <td>选择    <td>选择</td>
           <td>宝贝图片</td>
           <td>宝贝名称</td>
           <td>价格(元)</td>
           <td>数量</td>
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
                   <td><img src='<%# Eval("GoodsPic") %>_80x80.jpg' /></td>
                   <td><asp:Label ID="LB_GoodsName" Text='<%# Eval("GoodsName") %>' runat="server" /></td>
                   <td><%# Eval("GoodsPrice")%></td>
                   <td><%# Eval("GoodsCount")%></td>
                </tr>
            </ItemTemplate>
         
         </asp:Repeater>
         
         <tr>
           <td colspan="5" align="center"><asp:Button Text="推广宝贝" ID="BTN_ShowGoods" 
                   runat="server" onclick="BTN_ShowGoods_Click" /></td>
         </tr>
         
       </table>
    </div>
    </form>
</body>
</html>
