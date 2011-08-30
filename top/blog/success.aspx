<%@ Page Language="C#" AutoEventWireup="true" CodeFile="success.aspx.cs" Inherits="top_blog_success" ValidateRequest="false" %>

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

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 发布成功 (4.发布成功) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

        <b>发布成功，查看您发布的文章：</b><br />
        <%=blogUrl%>
        
        <br />
        <br />
        <br />
        <b>查看别人最近发布成功的文章：</b><br />

        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
                <a href='<%#Eval("blogurl") %>' target="_blank"><%#Eval("blogtitle") %></a>  &nbsp;&nbsp;&nbsp;&nbsp; 发布时间：<%#Eval("adddate") %> <br />
            </ItemTemplate>
        </asp:Repeater>

        <br />
        <br />
        <input type="button" value="继续发布" onclick="window.location.href='blogadd.aspx'" />
        <input type="button" value="查看我成功发布的文章" onclick="window.location.href='bloglist.aspx'" />
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function selectArticle(obj) {
        document.getElementById("t").value = obj.title;
        document.getElementById("form1").action = "blogadd1.aspx";
        document.getElementById("form1").submit();
    }
</script>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>
