﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="setting.aspx.cs" Inherits="top_review_setting" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<link href="../css/haopingx.css" rel="stylesheet" />
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

    <div class="haopingtitle">
        <span style="font-size:14px; font-weight:bold; margin:7px;">1、请选择好评有礼赠送的奖品：</span>
    </div>
        <table width="700">
            <tr id="couponArea">
                <td align="left" height="30" colspan="2">
                    <table width=600>
                        <tr>
                            <td width=120 align="center">
                                <img src='images/coupon.png' width=80 height=80 />
                                <br /> <input type="checkbox" name="iscoupon" value="1" /> 赠送优惠券
                            </td>
                            <td width=120 align="center"><img src='images/freecard.png' width=80 height=80 />
                                <br /> <input type="checkbox" name="iscoupon" value="1" /> 赠送包邮卡</td>
                            <td width=120 align="center"><img src='images/alipay.jpg'  width=80 height=80 />
                                <br /> <input type="checkbox" name="iscoupon" value="1" /> 赠送支付宝红包</td>
                            <td width=120 align="center"><img src='images/choujiang.gif' width=80 height=80 />
                                <br /> <input type="checkbox" name="iscoupon" value="1" /> 赠送抽奖次数</td>
                        </tr>
                    </table>
                </td>
            </tr>
       </table>
            
      <div class="haopingtitle">
        <span style="font-size:14px; font-weight:bold; margin:7px;">2、请设置好评有礼赠送的条件：</span>
        </div>

        
        <table width="700">
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
                    赠送<input name="iscancelauto" type="radio" value="0" <%=check(iscancelauto, "0") %> />
                    不赠送<input name="iscancelauto" type="radio" value="1" <%=check(iscancelauto, "1") %> />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">好评自动判定：</td>
                <td>
                    是否开启 <input name="iskeyword" type="checkbox" value="1" <%=check(iskeyword, "1") %> /> 
                </td>
            </tr>
            <tr>
                <td align="left" height="30"></td>
                <td>
                    <input type="button" value="设定内容自动判定规则" />
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
            </table>

      <div class="haopingtitle">
        <span style="font-size:14px; font-weight:bold; margin:7px;">3、请设置参加好评有礼的宝贝：</span>
        </div>

        <table width="700">
            <tr>
                <td align="left" height="30">请选择限制类型：</td>
                <td>
                    以下商品才赠送<input name="ispro" id="Radio1" type="radio" value="1" <%=check(iskefu, "1") %> />
                    以下商品不赠送<input name="ispro" id="Radio2" type="radio" value="0" <%=check(iskefu, "0") %> />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">选择宝贝：</td>
                <td>
                    
                </td>
            </tr>

            <tr>
                <td align="left" colspan="2">
                <span class="ui-button ui-button-ok" tabindex="4">
                    <asp:Button ID="Button1" CssClass="ui-button-text" runat="server" onclick="Button1_Click" Text="保存设置" OnClientClick="return check()" />
                </span>
                
                <span class="ui-button ui-button-ok" tabindex="4">
                    <input type="button" class="ui-button-text" value="将活动展示在店铺里" style="width:180px;" onclick="window.location.href='html.aspx'" />
               </span>
               
                </td>
            </tr>
        </table>
    </div>
</div>
</form>

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