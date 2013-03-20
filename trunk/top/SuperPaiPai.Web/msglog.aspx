<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msglog.aspx.cs" Inherits="msglog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>拍拍店长</title>
<script type="text/javascript" src="js/common.js"></script>
<script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>

<body>
<form runat="server">
<script type="text/javascript" src="js/nav.js"></script>
<div class="clear_height"></div>
<div class="main">
	<!-- left start -->
	<script id="menujs" type="text/javascript" src="js/setMenu.js?id=28&key1=msglog"></script>
	
	<!-- left end -->
  
  <!-- right start -->
  <div class="main_right">

    <div class="notice">
	账号：<%=Nick %> &nbsp;&nbsp;&nbsp;&nbsp;
	可发送短信：<%=CanPoCount %> &nbsp;&nbsp;&nbsp;&nbsp;
	累计已发送：<%=HadPoCount %>&nbsp;&nbsp;&nbsp;&nbsp;
	<a href="msgbuy.aspx" target="_blank">立即充值</a> 
	<div style="float: right;">
	</div>
</div>
<div class="clear_height"></div>

 			
 			<div id="tab_nav2">
 				<ul id="nav">
					<li><a href="msglog.aspx?type=5" class='<%=PostCss %>'>发货通知</a></li>
					<li><a href="msglog.aspx?type=9" class='<%=PingCss %>'>确认评价提醒</a></li>
					<li><a href="msglog.aspx?type=1" class='<%=PayCss %>'>未付款催单提醒</a></li>
				</ul>
				
				<div id="query_v">
						<input type="hidden" name="type" value="5"/>
						<table style="width: 100%;" class="shoplist cycle">
							<tr style="background-color: white;">
								<td width="80" style="text-align: right;">收货人QQ号：</td>
								<td width="100">
								    <asp:TextBox ID="txtQQ" runat="server"></asp:TextBox>
                                </td>
								<td width="80" style="text-align: right;">查询时间：</td>
								<td width="260">
									<asp:TextBox runat="server" ID="TB_StartTime" Width="70px" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />至
        <asp:TextBox ID="TB_EndTime" runat="server" Width="70px" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" />
								</td>
								<td>
									<asp:Button ID="Btn_Select" runat="server" CssClass="btn-small small-blue" 
                                        Text="查询" onclick="Btn_Select_Click" />
								</td>
							</tr>
						</table>
				</div>
				
				<table width="100%" border="0" cellspacing="0" cellpadding="0" class="shoplist cycle">
					<tr>
						<td width="150">QQ号</td>
						<td>手机号</td>
						<td>发送内容</td>
						<td>发送时间</td>
					</tr>
					<asp:Repeater ID="Rpt_Msg" runat="server">
					<ItemTemplate>
					  <tr>
					    <td><%# Eval("uId") %></td>
						<td><%# Eval("Mobile") %></td>
						<td><%# Eval("PostMsg") %></td>
						<td><%# Eval("AddDate") %></td>
					  </tr>
					  </ItemTemplate>
					</asp:Repeater>
				</table>
				  <div style="background-color:#dedede; margin-top:15px">
                    <asp:label ID="lblCurrentPage" runat="server"></asp:label>
                    <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
                    <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
                    <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
                    <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
                  </div>
 			</div>
 			
 		</div><!-- main_right end -->
	</div>
<script type="text/javascript" src="js/default.js"></script>
<script type="text/javascript" src="js/footer.js"></script>
</form>
</body>
</html>
