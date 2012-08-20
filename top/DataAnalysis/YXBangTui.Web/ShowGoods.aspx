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
		height:280px;
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
		margin-top:4px;
		padding-top:4px;
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
                <a href='PClick.aspx?<%# GetParam(Eval("Id").ToString(), Eval("AdsUrl").ToString()) %>' target="_blank"><img src='<%# GetPic(Eval("AdsPic").ToString()) %>' border="0" width="210px" height="210px" /></a><br />
                <span><a href='getclick.aspx?<%# GetParam(Eval("Id").ToString(), Eval("AdsUrl").ToString()) %>' target="_blank"><%# Eval("AdsTitle") %></a></span>
                <div><em>￥<%# Eval("Price") %></em>  <a target="_blank" href='http://amos.im.alisoft.com/msg.aw?v=2&uid=<%# Eval("AliWang") %>&site=cntaobao&s=1&charset=utf-8' ><img align="top" border="0" 
align="absmiddle" src='http://amos.im.alisoft.com/online.aw?v=2&uid=<%# Eval("AliWang") %>&site=cntaobao&s=1&charset=utf-8'  /></a></div>
            </li>
         </ItemTemplate>
       </asp:Repeater>

    	</ul>
    </div>
</div>

<br clear="all" />

 <div style="background-color:#dedede; margin-top:15px">
            <asp:label ID="lblCurrentPage" runat="server"></asp:label>
            <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
            <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
            <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
            <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
        </div>
    
     <asp:Repeater ID="RPT_GOODSCLASS" runat="server" Visible="false">
       <ItemTemplate>
         
       </ItemTemplate>
     </asp:Repeater>
</body>

</html>
