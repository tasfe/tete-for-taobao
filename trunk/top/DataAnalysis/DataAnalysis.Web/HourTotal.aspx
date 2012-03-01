<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HourTotal.aspx.cs" Inherits="HourPVTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>小时流量分析</title>
    
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <script type="text/javascript">
    
      function pageLoad() {
      }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
        
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
                        categories:<%=DateText %>
                    },
                    yAxis: {
                        title: {
                            text: '24小时IP/PV量'
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
					this.x + '： ' + this.y;
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
    <div style="text-align:center">
     <asp:TextBox 
                ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" class="Wdate" Width="120px"></asp:TextBox>
               <asp:Button ID="Btn_Select" runat="server" Text="查看"  onclick="Btn_Select_Click" /><br />
    </div>
    <div id="container" style="width: 800px; height: 400px; margin: 0 auto">
    </div>
    </form>
</body>
</html>
