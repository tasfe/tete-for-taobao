<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activityshowitem.aspx.cs" Inherits="top_groupbuy_activityshowitem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/all-min.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <style type="text/css">
    #dhtmltooltip {
        font-size: 9pt;
        BORDER-RIGHT: gray 1px solid; 
        PADDING-RIGHT: 4px; 
        BORDER-TOP: gray 1px solid; 
        PADDING-LEFT: 4px; 
        Z-INDEX: 100; 
        FILTER: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=120); 
        VISIBILITY: hidden; 
        PADDING-BOTTOM: 4px; 
        BORDER-LEFT: gray 1px solid; 
        PADDING-TOP: 4px; 
        BORDER-BOTTOM: gray 1px solid; 
        POSITION: absolute; 
        BACKGROUND-COLOR: #cccccc
    }
    </style>
     
</head>
<body style="margin:0px; padding:0px;">
    <form id="form1" runat="server">
 

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

            <br />
            <div class="msg24">
				 
	            <p class="tips">提示：在促销商品管理中，可以对某一已经参加过促销活动的商品进行取消促销活动的操作。</p>
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
        	                请输入您要管理的促销宝贝名称：<input name="q"  style="width:300px;" id="querySearch" /> 
        	                <input type="button" value="查找商品" onclick="getTaobaoItem()" />
				</p>	
			</div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:2px 10px 2px 10px;">
            <tr>
            <td valign="top" style="padding-left:10px;">
            
            <input type="hidden" id="pagenow" value="1" />


   <div> <table width="600px;" border="0" cellspacing="0" cellpadding="0" style="margin:4px; padding:0px">			<tr>
						    <th style=" width:100px;"></th>
							<th  style=" width:200px;">宝贝名称</th>
							<th  style=" width:100px;">原价<em>(元)</em></th>
							<th  style=" width:200px;">操作</th>
						</tr>
                         </table>
				  
 
<div  id="taobaoitem">	
<table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0px; padding:0px" >
                      <tr >
                        <td colspan=4  >
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

        	<input type="hidden" name="itemsStrValues" id="itemsStrValues" value="" />

    </form>

        	<input type="hidden" name="itemsStr" id="itemsStr" value="" />
        	<input type="hidden" name="itemsStrTxt" id="itemsStrTxt" value="" />



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

            var queryString = "/top/groupbuy/groupbuy/taobaoitemgetactivity.aspx?act=getResultStr&isradio=0&style=" + style + "&ids=" + str + "&t=" + new Date().getTime();
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
            var queryString = "/top/groupbuy/groupbuy/taobaoitemgetactivity.aspx?act=get&isradio=0&query=" + escape(q) + "&catid=" + catid + "&p=" + pagenow + "&t=" + new Date().getTime();
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
