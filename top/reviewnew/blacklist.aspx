﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="blacklist.aspx.cs" Inherits="top_reviewnew_blacklist" %>

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

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 短信黑名单 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">    
    <input type="button" value="返回短信设置页面" onclick="window.location.href='msg.aspx'" />
    
    <hr />
        <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
            在黑名单里面手机号码将不会发送短信，一行一个号码<br />
            例如：<br />13812345678<br />18600001111</b>
        </div>

        <table width="750">
        <tr>
        <td width="380" valign="top">
            <textarea rows="16" cols="20" name="blacklist"><%=blacklist %></textarea>
        </td>
        </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存设置" />
                </td>
            </tr>
        </table>
    </div>
</div>
</form>
</body>
</html>
