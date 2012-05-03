<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activityadd.aspx.cs" Inherits="top_groupbuy_activityadd" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特特团购</title>
    <link href="../css/common.css" rel="stylesheet" />
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 

    <script language="javascript" type="text/javascript" src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/cal.js"></script>
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        th{text-align:left; height:40px;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
<form id="form1" runat="server">
	<input type="hidden" name="submit" value="true"/>
<div class="navigation">
  <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 创建促销活动 </div>

    
    <div id="main-content">
    
    <table width="700" cellpadding="0" cellspacing="0">

		<tbody>
			<tr>
				<th class="required">活动名称：</th>
				<td style="">
					<input id="actionName" name="name" value="" type="text" style="width:80px;"/> 活动名称应为2~5个字符，支持中文、字母、数字、下划线。其他不支持！
				</td>
			</tr>
			<tr>
				<th class="required">活动备注：</th>
				<td style="">
					<input id="actionMemo" name="memo" value="" type="text" style="width:165px;"/>
					不显示在商品页面中，可不填，最多输入10个字符。
				</td>
			</tr>
			<tr>
				<th class="required">活动开始时间：</th>
				<td id="startDateId">
					<input  class="Wdate" type="text" name="startDate"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"    readonly="readonly" style="width:165px;cursor:pointer" value=""/>&nbsp;&nbsp;&nbsp;&nbsp;<span class="tips">如果点击日期没反应请<a href="javascript:manulDate('start')">点击这里手动输入</a></span>
				</td>
			</tr>
			<tr>
				<th class="required">活动结束时间：</th>
				<td id="endDateId">
					<input  class="Wdate" type="text" name="endDate"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"    readonly="readonly" style="width:165px;cursor:pointer"/>&nbsp;&nbsp;&nbsp;&nbsp;<span class="tips">如果点击日期没反应请<a href="javascript:manulDate('end')">点击这里手动输入</a></span>
                    <div class="tips" style="color: red">结束时间不可超过软件到期时间。您当前软件到期时间为：<%= teteendDate %></div>
				</td>
			</tr>
			<tr>
				<th class="required">活动形式：</th>
				<td style="">
					<input type="radio" id="noStyle" name="itemType" value="same" checked="checked" onclick="showDetail(1)"/>每个参加活动的宝贝设置相同促销力度<input id="noStyle"  type="radio" name="itemType" value="different" onclick="showDetail(2)"/>每个参加活动的宝贝设置不同促销力度
				</td>
			</tr>
			<tr class="itemDetail">
				<th class="required">促销方式：</th>
				<td style="">
					<input type="radio" id="noStyle" class="dzRadio" name="discountType" value="DISCOUNT" checked="checked" onclick="showPrice(1)"/>打折 <input id="noStyle" class="jjRadio" type="radio" name="discountType" value="PRICE" onclick="showPrice(2)"/>减价
				</td>
			</tr>
			<tr class="itemDetail">
				<th class="required">促销力度：</th>
				<td >
					<div id="dz"> 打 <input type="text" id="dzValue"  name="zhe" value="" style="width:30px"/> 折  （有效值范围：7.0-9.9，支持小数点后1位。）</div>
                    <div id="jj" style=" display:none"> 减 <input type="text" id="jjValue" name="yuan" value="" style="width:30px"/> 元  （支持小数点后2位。 <font color="red">注意：优惠不能低于原价7折</font>）</div>
				</td>
			</tr>
			<tr class="itemDetail">
				<th class="required">优惠数量限制：</th>
				<td>
				    <div id="jjDecreaseNumDIV"  style="display: none">
					<select name="decreaseNum" style="font-size: 12px;width: 300px">
					<option value="0" selected="selected">买家拍下的多件商品均享受优惠</option>
					<option value="1">只对买家拍下的第一件商品优惠</option>
					</select>
					</div>
				    <div id="dzDecreaseNumDIV">
				          买家拍下的多件商品均享受优惠
					</div>					
				</td>
			</tr>
            <tr>
                <th class="required">已参团人数：</th>
                <td> <input type="text" id="rcount" name="rcount" value="300" style="width:70px"/> 
                    这个参数只在团购模板用到</td>
            </tr>
			<tr>
				<th class="required">目标客户：</th>
				<td>
					<select name="tagId" id="tagUser">
					<option value="1">所有淘宝用户</option>
					</select>
					<input type=hidden name="teteendDate" id="teteendDateID" runat="server"  value=""/>
				</td>
			</tr>				
		</tbody>
		<tfoot>
			<tr>
				<td></td>
				<td>
                   <input type="submit" value="立刻创建营销活动" onclick="return checkValue();"/><span id="errorMsg" style="color: red"></span>
                   <input type="hidden" name="act" id="hact"  value="" />
				</td>
			</tr>
		</tfoot>
	</table>

    </div>
    <script type="text/javascript">




        document.getElementById("hact").value = "";
        function showDetail(n) {
            if (n == 1) {
                $(".itemDetail").show();
            } else {
                $(".itemDetail").hide();
            }
        }

        function showPrice(n) {
            if (n == 1) {
                $('#dz').show();
                $('#dzDecreaseNumDIV').show();
                $('#jj').hide();
                $('#jjDecreaseNumDIV').hide();

            }
            if (n == 2) {
                $('#jj').show();
                $('#jjDecreaseNumDIV').show();
                $('#dz').hide();
                $('#dzDecreaseNumDIV').hide();

            }
        }

        function manulDate(date) {
            if (date == 'start') {
                $('#startDateId').html('<input type="text" name="startDate" style="width:165px;cursor:pointer" value="2011-01-01 21:30:00"/><font color="red">&nbsp;&nbsp;&nbsp;&nbsp;日期格式必须为：2011-01-01 21:30:00</font>');
            }
            if (date == 'end') {
                $('#endDateId').html('<input type="text" name="endDate" style="width:165px;cursor:pointer" value="2011-01-01 21:30:00"/><font color="red">&nbsp;&nbsp;&nbsp;&nbsp;日期格式必须为：2011-01-01 21:30:00</font><div class="tips" style="color: red">结束时间不可超过软件到期时间。您当前软件到期时间为：2012-05-07 00:00:00</div>');
            }
        }


        function checkValue() {
 
            if (document.getElementById("actionName").value == undefined || document.getElementById("actionName").value == "") {
                alert('请输入活动名称');
                return false;
            }
            if (document.getElementById("actionName").value.length < 2 || document.getElementById("actionName").value.length > 5) {
                alert('活动名称不能超过5个字符,小于2个字符');
                return false;
            }
            if ($("input[name='itemType']:checked").val() == 'same') {
                if ($('.dzRadio').attr("checked")) {
                  

                    if ($('#dzValue').val() == '') {
                        alert('请输入折扣金额');
                        return false;
                    }

                    if (isNaN(document.getElementById("dzValue").value)) {
                        alert("折扣必须为数字");
                        return false;
                    }
                    if (document.getElementById("dzValue").value <7 ||document.getElementById("dzValue").value>=10) {
                        alert("折扣有效值范围：7.0-9.9");
                        return false;
                    }
                }
                if ($('.jjRadio').attr("checked")) {
                    if ($('#jjValue').val() == '') {
                        alert('请输入减价金额');
                        return false;
                    }

                    if (isNaN(document.getElementById("jjValue").value)) {
                        alert("减价金额必须为数字");
                        return false;
                    }
                    if (document.getElementById("jjValue").value <= 0) {
                        alert("减价金额必须大于0");
                        return false;
                    }
                }
            }
          
           
          

            var regNum = /^\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}$/;
            if (!regNum.test(document.form1.startDate.value)) {
                alert('您输入的开始时间日期格式不对，应为：2000-01-01 23:59:59。注意所有符号都是英文符号，不是中文');
                return false;
            }
            if (!regNum.test(document.form1.endDate.value)) {
                alert('您输入的结束时间日期格式不对，应为：2000-01-01 23:59:59。注意所有符号都是英文符号，不是中文');
                return false;
            }
 
         

            if (new Date($("input[name='endDate']").val().replace(/-/g, "/")).getTime() <= new Date($("input[name='startDate']").val().replace(/-/g, "/")).getTime()) {
                alert('活动结束时间必须大于活动开始时间');
                return false;
            }

            if (new Date($("input[name='endDate']").val().replace(/-/g, "/")).getTime() >= new Date(document.getElementById("teteendDateID").value.replace(/-/g, "/")).getTime()) {
                alert('活动结束时间必须小于软件到期时间');
                return false;
            }



            document.getElementById("hact").value = "post";
            $('#errorMsg').html('');
        }



    </script>
 
 
	</div>
</form>
</body>
</html>
