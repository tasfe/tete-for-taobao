<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAdsList.aspx.cs" Inherits="UserAdsList" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特推广</title>
    <link href="css/common.css" rel="stylesheet" />
      <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        dd,dl,dt{margin:0px;padding:0px;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
<form id="form1" runat="server">

<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="#" class="nolink">特推广</a> <asp:Label ID="Lbl_ShowAdsDec" runat="server" /> </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">

    <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
 您共有<%=AdsCount %>个广告位，已经投放了<%=TouCount%>个广告，还有<%=NoTouCount %>个广告没有投放，<a href='/qubie.html'>点击增加投放广告位</a>！
</div>
    
    <input type=button onclick="window.location.href='useradslist.aspx?istou=1'" value="投放中的广告" />
    <input type=button onclick="window.location.href='useradslist.aspx'" value="等待投放的广告" />
    <input type=button onclick="window.location.href='UserAddAds.aspx'" value="投放新广告" />

    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
      <tr>
        <td style=font-size:14px;font-weight:bold width=210>广告详情</td>
        <td style=font-size:14px;font-weight:bold width=171>展示应用</td>
        <td style=font-size:14px;font-weight:bold width=91>广告标题</td>
        <td style=font-size:14px;font-weight:bold width=91>广告尺寸</td>
        <td style=font-size:14px;font-weight:bold width=91>广告类型</td>
        <td style=font-size:14px;font-weight:bold width=71>状态</td>
        <td style=font-size:14px;font-weight:bold width=71></td>
      </tr>
       <asp:Repeater ID="RPT_AdsList" runat="server" 
                    onitemdatabound="RPT_AdsList_ItemDataBound" 
                    onitemcommand="RPT_AdsList_ItemCommand">
          <ItemTemplate>
             <tr>
                <td align="left" width=210 height=180>
                       <img src='<%# Eval("AdsPic") %>' height="80" width="80" />
                     <br>
                        <%# Eval("AdsTitle")%>
                     <br>
                        <%# Eval("AdsUrl")%>
                     <br>
                        <%# Eval("AliWang")%>
                     <br>
                        <%# Eval("AdsShowStartTime")%> 至 <br> <%# Eval("AdsShowFinishTime")%>
                  <asp:Label ID="Lbl_IsSend" runat="server" Visible="false" Text='<%# Eval("IsSend") %>'></asp:Label>
                </td>
                <td><%# GetSite(Eval("AdsId").ToString()) %></td>
                <td><a href='ShowAds.aspx?adsid=<%# Eval("AdsId") %>'><%# GetTitle(Eval("AdsId").ToString()) %></a></td>
                <td><%# GetSize(Eval("AdsId").ToString()) %></td>
                <td><%# GetAdsType(Eval("AdsId").ToString())%></td>
                <td><%# GetState(Eval("UserAdsState").ToString())%></td>
                <td align="left">
                  
                  <asp:Button ID="Btn_De" CommandName="De" CommandArgument='<%# Eval("Id") %>' runat="server" Text="删除投放" /><br>
                  <asp:Button ID="Btn_Insert" CommandName="Insert" CommandArgument='<%# Eval("Id") %>' runat="server" Text="我要投放" /><br>
                  <asp:Button ID="Btn_Result" CommandName="Result" CommandArgument='<%# Eval("Id") %>' runat="server" Text="效果分析" /><br>
                  <asp:Button ID="Btn_See" CommandName="See" CommandArgument='<%# Eval("AdsId") %>' runat="server" Text="查看投放" /><br>
                  <asp:Button ID="Btn_Stop" CommandName="Stop" CommandArgument='<%# Eval("Id") %>' runat="server" Text="暂停投放" /><br>
                  <asp:Button ID="Btn_Add" CommandName="Add" runat="server" Text="增加投放" Visible="false" />
                  
                </td>
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
    </div>
    </form>
</body>
</html>
