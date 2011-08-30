<%@ Page Language="C#" AutoEventWireup="true" CodeFile="linkaddnew.aspx.cs" Inherits="top_blog_linkaddnew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 添加历史搜索关键字 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        关 键 字：<asp:TextBox ID="TextBox1" runat="server" Width="150px"></asp:TextBox><br />
        <br />
        <asp:Button ID="btnSearch" runat="server" Text="添加" onclick="btnSearch_Click" />
        <input type="button" value="返回列表" onclick="window.location.href='linklistnew.aspx'" />
    </div>
</div>
</form>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>
