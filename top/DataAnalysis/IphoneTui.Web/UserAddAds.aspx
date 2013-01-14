<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAddAds.aspx.cs" Inherits="UserAddAds" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>手机推广魔盒</title>
    <link href="css/common.css" rel="stylesheet" />

    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <style>
        td
        {
            font-size: 12px;
            height: 40px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
    </style>
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div class="navigation" style="height: 600px;">
        <div class="crumbs">
            <a href="#" class="nolink">手机推广魔盒</a> 请选择您要推广宝贝
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
            <div>
                <div>
                    宝贝名：<asp:TextBox ID="TB_GoodsName" runat="server" Width="90px"></asp:TextBox>宝贝分类：<asp:DropDownList
                        ID="DDL_GoodsClass" runat="server">
                    </asp:DropDownList>
                    修改时间：<asp:TextBox runat="server" ID="TB_StartTime" Width="70px" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />至
                    <asp:TextBox ID="TB_EndTime" runat="server" Width="70px" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />
                    <asp:Button ID="BTN_SELECT" runat="server" Text="查询" OnClick="BTN_SELECT_Click" />
                </div>
                <hr />
                <table width="700" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="35">
                            <b style="font-size: 14px">选择</b>
                        </td>
                        <td>
                            <b style="font-size: 14px">宝贝图片</b>
                        </td>
                        <td>
                            <b style="font-size: 14px">宝贝名称</b>
                        </td>
                        <td>
                            <b style="font-size: 14px">价格(元)</b>
                        </td>
                        <td>
                            <b style="font-size: 14px">数量</b>
                        </td>
                    </tr>
                    <asp:Repeater ID="Rpt_GoodsList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CBOX_Goods" runat="server" />
                                    <asp:Label ID="LB_GoodsId" runat="server" Text='<%# Eval("GoodsId") %>' Visible="false" />
                                    <asp:Label ID="LB_CateId" runat="server" Text='<%# Eval("CateId") %>' Visible="false" />
                                    <asp:Label ID="LB_TaoBaoCId" runat="server" Text='<%# Eval("TaoBaoCId") %>' Visible="false" />
                                    <asp:Label ID="LB_Img" runat="server" Text='<%# Eval("GoodsPic") %>' Visible="false" />
                                </td>
                                <td height="85">
                                    <img src='<%# Eval("GoodsPic") %>_80x80.jpg' />
                                </td>
                                <td>
                                    <asp:Label ID="LB_GoodsName" Text='<%# Eval("GoodsName") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="LB_Price" Text='<%# Eval("GoodsPrice")%>' runat="server" />
                                </td>
                                <td>
                                    <%# Eval("GoodsCount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div style="background-color: #dedede; margin-top: 15px">
                <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                <asp:HyperLink ID="lnkFrist" runat="server">首页</asp:HyperLink>
                <asp:HyperLink ID="lnkPrev" runat="server">上一页</asp:HyperLink>
                <asp:HyperLink ID="lnkNext" runat="server">下一页</asp:HyperLink>
                <asp:HyperLink ID="lnkEnd" runat="server">尾页</asp:HyperLink>
            </div>
            <div>
                <table>
                    <tr>
                        <td>
                            选择应用：
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_App" runat="server"
                                AppendDataBoundItems="true">
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
                        <td colspan="2" align="left">
                            <asp:Button Text="开始推广选中的宝贝" ID="BTN_ShowGoods" runat="server" OnClick="BTN_ShowGoods_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
