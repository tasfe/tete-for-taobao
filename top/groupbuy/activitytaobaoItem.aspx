﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitytaobaoItem.aspx.cs" Inherits="top_groupbuy_activitytaobaoItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加促销宝贝</title>
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 
        <link href="js/all-min.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/cal.js"></script>
      <script>



          var offsetxpoint = -60
          var offsetypoint = 20
          var ie = document.all
          var ns6 = document.getElementById && !document.all
          var enabletip = false
          if (ie || ns6)
              var tipobj = document.all ? document.all["dhtmltooltip"] : document.getElementById ? document.getElementById("dhtmltooltip") : ""
          var tipimg = document.all ? document.all["tipimg"] : document.getElementById ? document.getElementById("tipimg") : "";
          var tmpimg;

          function ietruebody() {
              return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
          }

          function ddrivetip(thetext, thecolor, thewidth, imgid, src) {
              if (ns6 || ie) {
                  if (typeof thewidth != "undefined" && thewidth != "") {
                      tipobj.style.width = thewidth + "px";
                  }
                  if (typeof thecolor != "undefined" && thecolor != "") {
                      tipobj.style.backgroundColor = thecolor;
                  }

                  if (tipimg) {
                      tipimg.src = "images/loadimg.gif";
                      tipimg.src = thetext;
                  } else {
                      tipobj.innerHTML = thetext;
                      if (typeof imgid != "undefined" && imgid != "") {
                          tmpimg = document.getElementById(imgid);
                          tmpimg.src = "images/loadimg.gif";
                          tmpimg.src = src;
                      }
                  }
                  enabletip = true
                  return false
              }
          }

          function positiontip(e) {
              if (enabletip) {
                  var curX = (ns6) ? e.pageX : event.x + ietruebody().scrollLeft;
                  var curY = (ns6) ? e.pageY : event.y + ietruebody().scrollTop;
                  //Find out how close the mouse is to the corner of the window
                  var rightedge = ie && !window.opera ? ietruebody().clientWidth - event.clientX - offsetxpoint : window.innerWidth - e.clientX - offsetxpoint - 20
                  var bottomedge = ie && !window.opera ? ietruebody().clientHeight - event.clientY - offsetypoint : window.innerHeight - e.clientY - offsetypoint - 20

                  var leftedge = (offsetxpoint < 0) ? offsetxpoint * (-1) : -1000

                  //if the horizontal distance isn't enough to accomodate the width of the context menu
                  if (rightedge < tipobj.offsetWidth)
                  //move the horizontal position of the menu to the left by it's width
                      tipobj.style.left = ie ? ietruebody().scrollLeft + event.clientX - tipobj.offsetWidth + "px" : window.pageXOffset + e.clientX - tipobj.offsetWidth + "px"
                  else if (curX < leftedge)
                      tipobj.style.left = "5px"
                  else
                  //position the horizontal position of the menu where the mouse is positioned
                      tipobj.style.left = curX + offsetxpoint + "px"

                  //same concept with the vertical position
                  if (bottomedge < tipobj.offsetHeight)
                      tipobj.style.top = ie ? ietruebody().scrollTop + event.clientY - tipobj.offsetHeight - offsetypoint + "px" : window.pageYOffset + e.clientY - tipobj.offsetHeight - offsetypoint + "px"
                  else
                      tipobj.style.top = curY + offsetypoint + "px"
                  tipobj.style.visibility = "visible"
              }
          }

          function hideddrivetip() {
              if (ns6 || ie) {
                  enabletip = false
                  tipobj.style.visibility = "hidden"
                  tipobj.style.left = "-1000px"
                  tipobj.style.backgroundColor = ''
                  tipobj.style.width = ''
              }
          }

          document.onmousemove = positiontip


          function Allcheck(bool, id) {
              var index = document.getElementById(obj).getElementsByTagName("input");
              for (var num = 0; num < index.length; num++) {
                  r[num].checked = bool;
              }
          }
        
        </script>

</head>
<body>
    <form id="form1" runat="server">
        

<div class="msg">
	<p class="alert">提示：同一商品只能参加一种促销活动，您可以删除原有活动，再添加新的促销活动。</p>
</div>
<div class="explain">
	<div style="margin-bottom: 5px;">
		<strong>当前促销活动名称：活动进行中</strong>
	</div>
	<ul>
		<li>活动时间：<%=startDate %> 到 <%=endDate%></li>
        
        <li>促销形式：<%=discountTypeStr%></li>
        <li>促销力度：<%=decreaseNumStr%></li>
		<li>优惠数量限制：<%=itemTypeStr%> </li>
        
		<li>目标客户：全淘宝网会员</li>
	</ul>
</div>

            <div class="skin-gray" style="line-height:45px; ">
				<p>
				 宝贝分类： <select id="Select1">
        	                    <option value="0"></option>
                    	        
        	                    <asp:Repeater ID="Repeater1" runat="server">
        	                        <ItemTemplate>
        	                            <option value='<%#Eval("cid") %>'><%#Eval("name") %></option>
        	                        </ItemTemplate>
        	                    </asp:Repeater>
        	                </select>
        	                宝贝名称：<input name="q"  style="width:300px;" id="querySearch" /> 
        	                <input type="button" value="查找商品" onclick="getTaobaoItem()" />
				</p>	
			</div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:2px 10px 2px 10px;">
            <tr>
            <td valign="top" style="padding-left:10px;">
            
            <input type="hidden" id="pagenow" value="1" />


    <table width="800px;" border="0" cellspacing="0" cellpadding="0" style="margin:4px; padding:0px">			
   <tr>
                         <td  width="15px"></td>
					  <td  width="85px">商品图片</td>
		                <td  width="140px">名称</td>
                        <td  width="70px">原价(元)</td>
		                <td  width="70px">促销价(元)</td>
                        <td  width="70px">优惠类型</td>
                        <td  width="70px">优惠幅度</td>
                        <td  width="70px">优惠数量</td>
		                <td  width="70px">参团人数</td>
		                <td  width="140px"> 操作</td>
						</tr>
                       
                         </table>
				  <hr/>
 
<div  id="taobaoitem">	
<table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0px; padding:0px" >
                      <tr >
                        <td colspan=10  >
                            <asp:Repeater ID="rptItems" runat="server">
                                <ItemTemplate>
        	                        <input name="items" id='item_<%#Eval("NumIid") %>' type="checkbox" value='<%#Eval("NumIid") %>' title='<%#Eval("title") %>' onclick="SetInitArea(this)" /> <label for='item_<%#Eval("NumIid") %>'><%#Eval("title") %></label> <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                      </tr>
    </table>
</div>
        	    
            </td>
          </tr>
     
    </table>

        	<input type="hidden" name="itemsStrValues" id="Hidden1" value="" />

    </form>

    <input type="hidden" name="itemsStr" id="itemsStr" value="" />
        	<input type="hidden" name="itemsStrTxt" id="itemsStrTxt" value="" />

            	<input type="hidden" name="itemsStrValues" id="itemsStrValues" value="" />

                <script type="text/javascript">
                    function changeSelect(iid) {

                        var oldPrice = $('#price' + iid).val();
                        if ($('#discountType' + iid).val() == 'DISCOUNT') {
                            var zhe = $('#changeZhe' + iid).val();
                            if (zhe == '') zhe = 10;
                            var newPrice = formatnumber(oldPrice * zhe * 0.1, 2);
                            $('#newPrice' + iid).html(newPrice);
                            $('#zheDiv' + iid).show();
                            $('#jianDiv' + iid).hide();
                            $('#duojian' + iid).show();
                            $('#yijian' + iid).hide();
                        }
                        else {
                            var jian = $('#changeJian' + iid).val();
                            if (jian == '') jian = 0;
                            var newPrice = formatnumber(oldPrice - jian, 2);
                            $('#newPrice' + iid).html(newPrice);
                            $('#zheDiv' + iid).hide();
                            $('#jianDiv' + iid).show();
                            $('#duojian' + iid).hide();
                            $('#yijian' + iid).show();
                        }

                    }

                    function blurValue(iid) {
                        var oldPrice = $('#oldPrice' + iid).text();
                        if ($('#discountType' + iid).val() == 'DISCOUNT') {
                            var zhe = $('#changeZhe' + iid).val();
                            if (zhe == '') zhe = 10;
                            var newPrice = formatnumber(oldPrice * zhe * 0.1, 2);
                            $('#newPrice' + iid).html(newPrice);
                        } else {
                            var jian = $('#changeJian' + iid).val();
                            if (jian == '') jian = 0;
                            var newPrice = formatnumber(oldPrice - jian, 2);
                            $('#newPrice' + iid).html(newPrice);
                        }
                    } 
                </script>

    <script language="javascript" type="text/javascript">
        function InitArea(obj) {
            getResultStr(obj.value);
        }

        //保存选中商品
        function SetInitArea(obj) {
            if (document.getElementById("itemsStrValues").value.indexOf(obj.value) == -1) {
                document.getElementById("itemsStrValues").value = document.getElementById("itemsStrValues").value + "," + obj.value;
            }
        }

        //提交返回选中商品
        function InitAreaAll() {
            getResultStr(document.getElementById("itemsStrValues").value);
        }


        function CloseWindow(str) {
            if (navigator.appVersion.indexOf("MSIE") == -1) {
                window.opener.returnAction(str);
                window.close();
            } else {
                window.returnValue = str;
                window.close();
            }
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

        function updateCat() {
            createxmlHttpRequest();
            var queryString = "/top/groupbuy/groupbuy/taobaoitemgetactivity.aspx?act=getCat&t=" + new Date().getTime();
            xmlHttp.open("GET", queryString);
            xmlHttp.onreadystatechange = handleStateChangeCat;
            xmlHttp.send(null);
        }

        function handleStateChangeCat() {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                var catHTML = document.getElementById("catHTML");
                catHTML.innerHTML = decodeURI(xmlHttp.responseText);
            }
        }

        function getResultStr(str) {
            createxmlHttpRequest();

            //获取当前使用样式
            var style = "0";

            var queryString = "/top/groupbuy/groupbuy/taobaoitemgetactivity.aspx?act=getactivityitem&isradio=0&style=" + style + "&ids=" + str + "&t=" + new Date().getTime();
            xmlHttp.open("GET", queryString);
            xmlHttp.onreadystatechange = handleStateChangeResultStr;
            xmlHttp.send(null);
        }

        function handleStateChangeResultStr() {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                CloseWindow(xmlHttp.responseText);
            }
        }

        function spreadStat(pageid) {

            var q = document.getElementById("querySearch").value;
            var catObj = document.getElementById("Select1");
            var catid = catObj.options[catObj.options.selectedIndex].value;

            var pagenow = pageid;
            createxmlHttpRequest();
            var queryString = "/top/groupbuy/groupbuy/taobaoitemgetactivity.aspx?act=getactivityitem&isradio=0&query=" + escape(q) + "&catid=" + catid + "&p=" + pagenow + "&t=" + new Date().getTime();
            xmlHttp.open("GET", queryString);
            xmlHttp.onreadystatechange = handleStateChange;
            xmlHttp.send(null);
        }

        function handleStateChange() {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                var taobaoitem = document.getElementById("taobaoitem")
                taobaoitem.innerHTML = decodeURI(xmlHttp.responseText);

                formatCheckBoxAll();
            }
        }

        function getTaobaoItem() {

            spreadStat(1);
        }

        itemsStr = "0";
        itemsStrTxt = "0";

        /*function InitArea(obj){
        var tmpItemsStr = "," + itemsStr + ",";
        
        if(obj.checked == true){
        if(tmpItemsStr.indexOf("," + obj.value + ",") == -1){
        itemsStr = itemsStr + "," + obj.value;
        itemsStrTxt = itemsStrTxt + "," + obj.title;
        }
        }else{
        itemsStr = itemsStr.replace("," + obj.value, "");
        itemsStrTxt = itemsStrTxt.replace("," + obj.title, "");
        }
    
        formatHTML();
        }*/

        function saveHidden() {
            document.getElementById("itemsStr").value = itemsStr;
            document.getElementById("itemsStrTxt").value = itemsStrTxt;
        }

        function formatHTML() {
            var selectArea = document.getElementById("selectArea");
            var arr1 = itemsStr.split(',');
            var arr2 = itemsStrTxt.split(',');
            var tmp = "";

            for (var i = 1; i < arr1.length; i++) {
                tmp += arr2[i] + " <a color=red href=# onclick=\"del('" + arr1[i] + "',this)\" title=\"" + arr2[i] + "\">删</a><br>";
            }

            selectArea.innerHTML = tmp;

            saveHidden();
        }


        function formatCheckBoxAll() {
            var items = document.getElementsByName("items");
            var arr1 = itemsStr.split(',');
            for (var i = 1; i < arr1.length; i++) {
                for (var j = 0; j < items.length; j++) {
                    if (items[j].value == arr1[i]) {
                        items[j].checked = true;
                    }
                }
            }
        }

        function formatCheckBox(id) {
            var items = document.getElementsByName("items");

            for (var i = 0; i < items.length; i++) {
                if (items[i].value == id) {
                    items[i].checked = false;
                }
            }
        }

        function del(id, obj) {
            itemsStr = itemsStr.replace("," + id, "");
            itemsStrTxt = itemsStrTxt.replace("," + obj.title, "");

            formatCheckBox(id);

            formatHTML();
        }

        function showhidden(i) {
            if (i == "1") {
                document.getElementById("panel1").style.display = "none";
                document.getElementById("panel2").style.display = "block";
                document.getElementById("tuijian1").style.display = "none";
                document.getElementById("tuijian2").style.display = "block";
            } else {
                document.getElementById("panel1").style.display = "block";
                document.getElementById("panel2").style.display = "none";
                document.getElementById("tuijian1").style.display = "block";
                document.getElementById("tuijian2").style.display = "none";
            }
        }

        function InitItemsStr() {
            if (document.getElementById("style2").checked == true) {
                showhidden(1);
            }

            itemsStr = document.getElementById("itemsStr").value;
            if (itemsStr == '')
                return;
            itemsStrTxt = document.getElementById("itemsStrTxt").value;

            formatHTML();
            formatCheckBoxAll();
        }

        function checkSelect(nam, val) {
            var obj = document.getElementsByName(nam);
            for (i = 0; i < obj.length; i++) {
                if (obj[i].value == val) {
                    obj[i].checked = true;
                }
            }
        }


        function checkMuti(nam, val) {
            var obj = document.getElementsByName(nam);
            val = "," + val + ",";
            for (i = 0; i < obj.length; i++) {
                if (val.indexOf("," + obj[i].value + ",") != -1) {
                    obj[i].checked = true;
                }
            }
        }

        function checkSelectDrop(nam, val) {
            var obj = document.getElementById(nam);
            for (i = 0; i < obj.options.length; i++) {
                if (obj.options[i].value == val) {
                    obj.options[i].selected = true;
                }
            }
        }

        spreadStat(1);
</script>
</body>
</html>
