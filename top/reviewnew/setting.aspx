<%@ Page Language="C#" AutoEventWireup="true" CodeFile="setting.aspx.cs" Inherits="top_review_setting" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
</style>

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 基本设置 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
    优惠券、支付宝红包、包邮卡可以只选一种赠送，没有购买淘宝优惠券的卖家可以使用支付宝红包和包邮卡赠送给客户。
</div>


        <table width="700">
            <tr>
                <td align="left" width="180" height="30">是否开启系统自动赠送优惠券：</td>
                <td>
                    开启<input name="iscoupon" id="coupon1" type="radio" value="1" <%=check(iscoupon, "1") %> onclick="showArea(1)" />
                    不开启<input name="iscoupon" id="coupon2" type="radio" value="0" <%=check(iscoupon, "0") %> onclick="showArea(0)" />
                </td>
            </tr>
            <tr id="couponArea">
                <td align="left" height="30">选择赠送的优惠券：</td>
                <td>
                    <%=couponstr%>
                    <a href="couponadd.aspx">创建优惠券</a>
                    <a href="couponsend.aspx">查看优惠券赠送记录</a>
                </td>
            </tr>
            <tr>
                <td align="left" width="180" height="30">是否开启系统自动送支付宝红包：</td>
                <td>
                    开启<input name="isalipay" id="alipay1" type="radio" value="1" <%=check(isalipay, "1") %> onclick="showAreaAli(1)" />
                    不开启<input name="isalipay" id="alipay2" type="radio" value="0" <%=check(isalipay, "0") %> onclick="showAreaAli(0)" />
                </td>
            </tr>
            <tr id="aliArea">
                <td align="left" height="30">选择赠送的支付宝红包：</td>
                <td>
                    <%=alipaystr%>
                    <a href="alipayadd.aspx">导入支付宝红包</a>
                    <a href="alipay.aspx">查看赠送记录</a>
                </td>
            </tr>

            
            <tr>
                <td align="left" width="180" height="30">是否开启系统自动送包邮卡：</td>
                <td>
                    开启<input name="isfree" id="free1" type="radio" value="1" <%=check(isfree, "1") %> onclick="showAreaFree(1)" />
                    不开启<input name="isfree" id="free2" type="radio" value="0" <%=check(isfree, "0") %> onclick="showAreaFree(0)" />
                </td>
            </tr>
            <tr id="freeArea">
                <td align="left" height="30">选择赠送的包邮卡：</td>
                <td>
                    <%=freestr%>
                    <a href="../freecard/freecardlist.aspx">创建包邮卡</a>
                    <a href="../freecard/freecardcustomer.aspx">查看赠送记录</a>
                </td>
            </tr>

            <tr>
                <td align="left" height="30">淘宝提供物流跟踪订单最短评价时间：</td>
                <td>
                    <input id="mindate" name="mindate" type="text" value="<%=mindate %>" size="2" /> 天 
                    （物流状态在淘宝可追踪，签收后几天内给与好评赠送，过期不送。）

                </td>
            </tr>
            <tr>
                <td align="left" height="30">淘宝不提供物流跟踪最短评价时间：</td>
                <td>
                    <input id="maxdate" name="maxdate" type="text" value="<%=maxdate %>" size="2" /> 天 
                    （物流状态不可以追踪，发货后几天内好评赠送，过期不送。）
                </td>
            </tr>
            <tr>
                <td align="left" height="30">默认好评是否赠送：</td>
                <td>
                    赠送<input name="iscancelauto" type="radio" value="0" <%=check(iscancelauto, "0") %> onclick="showCancel(0)" />
                    不赠送<input name="iscancelauto" type="radio" value="1" <%=check(iscancelauto, "1") %> onclick="showCancel(1)" />
                    |
                    <span id="cancelArea">
                    <input name="cancel1" id="cancel1" type="checkbox" value="1" <%=check(cancel1, "1") %> /><label for="cancel1">好评！</label>
                    <input name="cancel2" id="cancel2" type="checkbox" value="1" <%=check(cancel2, "1") %> /><label for="cancel2">评价方未及时做出评价,系统默认好评！</label>
                    </span>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">好评自动判定：</td>
                <td>
                    开启<input name="iskeyword" type="radio" value="1" <%=check(iskeyword, "1") %> />
                    不开启<input name="iskeyword" type="radio" value="0" <%=check(iskeyword, "0") %> />
                    <a href="keyword.aspx">设置好评自动判定规则</a>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">是否开启评价审核：</td>
                <td>
                    开启<input name="iskefu" id="kefu1" type="radio" value="1" <%=check(iskefu, "1") %> />
                    不开启<input name="iskefu" id="kefu2" type="radio" value="0" <%=check(iskefu, "0") %> />
                    <a href="kefulist.aspx">查看待审核列表</a>
                </td>
            </tr>
            
            <tr style="display:none">
                <td align="left" height="30">店铺销售产品是否为虚拟商品：</td>
                <td>
                    <input id="isxuni" name="isxuni" type="checkbox" value="1" <%=check(isxuni, "1") %> onclick="showXuniObj(this)" />
                    （销售虚拟商品的店铺请勾选此项）
                </td>
            </tr>
            <tr id="xuniArea">
                <td align="left" height="30">虚拟商品最短评价时间：</td>
                <td>
                    <input id="xunidate" name="xunidate" type="text" value="<%=xunidate %>" size="2" /> 分钟
                    （虚拟商品按照买家付款成功算起，多少分钟内不确认就发送催评短信）
                </td>
            </tr>
            
            <tr>
                <td align="left" height="30">是否开启指定商品赠送：</td>
                <td>
                    <input id="isitem" name="isitem" type="checkbox" value="1" <%=check(isitem, "1") %> onclick="showItemObj(this)" />
                </td>
            </tr>
            <tr id="ItemArea">
                <td align="left" height="30">订单包含指定商品才赠送：</td>
                <td>
                    <input id="itemlist" name="itemlist" type="hidden" value="<%=itemlist %>" />
                    <input type="button" value="选择商品" onclick="OpenDialogLable('dialogProduct.aspx',650,560);"/>
                    <div id="productArea"></div>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存设置" OnClientClick="return check()" />
                    <input type="button" value="将活动展示在店铺里" onclick="window.location.href='html.aspx'" />
                    <input type="button" value="礼品黑名单" onclick="window.location.href='blacklistgift.aspx'" />
                </td>
            </tr>
        </table>
    </div>
</div>
</form>
<script language="javascript" type="text/javascript">
    function OpenDialogLable(url, w, h, editTxt) {
        if (typeof (editTxt) == "undefined") {
            editTxt = "";
        }
        alert(strResult);
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
</script>

<script language="javascript" type="text/javascript">
    function showArea(str) {
        if (str == 0) { 
            document.getElementById("couponArea").style.display = "none";
        }else if(str == 1){
            document.getElementById("couponArea").style.display = "block";
        }
    }

    function showAreaAli(str) {
        if (str == 0) { 
            document.getElementById("aliArea").style.display = "none";
        }else if(str == 1){
            document.getElementById("aliArea").style.display = "block";
        }
    }

    function showAreaFree(str) {
        if (str == 0) { 
            document.getElementById("freeArea").style.display = "none";
        }else if(str == 1){
            document.getElementById("freeArea").style.display = "block";
        }
    }

    function showArea1(str) {
        if (str == 0) { 
            document.getElementById("itemArea").style.display = "none";
        }else if(str == 1){
            document.getElementById("itemArea").style.display = "block";
        }
    }

    function check() {
        if (document.getElementById("mindate").value == "") {
            alert("请填写可查询物流评价时间");
            document.getElementById("mindate").focus();
            return false;
        }
        
        if (document.getElementById("maxdate").value == "") {
            alert("不可查询物流评价时间");
            document.getElementById("maxdate").focus();
            return false;
        }
        return checkConfirm();
    }

    function checkConfirm(){
        if(document.getElementById("coupon2").checked == true){
            return confirm('您选择了不自动赠送优惠券，系统将无法自动帮您赠送优惠券，您确定吗？');
        }

        if(document.getElementById("kefu1").checked == true){
            return confirm('您开启了评价客服审核，所有的评价都需要您手动进入我们服务审核才会自动赠送优惠券，您确定吗？');
        }

        return true;
    }

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

    var xmlHttp;
    var itemsStr;
    var itemsStrTxt;
    function createxmlHttpRequest() {
        if (window.ActiveXObject)
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        else if (window.XMLHttpRequest)
            xmlHttp = new XMLHttpRequest();
    }

    function getResultStr(str) {
        createxmlHttpRequest();

        //获取当前使用样式
        var style = "0";

        var queryString = "/top/reviewnew/taobaoitem.aspx?act=getResultStr&isradio=1&style=" + style + "&ids=" + str + "&t=" + new Date().getTime();
        xmlHttp.open("GET", queryString);
        xmlHttp.onreadystatechange = handleStateChangeResultStr;
        xmlHttp.send(null);
    }

    function handleStateChangeResultStr() {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
            document.getElementById("productArea").innerHTML = xmlHttp.responseText;
        }
    }

    /* if ("<%=itemid %>" != "") {
        getResultStr("<%=itemid %>");
    }*/
    
    showArea(<%=iscoupon %>);
    showAreaAli(<%=isalipay %>);
    showAreaFree(<%=isfree %>);

    showCancel(<%=iscancelauto %>)
    showXuni(<%=isxuni %>)
    showItem(<%=isitem %>)

    function showXuniObj(obj) {
        if(obj.checked == false)
        {
            document.getElementById("xuniArea").style.display = "none";
        }
        else
        {
            document.getElementById("xuniArea").style.display = "";
        }
    }
    
    function showItemObj(obj) {
        if(obj.checked == false)
        {
            document.getElementById("itemArea").style.display = "none";
        }
        else
        {
            document.getElementById("itemArea").style.display = "";
        }
    }
    
    function showXuni(flag) {
        if(flag == "0")
        {
            document.getElementById("xuniArea").style.display = "none";
        }
        else
        {
            document.getElementById("xuniArea").style.display = "";
        }
    }
    function showItem(flag) {
        if(flag == "0")
        {
            document.getElementById("itemArea").style.display = "none";
        }
        else
        {
            document.getElementById("itemArea").style.display = "";
        }
    }

    function showCancel(flag) {
        if(flag == "0")
        {
            document.getElementById("cancelArea").style.display = "none";
        }
        else
        {
            document.getElementById("cancelArea").style.display = "";
        }
    }

    function showAreaPhone() {
        document.getElementById("div1").style.display = "block";
        document.getElementById("div1").style.filter = "alpha(opacity=50)";
        document.getElementById("div2").style.display = "block";

        setTimeout('closeArea11()', 60000);
    }

    function closeArea11() {
        document.getElementById("div1").style.display = "none";
        document.getElementById("div2").style.display = "none";
    }

    function savephone() {
        var phone = document.getElementById("phone").value;
        var qq = document.getElementById("qq").value;

        var reg = /^0{0,1}(13[0-9]|14[0-9]|15[0-9]|18[0-9])[0-9]{8}$/;
        if (!reg.test(phone)) {
            alert('请输入正确的手机号码，谢谢您的合作！');
            document.getElementById('phone').focus();
            return false;
        }

        if (isNaN(qq)){
            alert('请输入正确的QQ号码，谢谢您的合作！');
            document.getElementById('qq').focus();
            return false;
        }

        var url = "http://haoping.7fshop.com/top/reviewnew/setting.aspx?act=savephone&phone=" + phone +"&qq=" + qq;

        createxmlHttpRequest();
        var queryString = url + "&t=" + new Date().getTime();
        xmlHttp.open("GET", queryString);
        xmlHttp.onreadystatechange = handleStateChangeCat;
        xmlHttp.send(null);
    }

    function handleStateChangeCat() {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
            alert('保存成功，谢谢您的合作，请联系客服人员验证联系方式并获取短信奖励！');
            closeArea11();
        }
    }
</script>


<div id="div1" style="position: absolute; display:none; top: 0pt; left: 0pt; 

background: none repeat scroll 0% 0% rgb(153, 153, 153); opacity: 0.4; width: 100%; height: 

2184px; z-index: 10002;"></div>
<div id="div2" style="display:none; position: absolute; display:none; top: 50px; left: 

100px;  z-index: 10003;">
    <a href='javascript:closeArea11();' style="font-size:16px; font-weight:bold; color:white">关闭窗口</a> 60秒后窗口自动关闭.. <br>
    <div style="BACKGROUND-COLOR:#fff; width:350px; height:120px;  padding:10px;">
    <span style="font-size:14px; font-weight:bold;">订购服务后，很多卖家因主旺旺跳转子帐号，因设置错误导致功能无法正常使用，客服联系不上！</span><br />
    <%if (versionpub == "2" || versionpub == "3")
      { %>
    <span style="font-size:13px; font-weight:bold; color:red">将以下资料补充完毕者并验证通过后，可以联系客服获得我们送出的100条短信 ：）</span><br />
    <%} %>
    请输入您的手机号码：
    <br />
    <input type="text" id="phone" name="phone" style="height:20px; width:150px;" /> 
    
    <br />
    请输入您的QQ号码：
    <br />
    <input type="text" id="qq" name="qq" style="height:20px; width:150px;" />  <input type="button" onclick="savephone()" style="width:60px; height:24px;" value="马上提交" />

    </div>
</div>


</body>
</html>