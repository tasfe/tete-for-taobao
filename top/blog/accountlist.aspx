<%@ Page Language="C#" AutoEventWireup="true" CodeFile="accountlist.aspx.cs" Inherits="top_blog_accountlist" %>

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

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 帐号管理 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<!--<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold">
    如果您需要使用自动发送请尽量使用百度空间或者搜狐博客，新浪博客可能会被封！
</div>-->

        <!--<input type="button" value="添加新浪博客帐号" onclick="window.location.href='accountadd.aspx'" /> -->
        <input type="button" value="添加QQ空间帐号" onclick="window.location.href='accountaddqq.aspx'" /> 
        <!--<input type="button" value="添加网易博客帐号" onclick="window.location.href='accountadd163.aspx'" />-->
        <input type="button" value="添加百度空间帐号" onclick="window.location.href='accountaddbaidu.aspx'" />
        <input type="button" value="添加搜狐博客帐号" onclick="window.location.href='accountaddsohu.aspx'" /> <br />

                <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30"><strong>帐号</strong></td>
        <td><strong>类型</strong></td>
        <td><strong>发表文章数量</strong></td>
        <td><strong>操作</strong></td>
      </tr>

        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
                 <tr>
                <td><%#Eval("uid") %></td>
                <td><%#Eval("typ") %></td>
                <td><%#Eval("count") %></td>
                <td><a href='accountlist.aspx?id=<%#Eval("id") %>&act=del'>删除</a> </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>

        </table>
    </div>
</div>
</form>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>