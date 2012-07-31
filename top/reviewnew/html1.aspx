<%@ Page Language="C#" AutoEventWireup="true" CodeFile="html1.aspx.cs" Inherits="top_review_html" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
    img{border:none}
</style>


</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 活动展示 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
                 <asp:Button ID="Button4" runat="server" onclick="Button4_Click" Text="一键清除" /><br />
                    <b>亲，如果您的店铺宝贝很多，请您耐心等待一会~</b>
    </div>
</div>
</form>

<script>
    function check(obj) {
        obj.value = "同步中，请您耐心等待..";
        //obj.disabled = true;
    }

    document.getElementById("<%=leftimgname %>").checked = true;
    document.getElementById("<%=detailimgname %>").checked = true;

    if ('0' == '<%=detailimgistop %>') {
        document.getElementById("detailimgistop").selectedIndex = 1;
    }
</script>

</body>
</html>