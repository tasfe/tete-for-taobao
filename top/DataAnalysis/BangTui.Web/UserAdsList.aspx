<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAdsList.aspx.cs" Inherits="UserAdsList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table>
      <tr>
        <td>广告信息</td>
        <td>投放网站</td>
        <td>广告标题</td>
        <td>广告尺寸</td>
        <td>广告类型</td>
        <td>状态</td>
        <td>操作</td>
      </tr>
       <asp:Repeater ID="RPT_AdsList" runat="server" 
                    onitemdatabound="RPT_AdsList_ItemDataBound" 
                    onitemcommand="RPT_AdsList_ItemCommand">
          <ItemTemplate>
             <tr>
                <td>
                   <dl>
                     <dt>
                       <img src='<%# Eval("AdsPic") %>' height="80" width="80" />
                     </dt>
                     <dd>
                        标题 <%# Eval("AdsTitle")%>
                     </dd>
                      <dd>
                        网址 <%# Eval("AdsUrl")%>
                     </dd>
                      <dd>
                        旺旺 <%# Eval("AliWang")%>
                     </dd>
                      <dd>
                        分类 <%# Eval("SellCateName")%>
                     </dd>
                      <dd>
                        有效时间 <%# Eval("AdsShowStartTime")%> 至 <%# Eval("AdsShowFinishTime")%>
                     </dd>
                   </dl>
                </td>
                <td><%# GetSite(Eval("AdsId").ToString()) %></td>
                <td><a href='ShowAds.aspx?adsid=<%# Eval("AdsId") %>'><%# GetTitle(Eval("AdsId").ToString()) %></a></td>
                <td><%# GetSize(Eval("AdsId").ToString()) %></td>
                <td><%# GetAdsType(Eval("AdsId").ToString())%></td>
                <td><%# GetState(Eval("UserAdsState").ToString())%></td>
                <td>
                  <dt>
                     <dd><asp:Button ID="Btn_De" CommandName="De" runat="server" Text="删除投放" /></dd>
                     <dd><asp:Button ID="Btn_Insert" CommandName="Insert" runat="server" Text="我要投放" /></dd>
                     <dd><asp:Button ID="Btn_Result" CommandName="Result" runat="server" Text="效果分析" /></dd>
                     <dd><asp:Button ID="Btn_See" CommandName="See" runat="server" Text="查看投放" /></dd>
                     <dd><asp:Button ID="Btn_Stop" CommandName="Stop" runat="server" Text="暂停投放" /></dd>
                     <dd><asp:Button ID="Btn_Add" CommandName="Add" runat="server" Text="增加投放" /></dd>
                  </dt>
                </td>
             </tr>
          </ItemTemplate>
       </asp:Repeater>
     </table>
    </div>
    </form>
</body>
</html>
