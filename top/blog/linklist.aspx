<%@ Page Language="C#" AutoEventWireup="true" CodeFile="linklist.aspx.cs" Inherits="top_blog_linklist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 关键字替换 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <input type="button" value="添加关键字" onclick="window.location.href='linkadd.aspx'" /> <br />

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30"><strong>关键字</strong></td>
        <td><strong>链接地址</strong></td>
        <td><strong>操作</strong></td>
      </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
        <tr>
                <td><a href='<%#Eval("link") %>' target="_blank"><%#Eval("keyword") %></a>  </td>
                <td><%#Eval("link") %> &nbsp;&nbsp;</td>
                <td><a href='linkadd.aspx?id=<%#Eval("id") %>&act=edit'>编辑</a>
                <a href='linklist.aspx?id=<%#Eval("id") %>&act=del'>删除</a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        </table>
    </div>
</div>
</form>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>