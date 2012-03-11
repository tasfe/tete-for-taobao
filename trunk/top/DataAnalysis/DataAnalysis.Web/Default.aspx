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
    <asp:Panel ID="Panel1" runat="server" Visible="false">
        <asp:Button ID="Btn_AddCId" runat="server" Text="一键添加统计代码" Font-Size="14" Height="50" Width="400" OnClick="Btn_AddCId_Click" />
    </asp:Panel>

    <div>
        <div style="font-size:18px; font-weight:bold;">销售数据统计 - （今日
        <asp:Repeater ID="Rpt_IpPV" runat="server">
                         <ItemTemplate>
                         【<%#Eval("Key")%>-<%#Eval("Value")%>】
                         </ItemTemplate>
        </asp:Repeater>）</div>
        <hr />
        <div>
            <table width="740" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="120"></td>
                    <td width="120"><b>销售额</b></td>
                    <td width="120"><b>订单数</b></td>
                    <td width="120"><b>回头订单数</b></td>
                    <td width="100"><b>客单价</b></td>
                    <td width="100"><b>销售单价</b></td>
                    <td width="100"><b>每单商品数</b></td>
                </tr>
                <tr>
                    <td height="25" style="font-size:16px; font-weight:bold;">今 日</td>
                    <td>12000</td>
                    <td>100</td>
                    <td>12</td>
                    <td>120</td>
                    <td>80</td>
                    <td>1.5</td>
                </tr>
                <tr>
                    <td height="25" style="font-size:16px; font-weight:bold;">昨 日</td>
                    <td>11000</td>
                    <td>100</td>
                    <td>12</td>
                    <td>110</td>
                    <td>80</td>
                    <td>1.5</td>
                </tr>
                <tr>
                    <td height="25" style="font-size:16px; font-weight:bold;">最近7天走势</td>
                    <td><span style="color:Red">↑8.33%</span></td>
                    <td><span style="color:green">↓8.33%</span></td>
                    <td><span style="color:green">↓12.22%</span></td>
                    <td><span style="color:green">↓11.44%</span></td>
                    <td><span style="color:Red">↑8.65%</span></td>
                    <td><span style="color:Red">↑1.52%</span></td>
                </tr>
            </table>
        </div>
    </div>


    <div style="padding-top:50px;">
        <div style="font-size:18px; font-weight:bold;">爆款宝贝排行</div>
        <hr />
        <div>
            <table width="740" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="70"></td>
                    <td width="120"><b>图片</b></td>
                    <td width="220"><b>宝贝名称</b></td>
                    <td width="120"><b>价格</b></td>
                    <td width="120"><b>购买数量</b></td>
                </tr>
                <asp:Repeater runat="server" ID="Rpt_GoodsSellTop">
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            NO.<%# Container.ItemIndex + 1%>
                        </td>
                        <td align="center">
                            <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' style="color:Black" target="_blank">
                            <img src='' width="100" height="100" border="0" />
                            </a>
                        </td>
                        <td align="center">
                            <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' style="color:Black" target="_blank">
                            <%#Eval("title")%>
                            </a>
                        </td>
                        <td align="center">
                            ￥<%#Eval("price")%>
                        </td>
                        <td align="center">
                            <%#Eval("price")%>
                        </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>


    
    <div style="padding-top:50px;">
        <div style="font-size:18px; font-weight:bold;">店铺浏览统计</div>
        <hr />
        <div>
            <table width="740" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="120"><b>来访者IP</b></td>
                    <td width="120"><b>来访者时间</b></td>
                    <td width="120"><b>来访者所在地区</b></td>
                    <td width="120"><b>来访者网络提供商</b></td>
                    <td width="120"><b>查看访问轨迹</b></td>
                </tr>
                <asp:Repeater ID="Repeater1" runat="server">
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
                </asp:Repeater>
            </table>
        </div>
    </div>


    
    <asp:Panel ID="Panel2" runat="server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="740" style="margin-top:10px;">
            <tr>
                <td width="50%" style="padding:2px;">
                    <table cellspacing="0" cellpadding="0" width="98%">
                      <tr>
                        <td colspan="2" style="background-color:#abcabc;font-weight:bold;color:#211511">
                           今天流量
                        </td>
                      </tr>
                      
                            <tr>
                              <td colspan="2" align="right"><a href="HourTotal.aspx">查看详细</a></td>
                            </tr>

                  </table>
                </td>
                <td width="50%">
                    <table width="98%">
                 <tr><td colspan="3" style="background-color:#abcabc;font-weight:bold;color:#211511">今天宝贝订购排行</td></tr>
                 
                    
                        <tr>
                          <td colspan="3" align="right"><a href="GoodsBuyTotal.aspx">查看更多</a></td>
                        </tr>

               </table>
                </td>
            </tr>
        </table>

       <table cellspacing="0" cellpadding="0" width="740" style="margin-top:10px;">
            <tr>
                <td>
                    <table width="98%">
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
                </td>
                </tr>
                <tr>
                <td>
                <table width="98%">
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
                </td>
            </tr>
        </table>
              
             
    </asp:Panel>


    </div>


    </div>
    </form>
</body>
</html>
