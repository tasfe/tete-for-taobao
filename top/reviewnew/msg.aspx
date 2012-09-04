<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msg.aspx.cs" Inherits="top_review_msg" %>

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
            短信内容设置中的[shopname]、[buynick]、[gift]、[shiptyp]、[shipnumber]、[freecard]在短信发送时会被自动替换成您的 店铺名称、买家昵称、优惠券、快递方式和快递单号、包邮卡名称，请不要随意删除..<br />
            快递方式和快递单号只能在发货短信中使用，放在其他短信中会无法正常解析<br />
            请大家尽量精简自己的短信内容，一条短信的内容不要超过64个字，否则您的短信内容将会被截取成64个字~ ：）
        </div>

        <table width="750">
        <tr>
            <td width="370" valign="top" height="400">
        <table width="370">
            <tr>
                <td align="left" width="250">
                    短信发送显示店铺名称 
                </td>
            </tr>
            <tr>
                <td align="left" width="500">
                    <input id="shopname" name="shopname" value="<%=shopname %>" /> <br />(尽量让您的店铺名精简，否则会使短信变的很长)
                </td>
            </tr>
            <tr>
                <td align="left" width="250">
                    开启优惠券赠送短信提示 <input id="giftflag" name="giftflag" type="checkbox" value="1" onclick="showArea(this)" />
                </td>
            </tr>
            <tr id="Area1">
                <td align="left" width="250">
                    <textarea id="giftcontent" name="giftcontent" cols="40" rows="3"><%=giftcontent %></textarea>
                    <input type="button" value="短信预览" onclick="yulanMsg('giftcontent')" />
                </td>
            </tr>
            <tr>
                <td align="left">
        开启订单发货后短信提示 <input id="fahuoflag" name="fahuoflag" type="checkbox" value="1" onclick="showArea4(this)" />
                </td>
            </tr>
            <tr id="Area4">
                <td align="left" width="250">
                    <textarea id="fahuocontent" name="fahuocontent" cols="40" rows="3"><%=fahuocontent%></textarea>
                    <input type="button" value="短信预览" onclick="yulanMsg('fahuocontent')" />
                </td>
            </tr>
            <tr>
                <td align="left">
        开启物流签收后短信提示 <input id="shippingflag" name="shippingflag" type="checkbox" value="1" onclick="showArea2(this)" />
                </td>
            </tr>
            <tr id="Area2">
                <td align="left" width="250">
                    <textarea id="shippingcontent" name="shippingcontent" cols="40" rows="3"><%=shippingcontent%></textarea>
                    <input type="button" value="短信预览" onclick="yulanMsg('shippingcontent')" />
                </td>
            </tr>
            <tr>
                <td align="left">
        开启过期未评价短信提示 <input id="reviewflag" name="reviewflag" type="checkbox" value="1" onclick="showArea3(this)" />
                </td>
            </tr>
            <tr id="Area3">
                <td align="left" width="250">
                    <textarea name="reviewcontent" cols="40" rows="3"><%=reviewcontent%></textarea>  
                    <input type="button" value="短信预览" onclick="yulanMsg('reviewcontent')" /><br />
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
            <tr>
                <td align="left">
        开启包邮卡赠送短信 <input id="freecardflag" name="freecardflag" type="checkbox" value="1" onclick="showArea5(this)" />
                </td>
            </tr>
            <tr id="Area5">
                <td align="left" width="250">
                    <textarea id="freecardcontent" name="freecardcontent" cols="40" rows="3"><%=freecardcontent%></textarea>
                    <input type="button" value="短信预览" onclick="yulanMsg('freecardcontent')" />
                </td>
            </tr>
        </table>
        </td>
            <td width="380" style="background:url('images/msgbg.jpg') 0 0 no-repeat;" valign="top">
                <input type="hidden" name="yulanContent" id="yulanContent" />
                <div id="yulan" style="margin:86px 0 0 52px; color:white; width:140px;"></div>
                <div id="countArea" style="margin:66px 0 0 52px; color:white; width:140px;"></div>
            </td>
        </tr>
            <tr>
                <td align="left">
                </td>
                <td align="left">
                    您可以在这里填写您自己的手机号码测试短信的实际收到效果！<br />
                    手机号码：<input name="testmobile" />
                    <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="测试发送" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存设置" />
                    <input type="button" value="短信黑名单" onclick="window.location.href='blacklist.aspx'" />
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

    function yulanMsg(id){
        var msg = document.getElementById(id).value;
        shopname = document.getElementById("shopname").value;
        msg = msg.replace("[shopname]", shopname);
        msg = msg.replace("[gift]", "优惠券");
        msg = msg.replace("[buynick]", "买家0001");
        msg = msg.replace("[shiptyp]", "圆通快递");
        msg = msg.replace("[shipnumber]", "1900209081740");

        showMsg = "本条短信共计个"+msg.length+"字符";

        if(msg.length > 64){
            showMsg = "本条短信共计个"+msg.length+"字符，被截取成64个字符";
            msg = msg.substring(0,64);
        }

        document.getElementById("yulanContent").value = msg;
        document.getElementById("yulan").innerHTML = msg;
        document.getElementById("countArea").innerHTML = showMsg;
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

    if(<%=freecardflag %> == 1){
        document.getElementById("freecardflag").checked = true;
        document.getElementById("Area5").style.display = "block";
    }else{
        document.getElementById("freecardflag").checked = false;
        document.getElementById("Area5").style.display = "none";
    }
    
    document.getElementById("reviewtime").value = "<%=reviewtime %>";
</script>

</body>
</html>