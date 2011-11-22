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

  <div class="crumbs"><a href="javascript:;" class="nolink">好评有礼</a> 基本设置 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
    因为淘宝政策调整，优惠接口目前最多优惠到7折，所以取消1分钱礼品的相关设置，<A href='http://dev.open.taobao.com/bbs/read.php?tid=22210' target="_blank">点击查看详情</A>
</div>

        <table width="700">
            <tr>
                <td align="left" width="180" height="30">是否开启按时好评送优惠券：</td>
                <td>
                    开启<input name="iscoupon" type="radio" value="1" <%=check(iscoupon, "1") %> onclick="showArea(1)" />
                    不开启<input name="iscoupon" type="radio" value="0" <%=check(iscoupon, "0") %> onclick="showArea(0)" />
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
                <td align="left" height="30">淘宝提供物流跟踪订单最短评价时间：</td>
                <td>
                    <input id="mindate" name="mindate" type="text" value="<%=mindate %>" size="2" /> 天 

                </td>
            </tr>
            <tr>
                <td align="left" height="30">淘宝不提供物流跟踪最短评价时间：</td>
                <td>
                    <input id="maxdate" name="maxdate" type="text" value="<%=maxdate %>" size="2" /> 天 
                </td>
            </tr>
            <tr>
                <td align="left" height="30">默认好评优惠券处理：</td>
                <td>
                    赠送<input name="iscancelauto" type="radio" value="0" <%=check(iscancelauto, "0") %> />
                    不赠送<input name="iscancelauto" type="radio" value="1" <%=check(iscancelauto, "1") %> />
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
                    开启<input name="iskefu" type="radio" value="1" <%=check(iskefu, "1") %> />
                    不开启<input name="iskefu" type="radio" value="0" <%=check(iskefu, "0") %> />
                    <a href="kefulist.aspx">查看待审核列表</a>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存设置" OnClientClick="return check()" />
                    <input type="button" value="将活动展示在店铺里" onclick="window.location.href='html.aspx'" />
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

    function showArea1(str) {
        if (str == 0) { 
            document.getElementById("itemArea").style.display = "none";
        }else if(str == 1){
            document.getElementById("itemArea").style.display = "block";
        }
    }

    function check() {
        if (document.getElementsByName("isfree")[0].checked && document.getElementById("productArea").innerHTML == "") {
            alert("请选择免费赠送的礼品");
            return false;
        }
        
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
    //showArea1(<%=isfree %>);
</script>

</body>
</html>