<%@ Page Language="C#" AutoEventWireup="true" CodeFile="info.aspx.cs" Inherits="weibo_info" %>

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
            <h3>个人中心    
            <script src="js_userscore.aspx" type="text/javascript"></script></h3>
          </div>
     
            <br />
            <b><%=str %></b>
            <br />
            您可以使用以下功能：
            <br />
            <a href="listen.aspx">一键收听</a>
            <br />
            <a href="hand.aspx">手动收听</a>
            <br />
            <a href="tuijian.aspx">微博推荐</a>
            <br />
            <a href="log.aspx">积分日志</a>
            
            <br />
            <br />
            积分是您增加粉丝的能量，1点积分可以增加1个粉丝<br />
            只要您使用上面的几个功能不断增加积分，您的粉丝数也会不断增加，如果积分是0那您的粉丝数就不会增加了<br />
            PS：您的粉丝数会慢慢增加，因为一次增加过多会给您带来隐患，大家懂的.. (^.^)
    </form>
    
</body>
</html>

