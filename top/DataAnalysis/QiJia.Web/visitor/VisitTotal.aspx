<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VisitTotal.aspx.cs" Inherits="VisitTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>店铺浏览统计(天)</title>
    <link href="../css/common.css" rel="stylesheet" />
<style type="text/css">
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
</style>
    <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div>

     <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特统计</a> 店铺浏览统计(天) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

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
                        text: '店铺浏览统计',
                        x: -20 //center
                    },
                    subtitle: {
                        text: 'PV/UV',
                        x: -20
                    },
                    xAxis: {
                        categories:<%=DateText %>
                    },
                    yAxis: {
                        title: {
                            text: 'PV/UV/回头浏览'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
}]
                        },
                        tooltip: {
                            formatter: function() {
                                return '<b>' + this.series.name + '</b>：' + this.y;
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
        
    <script type="text/javascript" src="../js/highcharts.js"></script>
    <script type="text/javascript" src="../js/modules/exporting.js"></script>
     <div style="text-align:center">
     <asp:HyperLink ID="HyperLink1" NavigateUrl="HourTotal.aspx" runat="server" Text="按小时查看" />
     <asp:Button ID="Btn_3Days" runat="server" OnClick="Btn_3Days_Click" Text="最近3天" />&nbsp;
     <asp:Button ID="Btn_7Days" runat="server" OnClick="Btn_7Days_Click" Text="最近7天" />&nbsp;
     <asp:Button ID="Btn_30Days" runat="server" OnClick="Btn_30Days_Click" Text="最近30天" />&nbsp;
     
     <asp:TextBox 
                ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" class="Wdate" Width="120px"></asp:TextBox> 至 
            <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
       &nbsp;<asp:Button ID="Btn_Select" runat="server" Text="检索"  onclick="Btn_Select_Click" />
     </div>  
        <div id="container" style="width: 750px; height: 400px; margin: 0 auto">
        </div>
    </div>

    </div>
    </div>
    
    </form>
</body>
</html>

