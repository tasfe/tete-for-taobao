<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .all
        {
            width: 500px;
        }
        .detail
        {
            width: 240px;
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="all">
            <asp:Repeater runat="server" ID="Rpt_TotalList">
                <ItemTemplate>
                    <div class="detail">
                        <div>
                            <%# Eval("SiteTotalDate")%>
                        </div>
                        <div>
                            <%# Eval("SitePVCount")%>
                        </div>
                        <div>
                            <%# Eval("SiteUVCount")%>
                        </div>
                        <div>
                            <%# Eval("ZhiTongFlow")%>
                        </div>
                        <div>
                            <%# Eval("CPC")%>
                        </div>
                        <div>
                            <%# Eval("AskOrder")%>
                        </div>
                        <div>
                            <%# Eval("LostOrder")%>
                        </div>
                        <div>
                            <%# Eval("SiteOrderCount")%>
                        </div>
                        <div>
                            <%# Eval("SiteOrderPay")%>
                        </div>
                        <div>
                            <%# Eval("OneOrderPrice")%>
                        </div>
                        <div>
                            <%# Eval("OneCustomerPrice")%>
                        </div>
                        <div>
                            <%# Eval("CreateAVG")%>
                        </div>
                        <div>
                            <%# Eval("SiteUVBack")%>
                        </div>
                        <div>
                            <%# Eval("BackSee") %>
                        </div>
                        <div>
                            <%# Eval("Refund")%>
                        </div>
                        <div>
                            <%# Eval("SeeDeepAVG")%>
                        </div>
                        <div>
                            <%# Eval("SeeTop")%>
                        </div>
                        <div>
                            <%# Eval("SellTop")%>
                        </div>
                        <div>
                            <%# Eval("Collection")%>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <%--<table>
      
       <tr>
           <td></td>
           <asp:Repeater ID="Rpt_Date" runat="server">
               <ItemTemplate>
                     <td><%# Eval("TotalDate") %></td>
               </ItemTemplate>
           </asp:Repeater> 
       </tr>
       
       <tr>
          <asp:Repeater ID="Rpt_Data" runat="server">
           <ItemTemplate>
                <td>
                   <%# Eval("TotalName")%>
                </td>
              
           </ItemTemplate>
       </asp:Repeater>
      
       <%
           DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());
           for (DateTime i = now; i > now.AddDays(-14); i = now.AddDays(-1))
           {
               foreach (TopSiteTotalInfo info in SiteTotalList)
               { 
                   
                   %>
                
       
       <%}
           } %>
        </tr>
      
      </table>--%>
    </div>
    </form>
</body>
</html>
