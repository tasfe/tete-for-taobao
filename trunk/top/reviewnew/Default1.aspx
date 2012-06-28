<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default1.aspx.cs" Inherits="top_reviewnew_Default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        .middle-blue {
    background-position: 0 -148px;
    text-shadow: 1px 1px 1px #227AA7;
}

.btn-middle {
    background: url("images/item-botton.png") no-repeat scroll -68px 0 transparent;
    border: medium none;
    color: #FFFFFF;
    cursor: pointer;
    display: inline-block;
    font-size: 14px;
    height: 29px;
    line-height: 29px;
    position: relative;
    text-align: center;
    text-decoration: none;
    width: 87px;
}

.item_list_btn {
    float: right;
    padding-top: 30px;
    width: 92px;
}

.item_list .title_contents, .item_list_adv .title_contents {
    float: left;
    position: relative;
    vertical-align: top;
    width: 250px;
}

.item_list_left_top {
    border-bottom: 1px solid #ABCCDC;
    border-right: 1px solid #C5DCE7;
    margin-bottom: 5px;
}

.item_list_right_top {
    border-bottom: 1px solid #ABCCDC;
    border-left: 1px solid #C5DCE7;
    margin-bottom: 5px;
    margin-left: 10px;
}

.item_list {
    background: none repeat scroll 0 0 #FEFEFE;
    padding-left: 5px;
    vertical-align: top;
}

h4 {
    font-family: "微软雅黑","宋体";
    font-size: 18px;
    padding-left: 5px;
    padding-top: 5px;
}

h4 a {
    color: #004499;
    text-decoration: none;
}

.item_list p, .item_list_adv p {
    color: #555555;
}

.qubie{color:Black; font-size:14px;}

    </style>
</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">

<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">好评秀秀</a> 首页 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content" style="padding:0px; margin:0px;">
            
            首次进入的用户，请点击此处获取您最近的评价记录<asp:Button ID="Button1" 
                runat="server" onclick="Button1_Click" Text="获取评价数据" />
            <br /><br />
            测试人员如果店铺内没有评价数据，请联系客户人员索取测试数据，非常感谢！
            <br /><br />
            <a href='reviewlist.aspx'>查看评价列表</a>
    </div>
</div>

    </form>

</body>
</html>
