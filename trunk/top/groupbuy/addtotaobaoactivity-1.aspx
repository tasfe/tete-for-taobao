<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addtotaobaoactivity-1.aspx.cs" Inherits="top_groupbuy_addtotaobaoactivity_1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;" onload="InitItemsStr()">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 同步到宝贝描述 (1.选择宝贝) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
          <!--<p class="help"><a href="http://service.taobao.com/support/help.htm" target="_blank">查看帮助</a></p>-->
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content">
  	<img src="images/title.gif" /><br />
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0 10px;">
    <form id="myform" action="<%=url %>" method="post">
        <input name="style" value="<%=style %>" type="hidden" />
        <input name="size" value="<%=size %>" type="hidden" />
        <input name="name" value="<%=name %>" type="hidden" />
      <tr>
        <td width="130" height="30">请选择您要推广商品：</td>
        <td align="left">
        	<input name="type" id="style0" type="radio" value="2" checked="checked" onclick="showhidden(2)" /><label for="style1" onclick="showhidden(2)">全部宝贝</label>
        	<input name="type" id="style1" type="radio" value="0" onclick="showhidden(0)" /><label for="style1" onclick="showhidden(0)">按分类选择</label>
        	<input name="type" id="style2" type="radio" value="1" onclick="showhidden(1)" /><label for="style2" onclick="showhidden(1)">手动选择</label>
            <span id="tuijian1" style="color:#ccc"></span>
            <span id="tuijian2" style="color:#ccc; display:none;"></span>
        </td>
      </tr>
      <tr id="panel3" style="display:block;">
        <td colspan=2></td>
      </tr>
      <tr id="panel1" style="display:none;">
         <td colspan=2>
         
        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0px; padding:0px">
          <tr>
      	    <td height="30" width="130">商品所在店铺分类：</td>
            <td>
                <div id="catHTML" style="float:left">
        	    <select id="shopcat" name="shopcat">
        	        <option value="0"></option>
        	        
        	        <asp:Repeater ID="rptShopCat" runat="server">
        	            <ItemTemplate>
        	                <option value='<%#Eval("cid") %>'><%#Eval("name") %></option>
        	            </ItemTemplate>
        	        </asp:Repeater>
        	    </select>
        	    </div>
        	    &nbsp;
        	    <div style="float:left; margin-left:10px;">
        	        <input type="button" value="更新分类" onclick="updateCat()" />
        	    </div>
            </td>
          </tr>
          </table>	 
      
       </td>
       </tr>
      
        <tr id="panel2" style="display:none">
         <td height="300" valign="top">
            下面为您选中的商品：
            <div id="selectArea" style="height:300px; width:120px; border:solid 1px; overflow:scroll;">
                
            </div>
         </td>
            <td valign="top" style="padding-left:10px;">
            
            <input type="hidden" id="pagenow" value="1" />
        	    
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0px; padding:0px">
        	        <tr>
                        <td colspan=2>
                            <select id="Select1">
        	                    <option value="0"></option>
                    	        
        	                    <asp:Repeater ID="Repeater1" runat="server">
        	                        <ItemTemplate>
        	                            <option value='<%#Eval("cid") %>'><%#Eval("name") %></option>
        	                        </ItemTemplate>
        	                    </asp:Repeater>
        	                </select>
        	                <input name="q" id="querySearch" /> 
        	                <input type="button" value="搜索" onclick="getTaobaoItem()" />
                        </td>
                      </tr>
                      <tr>
                        <td colspan=2 id="taobaoitem">
                            <asp:Repeater ID="rptItems" runat="server">
                                <ItemTemplate>
        	                        <input name="items" id='item_<%#Eval("NumIid") %>' type="checkbox" value='<%#Eval("NumIid") %>' title='<%#Eval("title") %>' onclick="InitArea(this)" /> <label for='item_<%#Eval("NumIid") %>'><%#Eval("title") %></label> <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                      </tr>
        	    </table>
        	    
            </td>
          </tr>

          <tr>
            <td height="30">选择关联团购活动：</td>
            <td>
                <select id="myads" name="myads">
        	        <asp:Repeater ID="rptAds" runat="server">
                        <ItemTemplate>
                             <option value='<%#Eval("id") %>'><%#Eval("Name")%></option>
                        </ItemTemplate>
                    </asp:Repeater>
        	    </select>
            </td>
          </tr>
          <tr>
            <td height="30">选择关联活动模板：</td>
            <td>
                <select id="myadstemp" name="myadstemp">
        	        <asp:Repeater ID="rptAdstemp" runat="server">
                        <ItemTemplate>
                             <option value='<%#Eval("id") %>'><%#Eval("title")%></option>
                        </ItemTemplate>
                    </asp:Repeater>
        	    </select>
            </td>
          </tr>
      
      <tr>
        <td>
        	<input type="button" value="上一步" onclick="history.go(-1)" />
        	<input type="submit" value="下一步" onclick="return checkdata()" />
        	
        	<input type="hidden" name="itemsStr" id="itemsStr" value="<%=items %>" />
        	<input type="hidden" name="itemsStrTxt" id="itemsStrTxt" value="<%=itemsStr %>" />
        </td>
        <td></td>
      </tr>
    </form>
    </table>

  </div>
</div>
<script language="javascript" type="text/javascript">
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
        var queryString = "http://groupbuy.7fshop.com/top/groupbuy/taobaoitemgetactivity.aspx?act=getCat&t=" + new Date().getTime();
        xmlHttp.open("GET", queryString);
        xmlHttp.onreadystatechange = handleStateChangeCat;
        xmlHttp.send(null);
    }

    function checkdata() {
        var id = document.getElementById("myads").value;
        if (id == "") {
            alert("请选择团购活动");
            return false;
        }

        document.getElementById("myform").action = "addtotaobaoactivity-2.aspx?id=" + id;

        return true;
    }

    function handleStateChangeCat() {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
            var catHTML = document.getElementById("catHTML");
            catHTML.innerHTML = decodeURI(xmlHttp.responseText);
        }
    }

    function spreadStat(pageid) {
        var q = document.getElementById("querySearch").value;
        var catObj = document.getElementById("Select1");
        var catid = catObj.options[catObj.options.selectedIndex].value;

        //绑定团购ID
        var groupbuyid = '<%=id %>';
        document.getElementById("myads").value = groupbuyid;

        var pagenow = pageid;
        createxmlHttpRequest();
        var queryString = "http://groupbuy.7fshop.com/top/groupbuy/taobaoitem.aspx?act=get&query=" + escape(q) + "&catid=" + catid + "&p=" + pagenow + "&t=" + new Date().getTime();
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

    function InitArea(obj) {
        var tmpItemsStr = "," + itemsStr + ",";

        if (obj.checked == true) {
            if (tmpItemsStr.indexOf("," + obj.value + ",") == -1) {
                itemsStr = itemsStr + "," + obj.value;
                itemsStrTxt = itemsStrTxt + "," + obj.title;
            }
        } else {
            itemsStr = itemsStr.replace("," + obj.value, "");
            itemsStrTxt = itemsStrTxt.replace("," + obj.title, "");
        }

        formatHTML();
    }

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
            document.getElementById("panel3").style.display = "none";
            document.getElementById("tuijian1").style.display = "none";
            document.getElementById("tuijian2").style.display = "block";
        } else if (i == "2") {
            document.getElementById("panel1").style.display = "none";
            document.getElementById("panel2").style.display = "none";
            document.getElementById("panel3").style.display = "block";
            document.getElementById("tuijian1").style.display = "none";
            document.getElementById("tuijian2").style.display = "none";
        } else {
            document.getElementById("panel1").style.display = "block";
            document.getElementById("panel2").style.display = "none";
            document.getElementById("panel3").style.display = "none";
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

    checkSelect("type", "<%=type %>");
    showhidden(2);
    //checkMuti("items", "<%=items %>")
    checkSelect("orderby", "<%=orderby %>");
    checkSelectDrop("shopcat", "<%=shopcat %>");
    spreadStat(1);
</script>


<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>
