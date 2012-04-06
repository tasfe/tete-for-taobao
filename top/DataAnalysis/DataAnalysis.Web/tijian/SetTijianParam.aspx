<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetTijianParam.aspx.cs" Inherits="tijian_SetTijianParam" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>体检参数设置</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style type="text/css">
        td
        {
            font-size: 12px;
            height: 20px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
        .paramname{ color:#AF4A92}
    </style>
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height: 600px;">
            <div class="crumbs">
                <a href="javascript:;" class="nolink">营销决策</a> 体检参数设置
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
                <table>
                <tr>
                  <td colspan="2" class="paramname">
                    所设的值为体检的最低标准
                  </td>
                </tr>
                    <asp:Repeater ID="Rpt_tijian" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label Text='<%# Eval("ParamName") %>' ID="Lb_ParamName" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="Tb_ParamValue" runat="server" Text='<%# Eval("ParamValue") %>'></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btn_Up" Text="确 定" OnClick="btn_Up_Click" />
                        </td>
                    </tr>
                </table>
                <table width="600px">
                    <tr>
                        <td>
                           <span class="paramname">客户浏览比率</span>
                           是用浏览店铺的用户总数除以浏览页面总数所得
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <span class="paramname">销售关联度</span>
                             是用订单均价除以销售单价所得
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <span class="paramname">浏览转换率</span>
                             是用订单总数除以浏览店铺的用户总数所得
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <span class="paramname">浏览回头率</span>
                             是用昨天以前(含昨天)除以浏览店铺的用户总数所得
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <span class="paramname">二次购买率</span>
                             是用昨天以前(含昨天)成功下单的用户总数除以成功下单的用户总数所得
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <span class="paramname">页面访问深度</span>
                             是用浏览页面总数除以浏览店铺的用户总数所得
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="paramname">爆款商品购买率</span>
                            是用浏览爆款宝贝的次数除以爆款宝贝的销售数量所得
                        </td>
                    </tr>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
