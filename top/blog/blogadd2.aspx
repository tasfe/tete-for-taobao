<%@ Page Language="C#" AutoEventWireup="true" CodeFile="blogadd2.aspx.cs" Inherits="top_blog_blogadd2" ValidateRequest="false" EnableViewStateMac="false"  enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 我要推广 (3.填写博客帐号密码) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <input type="hidden" name="ads" value="<%=ads %>" />
        <asp:TextBox ID="tbKey" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="tbTitle" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="FCKeditor1" runat="server" Visible="false"></asp:TextBox>
        <asp:Label ID="lbErrMessage" runat="server"></asp:Label>

        <asp:Panel ID="panel1" runat="server" Visible="false">
            您可以在此修改您新浪博客的帐号密码：<br />
            <span style="color:#ccc">为了您的账户安全，特特不会记录客户真实密码，如果您不放心可以<a href='http://login.sina.com.cn/signup/signupmail.php?entry=blog&r=&srcuid=&src=blog' style="color:#aaa" target="_blank">重新注册一个帐号</a>用于推广：）</span><br />
            帐 号：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
            密 码：<asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox><br />
            <a href='http://login.sina.com.cn/signup/signupmail.php?entry=blog&r=&srcuid=&src=blog' target="_blank">没博客帐号，现在就去注册一个</a><br />
        </asp:Panel>

        
        <asp:Panel ID="panel2" runat="server">
            <!--<input type="button" value="添加新浪博客" onclick="OpenDialogLable('accountadd.aspx?isdialog=1',450,360);"  />-->
            <input type="button" value="添加QQ空间帐号" onclick="OpenDialogLable('accountaddqq.aspx?isdialog=1',450,360);"  />
            <input type="button" value="添加百度空间帐号" onclick="OpenDialogLable('accountaddbaidu.aspx?isdialog=1',450,360);"  />
            <input type="button" value="添加搜狐博客帐号" onclick="OpenDialogLable('accountaddsohu.aspx?isdialog=1',450,360);"  />
            <br />
            请选择您用于发布的博客帐号：<br />
            <div id="accountArea">
            <asp:Repeater ID="rptAccount" runat="server">
                <ItemTemplate>
                    <input id="account<%#Eval("id") %>" type="checkbox" checked="checked" name="account" value="<%#Eval("uid") %>" /> <label for="account<%#Eval("id") %>"><%#Eval("uid") %></label> - <%#Eval("typ") %> <br />
                </ItemTemplate>
            </asp:Repeater>
            </div>
        </asp:Panel>
        <br />
        <input type="button" value="后退" onclick="history.go(-1)" />
        <asp:Button ID="btnSearch" runat="server" Text="自动发送博文" onclick="btnSearch_Click" />
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function submitForm() {
        document.getElementById("form1").action = "blogadd2.aspx";
        document.getElementById("form1").submit();
    }
    
    function OpenDialogLable(url, w, h, editTxt) {
        if (typeof (editTxt) == "undefined") {
            editTxt = "";
        }
        if (navigator.appVersion.indexOf("MSIE") == -1) {
            this.returnAction = function (strResult) {
                if (strResult != null) {
                    if (strResult != "") {
                        document.getElementById("accountArea").innerHTML += strResult;
                    }
                }
            }
            window.open(url + '&d=' + Date() + "&t=" + escape(editTxt), 'newWin', 'modal=yes,width=' + w + ',height=' + h + ',top=200,left=300,resizable=no,scrollbars=no');
            return;
        } else {
            var GetValue = showModalDialog(url + '&d=' + Date() + "&t=" + escape(editTxt), null, 'dialogWidth:' + w + 'px; dialogHeight:' + h + 'px;')
            if (GetValue != null) {
                if (GetValue != "") {
                    document.getElementById("accountArea").innerHTML += GetValue;
                }
            }
        }
    }
</script>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>
