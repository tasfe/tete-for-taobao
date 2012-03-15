﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="keyword.aspx.cs" Inherits="top_review_keyword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>二次销售魔方</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">二次销售魔方</a> 好评自动判定规则 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

        <table width="700">
            <tr>
                <td align="left" width="120" height="30">好评字数判定：</td>
                <td>
                    评价内容的字数必须大于<input 
                        type="text" name="wordcount" value="<%=wordcount %>" style="width: 28px" />字才算好评
                </td>
            </tr>
            <tr>
                <td align="left" width="120" height="30">包含关键字：</td>
                <td>
                <span style="font-size:16px; color:Red">只有包含了以下关键字的才算好评，多个关键字请用<b>回车</b>分开</span><br /> 
                    <textarea id="key" name="keyword" rows="8" cols="50"><%=keyword %></textarea> 
                    
                </td>
            </tr>
            
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存设置" OnClientClick="return check()" />
                    <input type="button" value="返回基本设置" onclick="window.location.href='setting.aspx'" />
                </td>
            </tr>
        </table>
    </div>
</div>
</form>

<script>
    function check() {
        var value = document.getElementById("key").value;

        if (value.indexOf("\r\n") == -1 && value.length > 6) {
            alert("请用回车将您设置的多个关键字分开，否则将无法正常判定好评！");
            return false;
        }
    }
</script>

</body>
</html>
