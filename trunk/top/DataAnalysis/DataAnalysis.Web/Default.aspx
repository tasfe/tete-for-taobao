<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>主页</title>
    <link href="css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="margin:0px; padding:0px;">
    <form id="form1" runat="server">
    <div>

    <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特店铺销售分析</a> 店铺统计总览 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
 <asp:Button ID="Btn_AddCId" runat="server" Text="一键添加统计代码" Font-Size="14" Height="50" Width="400" OnClick="Btn_AddCId_Click" />
          <div id="title">统计概括</div>
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
               
               <table>
                  <tr>
                    <td>在线客户</td>
                  </tr>
                  <asp:Repeater ID="Rpt_OnlineCustomer" runat="server">
                      <ItemTemplate>
                         <tr>
                            <td>
                              <%#Eval("VisitIP")%>
                            </td>
                            <td>
                              <%#Eval("VisitTime")%>
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
