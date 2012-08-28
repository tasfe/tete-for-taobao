<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddAds.aspx.cs" Inherits="AddAds" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
    <link href="css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">


    
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="#" class="nolink">特推广</a> 请选择您要推广宝贝 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">


    <div>
       <div>
         网站： <asp:DropDownList ID="DDL_SiteList" runat="server" AutoPostBack="True" 
               ontextchanged="DDL_SiteList_TextChanged"></asp:DropDownList>
       </div>
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
      
         <td><b>广告名称</b></td>
         <td><b>广告大小</b></td>
         <td><b>归属网站</b></td>
         <td><b>广告类型</b></td>
         <td></td>
      </tr>
      <asp:Repeater ID="RPT_AdsList" runat="server" onitemcommand="RPT_AdsList_ItemCommand">
        <ItemTemplate>
           <tr>
              <td>
                <%# Eval("AdsName")%>
              </td>
              <td>
                <%# Eval("AdsSize")%>
              </td>
              <td>
                <%# GetSiteName(Eval("SiteId").ToString()) %>
              </td>
              <td>
                 <%# GetAdsType(Eval("AdsType").ToString()) %>
              </td>
              <td>
                <img src='<%# Eval("AdsPic") %>' width="200px" height="100px" />
              </td>
              <td>
                <asp:Button ID="BTN_Update" runat="server" Text="修改" CommandArgument='<%# Eval("AdsId") %>' CommandName="Up" />
                <asp:Button ID="BTN_Delete" runat="server" Text="删除" CommandArgument='<%# Eval("AdsId") %>' CommandName="De"  /> 
              </td>
           </tr>
        </ItemTemplate>
      </asp:Repeater>
    </table>
        <table>
           <tr>
             <td>广告名称：</td>
             <td><asp:TextBox ID="TB_AdsName" runat="server" Width="370px" /></td>
           </tr>
           <tr>
             <td>广告位大小：</td> 
             <td><asp:TextBox ID="TB_AdsSize" runat="server" Width="370px" /></td>
           </tr>
           <tr>
              <td>广告类型：</td>
              <td>
                 <asp:DropDownList ID="DDL_AdsType" runat="server">
                   <asp:ListItem Text="不限个数" Value="1"></asp:ListItem>
                   <asp:ListItem Text="单个" Value="5"></asp:ListItem>
                 </asp:DropDownList>
              </td>
           </tr>
           <tr>
             <td>广告位置图片：</td>
             <td>
                <asp:FileUpload ID="FUP_Up" runat="server" />
             </td>
           </tr>
           <tr>
             <td colspan="2" align="center">
                    <asp:Button ID="BTN_Add" runat="server" Text="确定添加" 
                     onclick="BTN_Add_Click" />
                    <asp:Button ID="BTN_Up" runat="server" Text="确定修改" 
                     onclick="BTN_Up_Click" Visible="false" />
             </td>
           </tr>
        </table>
    </div>
    <script type="text/javascript">
        document.getElementById("TB_AdsName").focus();
    </script>
    </div>
    </div>


    </form>
</body>
</html>
