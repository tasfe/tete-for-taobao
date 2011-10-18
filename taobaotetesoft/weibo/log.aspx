﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="log.aspx.cs" Inherits="weibo_log" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style>
        .c-title {
border-bottom:1px solid #CDD7D9;
height:28px;
line-height:28px;
margin-bottom:20px;
position:relative;
}

.c-title h3 {
color:#000000;
font-size:14px;
font-weight:700;
}

.c-title a {
outline:medium none;
position:absolute;
right:2px;
font-size:14px;
text-decoration:none;
top:0;
}
a {
color:#003C9B;
}

p {
overflow:visible;
position:relative;
border-bottom:1px dotted #B1B3B4;
overflow:hidden;
padding:10px 0;
font-size:14px;
}

.form-list {
margin:0px 1px 0 25px;
overflow:hidden;
}

.form-label {
float:left;
padding-right:5px;
text-align:right;
width:70px;
}

.submit {
background:none repeat scroll 0 0 #FFFFFF;
border:0 none;
margin-top:-3px;
padding-left:84px;
z-index:99;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="c-title">
            <h3>积分日志
            <script src="js_userscore.aspx" type="text/javascript"></script></h3>
            <a href="info.aspx" id="pa:goCenter">个人中心首页&gt;&gt;</a>
          </div>

          <table width="600" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>内容</b></td>
                <td width="60"><b>积分 </b></td>
                <td width="60"><b>时间 </b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="18"><%#message(Eval("typ").ToString(), Eval("bak").ToString())%></td>
                <td ><%#Eval("num")%></td>
                <td ><%#Eval("adddate")%></td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    
    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>
    </form>
    
</body>
</html>