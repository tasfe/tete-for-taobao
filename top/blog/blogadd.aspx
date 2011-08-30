<%@ Page Language="C#" AutoEventWireup="true" CodeFile="blogadd.aspx.cs" Inherits="top_blog_blogadd" %>

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

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 我要推广 (1.选择软文) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <div>
        请输入店铺推广商品的关键字：<br />
        <asp:TextBox ID="tbKey" runat="server"></asp:TextBox> 
        <asp:Button ID="btnSearch" runat="server" Text="搜索" onclick="btnSearch_Click" />
        <span style="color:#ccc">比如女装类型的淘宝店铺可以用“冬季时尚女装”来搜索相关的文章</span>
        </div>
        <div id="showArea" runat="server" visible="false">
            <div><span style="color:#ccc">您可以点击文章后面的“查看原文”查看这篇文章，点击文章前面的按钮确认选择该文章作为您的推广软文</span> <br /></div>
            <div>
                <div style="float:left; width:450px;"><%=radio %></div>
                <div style="float:left; border:solid 1px #ccc; margin:5px; padding:5px;"><b>今日热门话题：</b><br />
                    <%=radio1 %>
                </div>
            </div>
        </div>
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function selectArticle(obj) {
        document.getElementById("t").value = obj.title;
    }

    function submitForm() {
        document.getElementById("form1").action = "blogadd1.aspx";
        document.getElementById("form1").submit();
    }
</script>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>

