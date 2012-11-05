<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowClick.aspx.cs" Inherits="ShowClick" Title="查看点击" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="right01">
                    <img src="images/04.gif" />
                    广告投放 &gt; <span>查看点击</span></div>
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
</asp:Content>

