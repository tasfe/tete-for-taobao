<%@ Page Language="C#" AutoEventWireup="true" CodeFile="missionlist.aspx.cs" Inherits="top_groupbuy_missionlist" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特团购</a> 任务列表 </div>
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
        如果一个任务里失败数很多的话，有可能是宝贝描述里面加入了太多的团购代码，您可以在 <a href='grouplist.aspx'>团购列表</a> 使用“清除关联描述 ”清除一些不用的或者过期的团购代码
    </div>

    <table width="100%" cellpadding="0" cellspacing="0" style="margin-top:6px;">
        <tr>
                <td width="120"><b>团购名称</b></td>
                <td width="100"><b>图片</b></td>
                <td width="60"><b>任务类型</b></td>
                <td width="60"><b>更新总数</b></td>
                <td width="60"><b>完成进度</b></td>
                <td width="60"><b>失败数</b></td>
                <td width="60"><b>开始时间</b></td>
                <td width="60"><b>状态</b></td>
                <td width="60"><b>日志</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="100" style="font-size:14px;"><%#Eval("groupbuyname").ToString()%></td>
                <td><a href='http://item.taobao.com/item.htm?id=<%#Eval("itemid").ToString() %>' target="_blank"><img style="border:solid 1px #000;" width="80" height="80" src='<%#Eval("groupbuypic").ToString() %>_80x80.jpg' alt='<%#Eval("groupbuyname").ToString() %>' border="0" /></a></td>
                <td><%#typ(Eval("typ").ToString())%></td>
                <td style="color:Red; font-weight:bold; font-size:14px"><%#Eval("total").ToString() %></td>
                <td> <%#Eval("success").ToString() %> </td>
                <td>  <%#Eval("fail").ToString() %> </td>
                <td><%#Eval("startdate").ToString() %></td>
                <td><%#result(Eval("isok").ToString())%></td>
                <td>
                    <a target="_blank" href="ErrLog/<%#typSuccess(Eval("typ").ToString())%><%#typSTRfile(Eval("groupbuyid").ToString())%>">查看成功日志</a>
                <br /><a target="_blank" href="ErrLog/<%#typErr(Eval("typ").ToString())%><%#typSTRfile(Eval("groupbuyid").ToString())%>"> 查看错误日志</a>
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
