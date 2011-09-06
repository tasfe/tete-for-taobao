<%@ Page Language="C#" AutoEventWireup="true" CodeFile="groupbuyadd.aspx.cs" Inherits="top_groupbuy_groupbuyadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
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
    </script>
</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">特特营销推广专家</a> 添加活动 </div>
  
  <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
    因为淘宝政策调整，优惠接口目前最多优惠到7折，<A href='http://dev.open.taobao.com/bbs/read.php?tid=22210' target="_blank">点击查看详情</A>
</div>
  
    <div id="main-content">
        <div style="float:left; width:520px;" class="otherarea">
            <b style="font-size:20px">1、设置活动名称和促销时段</b> <span id="area1msg" style="display:none"> <span id="area1msgstr"></span> &nbsp; <a href='#' onclick="modifyArea1()">修改</a> </span>             <br />
            <div id="area1">
                <div id="errmsg" style="color:white; background-color:#ff0000; width:150px; border:solid 1px red; display:none"></div>

                设置活动名称：<input type="text" name="groupbuyname" id="groupbuyname" size="25" maxlength="10" />

                <br />

                活动开始时间：<input type="text" name="starttime" id="starttime" size="13" value="" readonly="readonly" /> 
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
                <br />

                活动结束时间：<input type="text" name="endtime" id="endtime" size="13" value="" readonly="readonly" /> 
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
                <br />
                <input type="button" value="确定" onclick="checkArea1()" /> 
            </div>
            <br />
            <b style="font-size:20px">2、选择宝贝</b>
            <br />
            <div id="area2" style="display:none">
                <input type="button" value="选择宝贝" onclick="OpenDialogLable('dialogProduct.aspx',650,560);" /> <br />
                <div id="productArea"></div>
                <br />
                <input type="button" value="完成选择" onclick="checkArea2()" /> <span id="errmsg1" style="color:Red"></span>
            </div>
            <br />
            <b style="font-size:20px">3、设置活动折扣</b>
            <br />
            <div id="area3" style="display:none">
                商品原价：<span id="oldprice"></span>
                <br />
                折 扣 价：<input type="text" id="zhekou" name="zhekou" />  <span id="errmsg2" style="color:Red"></span>
                <!--<br />
                团 购 价：<span id="newprice"></span> <input type="hidden" name="groupbuyprice" id="groupbuyprice" />-->
                <!--<br />
                限购数量：<input type="text" name="maxcount" value="20" />
                <br />
                最短支付时间：<input type="text" name="mintime" value="20" /> 分钟，填0则不限制
                <br />
                <input type="button" value="完成创建" onclick="checkArea3()" />-->
            </div>
        <asp:Button ID="btnsubmit" runat="server" Text="完成创建" onclick="btnsubmit_Click" />
        </div>

        
        <div style="float:left; border:solid 1px; display:none">
            展示效果预览
            <img src="" />
        </div>
    </div>
</div>

</form>

<script language="javascript" type="text/javascript">
    function OpenDialogLable(url, w, h, editTxt) {
        if (typeof (editTxt) == "undefined") {
            editTxt = "";
        }
        if (navigator.appVersion.indexOf("MSIE") == -1) {
            this.returnAction = function (strResult) {
                if (strResult != null) {
                    if (strResult != "") {
                        document.getElementById("productArea").innerHTML = strResult;
                    }
                }
            }
            window.open(url + '?d=' + Date() + "&t=" + escape(editTxt), 'newWin', 'modal=yes,width=' + w + ',height=' + h + ',top=200,left=300,resizable=no,scrollbars=no');
            return;
        } else {
            var GetValue = showModalDialog(url + '?d=' + Date() + "&t=" + escape(editTxt), null, 'dialogWidth:' + w + 'px; dialogHeight:' + h + 'px;')
            if (GetValue != null) {
                if (GetValue != "") {
                    document.getElementById("productArea").innerHTML = GetValue;
                    //alert(document.getElementById("html").value);
                }
            }
        }
    }

    function checkArea1() {
        //判断团购名是否为空
        var name = document.getElementById("groupbuyname");
        var time1 = document.getElementById("starttime");
        var time2 = document.getElementById("endtime");

        var errmsg = document.getElementById("errmsg");
        if (name.value == "") {
            errmsg.style.display = "";
            errmsg.innerHTML = "活动名称不能为空";
            name.focus();
            return;
        }

        if (time1.value == "") {
            errmsg.style.display = "";
            errmsg.innerHTML = "活动开始时间不能为空";
            return;
        }

        if (time2.value == "") {
            errmsg.style.display = "";
            errmsg.innerHTML = "活动结束时间不能为空";
            return;
        }

        if (time2.value == "") {
            errmsg.style.display = "";
            errmsg.innerHTML = "活动开始时间不能大于结束时间";
            return;
        }

        if (time2.value == "") {
            errmsg.style.display = "";
            errmsg.innerHTML = "活动开始时间必须大于现在";
            return;
        }

        errmsg.style.display = "none";

        //团购信息显示
        document.getElementById("area1msg").style.display = "";
        document.getElementById("area1msgstr").innerHTML = formatDataInfo();

        document.getElementById("area1").style.display = "none";
        document.getElementById("area2").style.display = "";
    }

    function formatDataInfo() {
        str = document.getElementById("starttime").value;
        str += " " + document.getElementById("startSelect").value;
        str += " - ";
        str += document.getElementById("endtime").value + " " + document.getElementById("endSelect").value;

        return str;
    }

    function checkArea2() {
        //判断商品是否为空
        var productArea = document.getElementById("productArea");
        var errmsg = document.getElementById("errmsg1");
        if (name.value == "") {
            errmsg.innerHTML = "请选择宝贝";
            return;
        }

        document.getElementById("area2").style.display = "none";
        document.getElementById("area3").style.display = "";

        //写价格进去
        document.getElementById("oldprice").innerHTML = document.getElementById("price").value;
        document.getElementById("zhekou").focus();
    }

    function modifyArea1() {
        document.getElementById("area1").style.display = "";
        document.getElementById("area2").style.display = "none";
        document.getElementById("area3").style.display = "none";
        document.getElementById("area1msg").style.display = "none";
    }

    function checkzhekou(obj) {
        var errmsg = document.getElementById("errmsg2");
        var oldprice = document.getElementById("oldprice");
        var newprice = document.getElementById("newprice");
        var newpricetxt = document.getElementById("groupbuyprice");

        if (isNaN(obj.value)) {
            errmsg.innerHTML = "折扣必须为数字";
            return false;
        }
        //计算折扣价
        newprice.innerHTML = oldprice.innerHTML;
        newpricetxt.value = oldprice.innerHTML;
    }
</script>

</body>
</html>