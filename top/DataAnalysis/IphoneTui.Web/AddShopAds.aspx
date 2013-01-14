<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddShopAds.aspx.cs" Inherits="AddShopAds"
    ValidateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>手机推广魔盒</title>
    <link href="css/common.css" rel="stylesheet" />

    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <style>
        td
        {
            font-size: 12px; height:40px
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
        dd, dl, dt
        {
            margin: 0px;
            padding: 0px;
        }
    </style>
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div class="navigation" style="height: 600px;">
        <div class="crumbs">
            <a href="#" class="nolink">手机推广魔盒</a> 推广店铺
        </div>
        <div class="absright">
            <ul>
                <li>
                    <div class="msg">
                    </div>
                </li>
            </ul>
        </div>
        <div id="main-content">
            <input type="button" onclick="window.location.href='useradslist.aspx?istou=1'" value="投放中的广告" />
            <input type="button" onclick="window.location.href='useradslist.aspx'" value="等待投放的广告" />
            <hr />
            <table width="700" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="twid">
                        店铺名称：
                    </td>
                    <td>
                        <asp:TextBox ID="TB_ShppName" runat="server" Width="300px"></asp:TextBox>*广告词
                    </td>
                </tr>
                <tr>
                    <td>
                        显示价格：
                    </td>
                    <td>
                        <asp:TextBox ID="TB_Description" runat="server" Width="300px" Text="0"></asp:TextBox>*显示价格
                    </td>
                </tr>
                <tr>
                    <td>
                        店铺图标：
                    </td>
                    <td>
                        <img src='<%=ShopImg %>' /><asp:FileUpload ID="FUD_Img" runat="server" />(仅支持gif,jpg,png)
                        *广告展示图
                    </td>
                </tr>
                <tr>
                    <td>
                        店铺网址：
                    </td>
                    <td>
                        <asp:TextBox ID="TB_ShowUrl" runat="server" Width="300px"></asp:TextBox>*广告链接地址
                    </td>
                </tr>
                <tr>
                    <td>
                        选择应用：
                    </td>
                    <td>
                        <asp:DropDownList ID="DDL_App" runat="server" AppendDataBoundItems="true">
                        </asp:DropDownList>
                        *广告在该应用上
                    </td>
                </tr>
                <tr>
                    <td>
                        选择广告类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="DDL_AdsType" runat="server" AutoPostBack="True" 
                            ontextchanged="DDL_AdsType_TextChanged">
                        </asp:DropDownList>
                        *广告类型
                    </td>
                </tr>
                <tr>
                    <td>
                        选择位置：
                    </td>
                    <td>
                        <asp:DropDownList ID="DDL_Position" runat="server">
                        </asp:DropDownList>
                        *广告展示位置
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="BTN_Tui" Text="推广店铺" runat="server" OnClick="BTN_Tui_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
