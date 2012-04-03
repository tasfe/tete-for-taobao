<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReceiveCustomer.aspx.cs" Inherits="ReceiveCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>接待人次</title>
    <link rel="stylesheet" type="text/css" href="jxkh/css/theme.css" />
    <link rel="stylesheet" type="text/css" href="jxkh/css/style.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

    <script type="text/javascript">
        var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
        document.writeln('<link rel="stylesheet" type="text/css" href="jxkh/css/' + StyleFile + '">');
    </script>

    <!--[if IE]>
<link rel="stylesheet" type="text/css" href="css/ie-sucks.css" />
<![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="header">
            <h2>客服绩效考核</h2>
            <div id="topmenu">
                <ul>
                    <li><a href="default2.aspx">主页</a></li>
                    <li class="current"><a href="#">日统计</a></li>
                    <li><a href="YejiTotal2.aspx">业绩统计</a></li>
                    <li><a href="AllTalkContent.aspx">聊天记录</a></li>
                    <li><a href="GroupReceiveCustomer.aspx">客服对比</a></li>
                </ul>
            </div>
        </div>
        <div id="top-panel">
            <div id="panel">
                <ul>
                   <li><a href="CustomerList.aspx" class="report">接待人数</a></li>
                   <li><a href="#" class="report_seo">成功订单</a></li>
                   <li><a href="#" class="report_seo">付款金额</a></li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
            <div id="content">
              <script type="text/javascript">
            var chart;
            $(document).ready(function() {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'divchart',
                        defaultSeriesType: 'line',
                        marginRight: 130,
                        marginBottom: 25
                    },
                    title: {
                        text: '店铺接待统计(小时)',
                        x: -20 //center
                    },
                    subtitle: {
                        text: '接待人次',
                        x: -20
                    },
                    xAxis: {
                        categories:<%=DateText %>
                    },
                    yAxis: {
                        title: {
                            text: '24小时接待人次'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
}]
                        },
                        tooltip: {
                            formatter: function() {
                                return  '<b>' + this.series.name + '</b>：' + this.y;
                            }
                        },
                        legend: {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'top',
                            x: -10,
                            y: 100,
                            borderWidth: 0
                        },
                        series:<%=SeriseText %>
                        });
                    });

                </script>

                <script type="text/javascript" src="js/highcharts.js"></script>
                <script type="text/javascript" src="js/modules/exporting.js"></script>
                 <div id="divchart" style="width: 750px; height: 400px; margin: 0 auto">
                </div>
            </div>
            <div id="sidebar">
                <ul>
                    <li>
                        <h3>
                            <a href="#" class="report">日统计</a></h3>
                        <ul>
                            <li><a href="#" class="report">接待人数</a></li>
                            <li><a href="#" class="report">成功订单</a></li>
                            <li><a href="#" class="report">付款金额</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="folder_table">业绩统计</a></h3>
                        <ul>
                            <li><a href="#" class="folder_table">成功订单</a></li>
                            <li><a href="#" class="folder_table">接待人次</a></li>
                            <li><a href="#" class="folder_table">日统计</a></li>
                            <li><a href="#" class="folder_table">月统计</a></li>
                            <li><a href="#" class="folder_table">年统计</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="invoices">聊天记录</a></h3>
                        <ul>
                            <li><a href="#" class="invoices">超长响应聊天</a></li>
                            <li><a href="#" class="invoices">流失客户聊天</a></li>
                            <li><a href="#" class="invoices">成功客户聊天</a></li>
                            <li><a href="#" class="invoices">未回复聊天</a></li>
                            <li><a href="#" class="invoices">内部聊天</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="user">客服对比</a></h3>
                        <ul>
                            <li><a href="#" class="user">接待人数</a></li>
                            <li><a href="#" class="user">平均首次响应速度</a></li>
                            <li><a href="#" class="user">平均响应速度s</a></li>
                            <li><a href="#" class="user">未回复人数</a></li>
                            <li><a href="#" class="user">回复次数</a></li>
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
    </form>
</body>
</html>
