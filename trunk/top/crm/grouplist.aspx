<%@ Page Language="C#" AutoEventWireup="true" CodeFile="grouplist.aspx.cs" Inherits="top_crm_grouplist" %>

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
            
  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 客户分组管理 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">
    
        <input type="button" value="添加分组" onclick="window.location.href='groupadd.aspx'" />

        <hr />

    </div>

    
        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>客户昵称</b></td>
                <td width="50"><b>省</b></td>
                <td width="85"><b>手机</b></td>
                <td width="40"><b>性别</b></td>
                <td width="60"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("buynick") %> <img src='level/<%#Eval("buyerlevel") %>.gif' valign="middle" /></td>
                <td><%#Eval("sheng")%></td>
                <td><%#Eval("mobile")%></td>
                <td><%#Eval("tradeamount")%></td>
                <td> <a href='custommodify.aspx?id=<%#Eval("guid")%>' target="_blank">编辑</a> | <a href=''>删除</a> </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>


        </div>
    </div>
    </form>
</body>
</html>
