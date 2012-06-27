﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAdsList.aspx.cs" Inherits="UserAdsList" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>帮推广</title>
    <link href="css/common.css" rel="stylesheet" />
      <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        dd,dl,dt{margin:0px;padding:0px;}
    </style>
</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">


    
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="#" class="nolink">帮推广</a> 推广中的宝贝 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">
    
    <table width="700" cellpadding="0" cellspacing="0">
      <tr>
        <td style=font-size:14px;font-weight:bold width=171>广告信息</td>
        <td style=font-size:14px;font-weight:bold width=171>投放网站</td>
        <td style=font-size:14px;font-weight:bold width=91>广告标题</td>
        <td style=font-size:14px;font-weight:bold width=91>广告尺寸</td>
        <td style=font-size:14px;font-weight:bold width=91>广告类型</td>
        <td style=font-size:14px;font-weight:bold width=71>状态</td>
        <td style=font-size:14px;font-weight:bold width=71>操作</td>
      </tr>
       <asp:Repeater ID="RPT_AdsList" runat="server" 
                    onitemdatabound="RPT_AdsList_ItemDataBound" 
                    onitemcommand="RPT_AdsList_ItemCommand">
          <ItemTemplate>
             <tr>
                <td align="left">
                       <img src='<%# Eval("AdsPic") %>' height="80" width="80" />
                     <br>
                        标题 <%# Eval("AdsTitle")%>
                     <br>
                        网址 <%# Eval("AdsUrl")%>
                     <br>
                        旺旺 <%# Eval("AliWang")%>
                     <br>
                        分类 <%# Eval("SellCateName")%>
                     <br>
                        有效时间 <%# Eval("AdsShowStartTime")%> 至 <%# Eval("AdsShowFinishTime")%>
                  
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
                  <asp:Button ID="Btn_See" CommandName="See" CommandArgument='<%# Eval("Id") %>' runat="server" Text="查看投放" /><br>
                  <asp:Button ID="Btn_Stop" CommandName="Stop" CommandArgument='<%# Eval("Id") %>' runat="server" Text="暂停投放" /><br>
                  <asp:Button ID="Btn_Add" CommandName="Add" runat="server" Text="增加投放" />
                  
                </td>
             </tr>
          </ItemTemplate>
       </asp:Repeater>
     </table>
    </div>

    </div>
    </form>
</body>
</html>