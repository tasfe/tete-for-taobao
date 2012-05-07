<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateCarriage.aspx.cs" Inherits="UpdateCarriage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>邮费设置</title>

    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

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
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height: 600px;">
            <div class="crumbs">
                <a href="javascript:;" class="nolink">营销决策</a> 邮费设置
            </div>
            <div class="absright">
                <ul>
                    <li>
                        <div class="msg">
                        </div>
                    </li>
                </ul>
            </div>
            <div id="main-content" style="height: 750px;">
                <div>
                    <table width="750px">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddl_Express" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="Rbl_Huo" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="款到" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="货到" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                省份：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_Pr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Pr_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                城市：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_City" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                邮费：
                            </td>
                            <td>
                                <asp:TextBox ID="TB_Carriage" runat="server" />
                            </td>
                        </tr>
                        <tr>
                          <td align="center">
                            <asp:Button ID="Btn_Ok" runat="server" Text="确 定" onclick="Btn_Ok_Click" />
                          </td>
                          <td>
                             <asp:Button ID="Btn_UpAll" runat="server" Text="修改以下所有邮费" 
                                  onclick="Btn_UpAll_Click" />
                          </td>
                        </tr>
                    </table>
                    <table width="750px">
                       <tr><td>省市</td><td>快递</td><td>款到/货到</td><td>邮费</td></tr>
                       <asp:Repeater ID="Rpt_ExpressCarriage" runat="server">
                          <ItemTemplate>
                             <tr>
                                <td><%# GetCity(Eval("CityId").ToString())%></td>
                                <td><%# GetExpress(Eval("ExpressId").ToString())%></td>
                                <td><%# GetGoods(Eval("Goods").ToString())%></td>
                                <td>
                                    <asp:Label ID="Lbl_Id" Text='<%# Eval("ID") %>' runat="server" Visible="false">
                                    </asp:Label>
                                    <asp:Label ID="Lbl_Carri" Text='<%# Eval("Carriage") %>' runat="server" Visible="false">
                                    </asp:Label>
                                    
                                    <asp:TextBox ID="Tb_Carri" runat="server" Text='<%# Eval("Carriage")%>'>
                                    </asp:TextBox>
                                </td>
                             </tr>
                          </ItemTemplate>
                          <SeparatorTemplate>
                            <tr>
                              <td colspan="4"><hr /></td>
                            </tr>
                          </SeparatorTemplate>
                       </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
