﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Dashboard - Admin Template</title>
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
            <h2>
                My eCommerce Admin area</h2>
            <div id="topmenu">
                <ul>
                    <li class="current"><a href="default.aspx">Dashboard</a></li>
                    <li><a href="#">Orders</a></li>
                    <li><a href="YejiTotal.aspx">Users</a></li>
                    <li><a href="#">Manage</a></li>
                    <li><a href="#">CMS</a></li>
                    <li><a href="#">Statistics</a></li>
                    <li><a href="#">Settings</a></li>
                </ul>
            </div>
        </div>
        <div id="top-panel">
            <div id="panel">
                <ul>
                    <li><a href="#" class="report">Sales Report</a></li>
                    <li><a href="#" class="report_seo">SEO Report</a></li>
                    <li><a href="#" class="search">Search</a></li>
                    <li><a href="#" class="feed">RSS Feed</a></li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
            <div id="content">
                <div id="rightnow">
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
                </div>
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
                            <a href="#" class="house">Dashboard</a></h3>
                        <ul>
                            <li><a href="#" class="report">Sales Report</a></li>
                            <li><a href="#" class="report_seo">SEO Report</a></li>
                            <li><a href="#" class="search">Search</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="folder_table">Orders</a></h3>
                        <ul>
                            <li><a href="#" class="addorder">New order</a></li>
                            <li><a href="#" class="shipping">Shipments</a></li>
                            <li><a href="#" class="invoices">Invoices</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="manage">Manage</a></h3>
                        <ul>
                            <li><a href="#" class="manage_page">Pages</a></li>
                            <li><a href="#" class="cart">Products</a></li>
                            <li><a href="#" class="folder">Product categories</a></li>
                            <li><a href="#" class="promotions">Promotions</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="user">Users</a></h3>
                        <ul>
                            <li><a href="#" class="useradd">Add user</a></li>
                            <li><a href="#" class="group">User groups</a></li>
                            <li><a href="#" class="search">Find user</a></li>
                            <li><a href="#" class="online">Users online</a></li>
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
