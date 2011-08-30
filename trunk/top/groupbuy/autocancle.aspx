<%@ Page Language="C#" AutoEventWireup="true" CodeFile="autocancle.aspx.cs" Inherits="top_groupbuy_autocancle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="javascript:;" class="nolink">特特团购</a> 自动取消未付款团购订单 </div>
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
    
<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold; width:400px;">
    <%=str%> 您可以通过设置开启自动取消未付款的团购及默认自动取消时间
</div>

<br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:0 10px;">
      <tr>
        <td width="130" height="30">是否开启：</td>
        <td align="left">
            <input name="type" id="style0" type="radio" value="0" checked="checked" onclick="showhidden(0)" /><label for="style1" onclick="showhidden(0)">未开启</label>
        	<input name="type" id="style1" type="radio" value="1" onclick="showhidden(1)" /><label for="style1" onclick="showhidden(1)">开启</label>
        </td>
     </tr>
     <tr id="panel1">
        <td width="130" height="30">设置自动取消时间：</td>
        <td align="left">
            <asp:TextBox ID="mintime" runat="server" Width="40px" Text="30"></asp:TextBox> 分钟
        </td>
     </tr>
     <tr>
        <td width="130" height="30"></td>
        <td align="left">
            <asp:Button ID="btn1" runat="server" Text="保存设置" onclick="btnSearch_Click" />
        </td>
     </tr>
    </table>

  </div>
</div>

</form>

<script language="javascript" type="text/javascript">
    function showhidden(i) {
        if (i == "0") {
            document.getElementById("panel1").style.display = "none";
        } else {
            document.getElementById("panel1").style.display = "block";
        }
    }
    showhidden(<%=isautocancle %>);
    document.getElementById("style" + <%=isautocancle %>).checked = true;
</script>
</body>
</html>
