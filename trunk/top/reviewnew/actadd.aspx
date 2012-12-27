<%@ Page Language="C#" AutoEventWireup="true" CodeFile="actadd.aspx.cs" Inherits="top_reviewnew_actadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 创建新活动 </div>
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
    活动里面的赠送礼品不可以跟基本设置里面的条件同时生效，如果要赠送支付包红包请先在基本设置里面关闭支付宝红包赠送。
</div>

    <input type="button" value="返回列表" onclick="window.location.href='actlist.aspx'" />

    <hr />

    <table width="700">
            <tr>
                <td align="left" height="30">活动名称：</td>
                <td>
                    <input id="name" name="name" type="text" value="" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">开始日期：</td>
                <td>
                    <input id="startdate" name="startdate" type="text" value="<%=startdate %>" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">结束日期：</td>
                <td>
                    <input id="enddate" name="enddate" type="text" value="<%=enddate %>" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">满足金额：</td>
                <td>
                    <input id="condprice" name="condprice" type="text" value="0.00" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">指定商品：</td>
                <td>
                    <input type="button" value="选择商品" onclick="OpenDialogLable('dialogProduct.aspx',650,560);"/><br /><br />
                    <div id="productArea" style="width:470px; height:300px; overflow:scroll"></div>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">选择礼品：</td>
                <td>
                    选择优惠券：<%=couponstr %>
                    <a href="couponadd.aspx">创建优惠券</a>
                    <a href="couponsend.aspx">查看优惠券赠送记录</a> <br />

                    选择支付宝红包：<%=alipaystr%>
                    <a href="alipayadd.aspx">导入支付宝红包</a>
                    <a href="alipay.aspx">查看赠送记录</a> <br />

                    选择包邮卡：<%=freestr%>
                    <a href="../freecard/freecardlist.aspx">创建包邮卡</a>
                    <a href="../freecard/freecardcustomer.aspx">查看赠送记录</a> <br />
                </td>
            </tr>
            
            <tr>
                <td align="left" height="30" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建新活动" OnClientClick="return check()" />
                </td>
            </tr>
        </table>
    </div>
</div>
    </form>

    <script>
        var itemdata = '<%=itemliststr %>';

        function delitem(id) {
            reg = new RegExp("<div id=item_" + id + "(.*?)</div>", "g");
            itemdata = itemdata.replace(reg, '');
            document.getElementById("productArea").innerHTML = itemdata;
        }

        function check() {
            if (document.getElementById("name").value == "") {
                alert('请输入活动名称！');
                return false;
            }

            if (document.getElementById("startdate").value == "") {
                alert('请输入开始日期！');
                return false;
            }

            if (document.getElementById("enddate").value == "") {
                alert('请输入结束日期！');
                return false;
            }

            if (document.getElementById("condprice").value == "") {
                alert('请输入满足金额！');
                return false;
            }
        }

        function OpenDialogLable(url, w, h, editTxt) {
            if (typeof (editTxt) == "undefined") {
                editTxt = "";
            }
            if (navigator.appVersion.indexOf("MSIE") == -1) {
                this.returnAction = function (strResult) {
                    if (strResult != null) {
                        if (strResult != "") {
                            if (itemdata.indexOf(strResult) == -1) {
                                itemdata = itemdata + strResult;
                            }
                            document.getElementById("productArea").innerHTML = itemdata;
                        }
                    }
                }
                window.open(url + '?d=' + Date() + "&t=" + escape(editTxt), 'newWin', 'modal=yes,width=' + w + ',height=' + h + ',top=200,left=300,resizable=no,scrollbars=no');
                return;
            } else {
                var GetValue = showModalDialog(url + '?d=' + Date() + "&t=" + escape(editTxt), null, 'dialogWidth:' + w + 'px; dialogHeight:' + h + 'px;')
                if (GetValue != null) {
                    if (GetValue != "") {
                        if (itemdata.indexOf(GetValue) == -1) {
                            itemdata = itemdata + GetValue;
                        }
                        document.getElementById("productArea").innerHTML = itemdata;
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
    
    </script>
</body>
</html>
