<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

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
                    <table width="750px" style="margin:0; width:750px">
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
                <div id="infowrap">
                    <div id="infobox">
                        <h3>
                            Sales for July</h3>
                        <p>
                            sss
                        </p>
                    </div>
                    <div id="infobox" class="margin-left">
                        <h3>
                            Traffic for July</h3>
                        <p>
                            <img src="jxkh/img/graph2.jpg" alt="a" width="359" height="266" /></p>
                    </div>
                    <div id="infobox">
                        <h3>
                            Last 5 Orders</h3>
                        <table>
                            <thead>
                                <tr>
                                    <th>
                                        Customer
                                    </th>
                                    <th>
                                        Items
                                    </th>
                                    <th>
                                        Grand Total
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <a href="#">Jennifer Kyrnin</a>
                                    </td>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        14.95 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Mark Kyrnin</a>
                                    </td>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        34.27 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Virgílio Cezar</a>
                                    </td>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        61.39 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Todd Simonides</a>
                                    </td>
                                    <td>
                                        5
                                    </td>
                                    <td>
                                        1472.56 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Carol Elihu</a>
                                    </td>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        9.95 €
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="infobox">
                        <h3>
                            New Customers</h3>
                        <table>
                            <thead>
                                <tr>
                                    <th>
                                        Customer
                                    </th>
                                    <th>
                                        Orders
                                    </th>
                                    <th>
                                        Average
                                    </th>
                                    <th>
                                        Total
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <a href="#">Jennifer Kyrnin</a>
                                    </td>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        5.6€
                                    </td>
                                    <td>
                                        14.95 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Mark Kyrnin</a>
                                    </td>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        14.97€
                                    </td>
                                    <td>
                                        34.27 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Virgílio Cezar</a>
                                    </td>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        15.31€
                                    </td>
                                    <td>
                                        61.39 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Todd Simonides</a>
                                    </td>
                                    <td>
                                        5
                                    </td>
                                    <td>
                                        502.61€
                                    </td>
                                    <td>
                                        1472.56 €
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Carol Elihu</a>
                                    </td>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        5.1€
                                    </td>
                                    <td>
                                        9.95 €
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="infobox" class="margin-left">
                        <h3>
                            Last 5 Reviews</h3>
                        <table>
                            <thead>
                                <tr>
                                    <th>
                                        Reviewer
                                    </th>
                                    <th>
                                        Product
                                    </th>
                                    <th>
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <a href="#">Jennifer Kyrnin</a>
                                    </td>
                                    <td>
                                        <a href="#">Apple iPhone 3G 8GB</a>
                                    </td>
                                    <td>
                                        <a href="#">
                                            <img src="jxkh/img/icons/page_white_link.png" /></a><a href="#"><img src="jxkh/img/icons/page_white_edit.png" /></a><a
                                                href="#"><img src="jxkh/img/icons/page_white_delete.png" /></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Mark Kyrnin</a>
                                    </td>
                                    <td>
                                        <a href="#">Prenosnik HP 530 1,6GHz</a>
                                    </td>
                                    <td>
                                        <a href="#">
                                            <img src="jxkh/img/icons/page_white_link.png" /></a><a href="#"><img src="jxkh/img/icons/page_white_edit.png" /></a><a
                                                href="#"><img src="jxkh/img/icons/page_white_delete.png" /></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Virgílio Cezar</a>
                                    </td>
                                    <td>
                                        <a href="#">Fuji FinePix S5800</a>
                                    </td>
                                    <td>
                                        <a href="#">
                                            <img src="jxkh/img/icons/page_white_link.png" /></a><a href="#"><img src="jxkh/img/icons/page_white_edit.png" /></a><a
                                                href="#"><img src="jxkh/img/icons/page_white_delete.png" /></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Todd Simonides</a>
                                    </td>
                                    <td>
                                        <a href="#">Canon PIXMA MP140</a>
                                    </td>
                                    <td>
                                        <a href="#">
                                            <img src="jxkh/img/icons/page_white_link.png" /></a><a href="#"><img src="jxkh/img/icons/page_white_edit.png" /></a><a
                                                href="#"><img src="jxkh/img/icons/page_white_delete.png" /></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <a href="#">Carol Elihu</a>
                                    </td>
                                    <td>
                                        <a href="#">Prenosnik HP 530 1,6GHz</a>
                                    </td>
                                    <td>
                                        <a href="#">
                                            <img src="jxkh/img/icons/page_white_link.png" /></a><a href="#"><img src="jxkh/img/icons/page_white_edit.png" /></a><a
                                                href="#"><img src="jxkh/img/icons/page_white_delete.png" /></a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
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
                Made by <a href="http://www.7fshop.com/">Bloganje</a>
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
