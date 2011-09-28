<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addSpecial.aspx.cs" Inherits="top_special_addSpecial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加特价</title>
    <link href="../css/common.css" rel="stylesheet" />
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="js/cal.js"></script>
    <script type="text/javascript">
        var startdatestr = '<%=startdate %>';
        var todatestr = '<%=todate %>';
        var enddatestr = '<%=enddate %>';

        jQuery(document).ready(function () {
            $('input#starttime').simpleDatepicker({ chosendate: startdatestr, startdate: startdatestr, enddate: enddatestr });
            $('input#endtime').simpleDatepicker({ chosendate: todatestr, startdate: todatestr, enddate: enddatestr });
        });

        function getTaobaoItem() {

            spreadStat(1);
        }

        var xmlHttp;
        var itemsStr;
        var itemsStrTxt;
        function createxmlHttpRequest() {
            if (window.ActiveXObject)
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            else if (window.XMLHttpRequest)
                xmlHttp = new XMLHttpRequest();
        }


        function spreadStat(pageid) {
            var q = document.getElementById("querySearch").value;
            var catObj = document.getElementById("Select1");
            var catid = catObj.options[catObj.options.selectedIndex].value;

            var pagenow = pageid;
            createxmlHttpRequest();
            var queryString = "/top/groupbuy/taobaoitem.aspx?act=get&isradio=1&query=" + escape(q) + "&catid=" + catid + "&p=" + pagenow + "&t=" + new Date().getTime();
            xmlHttp.open("GET", queryString);
            xmlHttp.onreadystatechange = handleStateChange;
            xmlHttp.send(null);
        }

        function handleStateChange() {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                var taobaoitem = document.getElementById("taobaoitem")
                taobaoitem.innerHTML = decodeURI(xmlHttp.responseText);
            }
        }
    </script>
    <style type="text/css">
        #Text1
        {
            width: 64px;
        }
        #Text2
        {
            width: 96px;
        }
        #zkprice1
        {
            width: 116px;
        }
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">天天特价</a> 添加特价 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
      <div id="main-content">
        宝贝名称：  <input name="q" id="querySearch" style="width:230px;" runat="server" />
          <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="搜索" />
&nbsp;&nbsp;<span 
              id="Label1" style="COLOR: #ff3300; FONT-WEIGHT: bold">请根据您宝贝的标题或者标题里的关键词搜索您要做特价的商品</span></div>
       <div style=" padding-left:10px;">
             <b style=" font-size:12px;">活动开始时间：</b>  
             <input type="text" name="starttime" id="starttime" size="13" value="" readonly="readonly" runat="server" /> 
                <select id="startSelect" name="startSelect" runat="server">
                    <option>00:00</option>
                    <option>01:00</option>
                    <option>02:00</option>
                    <option>03:00</option>
                    <option>04:00</option>
                    <option>05:00</option>
                    <option>06:00</option>
                    <option>07:00</option>
                    <option>08:00</option>
                    <option>09:00</option>
                    <option>10:00</option>
                    <option>11:00</option>
                    <option>12:00</option>
                    <option>13:00</option>
                    <option>14:00</option>
                    <option selected="selected">15:00</option>
                    <option>16:00</option>
                    <option>17:00</option>
                    <option>18:00</option>
                    <option>19:00</option>
                    <option>20:00</option>
                    <option>21:00</option>
                    <option>22:00</option>
                    <option>23:00</option>
                </select> 
                <b>活动结束时间：</b>  
                
                <input type="text" name="endtime" id="endtime" size="13" value="" runat=server readonly="readonly" /> 
                <select id="endSelect" name="endSelect" runat="server">
                    <option>00:00</option>
                    <option>01:00</option>
                    <option>02:00</option>
                    <option>03:00</option>
                    <option>04:00</option>
                    <option>05:00</option>
                    <option>06:00</option>
                    <option>07:00</option>
                    <option>08:00</option>
                    <option>09:00</option>
                    <option>10:00</option>
                    <option>11:00</option>
                    <option>12:00</option>
                    <option>13:00</option>
                    <option>14:00</option>
                    <option selected="selected">15:00</option>
                    <option>16:00</option>
                    <option>17:00</option>
                    <option>18:00</option>
                    <option>19:00</option>
                    <option>20:00</option>
                    <option>21:00</option>
                    <option>22:00</option>
                    <option>23:00</option>
                </select>    
                <br /><br />
                
             

              </div>
                 <br />
             <div style=" vertical-align:top;padding-left:10px;">
            
                <asp:ListBox ID="ListBox1" runat="server" Height="399px" Width="381px"></asp:ListBox> 
                <div style="padding-left:10px;  ">
                原价:<asp:Label ID="lbprice" runat="server" Text=""></asp:Label>
                &nbsp;折扣:<input id="Text1" type="text" /> 折扣后的价格:<input id="zkprice" type="text" />&nbsp; 
                    活动标题: <input id="zkprice1" type="text" size="4" /> 活动描述:<input id="zkprice0" type="text" value="特价促销!" />
                    <br />
                 </div>
                 <div style=" text-align:center">
                   <br />
                     <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                         Text="添加到天天特价" />
                 </div>
              </div>

               
    </div>
    </form>
</body>
</html>
