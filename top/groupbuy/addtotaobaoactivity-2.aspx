﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addtotaobaoactivity-2.aspx.cs" Inherits="top_groupbuy_addtotaobaoactivity_2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 同步到宝贝描述 (2.预览效果)  </div>
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
    <div style="width:750px; height:400px; overflow:scroll;" id="lbView" runat="server" visible="false">
         
    </div>
     正在更新到淘宝描述,请等待........
         <table bgcolor="#dddddd" height=20 ALIGN=CENTER BORDER="0" WIDTH="500">
            <tr>
                <td align=left >
                 <table  id=Table1 bgcolor=#98CC00 height=20>
                     <tr align=center><td ><span id=Span1 >10%</span></td></tr>
                 </table>
                </td>
            </tr>
     </table>
    <br />    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <form id="myform" action="<%=url %>" method="post">
      
        <input name="act" value="save" type="hidden" />
        <input name="name" value="<%=name %>" type="hidden" />
        <input name="style" value="<%=style %>" type="hidden" />
        <input name="size" value="<%=size %>" type="hidden" />
        <input name="type" value="<%=type %>" type="hidden" />
        <input name="orderby" value="<%=orderby %>" type="hidden" />
        <input name="query" value="<%=query %>" type="hidden" />
        <input name="shopcat" value="<%=shopcat %>" type="hidden" />
        <input name="items" value="<%=items %>" type="hidden" />
        <input name="ads" value="<%=ads %>" type="hidden" />
        <input name="myadstemp" value="<%=myadstemp %>" type="hidden" />
      <tr>
        <td>
        	<!--<input type="button" value="上一步" onclick="history.go(-1)" />
        	<input type="submit" value="下一步"  />-->
        </td>
        <td></td>
      </tr>
    </form>
    </table>

  </div>
</div>

<script language="javascript" type="text/javascript">
    document.getElementById("myform").submit();
</script>

</body>
</html>