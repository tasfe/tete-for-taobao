<%@ Page Language="C#" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=nick %>的淘营销个人中心</title>
    <style>
    .head {
background-position:left top;
background-repeat:repeat-x;
}

.head {
background-repeat:repeat-x;
height:77px;
position:relative;
width:100%;
}

.head {
background:url("images/back.png") repeat-x scroll transparent;
}

html, body, div, span, object, iframe, h1, h2, h3, h4, h5, h6, p, blockquote, pre, a, abbr, acronym, address, code, del, dfn, em, img, q, dl, dt, dd, ol, ul, li, fieldset, form, label, legend, table, caption, tbody, tfoot, thead, tr, th, td {
border:0 none;
font-family:inherit;
font-size:100%;
font-style:inherit;
font-weight:inherit;
margin:0;
padding:0;
text-decoration:none;
vertical-align:baseline;
}

.head h1 {
height:44px;
left:22px;
position:absolute;
top:19px;
width:133px;
font-size:22px;
}

.head h2 {
color:#2C2C2C;
font-size:14px;
font-weight:700;
left:178px;
position:absolute;
top:45px;
}

.head .nav {
height:18px;
line-height:18px;
padding-top:4px;
position:absolute;
right:12px;
top:4px;
}

ul {
list-style:none outside none;
}

.r-container .inner-content {
background:none repeat scroll 0 0 #FAFDFE;
border:1px solid #FFFFFF;
overflow:hidden;
padding:14px 0 9px 8px;
}

.r-container {
background:none repeat scroll 0 0 #F3F5FD;
margin-bottom:9px;
padding:2px;
position:relative;
margin:8px 0 0 8px;
border:1px solid #D3DCE5;
float:left;
}

.r-container h4 {
color:#000000;
font-size:14px;
font-weight:700;
padding-left:5px;
}

.pro-list li {
font-size:14px;
font-weight:700;
height:42px;
line-height:42px;
padding-left:25px;
}

.pro-list {
margin-top:10px;
}

a {
color:#003C9B;
}

.foot {
border-top:1px solid #CFE1E9;
clear:both;
color:#86A0BA;
font-family:Arial;
font-size:12px;
margin:75px 0 0 83px;
padding:16px 0;
text-align:center;
width:940px;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="head">
  <h1><strong>淘营销 V1.0</strong></h1>
  <h2><a href="#"><%=nick %>的个人中心</a>，<a href="logout.aspx">退出</a></h2>
</div>

<div style="height:500px;">
    <div class="r-container" style="width:204px;">
              <div class="inner">
                <div class="inner-content">
                  <div style="" class="user-service-used">
                    <h4>用户资料营销</h4>
                    <ul id="useddiv" class="pro-list">
                    
                    <li class="rightC"><a href="User/getUserList.aspx" target="actWindow">客户资料导出</a></li>
                    <li class="rightC"><a href="User/getUserInfo.aspx" target="actWindow">测试客户资料导出</a></li>
                    <li class="rightC"><a href="User/getUserList.aspx" target="actWindow">历史数据查看</a></li>
                    <li class="rightC"><a href="User/MobileSend.aspx" target="actWindow">短信群发</a></li>
                    <li class="rightC"><a href="#" target="_blank">购买此软件</a></li>

                  </div>
                </div>
              </div>
            </div>

            <div class="r-container" style="width:704px; ">
            <div class="inner">
                <div class="inner-content" style="height:480px">
                    <iframe name="actWindow" src="User/getUserList.aspx" width="680" height="470" frameborder="0"></iframe>
            </div></div>
            </div>
</div>

            <div class="foot">
             &copy;2011 tetesoft.com
        </div>

    </form>
</body>
</html>
