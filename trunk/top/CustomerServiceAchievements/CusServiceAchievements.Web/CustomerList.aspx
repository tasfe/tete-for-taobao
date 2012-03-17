<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerList.aspx.cs" Inherits="CustomerList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
      td{text-align:center}
      th{text-align:center}
    </style>
    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:TextBox ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"
                        class="Wdate" Width="120px"></asp:TextBox>&nbsp;
                    <asp:Button ID="Btn_Select" runat="server" Text="查 看" OnClick="Btn_Select_Click" />
                    
                    <table>
                    <tr>
                       <th>序号</th>
                       <th width="150px">买家</th>
                       <th width="220px">接待人</th>
                       <th width="150px">接待时间</th>
                       <th width="220px">接待时长</th>
                       <th>交易</th>
                    </tr>
                    <asp:Repeater ID="Rpt_CustomerList" runat="server">
                    
                      <ItemTemplate>
                         <tr>
                           <td align="center">
                             <%# Container.ItemIndex + 1%>
                           </td>
                           <td><%# Eval("CustomerNick") %></td>
                           <td><%# Eval("FromNick") %></td>
                           <td><%# Eval("StartTime")%></td>
                           <td><%# Eval("TimeSpan")%></td>
                           <td><%# Eval("ShowJ")%></td>
                         </tr>
                      </ItemTemplate>
                    
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
    </form>
</body>
</html>
