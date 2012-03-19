<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YejiTotal.aspx.cs" Inherits="YejiTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
     <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
           <asp:TextBox ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" class="Wdate" Width="120px"></asp:TextBox> 至 
            <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
       &nbsp;<asp:Button ID="Btn_Select" runat="server" Text="检 索"  onclick="Btn_Select_Click" />
       
       <table>
         <tr>
           <td>客服</td>
           <td>付款金额</td>
           <td>宝贝金额</td>
           <td>运费</td>
           <td>订单数</td>
           <td>宝贝数</td>
           <td>回复次数</td>
           <td>接待人数</td>
         </tr>
       <asp:Repeater ID="Rpt_KefuTotal" runat="server">
         <ItemTemplate>
           <tr>
             <td><%# Eval("Nick") %></td>
             <td><%# Eval("Payment") %></td>
             <td><%# Eval("GoodsPay") %></td>
             <td><%# Eval("PostFee") %></td>
             <td><%# Eval("OrderCount") %></td>
             <td><%# Eval("GoodsCount") %></td>
             <td><%# Eval("ReceiveCount") %></td>
             <td><%# Eval("CustomerCount") %></td>
           </tr>
         
         </ItemTemplate>
       </asp:Repeater>
       
       </table>
    </div>
    </form>
</body>
</html>
