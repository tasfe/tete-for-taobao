<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTotal.aspx.cs" Inherits="visitor_DataTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>主页</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style type="text/css">
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
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server">
    <div class="navigation" style="height: 600px;">
        <div class="crumbs">
            <a href="javascript:;" class="nolink">特特统计</a> 店铺统计总览
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
            <div style="padding-top: 40px;">
                <div style="font-size: 18px; font-weight: bold;">
                    店铺浏览统计</div>
                <hr />
                <div>
                    <table width="740" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="120" align="center">
                                <b>来访者IP</b>
                            </td>
                            <td width="120" align="center">
                                <b>来访者时间</b>
                            </td>
                            <td width="120" align="center">
                                <b>来访者所在地区</b>
                            </td>
                            <td width="120" align="center">
                                <b>来访者网络提供商</b>
                            </td>
                            <td width="120" align="center">
                                <b>查看访问轨迹</b>
                            </td>
                        </tr>
                        <asp:Repeater ID="Rpt_OnlineCustomer" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <%#Eval("VisitIP")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("VisitTime")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("IPLocation")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("NetWork")%>
                                    </td>
                                    <td align="center">
                                        <a href='UVisitPage.aspx?visitip=<%#Eval("VisitIP")%>' style="color: Black">查看 </a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <SeparatorTemplate>
                              <tr>
                                <td colspan="5"><hr /></td>
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
