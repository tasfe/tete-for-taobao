<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addtotaobao-2.aspx.cs" Inherits="top_addtotaobao_2" %>

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
  	<img src="images/title.gif" /><br />
    <br />
    预览效果如下：
    <br />    <br />
    <div style="width:750px; height:400px; overflow:scroll;" id="lbView" runat="server">
        
    </div>
    <br />    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <form action="<%=url %>" method="post">
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
      <tr>
        <td>
        	<input type="button" value="上一步" onclick="history.go(-1)" />
        	<input type="submit" value="下一步" />
        </td>
        <td></td>
      </tr>
    </form>
    </table>

  </div>
</div>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>

</body>
</html>