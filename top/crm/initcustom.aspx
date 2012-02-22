<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initcustom.aspx.cs" Inherits="top_crm_initcustom" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特特CRM_客户营销</title>
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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 导入历史数据 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
            <asp:Button ID="Button1" runat="server" Text="导入历史客户" 
                onclick="Button1_Click1" />
    </div>
    </div>
    </div>
    </form>
</body>
</html>
