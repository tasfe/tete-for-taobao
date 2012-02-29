﻿<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

    <script type="text/javascript">
        var chart;
        $(document).ready(function() {
            chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'container',
                    defaultSeriesType: 'line',
                    marginRight: 130,
                    marginBottom: 25
                },
                title: {
                    text: '测试',
                    x: -20 //center
                },
                subtitle: {
                    text: '测试test',
                    x: -20
                },
                xAxis: {
                    categories: [<%=DateText %>]
                },
                yAxis: {
                    title: {
                        text: '温度 (°C)'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
}]
                    },
                    tooltip: {
                        formatter: function() {
                            return '<b>' + this.series.name + '</b><br/>' +
					this.x + ': ' + this.y + '°C';
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
    <img src="GetData.ashx" />
    <script type="text/javascript" src="js/highcharts.js"></script>

    <script type="text/javascript" src="js/modules/exporting.js"></script>

    <div id="container" style="width: 800px; height: 400px; margin: 0 auto">
    </div>
    </div>
    </form>
</body>
</html>
