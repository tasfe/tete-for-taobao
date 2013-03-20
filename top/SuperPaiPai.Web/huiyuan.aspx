<%@ Page Language="C#" AutoEventWireup="true" CodeFile="huiyuan.aspx.cs" Inherits="huiyuan" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>拍拍店长</title>
<script type="text/javascript" src="js/common.js"></script>
<link href="Style/customer.css" rel="stylesheet" type="text/css"/>
</head>

<body>
<script type="text/javascript" src="js/nav.js"></script>
<script type="text/javascript" src="js/marketingsetting.js"></script>
<div class="clear_height"></div>
<div class="main">
	<!-- left start -->
	<script id="menujs" type="text/javascript" src="js/setMenu.js?id=28&key1=huiyuan"></script>
	
	<!-- left end -->
  
  <!-- right start -->
  <div class="main_right">

        <div class="notice">
	账号：<%=Nick %> &nbsp;&nbsp;&nbsp;&nbsp;
	可发送短信：<%=CanPoCount %> &nbsp;&nbsp;&nbsp;&nbsp;
	累计已发送：<%=HadPoCount %>&nbsp;&nbsp;&nbsp;&nbsp;
	<a href="msgbuy.aspx">立即充值</a> 
	<div style="float: right;">
	</div>
</div>
<div class="clear_height"></div>     

 			<div class="kuang">
 				<div class="title_list">
					<div class="title_left">快递提醒</div>
					<div class="title_right"><a style="color:#133F9A" href="msgset.aspx?type=5">设置>></a></div>
					<div class="clear"></div>
				</div>
				<div class="contents tag_list" style="padding: 15px 10px;">
					<input type="hidden" value="【特特营销推广专家】" id="shopName"/>
					<table width="100%">
						<tr>
							<td>
								为了提高服务质量、增加买家对店铺的好感，当快递发出的时候提醒用户，将自动给买家发送一条已经发货的短信，让买家及时掌握订单发货状态！
							</td>
							<td width="100px">
								
									<a id="setting_5" href='<%=PostJsHref %>'><img id="setting_5 img" width="122" height="48" border="0" src='<%=PostImgSrc %>'/></a>
								    
							</td>
						</tr>
					</table>
				</div>
 			</div>
 			<div class="clear_height"></div>
 			<div class="kuang">
 				<div class="title_list">
					<div class="title_left">提醒买家确认收货</div>
					<div class="title_right"><a class="" href="msgset.aspx?type=9">设置>></a></div>
					<div class="clear"></div>
				</div>
				<div class="contents tag_list" style="padding: 15px 10px;">
					<table width="100%">
						<tr>
							<td>
								当买家签收货品后，自动提醒对订单的确认，以提高回款速度！
							</td>
							<td width="100px">
								
									<a id="setting_9" href='<%=NoPingJsHref %>'><img id="setting_9 img" width="122" height="48" border="0" src='<%=NoPingImgSrc %>' /></a>
								    
							</td>
						</tr>
					</table>	
				</div>
 			</div>
 			<div class="clear_height"></div>
 			<div class="kuang">
 				<div class="title_list">
					<div class="title_left">买家付款提醒</div>
					<div class="title_right"><a class="" href="msgset.aspx?type=1">设置>></a></div>
					<div class="clear"></div>
				</div>
				<div class="contents tag_list" style="padding: 15px 10px;">
					<table width="100%">
						<tr>
							<td>
								当买家下单后一段时间内未付款进行友好提醒！
							</td>
							<td width="100px">
								
									<a id="setting_1" href='<%=NoPayJsHref %>'><img id="setting_1 img" width="122" height="48" border="0" src='<%=NoPayImgSrc %>' /></a>
								    
							</td>
						</tr>
					</table>	
				</div>
 			</div>
 			
 		</div><!-- main_right end -->
	</div>

</div>
</div>

<script type="text/javascript" src="js/default.js"></script>
<script type="text/javascript" src="js/footer.js"></script>
</body>
</html>
