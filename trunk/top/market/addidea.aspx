<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addidea.aspx.cs" Inherits="top_market_addidea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 我要推广 (1.选择风格) </div>
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
    <form action="<%=url %>" method="post">
      <tr>
        <td height="30" width="130">请输入广告标题：</td>
        <td align="left">
        	<input id="ideaName" runat="server" value=""/> <span style="color:#ccc">广告标题将显示在广告的左上角,您也可以把自己的联系方式写进去</span>
        </td>
      </tr>
      <tr style="display:none;">
        <td height="30">请选择风格：</td>
        <td align="left">
        	<input name="style" id="style1" type="radio" value="Tpl1" checked="checked" /><label for="style1">盎然春意</label>
        	<input name="style" id="style2" type="radio" value="Tpl2" /><label for="style2">清爽夏日</label>
        	<input name="style" id="style3" type="radio" value="Tpl3" /><label for="style3">金色秋季</label>
        	<input name="style" id="style4" type="radio" value="Tpl4" /><label for="style4">冰天雪地</label>
        </td>
      </tr>
      <tr>
        <td height="30">请选择尺寸：</td>
        <td align="left">
        	<input name="size" id="size1" type="radio" onclick="showview(this)" value="514*160" /><label for="size1">514*160</label> 
        	<input name="size" id="size2" type="radio" onclick="showview(this)" value="514*288" /><label for="size2">514*288</label> 
        	<input name="size" id="size4" type="radio" onclick="showview(this)" value="664*160" /><label for="size4">664*160</label> 
        	<input name="size" id="size3" type="radio" onclick="showview(this)" value="312*288" /><label for="size3">312*288</label> <br />
        	<input name="size" id="size7" type="radio" onclick="showview(this)" value="218*286" /><label for="size7">218*286</label>
        	<input name="size" id="size5" type="radio" onclick="showview(this)" value="714*160" /><label for="size5">714*160</label> 
        	<input name="size" id="size6" type="radio" onclick="showview(this)" value="114*418" /><label for="size6">114*418</label> 
        	<input name="size" id="size8" type="radio" onclick="showview(this)" value="743*308" checked="checked" /><label for="size8">743*308</label> <br />
        </td>
      </tr>
      <tr>
        <td>
        	<input type="submit" value="下一步" onclick="return verify()" />
        </td>
        <td></td>
      </tr>
    </form>
    </table>
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0 10px;">
      <tr>
        <td align="left" colspan="2">
        	下面是您推广的预览图:
        </td>
      </tr>
      <tr>
        <td align="left" colspan="2">
        	<img src="images/view-size8.gif" id="viewImg" />
        </td>
      </tr>
    </table>
    <br /><br /><br />
    
    <table width="680" border="0" cellspacing="0" cellpadding="0" style="margin:0 10px;">
      <tr>
        <td width="108" align="center" height="90">
        	<a href="http://seller.taobao.com/fuwu/service.htm?service_id=7888" title="淘模板" target="_blank"><img src="http://img01.taobaocdn.com/imgextra/i1/85662775/T2uMpRXgFaXXXXXXXX_!!85662775.jpg" border="0" /></a>
        </td>
        <td width="108" align="center">
        	<a href="http://seller.taobao.com/fuwu/service.htm?service_id=4545&f=mianfei" title="好评有礼" target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T1mGx6XmBBXXb1upjX.jpg
" border="0" /></a>
        </td>
        <td width="108" align="center">
        	<a href="http://seller.taobao.com/fuwu/service.htm?service_id=11807&f=mianfei" title="特特团购" target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T1VvaiXX4lXXb1upjX.jpg
" width=80 height=80 border="0" /></a>
        </td>
        <td width="108" align="center">
        	<a href="http://fuwu.taobao.com/serv/detail.htm?service_id=6704" title="聪明店长_宝贝推广专家 " target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T1QDRZXo4yXXb1upjX.jpg
" border="0" /></a>
        </td>
        <td width="108" align="center">
        	<a href="http://fuwu.taobao.com/serv/detail.htm?service_id=415&ref=zxb" title="库存宝 " target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T10oJ8XcXFXXaCwpjX.png" border="0" /></a>
        </td>
        <td width="108" align="center">
        	<a href="http://fuwu.taobao.com/serv/detail.htm?service_id=12938#teteyx" title="起赛推广" target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T1FpakXd8XXXb1upjX.jpg" border="0" /></a>
        </td>
        <td width="108" align="center">
        	<a href="http://fuwu.taobao.com/service/service.htm?service_id=3543" title="淘博会开店王" target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T1N6elXbtTXXb1upjX.jpg" border="0" /></a>
        </td>
      </tr>


      <tr>
        <td width="108" align="center" height="90">
        	<a href="http://fuwu.taobao.com/serv/detail.htm?service_id=296" title="数宝网店掌柜" target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T1w49nXnJYXXb1upjX.jpg" border="0" /></a>
        </td>
        <td width="108" align="center">
            <a href="http://seller.taobao.com/fuwu/service.htm?service_id=6459" title="团购专家" target="_blank"><img src="http://img03.taobaocdn.com/imgextra/i3/1204883/T20dpRXcdcXXXXXXXX_!!1204883.png
" border="0" /></a>
        </td>
        <td width="108" align="center">
            <a href="http://fuwu.taobao.com/serv/detail.htm?service_id=415&f=ttyx" title="库存宝" target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T10oJ8XcXFXXaCwpjX.png
" border="0" /></a>
        </td>
        <td width="108" align="center">
        	<a href="http://seller.taobao.com/serv/detail.htm?service_id=13300&f=YLtete" title="微博营销推广，粉丝要多少自己定" target="_blank"><img src="http://img01.taobaocdn.com/top/i1/T1F5WzXbdiXXaCwpjX.png
" border="0" /></a>
        </td>
        <td width="108" align="center">
        </td>
        <td width="108" align="center">
        </td>
        <td width="108" align="center">
        </td>
      </tr>

    </table>
<br>
<table width="540" border="0" cellspacing="0" cellpadding="0" style="margin:0 10px;">
<tr>

        <td colspan="4" height="120" align="left">
        	<div id="mzif-container" style="margin: -20px auto 0 auto; width:530px; height:98px; overflow:hidden;">
    <iframe scrolling="no" src="http://applink.goodtp.com/f/show?id=10&count=6" width="530" height="98" frameborder="0" allowTransparency="true">
    </iframe>
    </div> 
        </td>
</tr>
    </table>




    <script language="javascript" type="text/javascript">
        function verify(){
            var ideaName = document.getElementById("ideaName").value;
            if(ideaName == ''){
                alert('请输入广告名称');
                document.getElementById("ideaName").focus();
                return false;
            }
            
            return true;
        }
    
        function checkSelect(nam, val){
            var obj = document.getElementsByName(nam);
            for(i=0;i<obj.length;i++){
                if(obj[i].value == val){
                    obj[i].checked = true;
                }
            }
        }
    
        function checkSelectSize(nam, val){
            var obj = document.getElementsByName(nam);
            for(i=0;i<obj.length;i++){
                if(obj[i].value == val){
                    obj[i].checked = true;
                    var str = "images/view-" + obj[i].id + ".gif";
                    document.getElementById("viewImg").src = str;
                }
            }
        }
        
        function showview(obj){
            var str = "images/view-" + obj.id + ".gif";
            document.getElementById("viewImg").src = str;
        }
        
        checkSelect("style", "<%=style %>");
        checkSelectSize("size", "<%=size %>");
    </script>

  </div>
</div>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>
