<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowGoods.aspx.cs" Inherits="ShowGoods" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title>宝贝展示</title>
</head>
<style>
	a {
		color: #0063DC;
		cursor: pointer;
		text-decoration: none;
	}
	.showlist{
		list-style:none;
		margin:0px;
		padding:0px;
	}
	.showlist li{
		float:left;
		padding:10px;
		border:solid 1px #ccc;
		margin:2px;
		width: 210px;
	}
	.showlist li img{
		border:0px;
	}
	.showlist li span{
		font-size: 14px;
		font-weight: normal;
		height: 42px;
		overflow: hidden;
		width: 210px;	
	}
	.showlist li div{
		line-height: 22px;
		vertical-align:middle;
		padding-top:6px;
	}
	.showlist li em{
		color: #FF2900;
		font: bold 16px Arial;
	}
	.left{
	}
	.right{
	}
</style>
<body>

<div>
	<div class="left"></div>
    <div class="right">
    	<ul class="showlist">

       <asp:Repeater ID="RPT_AdsList" runat="server" Visible="false">
         <ItemTemplate>
         <li>
                <a href="#"><img src='http://img04.taobaocdn.com/bao/uploaded/i4/T1RBu9XeRoXXa3rKg8_100228.jpg_210x210.jpg' width="210" height="210" /></a><br />
                <span><a href="#"><%# Eval("goodsname") %><</a></span>
                <div><em>￥4250.40</em>  <a target="_blank" href="http://amos.im.alisoft.com/msg.aw?v=2&uid=%E7%BE%8E%
E6%9D%9C%E8%8E%8E%E4%B9%8B%E5%BF%83&site=cntaobao&s=1&charset=utf-8" ><img align="top" border="0" 
align="absmiddle" src="http://amos.im.alisoft.com/online.aw?v=2&uid=%E7%BE%8E%E6%9D%9C%E8%
8E%8E%E4%B9%8B%E5%BF%83&site=cntaobao&s=1&charset=utf-8" alt="有问题请点这里" /></a></div>
            </li>
         </ItemTemplate>
       </asp:Repeater>

    	</ul>
    </div>
</div>


    
     <asp:Repeater ID="RPT_GOODSCLASS" runat="server">
       <ItemTemplate>
         
       </ItemTemplate>
     </asp:Repeater>
</body>

</html>
