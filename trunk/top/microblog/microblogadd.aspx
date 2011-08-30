<%@ Page Language="C#" AutoEventWireup="true" CodeFile="microblogadd.aspx.cs" Inherits="top_microblog_microblogadd" %>

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

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">微博营销</a> 我要推广 (1.选择推广话题) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

        <b>今日热门话题：</b><br />
        <%=radio2 %> 
        <br />

        <b>焦点话题：</b><br />
        <%=radio3 %>
        <br />

        <b>自定义话题：</b><br />
        <input type=radio onclick='return selectTopicSelf(this)' title="自定义话题" name=title value="自定义话题">
        <input ID="tbKey" name="tbKey" />
        <span style="color:#ccc">比如女装类型的淘宝店铺可以用“冬季时尚女装”作为您的话题</span>
        <%=radio %>
        <br />
        <br />

    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function selectTopic(obj) {
        document.getElementById("t").value = obj.title;
        document.getElementById("form1").action = "microblogadd1.aspx";
        document.getElementById("form1").submit();
    }

    function selectTopicSelf(obj) {
        if (document.getElementById("tbKey").value == '') {
            alert("请输入自定义话题");
            return false;
        }

        document.getElementById("t").value = document.getElementById("tbKey").value;
        document.getElementById("form1").action = "microblogadd1.aspx";
        document.getElementById("form1").submit();
    }
</script>

</body>
</html>