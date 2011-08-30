<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weiboindex.aspx.cs" Inherits="top_microblog_weiboindex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微博营销</title>
    <link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">特特团购</a> 团购微博营销 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    
    <asp:Button ID="Button4" runat="server" Text="关联QQ微博" onclick="Button4_Click" />
    <asp:Button ID="Button6" runat="server" Text="关联新浪微博" onclick="Button6_Click" />
    <br />
    目前已经关联微博：<br />

    <asp:Repeater ID="rptMicroBlog" runat="server">
        <ItemTemplate>
            <%#Eval("uid") %> - <%#Eval("typ") %>  <a href='<%#Eval("pass") %>' target="_blank">查看微博</a> <a href='weiboindex.aspx?act=del&uid=<%#Eval("uid") %>&typ=<%#Eval("typ") %>' onclick='return confirm("您确定吗，该操作不可恢复？")'>取消微博</a> <br />
        </ItemTemplate>
    </asp:Repeater>

    <hr />
        <div style="height:22px;">
            活动开始自动发微博：
            已发布：<%=count1 %>条 
        </div>
        <div id="area1">
            <textarea id="text1" name="text1" runat="server" cols="50" rows="3"></textarea>
        </div>
        <br />
        <div style="height:22px;">
            宝贝售出自动发微博：
            已发布：<%=count2 %>条
        </div>
        <div id="area2">
            <textarea id="text2" name="text2" runat="server" cols="50" rows="3"></textarea>
        </div>
        <br />
        <div style="height:22px;">
            买家好评自动发微博：
            已发布：<%=count3 %>条
        </div>
        <div id="area3">
            <textarea id="text3" name="text3" runat="server" cols="50" rows="3"></textarea>
        </div>
        <br />
        <!--<div style="height:22px;">
            橱窗推荐自动发微博：
             已发布：<%=count4 %>条
        </div>
        <div id="area4">
            <textarea id="text4" name="text4" runat="server" cols="50" rows="3"></textarea>
        </div>-->
        <hr />
    <br />

        <div>
            &nbsp;<asp:Button ID="Button3" runat="server" Text="保 存" 
                onclick="Button3_Click" />
            &nbsp;<asp:Button ID="Button1" runat="server" Text="恢复默认" 
                onclick="Button14_Click" />
        </div>
    </div>
</div>
</form>

</body>
</html>
