<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowGoods.aspx.cs" Inherits="ShowGoods" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>广告展示</title>
</head>
<style type="text/css">
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

       <asp:Repeater ID="RPT_AdsList" runat="server">
         <ItemTemplate>
         <li>
                <a href="#"><img src='<%# Eval("AdsPic") %>' width="210" height="210" /></a><br />
                <span><a href='<%# Eval("AdsUrl") %>'><%# Eval("AdsTitle") %></a></span>
                <div> <a target="_blank" href='http://amos.im.alisoft.com/msg.aw?v=2&uid=<%# Eval("AliWang") %>&site=cntaobao&s=1&charset=utf-8' ><img align="top" border="0" 
align="absmiddle" src='http://amos.im.alisoft.com/online.aw?v=2&uid=<%# Eval("AliWang") %>&site=cntaobao&s=1&charset=utf-8'  /></a></div>
            </li>
         </ItemTemplate>
       </asp:Repeater>

    	</ul>
    </div>
</div>


    
     <asp:Repeater ID="RPT_GOODSCLASS" runat="server" Visible="false">
       <ItemTemplate>
         
       </ItemTemplate>
     </asp:Repeater>
</body>

</html>
