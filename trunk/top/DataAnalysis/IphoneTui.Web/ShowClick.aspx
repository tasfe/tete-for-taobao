<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowClick.aspx.cs" Inherits="ShowClick" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>投放效果查看</title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
                        text: '投放效果(点击量)',
                        x: -20 //center
                    },
                    subtitle: {
                        text: '<%=ShowDate %>',
                        x: -20
                    },
                    xAxis: {
                        categories:<%=DateText %>
                    },
                    yAxis: {
                        title: {
                            text: '点击量'
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
                
                <div id="container" style="width: 850px; height: 400px; margin: 0 auto">
                </div>
    </div>
    </form>
</body>
</html>
