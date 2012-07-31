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

  <div class="crumbs">过期用户清除宝贝描述中好评图片 </div>
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
    </div>
</div>
</form>

</body>
</html>