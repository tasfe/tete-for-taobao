<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addtotaobao-3.aspx.cs" Inherits="top_addtotaobao_3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
<link href="images/tab.css" rel="stylesheet" />
<script type="text/ecmascript" src="js/jquery-1.3.2.min.js"></script>
<script type="text/javascript">
    function showNumber(dateStr) {
        alert($('#jd'));
        $.ajax({
            url: dateStr,
            success: function (data) {
                $('#jd').html(Math.ceil(data * 0.2) + "%");
                $('#lpc').attr("width", data);
                if (data < 400) {
                    alert("DCC");
                    setTimeout("showNumber('LoadAjax.aspx?date=" + new Date() + "')", 1000);
                }
                else {
                    alert("DDDDDDDD");
                    $('#jd').html("100%");
                    $('#lpc').attr("width", 500);
                    window.location.href = "missionlist.aspx";
                }

            }
        });
    }

    alert($('#lpc'));
        showNumber('LoadAjax.aspx?date=' + new Date());
   
</script>
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 我要推广 (3.获取发布代码)  </div>
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
        <table bgcolor="#dddddd" height=20 ALIGN=CENTER BORDER="0" WIDTH="500">
            <tr>
                <td align=left >
                 <table  id=lpc bgcolor=#98CC00 height=20>
                     <tr align=center><td ><span id=jd >10%</span></td></tr>
                 </table>
                </td>
            </tr>
     </table>
  </div>
     

</div>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>
