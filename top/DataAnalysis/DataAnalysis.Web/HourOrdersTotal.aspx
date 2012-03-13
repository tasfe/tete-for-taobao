﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HourOrdersTotal.aspx.cs" Inherits="HourOrdersTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>小时订单分析</title>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

    <script type="text/javascript">

        function pageLoad() {
        }
    
    </script>

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
                <a href="javascript:;" class="nolink">特特店铺销售分析</a> 订单量分析(小时)
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

                <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

                <script type="text/javascript">
            var chart;
            $(document).ready(function() {
                chart = new Highcharts.Chart({
                    chart: {
                        renderTo: 'container',
                        defaultSeriesType: 'column',
                        marginRight: 130,
                        marginBottom: 25
                    },
                    title: {
                        text: '店铺销售统计',
                        x: -20 //center
                    },
                    subtitle: {
                        text: '订单数量',
                        x: -20
                    },
                    xAxis: {
                        categories:<%=DateText %>
                    },
                    yAxis: {
                        title: {
                            text: '24小时订单量'
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

                <div style="text-align: center">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="OrderTotal.aspx" runat="server" Text="按天查看" />
                    <asp:Button ID="Btn_LastDays" runat="server" OnClick="Btn_LastDays_Click" Text="昨天" />&nbsp;
                    <asp:Button ID="Btn_Totay" runat="server" OnClick="Btn_Totay_Click" Text="今天"  Visible="false" />&nbsp;
                    
                    <asp:TextBox ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"
                        class="Wdate" Width="120px"></asp:TextBox>&nbsp;
                    <asp:Button ID="Btn_Select" runat="server" Text="查看" OnClick="Btn_Select_Click" />
                    <asp:HiddenField ID="HF_Date" runat="server" />
                    <br />
                </div>
                <div id="container" style="width: 750px; height: 400px; margin: 0 auto">
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
