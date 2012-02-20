<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customlist.aspx.cs" Inherits="top_crm_customlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特特CRM_客户营销</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户营销</a> 客户列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    请输入客户昵称：<asp:TextBox ID="search" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="搜索" />
    <hr />
    <div style="margin-bottom:10px;">
        <input type="checkbox" onclick="selectAll()" />
        <input type="button" value="为选中客户赠送优惠券" onclick="setOK()" />
    </div>
        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="30"><input type="checkbox" onclick="selectAll()" /></td>
                <td width="100"><b>客户昵称</b></td>
                <td width="40"><b>性别</b></td>
                <td width="60"><b>生日</b></td>
                <td width="50"><b>等级</b></td>
                <td width="50"><b>交易量</b></td>
                <td width="50"><b>交易额</b></td>
                <td width="60"><b>最后交易</b></td>
                <td width="60"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><input name="id" type="checkbox" value="<%#Eval("guid") %>" /></td>
                <td><%#Eval("buynick") %> <img src='level/<%#Eval("buyerlevel") %>.gif' /></td>
                <td><%#getsex(Eval("sex").ToString())%></td>
                <td><%#Eval("birthday").ToString().Replace(" 00:00:00.000", "")%></td>
                <td><%#Eval("lastorderdate").ToString().Replace(" 0:00:00", "")%></td>
                <td><%#getgrade(Eval("grade").ToString())%></td>
                <td><%#Eval("tradecount")%></td>
                <td><%#Eval("tradeamount")%></td>
                <td><%#Eval("lastorderdate").ToString().Replace(" 0:00:00", "")%></td>
                <td>
                    <a href="#">修改</a>
                </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    </div>
</div>
    </div>
    </form>
</body>
</html>
