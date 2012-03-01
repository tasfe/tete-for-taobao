<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>主页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
          <div id="title">统计概括</div>
          <div style="width:100%; float:left">
            <div style="width:50%; float:left">
              <table cellspacing="0" cellpadding="0" width="40%" border="1">
                  <tr>
                    <td colspan="2">
                       今日流量
                    </td>
                  </tr>
                 
                  <asp:Repeater ID="Rpt_IpPV" runat="server">
                     <ItemTemplate>
                         <tr>
                           <td><%#Eval("Key")%></td>
                           <td><%#Eval("Value")%></td>
                         </tr>
                     </ItemTemplate>
                  </asp:Repeater>
              </table>
            </div>
            <div style="width:50%; float:left">
              <table>
                 <tr><td colspan="2">宝贝订购排行</td></tr>
                 <asp:Repeater runat="server" ID="Rpt_GoodsSellTop">
                    <ItemTemplate>
                       <tr>
                            <td>
                              <%# Container.ItemIndex + 1%>
                            </td>
                          <td>
                                <%#Eval("GoodsName")%>
                          </td>
                       </tr>
                    </ItemTemplate>
                 </asp:Repeater>
               </table>
            </div>
          </div>
    </div>
    </form>
</body>
</html>
