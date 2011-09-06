<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deletegrouplist.aspx.cs" Inherits="top_groupbuy_deletegrouplist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">特特营销推广专家</a> 取消活动列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>


    <div id="main-content">
<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold">
    尊敬的用户您好，新的站内打折功能正在测试中，如果遇到什么问题请尽快与我们的旺旺客服联系，谢谢~：）
</div>

        <input type="button" value="添加活动" onclick="window.location.href='groupbuyadd.aspx'" />
    <hr />

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
                <td width="150"><b>活动名称</b></td>
                <td width="100"><b>宝贝</b></td>
                <td width="60"><b>原价</b></td>
                <td width="60"><b>折扣价</b></td>
                <td width="80"><b>开始时间</b></td>
                <td width="80"><b>结束时间</b></td>
                <td width="120"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="90"><%#Eval("name").ToString()%></td>
                <td>
                    <a href='http://item.taobao.com/item.htm?id=<%#Eval("productid").ToString() %>' target="_blank"><img width="80" height="80" src='<%#Eval("productimg").ToString() %>_80x80.jpg' alt='<%#Eval("productname").ToString() %>' border="0" /></a> 
                </td>
                <td><s><%#Eval("productprice").ToString() %></s></td>
                <td style="color:Red; font-weight:bold; font-size:14px"><%#Eval("groupbuyprice").ToString() %></td>
                <td><%#Eval("starttime")%></td>
                <td><%#Eval("endtime").ToString() %></td>
                <td>
                    <%#checkdel(Eval("id").ToString(), Eval("isdelete").ToString())%>
                </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>

    </div>
</div>
</form>

</body>
</html>
