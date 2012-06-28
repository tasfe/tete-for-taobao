<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddShopAds.aspx.cs" Inherits="AddShopAds" %>

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

<div class="navigation" style="height:600px;">

    <form id="form1" runat="server">



  <div class="crumbs"><a href="#" class="nolink">帮推广</a> 推广店铺 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">    
    
    <input type=button onclick="window.location.href='useradslist.aspx?istou=1'" value="投放中的广告" />
    <input type=button onclick="window.location.href='useradslist.aspx'" value="等待投放的广告" />

    <hr />
    
    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
          <td class="twid">店铺名称</td>
          <td><asp:TextBox ID="TB_ShppName" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>店铺介绍</td>
          <td><asp:TextBox ID="TB_Description" runat="server" Height="207px" Width="418px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>客服旺旺</td>
          <td><asp:TextBox ID="TB_AliWang" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td>店铺图标</td>
          <td><img src='<%=ShopImg %>' /><asp:FileUpload ID="FUD_Img" runat="server" />(仅支持gif,jpg,png) </td>
        </tr>
         <tr>
          <td>店铺网址</td>
          <td><asp:TextBox ID="TB_ShowUrl" runat="server" Width="300px"></asp:TextBox></td>
        </tr>
         <tr>
          <td colspan="2" align="center"><asp:button ID="BTN_Tui" Text="推广店铺" runat="server" 
                  onclick="BTN_Tui_Click" /></td>
        </tr>
      </table>
    </div>

    </div>
    </form>
</body>
</html>
