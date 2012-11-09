<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initcustombycsv1.aspx.cs" Inherits="top_crm_initcustombycsv" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 通过订单CSV文件导入客户信息 </div>
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
    请上传淘宝后台导出的订单CSV文件来导入历史会员数据
</div>

<asp:FileUpload ID="fuAlipay" Width="200px" runat="server" />  当历史数据很大的时候导入时间会很长，请您耐心等待。。。

<br />
<asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="导入会员数据" OnClientClick="check(this)" />


<script>
    function check(obj) {
        obj.value = "导入中，请您耐心等待..";
        //obj.disabled = true;
    }
</script>

    </div>
</div>
    </div>
    </form>
</body>
</html>
