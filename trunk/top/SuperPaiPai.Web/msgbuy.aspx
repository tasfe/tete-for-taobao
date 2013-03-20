<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgbuy.aspx.cs" Inherits="msgbuy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>拍拍店长</title>
<script type="text/javascript" src="js/common.js"></script>

	<style type="text/css"> 
	#lockDiv {
		    position:absolute; top:0; left:0;
			width:1000px;
			height:800px;
			background-color:#777777;
			z-index:2; 
			display:none;
		    opacity: 0.70;
		    filter : progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=40,finishOpacity=100);
		    width:expression(documentElement.clientWidth < 900?(documentElement.clientWidth==0?(body.clientWidth<900?'900':'auto'):'900px'):'auto');
	    }  
	    
	    
	    #tip{
		    position:absolute;
		    background-color:#FFFFFF;
		    z-index:3;
		    display:none;
	    }
  	   .layer_2{width:410px; border:3px #000 solid; background:#fff; padding:5px;position:absolute;z-index:3;display:none;filter:alpha(opacity=100);opacity:1;}
</style>

</head>

<body>
<script type="text/javascript" src="js/nav.js"></script>
<div class="clear_height"></div>
<div class="main">
	<!-- left start -->
	<script id="menujs" type="text/javascript" src="js/setMenu.js?id=28&key1=msgbuy"></script>
	
	<!-- left end -->
  
  <!-- right start -->
  <div class="main_right">
  <div class="kuang">
				<div class="title_list">
					<div class="title_left">短信充值</div>
				</div>
				
			<div class="contents tag_list">
				
				<table width="100%" border="0" cellspacing="6" cellpadding="0">
					<tbody>
			            <tr>
			                <td width="100px" align="right" style="text-align: right;">当前用户:</td>
			                <td  style="text-align: left;">
			                    <span ><%=Nick %></span>
			                </td>     
			            </tr>
			            
			    
			            
			             <tr>
			                <td width="100px" align="right" style="text-align: right;">短信充值数:</td>
			                <td  style="text-align: left;">
					            <select onchange="smschange(this.value);" id="smsSelect">
							    	<option value=0 >500</option>
			    					<option value=1 >1000</option>
			    					<option value=2 selected="selected">2000</option>
			    					<option value=3 >5000</option>
			    					<option value=4 >10000</option>
			    					<option value=5 >20000</option>
							    </select>
							    条
			                </td>     
			            </tr>   
			
			             <tr>
			                <td width="100px" align="right" style="text-align: right;">金额:</td>
			                <td  style="text-align: left;">
			                    <span style="color:red;font-size:18px"  id="amount">0</span>元
			                </td>     
			            </tr>   
			            
			          <tr>
			                <td width="100px" align="right" style="text-align: right;">节省:</td>
			                <td  style="text-align: left;">
			                    <span style="color:red;font-size:15px" id="saveamount">0</span>元
			                </td>     
			            </tr>       

			            
			             
			            <tr>
			                <td style="text-align: center" colspan="2">
			                    <div class="clear_height"></div>
			                          		<input type="hidden" name=price id="price" value="0.06">
			                    <input type="submit" value="  马上充值  " class="btn-middle middle-blue" onclick="recharge();">
			                </td>
			            </tr>
		        	</tbody>
				</table>					
		</div>
		
		
   <div id="lockDiv"></div>
  <div id="tip" class="layer_2" style="width:310px" onmousedown="MouseDown(this)" onmousemove="MouseMove()" onmouseup="MouseUp()">
	 <table width="100%" border="0" cellspacing="0" cellpadding="0" height="25" bgcolor="#125ebd">
		<tr>
			<td style="padding-left:5px; color:#fff" class="titleDiv">如果已付款成功，点击下面按钮可自动开通生效</td>
			<td width="20" style="padding-right:2px;"><img src="images/close.gif" alt="关闭" onclick="cancelOpera('tip');"/></td>
		</tr>
	 </table>
	 <table width="100%" border="0" cellspacing="5" cellpadding="0" style="margin-top:10px;">
		<tr>
			<td align="center">
				<input type="button" value=" 确认已付款 " class="btn-middle middle-blue"  onclick="ok();"/>
			</td>
		 </tr>
	</table>
  </div>

</div>
</div>


<script language="javascript">
    var Obj;
    var isIE = document.all ? true : false;

    function MouseDown(obj) {
        var e = e ? e : event;
        var className = e.srcElement.className;
        if (className != "titleDiv") {
            return;
        }
        Obj = obj;
        if (isIE) {
            Obj.setCapture();
            Obj.filters.alpha.opacity = 70;
        } else {
            window.captureEvents(Event.MOUSEMOVE);
            Obj.style.opacity = 0.7;
        }
        Obj.l = event.x - Obj.style.pixelLeft;
        Obj.t = event.y - Obj.style.pixelTop;
    }
    function MouseMove() {
        if (Obj != null) {
            Obj.style.left = event.x - Obj.l;
            Obj.style.top = event.y - Obj.t;
        }
    }

    function MouseUp() {
        if (Obj != null) {
            if (isIE) {
                Obj.releaseCapture();
                Obj.filters.alpha.opacity = 100;
            } else {
                window.releaseEvents(Obj.MOUSEMOVE);
                Obj.style.opacity = 1;
            }
            Obj = null;
        }
    }


    function cancelOpera(id) {
        document.getElementById(id).style.display = "none";
        document.getElementById("lockDiv").style.display = "none";
    }

    function setLockDivAttr(doc, objWindow, width, height) {

        doc.getElementById("lockDiv").style.display = "block";
        var tWidth = doc.body.scrollWidth;
        var tHeight = doc.body.scrollHeight;
        var lockObj = doc.getElementById("lockDiv");
        if (doc.body.scrollWidth < doc.body.clientWidth) {
            tWidth = doc.body.clientWidth;
        }
        if (doc.body.scrollHeight < doc.body.clientHeight) {
            tHeight = doc.body.clientHeight;
        }
        lockObj.style.width = tWidth + "px";
        lockObj.style.height = tHeight + "px";
        lockObj.style.minWidth = tWidth + "px";
        lockObj.style.minHeight = tHeight + "px";
        // 将弹出层垂直居中
        var objWidth = 330;
        var objHeight = 160;
        if (width != null) {
            objWidth = width;
        }
        if (height != null) {
            objHeight = height;
        }

        objWindow.style.left = (tWidth - objWidth) / 2 + "px";
        objWindow.style.top = ((window.screen.availHeight - objHeight) / 2 + doc.documentElement.scrollTop - 20) + "px";
        objWindow.style.display = "block";
    }


    function sleep(n) {
        var start = new Date().getTime(); //定义起始时间的毫秒数
        while (true) {
            var time = new Date().getTime(); //每次执行循环取得一次当前时间的毫秒数
            if (time - start > n) {//如果当前时间的毫秒数减去起始时间的毫秒数大于给定的毫秒数，即结束循环
                break;
            }
        }
    }





    function getTextValue(obj) {
        if (navigator.appName.indexOf("Explorer") > -1) {
            return obj.innerText;
        } else {
            return obj.textContent;
        }
    }

    function setTextValue(obj, value) {
        if (navigator.appName.indexOf("Explorer") > -1) {
            obj.innerText = value;
        } else {
            obj.textContent = value;
        }
    }

    //var rechargeCount=new Array(500,1000,2000,4000);
    //var totalCost=new Array(35,65,120,232);
    //var price=new Array(0.07,0.065,0.06,0.058);
    //var saveCost=new Array(0,5,20,48);


    var rechargeCount = new Array(500, 1000, 2000, 5000, 10000, 20000);
    var totalCost = new Array(0.01, 90, 170, 375, 650, 1240);
    var price = new Array(0.1, 0.09, 0.085, 0.075, 0.065, 0.062);
    var saveCost = new Array(0, 10, 30, 125, 350, 760);



    function smschange(obj) {
        setTextValue(document.getElementById("amount"), totalCost[obj]);
        setTextValue(document.getElementById("saveamount"), saveCost[obj]);
    }

    function recharge() {
        var smstype = document.getElementById("smsSelect").value;
        window.open("goToTenpay.aspx?smstype=" + smstype + "&rechargeCount=" + rechargeCount[smstype] + "&totalCost=" + totalCost[smstype] + "&price=" + price[smstype]);
        setLockDivAttr(document, document.getElementById('tip'), 330, 160);

    }


    function ok() {
        window.location = "huiyuan.aspx";
    }
</script>

<script type="text/javascript">
    smschange(document.getElementById("smsSelect").value);
</script>

<script type="text/javascript" src="js/default.js"></script>
<script type="text/javascript" src="js/footer.js"></script>
</body>
</html>
