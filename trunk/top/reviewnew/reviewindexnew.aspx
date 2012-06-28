<%@ Page Language="C#" AutoEventWireup="true" CodeFile="reviewindexnew.aspx.cs" Inherits="top_reviewnew_reviewindex" %>

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
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 首页展示评价 </div>
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
 此处的评价会展示在前台店铺模块里面，最多可以展示20条评价。
 </div>

    <table width="720" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>评价人</b></td>
                <td width="180"><b>内容 </b></td>
                <td width="100"><b>礼品赠送内容</b></td>
                <td width="100"><b>排序号</b></td>
                <td width="70"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="30"><%#Eval("buynick") %></td>
                <td><%#left(Eval("content").ToString())%></td>
                <td><%#Eval("showcontent")%></td>
                <td><input name="index_<%#Eval("orderid")%>" size=4 value="<%#Eval("showindex") %>" /></td>
                <td><a href="reviewindexnew.aspx?act=del&id=<%#Eval("orderid")%>">删除</a></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <input type="hidden" name="action" value="save" />
    <input type=submit value="保存" />
    </div>
</div>
</form>

</body>
</html>
