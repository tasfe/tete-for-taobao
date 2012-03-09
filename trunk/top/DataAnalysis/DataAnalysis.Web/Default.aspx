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
          <div id="title" style="font-weight:bold;font-size:16px;color:#211511">统计概括</div>
              <table cellspacing="0" cellpadding="0" width="40%" border="1">
                  <tr>
                    <td colspan="2" style="background-color:#abcabc;font-weight:bold;color:#211511">
                       今天流量
                    </td>
                  </tr>
                  <asp:Repeater ID="Rpt_IpPV" runat="server">
                     <ItemTemplate>
                         <tr>
                           <td align="center"><%#Eval("Key")%></td>
                           <td align="center"><%#Eval("Value")%></td>
                         </tr>
                     </ItemTemplate>
                     <FooterTemplate>
                        <tr>
                          <td colspan="2" align="right"><a href="HourTotal.aspx">查看详细</a></td>
                        </tr>
                     </FooterTemplate>
                  </asp:Repeater>
              </table><br />
              <table width="80%">
                 <tr><td colspan="3" style="background-color:#abcabc;font-weight:bold;color:#211511">今天宝贝订购排行</td></tr>
                 <asp:Repeater runat="server" ID="Rpt_GoodsSellTop">
                    <ItemTemplate>
                       <tr>
                            <td align="center">
                              <%# Container.ItemIndex + 1%>
                            </td>
                          <td align="center">
                              <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' style="color:Black" target="_blank">
                                <%#Eval("title")%>
                              </a>
                          </td>
                          <td align="center">
                               单价：￥<%#Eval("price")%>
                          </td>
                       </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                          <td colspan="3" align="right"><a href="GoodsBuyTotal.aspx">查看更多</a></td>
                        </tr>
                     </FooterTemplate>
                 </asp:Repeater>
               </table>
               <br />
               <table width="80%">
                  <tr>
                    <td colspan="5" style="background-color:#abcabc;font-weight:bold;color:#211511">最近在线客户</td>
                  </tr>
                  <tr>
                      <td align="center">来访者IP</td>
                      <td align="center">来访者时间</td>
                      <td align="center">来访者所在地区</td>
                      <td align="center">来访者网络提供商</td>
                      <td align="center">查看访问轨迹</td>
                  </tr>
                  <asp:Repeater ID="Rpt_OnlineCustomer" runat="server">
                      <ItemTemplate>
                         <tr>
                            <td align="center">
                              <%#Eval("VisitIP")%>
                            </td>
                            <td align="center">
                              <%#Eval("VisitTime")%>
                            </td>
                               <td align="center">
                              <%#Eval("IPLocation")%>
                            </td>
                            <td align="center">
                              <%#Eval("NetWork")%>
                            </td>
                            <td align="center">
                               <a href='UVisitPage.aspx?visitip=<%#Eval("VisitIP")%>' style="color:Black">
                                查看
                               </a>   
                            </td>
                         </tr>
                      </ItemTemplate>
                      <FooterTemplate>
                         <tr>
                           <td colspan="5" align="right">
                               <a href="PageVisitTotal.aspx">查看页面访问排行</a>
                           </td>
                         </tr>
                      </FooterTemplate>
                  </asp:Repeater>
               </table>
               
               <table  width="60%">
                  <tr>
                    <td colspan="5" style="background-color:#abcabc;font-weight:bold;color:#211511">店铺当天统计</td>
                    </tr>
                    <tr>
                      <td align="center">订单数量</td>
                      <td align="center">订单金额</td>
                      <td align="center">PV量</td>
                      <td align="center">UV量</td>
                      <td align="center">回头浏览</td>
                  </tr>
                   <asp:Repeater ID="Rpt_OrderTotal" runat="server">
                      <ItemTemplate>
                         <tr>
                            <td align="center">
                              <%#Eval("SiteOrderCount")%>
                            </td>
                            <td align="center">
                              <%#Eval("SiteOrderPay")%>
                            </td>
                               <td align="center">
                              <%#Eval("SitePVCount")%>
                            </td>
                            <td align="center">
                              <%#Eval("SiteUVCount")%>
                            </td>
                            <td align="center">
                               <%#Eval("SiteUVBack")%>
                            </td>
                         </tr>
                      </ItemTemplate>
                      <FooterTemplate>
                         <tr>
                           <td colspan="5" align="right">
                               <a href="OrderTotal.aspx">查看更多订单统计</a>
                           </td>
                         </tr>
                      </FooterTemplate>
                  </asp:Repeater>
                  
               </table>
    </div>
    </div>


    </div>
    </form>
</body>
</html>
