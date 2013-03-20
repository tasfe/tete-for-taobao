<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgset.aspx.cs" Inherits="msgset" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>拍拍店长</title>
<script type="text/javascript" src="js/common.js"></script>

    <style type="text/css">
        .middle-red
        {
            height: 26px;
        }
    </style>

</head>

<body>
<script type="text/javascript" src="js/nav.js"></script>
<form runat="server">
<div class="clear_height"></div>
<div class="main">
	<!-- left start -->
	<script id="menujs" type="text/javascript" src="js/setMenu.js?id=28&key1=msgbuy"></script>
	
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
					<div class="title_left">短信模板设置</div>
					<div class="title_right">
						<a href="huiyuan.aspx">&lt;&lt;返 回</a>
					</div>
					<div class="clear"></div>
				</div>
				<div class="contents tag_list" style="padding: 15px 10px;">
					<div id="tab_nav">
						<input type="hidden" id="template_type"
							value="1" />
						
						<div class="tab_contents">
							
								



<div class="tab_text" style="display: block;">
<input type="hidden" id="shopName" value="特特营销推广专家"/>


<asp:Panel ID="panel1" runat="server" Visible="false">
	<span class="notify_notice">当卖家发货后短信通知买家货已发出</span>
	<div class="clear_height"></div>
    <div class="template_action">
	    <table width="100%">
		    <tr>
			    <td width="410"><textarea id="template_content5" name = "content"  runat="server"
					    style="width: 100%; height: 100px;">亲！您购买的宝贝已发货，请注意查收！如有问题请及时联系我们。物流公司：(****(物流公司))，运单号：(****(运单号))。【(****(店铺名))】</textarea></td>
			    <td valign="bottom"></td>
		    </tr>
	    </table>
    </div>
</asp:Panel>

<asp:Panel ID="panel2" runat="server" Visible="false">
	<span class="notify_notice">当订单发货多日后买家依然未评价，自动提醒对订单的确认，以提高回款速度！</span>
	<div class="clear_height"></div>
    <div style="margin-left: 10px;">
		发货<input value="" class="text"  runat="server" size="5" id="pay_hour" name="setting_param_4" type="text" vtype="num" vname="提醒小时"/>天后未评价提醒确认
	</div>
	<div class="clear_height"></div>
    <div class="template_action">
	    <table width="100%">
		    <tr>
			    <td width="410"><textarea id="template_content9" name = "content" runat="server"
					    style="width: 100%; height: 100px;">亲！您对在我店购买的宝贝收到了吗？还满意吗？麻烦您给我们及时确认收货并给评价一下我们的宝贝，非常感谢。【(****(店铺名))】</textarea></td>
			    <td valign="bottom"></td>
		    </tr>
	    </table>
    </div>
</asp:Panel>

<asp:Panel ID="panel3" runat="server" Visible="false">
	<span class="notify_notice">当买家下单后一段时间内未付款进行友好提醒！</span>
	<div class="clear_height"></div>
    <div style="margin-left: 10px;">
		买家下单<input value="" class="text" runat="server" size="5" id="Text1" name="setting_param_4" type="text" vtype="num" vname="提醒小时"/>分钟后未付款提醒付款
	</div>
	<div class="clear_height"></div>
    <div class="template_action">
	    <table width="100%">
		    <tr>
			    <td width="410"><textarea id="template_content1" name = "content" runat="server"
					    style="width: 100%; height: 100px;">您好，您拍下的宝贝我一直为您留着，麻烦您确认一下，付款后我们会立即安排发货，谢谢。【(****(店铺名))】</textarea></td>
			    <td valign="bottom"></td>
		    </tr>
	    </table>
    </div>
</asp:Panel>

<div style="margin-top: 10px;margin-left: 7px;">
    <asp:Button ID="Btn_Save" runat="server" Text="保 存" CssClass="btn-middle middle-red" 
        onclick="Btn_Save_Click" />	
	&nbsp;&nbsp;&nbsp;&nbsp;
</div>

</div>
							
<div class="tab_text" style="display: none;"></div>
							

							<div style="height: 300px;">
								<p style="color: red;">提示：</p>
								<p>1、{ShopName}(指店铺名称)、{ExpressName}(指快递公司)、{ExpressNo}(指快递单号)，是指发送信息时的根据您的信息自动替换这些内容</p>
								<p>2、一条短信不能超过64个字符，如果超过则会发前64个字符。</p>
							</div>
						</div>
					</div>
				</div>
			</div>

		</div>
		<!-- main_right end -->
	</div>


</div>
</form>
<script type="text/javascript" src="js/default.js"></script>
<script type="text/javascript" src="js/footer.js"></script>
</body>
</html>
