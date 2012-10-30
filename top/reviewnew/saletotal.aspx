<%@ Page Language="C#" AutoEventWireup="true" CodeFile="saletotal.aspx.cs" Inherits="top_reviewnew_saletotal" %>

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
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 二次销售统计 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content" style="font-size:16px;">
        好评有礼送出的优惠券共为您带来了<%=totalcount %>个二次销售，总金额为<%=totalprice %>。<br />

        <a href='salelist.aspx'>点击查看详细清单</a>
        <hr />
        
        包邮卡共为您带来了<%=totalcount1 %>个二次销售，总金额为<%=totalprice1 %>。<br />

        <a href='salelistfreecard.aspx'>点击查看详细清单</a> | <a href='../freecard/freecardadd.aspx'>立即使用</a>
        <hr />
        
        催单短信共为您带来了<%=totalcount2 %>个销售，总金额为<%=totalprice2 %>。<br />

        <a href='salelistcui.aspx'>点击查看详细清单</a> | <a href='../crm/missionadd.aspx'>立即使用</a>
    </div>
    </form>
</body>
</html>
