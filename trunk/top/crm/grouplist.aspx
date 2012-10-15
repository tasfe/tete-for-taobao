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
        <input type="button" value="更新会员" onclick="window.location.href='grouplist.aspx?act=update'" />

        <hr />

    
        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="80"><b>组名</b></td>
                <td width="120"><b>满足金额</b></td>
                <td width="100"><b>地区</b></td>
                <td width="160"><b>消费时间</b></td>
                <td width="85"><b>会员数</b></td>
                <td width="160"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("name") %> </td>
                <td><%#Eval("price")%>-<%#Eval("priceend")%></td>
                <td><%#Eval("arealist")%></td>
                <td><%#Eval("actdate")%>-<%#Eval("actdateend")%></td>
                <td><%#Eval("count")%></td>
                <td> <a href='groupmodify.aspx?id=<%#Eval("guid")%>'>编辑</a> | <a href='grouplist.aspx?act=del&id=<%#Eval("guid")%>'>删除</a> </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>


        </div>

        </div>
        </div>
    </div>
    </form>
</body>
</html>
