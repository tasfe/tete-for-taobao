<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitygetitem.aspx.cs" Inherits="top_groupbuy_activitygetitem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>管理促销宝贝</title>
  <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 
        <link href="js/all-min.css" rel="stylesheet" /> 
    <script type="text/javascript" src="js/cal.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="msg">
	<p class="alert">提示：同一商品只能参加一种促销活动，您可以删除原有活动，再添加新的促销活动。</p>
</div>
<div class="explain">
	<div style="margin-bottom: 5px;">
		<strong>当前促销活动名称：<%=activitystatusstr %></strong> <input type="hidden" name="activityIDstr" id="activityIDstr" value="" runat="server" />
	</div>
	<ul>
		<li>活动时间：<%=activitystatrdatestr%> 到 <%=activityenddatestr%></li>
        
        <li>促销形式：<%=activitydiscountTypestr%></li>
        <li>促销力度：<%=activitydiscountValuestr%></li>
		<li>优惠数量限制：<%=activitydecreaseNumstr%> <%=addactivity%></li>
        
		<li>目标客户：全淘宝网会员    </li>
	</ul>
</div>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin:2px 10px 2px 10px;">
            <tr>
            <td valign="top" style="padding-left:10px;">
            
            <input type="hidden" id="pagenow" value="1" />


    <table width="800px;" border="0" cellspacing="0" cellpadding="0" style="margin:4px; padding:0px">			
   <tr>
                 
					  <td  width="100px">商品图片</td>
		                <td  width="140px">名称</td>
                        <td  width="70px">原价(元)</td>
 
                        <td  width="70px">优惠类型</td>
                        <td  width="70px">优惠幅度</td>
                        <td  width="70px">优惠数量</td>
		                <td  width="70px">参团人数</td>
		                <td  width="140px"> 操作</td>
						</tr>
                       
                         </table>
				  <hr/>
 
    <table width="800px;" border="0" cellspacing="0" cellpadding="0" style="margin:4px; padding:0px">	
                     
                            <asp:Repeater ID="rptItems" runat="server">
                                <ItemTemplate>
        	                      <tr>
                                    <td width="100px"><a href="<%#Eval("ProductUrl").ToString()%>" target="_blank"><img src="<%#Eval("ProductImg").ToString()%>" width="80px;" height="80px;" /></a></td>
                                    <td  width="140px"><%#Eval("Productname").ToString()%></td>
                                    <td  width="70px">
                                     <%#Eval("Productprice").ToString()%> 
                                    </td>
                                    <td  width="70px"><%# Eval("itemType").ToString() != "same" ? "不同促销形式" : Eval("discountType").ToString() == "DISCOUNT"?"打折":"减价"%></td>
                                    <td  width="70px"><%# Eval("itemType").ToString() != "same" ? "不同促销力度" : Eval("discountType").ToString() == "DISCOUNT" ? Eval("discountValue").ToString() + "折" : Eval("discountValue").ToString() + "元"%></td>
                                    <td  width="70px"><%#Eval("decreaseNum").ToString()== "0" ? "多件" :"一件"%> </td>
                                    <td  width="70px"> <%#Eval("Rcount").ToString()%> </td>
                           
                                    <td  width="140px">
                                        <div id="del"></div>
                                        <%# outShowHtml(Eval("ActivityID").ToString(), Eval("ID").ToString(), Eval("ProductID").ToString())%>
                                    </td>
                                 </tr>
                                </ItemTemplate>
                            </asp:Repeater>
 
    </table>
       <div>
                        <asp:Label ID="lbPage" runat="server"></asp:Label>
                    </div>
        	    
            </td>
          </tr>
     
    </table>
    </form>
    <script type="text/javascript">
        function delItemAction(iid) {
            //if(!shortAuth(1335686645139))return;
            var actionID = document.getElementById("activityIDstr").value;
            $.ajax({
                url: 'LoadAjax.aspx?actionId=' + actionID + '&iid=' + iid + '&actionType=del&t=' + new Date().getTime() + '',
                type: 'GET',
                dataType: 'text',
                async: true,
                timeout: 2000000,
                beforeSend: function () {
                    $('#del' + iid).html('正在删除...');
                },
                error: function () {
                    alert('网络错误，请重试！');
                },
                success: function (msg) {
                    if (msg == 'true') {
                        $('#del' + iid).hide();

                    } else {
                        $('#del' + iid).html('删除失败:' + msg + '<a href="javascript:delItemAction(' + iid + ')">重试</a>');
                    }
                }
            });
        } 

    </script>
</body>
</html>
