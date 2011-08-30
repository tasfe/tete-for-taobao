<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bloglist.aspx.cs" Inherits="top_blog_bloglist" %>

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

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 推广文章列表 </div>
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
    <!--尊敬的用户您好，由于使用本软件推广的人非常多，正常在队列中的文章会在2个小时内发送完毕，最长不超过24个小时，请您耐心等待，谢谢合作~-->
    尊敬的用户您好，近期由于相关博客接口升级，导致部分博客的文章不能自动发送，我们的技术正在加班加点的升级，这段时间给您少发漏发的文章在接口升级完毕后会统一补上，给您带来的不便我们深表抱歉，谢谢您的支持！~
</div>

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
                <td width="240"><b>标题</b></td>
                <td width="60"><b>博客</b></td>
                <td width="110"><b>帐号</b></td>
                <td width="30"><b>类型</b></td>
                <td width="130"><b>日期</b></td>
                <td width="180"><b>状态</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="24"><span title="<%#Eval("blogtitle") %>"><%#left(Eval("blogtitle").ToString(), 18)%></span></td>
                <td><%#getblog(Eval("typ").ToString()) %></td>
                <td><%#Eval("uid") %></td>
                <td><%#showtype(Eval("isauto").ToString()) %></td>
                <td><%#Eval("adddate").ToString() %></td>
                <td><%#show(Eval("sendStatus").ToString(), Eval("blogurl").ToString(), Eval("err").ToString(), Eval("id").ToString(), Eval("typ").ToString(), Eval("isauto").ToString())%></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>

        <!--<input type="button" value="继续推广" onclick="window.location.href='blogadd.aspx'" />  id,blogtitle,typ,uid,adddate,sendStatus,blogurl,err -->
    </div>
</div>
</form>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>