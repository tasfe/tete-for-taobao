<%@ Page Language="C#" AutoEventWireup="true" CodeFile="success.aspx.cs" Inherits="top_microblog_success" %>

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

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">微博营销</a> 发布成功 (4.发布成功) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

        <b>发布成功，查看您发布的微博：</b><br />
        <a href='<%=blogUrl%>' target="_blank"><%=blogUrl%></a>
        
        <br />
        <br />
        <br />

        <input type="button" value="继续发布" onclick="window.location.href='microblogadd.aspx'" />
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

</body>
</html>
