<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>客服绩效考核</title>
    <link rel="stylesheet" type="text/css" href="jxkh/css/theme.css" />
    <link rel="stylesheet" type="text/css" href="jxkh/css/style.css" />

    <script type="text/javascript">
        var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
        document.writeln('<link rel="stylesheet" type="text/css" href="jxkh/css/' + StyleFile + '">');
    </script>

    <!--[if IE]>
<link rel="stylesheet" type="text/css" href="css/ie-sucks.css" />
<![endif]-->
</head>
<body>
    <div id="container">
        <div id="header">
            <h2>客服绩效考核</h2>
            <div id="topmenu">
                <ul>
                    <li class="current"><a href="default2.aspx">主页</a></li>
                    <li><a href="ReceiveCustomer.aspx">日统计</a></li>
                    <li><a href="YejiTotal.aspx">业绩统计</a></li>
                    <li><a href="AllTalkContent.aspx">聊天记录</a></li>
                    <li><a href="#">客服对比</a></li>
                </ul>
            </div>
        </div>
        <div id="top-panel">
            <div id="panel">
                <ul>
                   <li><a href="#" class="report">接待人数</a></li>
                   <li><a href="#" class="report_seo">成功订单</a></li>
                   <li><a href="#" class="report_seo">付款金额</a></li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
            <div id="content">
                <%--<div id="rightnow">
                    <h3 class="reallynow">
                        <span>Right Now</span> <a href="#" class="add">Add New Product</a> <a href="#" class="app_add">
                            Some Action</a>
                        <br />
                    </h3>
                    <p class="youhave">
                        You have <a href="#">19 new orders</a>, <a href="#">12 new users</a> and <a href="#">
                            5 new reviews</a>, today you made <a href="#">$1523.63 in sales</a> and a total
                        of <strong>$328.24 profit </strong>
                    </p>
                </div>--%>
                <div>
                 <p class="youhave">
                  <a href="CustomerList.aspx">客服今天接待情况</a>
                 </p>
                    <table style="margin:0; width:740px; margin-top:5px;">
                      <thead>
                        <tr>
                            <th style="width:15%">
                                序号
                            </th>
                            <th style="width:15%">
                                买家
                            </th>
                            <th style="width:15%">
                                接待人
                            </th>
                            <th style="width:15%">
                                接待时间
                            </th>
                            <th style="width:15%">
                                接待时长
                            </th>
                            <th style="width:15%">
                                交易
                            </th>
                        </tr>
                      </thead>
                        <asp:Repeater ID="Rpt_Jie" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <%# Container.ItemIndex + 1%>
                                    </td>
                                    <td>
                                        <%# Eval("CustomerNick") %>
                                    </td>
                                    <td>
                                        <%# Eval("FromNick") %>
                                    </td>
                                    <td>
                                        <%# Eval("StartTime")%>
                                    </td>
                                    <td>
                                        <%# Eval("TimeSpan")%>
                                    </td>
                                    <td>
                                        <%# Eval("ShowJ")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <div id="sidebar">
                <ul>
                    <li>
                        <h3>
                            <a href="#" class="house">日统计</a></h3>
                        <ul>
                            <li><a href="#" class="report">接待人数</a></li>
                            <li><a href="#" class="report_seo">成功订单</a></li>
                            <li><a href="#" class="report_seo">付款金额</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="folder_table">业绩统计</a></h3>
                        <ul>
                            <li><a href="#" class="addorder">成功订单</a></li>
                            <li><a href="#" class="shipping">接待人次</a></li>
                            <li><a href="#" class="invoices">日统计</a></li>
                            <li><a href="#" class="invoices">月统计</a></li>
                            <li><a href="#" class="invoices">年统计</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="manage">聊天记录</a></h3>
                        <ul>
                            <li><a href="#" class="manage_page">超长响应聊天</a></li>
                            <li><a href="#" class="manage_page">流失客户聊天</a></li>
                            <li><a href="#" class="cart">成功客户聊天</a></li>
                            <li><a href="#" class="folder">未回复聊天</a></li>
                            <li><a href="#" class="promotions">内部聊天</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="user">客服对比</a></h3>
                        <ul>
                            <li><a href="#" class="useradd">接待人数</a></li>
                            <li><a href="#" class="useradd">平均首次响应速度</a></li>
                            <li><a href="#" class="group">平均响应速度s</a></li>
                            <li><a href="#" class="search">未回复人数</a></li>
                            <li><a href="#" class="online">回复次数</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div id="footer">
            <div id="credits">
                Made by <a href="http://www.7fshop.com/">TeTeSoft</a>
            </div>
            <div id="styleswitcher">
                <ul>
                    <li><a href="javascript: document.cookie='theme='; window.location.reload();" title="Default"
                        id="defswitch">d</a></li>
                    <li><a href="javascript: document.cookie='theme=1'; window.location.reload();" title="Blue"
                        id="blueswitch">b</a></li>
                    <li><a href="javascript: document.cookie='theme=2'; window.location.reload();" title="Green"
                        id="greenswitch">g</a></li>
                    <li><a href="javascript: document.cookie='theme=3'; window.location.reload();" title="Brown"
                        id="brownswitch">b</a></li>
                    <li><a href="javascript: document.cookie='theme=4'; window.location.reload();" title="Mix"
                        id="mixswitch">m</a></li>
                    <li><a href="javascript: document.cookie='theme=5'; window.location.reload();" title="Mix"
                        id="defswitch">m</a></li>
                </ul>
            </div>
            <br />
        </div>
    </div>
</body>
</html>
