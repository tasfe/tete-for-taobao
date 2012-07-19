<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddGoods.aspx.cs" Inherits="AddGoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加商品</title>
    <style type="text/css">
     td
     {
     	height:40px
     }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table>
        <tr>
           <td>宝贝ID：</td>
           <td><asp:TextBox ID="TB_ID" runat="server" Width="400px"></asp:TextBox></td>
        </tr>
        <tr>
           <td>宝贝名：</td>
           <td><asp:TextBox ID="TB_GoodsName" runat="server" Width="400px"></asp:TextBox></td>
        </tr>
         <tr>
           <td>宝贝价格：</td>
           <td><asp:TextBox ID="TB_Price" runat="server"></asp:TextBox></td>
        </tr>
          <tr>
           <td>宝贝库存：</td>
           <td><asp:TextBox ID="TB_Count" runat="server"></asp:TextBox></td>
        </tr>
         <tr>
           <td>宝贝主图地址：</td>
           <td><asp:TextBox ID="TB_Url" runat="server" Width="400px"></asp:TextBox></td>
        </tr>
         <tr>
           <td>所属分类：</td>
           <td><asp:DropDownList ID="DDL_Cate" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
          <td colspan="2" align="center">
            <asp:Button ID="Btn_Add" runat="server" Text="添 加" onclick="Btn_Add_Click" />
          </td>
        </tr>
      </table>
    </div>
    </form>
</body>
</html>
