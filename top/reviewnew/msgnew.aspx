﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgnew.aspx.cs" Inherits="top_review_msg" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 手机短信通知 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <div style="width:700px;">
        目前已经使用<%=used %>条短信，剩余短信<%=total%>条...<br />
        <a href='msglist.aspx'>查看短信发送情况</a> |
        <a href='msgaddlist.aspx'>查看历史购买记录</a> |
        <a href='msgbuy.aspx'><b style="color:red">短信购买</b></a>
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="更新短信数量" />
        </div>
        <hr />

        <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
            请大家尽量精简自己的短信内容，一条短信的内容不要超过66个字，否则您的短信内容将会被截取成66个字~ ：）
        </div>


        <table width="700">
            <tr>
                <td align="left" width="250">
                    短信发送显示店铺名称 
                </td>
            </tr>
            <tr>
                <td align="left" width="500">
                    <input id="shopname" name="shopname" value="<%=shopname %>" /> (尽量让您的店铺名精简，否则会使短信变的很长)
                </td>
            </tr>
        </table>

        <div class="haopingtitle">
            <span style="font-size:14px; font-weight:bold; margin:7px;">1、开启优惠券赠送短信提示： </span>
                <input id="giftflag" name="giftflag" type="checkbox" value="1" onclick="showArea(this)" style="margin:10px 0 0 0" />
        </div>

        <table width="700">
            <tr id="Area1">
                <td align="left" width="250">
                    <textarea id="giftcontent" name="giftcontent" cols="40" rows="3"><%=giftcontent %></textarea>
                </td>
                <td>
                    <input type="button" value="店名" onclick="insertText('giftcontent', '[shopname]')" />
                </td>
            </tr>
        </table>




        <div class="haopingtitle">
            <span style="font-size:14px; font-weight:bold; margin:7px;">2、开启订单发货后短信提示：</span>
            <input id="fahuoflag" name="fahuoflag" type="checkbox" value="1" onclick="showArea4(this)" style="margin:10px 0 0 0" />
        </div>

        <table width="700">
            <tr id="Area4">
                <td align="left" width="250">
                    <textarea name="fahuocontent" cols="40" rows="3"><%=fahuocontent%></textarea>
                </td>
            </tr>
        </table>


        <div class="haopingtitle">
            <span style="font-size:14px; font-weight:bold; margin:7px;">3、开启物流签收后短信提示：</span>
            <input id="shippingflag" name="shippingflag" type="checkbox" value="1" onclick="showArea2(this)" style="margin:10px 0 0 0" />
        </div>

        <table width="700">
            <tr id="Area2">
                <td align="left" width="250">
                    <textarea name="shippingcontent" cols="40" rows="3"><%=shippingcontent%></textarea>
                </td>
            </tr>
        </table>

        <div class="haopingtitle">
            <span style="font-size:14px; font-weight:bold; margin:7px;">4、开启过期未评价短信提示：</span>
            <input id="reviewflag" name="reviewflag" type="checkbox" value="1" onclick="showArea3(this)" style="margin:10px 0 0 0" />
        </div>
        
        <table width="700">
            <tr id="Area3">
                <td align="left" width="250">
                    <textarea name="reviewcontent" cols="40" rows="3"><%=reviewcontent%></textarea><br />
                    短信自动发送时间
                    <select name="reviewtime" id="reviewtime">
                        <option value="0">0</option>
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        <option value="6">6</option>
                        <option value="7">7</option>
                        <option value="8">8</option>
                        <option value="9" selected="selected">9</option>
                        <option value="10">10</option>
                        <option value="11">11</option>
                        <option value="12">12</option>
                        <option value="13">13</option>
                        <option value="14">14</option>
                        <option value="15">15</option>
                        <option value="16">16</option>
                        <option value="17">17</option>
                        <option value="18">18</option>
                        <option value="19">19</option>
                        <option value="20">20</option>
                        <option value="21">21</option>
                        <option value="22">22</option>
                        <option value="23">23</option>
                    </select>点
                </td>
            </tr>
</table>
                    
                    <div class="haopingtitle">
            <span style="font-size:14px; font-weight:bold; margin:7px;">5、开启延迟发货短信提醒：</span>
            <input id="delayflag" name="delayflag" type="checkbox" value="1" onclick="showArea5(this)" style="margin:10px 0 0 0" />
        </div>
            

        <table width="700">
            <tr id="Area5">
                <td align="left" width="250">
                    <textarea name="delaycontent" cols="40" rows="3"><%=shippingcontent%></textarea>
                </td>
            </tr>
</table>


<div class="haopingtitle">
            <span style="font-size:14px; font-weight:bold; margin:7px;">6、开启未付款订单催单：</span>
            <input id="unpayflag" name="unpayflag" type="checkbox" value="1" onclick="showArea6(this)" style="margin:10px 0 0 0" />
        </div>

        
        <table width="700">
            <tr id="Area6">
                <td align="left" width="250">
                    <textarea name="unpaycontent" cols="40" rows="3"><%=shippingcontent%></textarea>
                </td>
            </tr>
</table>


<div class="haopingtitle">
            <span style="font-size:14px; font-weight:bold; margin:7px;">7、开启物流到达城市短信提醒：</span>
            <input id="cityflag" name="cityflag" type="checkbox" value="1" onclick="showArea7(this)" style="margin:10px 0 0 0" />
        </div>

        
        <table width="700">
            <tr id="Area7">
                <td align="left" width="250">
                    <textarea name="citycontent" cols="40" rows="3"><%=shippingcontent%></textarea>
                </td>
            </tr>
</table>


        <table width="700">
            <tr>
                <td align="left">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存设置" />
                </td>
            </tr>
        </table>
    </div>
</div>
</form>


<script language="javascript" type="text/javascript">
    function showArea(obj) {
        var str = obj.checked;
        if (str == false) {
            document.getElementById("Area1").style.display = "none";
        } else {
            document.getElementById("Area1").style.display = "block";
        }
    }
    function showArea2(obj) {
        var str = obj.checked;
        if (str == false) {
            document.getElementById("Area2").style.display = "none";
        } else {
            document.getElementById("Area2").style.display = "block";
        }
    }
    function showArea3(obj) {
        var str = obj.checked;
        if (str == false) {
            document.getElementById("Area3").style.display = "none";
        } else {
            document.getElementById("Area3").style.display = "block";
        }
    }
    function showArea4(obj) {
        var str = obj.checked;
        if (str == false) {
            document.getElementById("Area4").style.display = "none";
        } else {
            document.getElementById("Area4").style.display = "block";
        }
    }

    
    function showArea5(obj) {
        var str = obj.checked;
        if (str == false) {
            document.getElementById("Area5").style.display = "none";
        } else {
            document.getElementById("Area5").style.display = "block";
        }
    }

    
    function showArea6(obj) {
        var str = obj.checked;
        if (str == false) {
            document.getElementById("Area6").style.display = "none";
        } else {
            document.getElementById("Area6").style.display = "block";
        }
    }

    
    function showArea7(obj) {
        var str = obj.checked;
        if (str == false) {
            document.getElementById("Area7").style.display = "none";
        } else {
            document.getElementById("Area7").style.display = "block";
        }
    }

    if(<%=giftflag %> == 1){
        document.getElementById("giftflag").checked = true;
        document.getElementById("Area1").style.display = "block";
    }else{
        document.getElementById("giftflag").checked = false;
        document.getElementById("Area1").style.display = "none";
    }

    if(<%=shippingflag %> == 1){
        document.getElementById("shippingflag").checked = true;
        document.getElementById("Area2").style.display = "block";
    }else{
        document.getElementById("shippingflag").checked = false;
        document.getElementById("Area2").style.display = "none";
    }

    if(<%=reviewflag %> == 1){
        document.getElementById("reviewflag").checked = true;
        document.getElementById("Area3").style.display = "block";
    }else{
        document.getElementById("reviewflag").checked = false;
        document.getElementById("Area3").style.display = "none";
    }

    if(<%=fahuoflag %> == 1){
        document.getElementById("fahuoflag").checked = true;
        document.getElementById("Area4").style.display = "block";
    }else{
        document.getElementById("fahuoflag").checked = false;
        document.getElementById("Area4").style.display = "none";
    }


     if(<%=delayflag %> == 1){
        document.getElementById("delayflag").checked = true;
        document.getElementById("Area5").style.display = "block";
    }else{
        document.getElementById("delayflag").checked = false;
        document.getElementById("Area5").style.display = "none";
    }

     if(<%=unpayflag %> == 1){
        document.getElementById("unpayflag").checked = true;
        document.getElementById("Area6").style.display = "block";
    }else{
        document.getElementById("unpayflag").checked = false;
        document.getElementById("Area6").style.display = "none";
    }

     if(<%=cityflag %> == 1){
        document.getElementById("cityflag").checked = true;
        document.getElementById("Area7").style.display = "block";
    }else{
        document.getElementById("cityflag").checked = false;
        document.getElementById("Area7").style.display = "none";
    }
    
    document.getElementById("reviewtime").value = "<%=reviewtime %>";




    function insertText(objid,str) {
        obj = document.getElementById(objid);
        obj.focus();
        if (document.selection) {
            var sel = document.selection.createRange();
            sel.text = str;
        } else if (typeof obj.selectionStart === 'number' && typeof obj.selectionEnd === 'number') {
            var startPos = obj.selectionStart,
                endPos = obj.selectionEnd,
                cursorPos = startPos,
                tmpStr = obj.value;
            obj.value = tmpStr.substring(0, startPos) + str + tmpStr.substring(endPos, tmpStr.length);
            cursorPos += str.length;
            obj.selectionStart = obj.selectionEnd = cursorPos;
        } else {
            obj.value += str;
        }
    }

</script>

</body>
</html>