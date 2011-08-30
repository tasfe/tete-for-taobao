<%@ Page Language="C#" AutoEventWireup="true" CodeFile="microblogadd1.aspx.cs" Inherits="top_microblog_microblogadd1" ValidateRequest="false" EnableViewStateMac="false" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">微博营销</a> 我要推广 (2.编写推广内容) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <input type="hidden" value="<%=topic %>" name="topic" />
        <b>您选择的话题是：</b> <%=topic %> <br /><br />
        <b>请输入您的推广内容：</b><br />
        <asp:TextBox ID="tbContent" runat="server" TextMode="MultiLine" Width="460px" Height="120px" MaxLength="120"></asp:TextBox> 内容请不要超过120个字 <br />
        <br />
        <input type="button" value="后退" onclick="history.go(-1)" />
        <input type="button" value="下一步" onclick="return submitForm()" />
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function submitForm() {
        var content = document.getElementById("tbContent").value;
        if (content.length > 120) {
            alert("内容请不要超过120个字");
            return false;
        }
        document.getElementById("form1").action = "microblogadd2.aspx";
        document.getElementById("form1").submit();
        parent.scroll(0, 0);
    }
</script>

</body>
</html>
