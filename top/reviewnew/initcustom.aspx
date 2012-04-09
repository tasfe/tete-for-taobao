<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initcustom.aspx.cs" Inherits="top_crm_initcustom" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
        <div>
        <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 导入历史数据 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>


    <div id="main-content">
    
  <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
最多只能导入3个月的历史会员数据，导入时间会稍长，请您耐心等待...
</div>

            <asp:Button ID="Button1" runat="server" Text="导入历史客户" 
                onclick="Button1_Click1" />

            <asp:Button ID="Button2" runat="server" Text="根据好评数据库导入历史客户" 
                onclick="Button1_Click2" />
    </div>
    </div>
    </div>
    </form>
</body>
</html>
