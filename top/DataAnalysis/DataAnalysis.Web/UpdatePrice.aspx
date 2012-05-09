<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdatePrice.aspx.cs" Inherits="UpdatePrice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>进货价设置</title>
         <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>    <link href="css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
</style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div>
         <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">营销决策</a> 进货价设置 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content" style="height:1000px;">

     <div>
     商品名：<asp:TextBox ID="Tb_GoodsName" runat="server"></asp:TextBox>&nbsp;
     <asp:Button ID="Btn_Search" runat="server" Text="搜 索" onclick="Btn_Search_Click" />&nbsp;
     <asp:Button ID="Btn_Update" runat="server" Text="确定更新" onclick="Btn_Update_Click" />
       <br />
        </div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center">
                    <b>商品图片</b>
                </td>
                <td align="center">
                    <b>商品名</b>
                </td>
                <td align="center">
                    <b>销售价格</b>
                </td>
                 <td width="110px" align="center">
                    <b>进货价格</b>
                </td>
            </tr>
            <asp:Repeater ID="Rpt_GoodsList" runat="server">
                <ItemTemplate>
                    <tr>
                     <td align="center">
                            <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' style="color: Black"
                                target="_blank">
                                <img src='<%# Eval("pic_url") %>_80x80.jpg' border="0" alt='<%#Eval("title")%>' />
                            </a>
                         </td>
                        <td height="35" valign="top">
                           <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' target="_blank">
                            <%#Eval("title")%>
                          </a>  
                        </td>
                       
                         <td height="35" align="center" valign="top">
                            <%#Eval("price")%>
                        </td>
                        <td align="center" valign="top">
                        <asp:Label ID="Lbl_GoodsId" runat="server" Text='<%#Eval("num_iid") %>' Visible="false" />
                        <asp:Label ID="Lbl_PurchasePrice" runat="server" Text='<%#Eval("PurchasePrice") %>' Visible="false" />
                         <asp:TextBox ID="TB_PurchasePrice" Text='<%#Eval("PurchasePrice")%>' runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </ItemTemplate>
                <SeparatorTemplate>
                 <tr><td colspan="4"><hr /></td></tr>
                </SeparatorTemplate>
            </asp:Repeater>
        </table>
       
        <div style="background-color:#dedede; margin-top:15px">
            <asp:label ID="lblCurrentPage" runat="server"></asp:label>
            <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
            <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
            <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
            <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
        </div>
    </div>
    </div>
    </div>
    </form>
</body>
</html>

