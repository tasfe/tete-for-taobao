<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsBuyTotal.aspx.cs" Inherits="GoodsBuyTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>爆款宝贝排行</title>
    <link href="css/common.css" rel="stylesheet" />
    <style>
        td
        {
            font-size: 12px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
    </style>

    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height: 600px;">
            <div class="crumbs">
                <a href="javascript:;" class="nolink">销售分析王</a> 爆款宝贝排行
            </div>
            <div class="absright">
                <ul>
                    <li>
                        <div class="msg">
                        </div>
                    </li>
                </ul>
            </div>
            <div id="main-content" style="height:1000px;">
                <div>
                <asp:Button ID="Btn_3Days" runat="server" OnClick="Btn_3Days_Click" Text="最近3天" />&nbsp;
     <asp:Button ID="Btn_7Days" runat="server" OnClick="Btn_7Days_Click" Text="最近7天" />&nbsp;
     <asp:Button ID="Btn_30Days" runat="server" OnClick="Btn_30Days_Click" Text="最近30天" />&nbsp;
                
                    <asp:TextBox ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"
                        class="Wdate" Width="120px"></asp:TextBox>
                    至
                    <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                    <asp:Button ID="Btn_Select" runat="server" Text="检索" OnClick="Btn_Select_Click" />
                    <br />
                </div>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                         <td align="center">
                            <b>商品图片</b>
                        </td>
                        <td align="center" width="220px">
                            <b>商品名</b>
                        </td>
                        <td align="center">
                            <b>商品价格</b>
                        </td>
                        <td width="220" align="center">
                            <b>已售数量</b>
                        </td>
                    </tr>
                    <asp:Repeater ID="Rpt_PageVisit" runat="server">
                        <ItemTemplate>
                            <tr>  
                            <td align="center">
                                    <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' style="color: Black"
                                        target="_blank">
                                        <img src='<%# Eval("pic_url") %>_80x80.jpg' border="0" alt='<%#Eval("title")%>' />
                                    </a>
                                </td>
                                <td height="35" valign="top">
                                    <a href='http://item.taobao.com/item.htm?id=<%#Eval("num_iid") %>' target="_blank">
                                        <%#Eval("title")%>
                                    </a>
                                </td>
                              
                                <td height="35" align="center" valign="top">
                                    <%#Eval("price")%>
                                </td>
                                <td align="center" valign="top">
                                    <%#Eval("Count")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                         <SeparatorTemplate>
                 <tr><td colspan="4"><hr /></td></tr>
                </SeparatorTemplate>
                    </asp:Repeater>
                </table>
                <div style="background-color: #dedede; margin-top: 15px">
                    <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                    <asp:HyperLink ID="lnkFrist" runat="server">首页</asp:HyperLink>
                    <asp:HyperLink ID="lnkPrev" runat="server">上一页</asp:HyperLink>
                    <asp:HyperLink ID="lnkNext" runat="server">下一页</asp:HyperLink>
                    <asp:HyperLink ID="lnkEnd" runat="server">尾页</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
