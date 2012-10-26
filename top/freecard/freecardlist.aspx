<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freecardlist.aspx.cs" Inherits="top_freecard_freecardlist" %>


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

  <div class="crumbs"><a href="../reviewnew/default.aspx" class="nolink">好评有礼</a> 包邮卡 </div>
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
    买家查询包邮卡链接：<a href="http://container.api.taobao.com/container?action=freecard&appkey=12690739&newnick=<%=nickencode %>" target="_blank">http://container.api.taobao.com/container?action=freecard&appkey=12690739&newnick=<%=nickencode %></a>
</div>

                <input type="button" value="创建包邮卡" onclick="window.location.href='freecardadd.aspx'" />
                <input type="button" value="手动赠送" onclick="window.location.href='freecardsend.aspx'" />
    
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>名称</b></td>
                <td width="140"><b>创建时间</b></td>
                <td width="60"><b>免邮时间</b></td>
                <td width="60"><b>使用次数</b></td>
                <td width="60"><b>满金额</b></td>
                <td width="80"><b>地区</b></td>
                <td width="80"><b>已领用</b> </td>
                <td width="100"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("name")%></td>
                <td><%#Eval("adddate")%></td>
                <td><%#Eval("carddate")%>个月</td>
                <td><%#Eval("usecount")%></td>
                <td><%#Eval("price")%>元</td>
                <td><a title='<%#checkArea(Eval("areaisfree").ToString())%>\r\n<%#Eval("arealist")%>'>查看限制地区</a></td>
                <td><%#Eval("sendcount")%></td>
                <td><a href='freecardlist.aspx?act=del&id=<%#Eval("guid")%>' onclick="return confirm('您确定要删除吗，该操作不可恢复？')">删除</a>
                | <a href='freecardcustomer.aspx?id=<%#Eval("guid")%>'>查看赠送记录</a></td>
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