﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="missionlist.aspx.cs" Inherits="top_crm_missionlist" %>


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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 营销计划列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>


    <div id="main-content">
    
        <input type="button" value="创建营销计划" onclick="window.location.href='missionadd.aspx'" />
        <hr />

        <table width="740" cellpadding="0" cellspacing="0">
            <tr>
                    <td width="100"><b>任务类型</b></td>
                    <td width="100"><b>会员组</b></td>
                    <td width="100"><b>创建时间</b></td>
                    <td width="100"><b>状态</b></td>
                    <td width="100"><b>操作</b></td>
                </tr>
            <asp:Repeater ID="rptArticle" runat="server">
                <ItemTemplate>
                <tr>
                    <td height="35"><%#Eval("typ") %></td>
                    <td><%#Eval("group")%></td>
                    <td><%#Eval("adddate")%></td>
                    <td><%#Eval("isact")%></td>
                    <td>编辑 | 删除</td>
                </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>

    </div>
    </div>
    </div>
    </form>
</body>
</html>