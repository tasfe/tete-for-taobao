<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activityList.aspx.cs" Inherits="top_groupbuy_activityList" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特特团购</title>
    <link href="../css/common.css" rel="stylesheet" />
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/all-min.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 

    <script language="javascript" type="text/javascript" src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/cal.js"></script>
    <style type='text/css'> 
	#simplemodal-container a.modalCloseImg {
	background:url(http://img01.taobaocdn.com/imgextra/i1/570102268/T2XgxuXjtMXXXXXXXX_!!570102268.png) no-repeat; /* adjust url as required */
	width:25px;
	height:29px;
	display:inline;
	z-index:3200;
	position:absolute;
	top:-15px;
	right:-18px;
	cursor:pointer;
	}
</style>
  
</head>
<body style="padding:0px; margin:0px;">
<form id="form1" runat="server"> 

<div class="navigation">
  <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 促销活动管理 </div>

    
    <div id="main-content">

        <div class="msg24">
					<p class="tips">
						<span>您可以根据自己的需要定制各种促销活动</span>
						<span style="padding-left:20px;"><a href="activityadd.aspx" class="long-btn">创建新活动</a></span>
					</p>
				</div>	
              
        
  <div class="data-list">
    <div class="hd">
		<ul class="tabs">
			<li id="li1" class="selected"><a hidefocus="true" href="activityList.aspx?actionType=all">查看所有活动</a></li>
			<li id="li2"><a hidefocus="true" href="activityList.aspx?actionType=before">未开始的活动</a></li>
			<li id="li3"><a hidefocus="true" href="activityList.aspx?actionType=start">进行中的活动</a></li>
			<li id="li4"><a hidefocus="true" href="activityList.aspx?actionType=pause">已暂停的活动</a></li>
            <li id="li5"><a hidefocus="true" href="activityList.aspx?actionType=stop">已结束的活动</a></li>
         
              
		</ul>
	</div>

         <div class="bd" style=" width:760px;">
			<div id="J_DataTabelDiv" style=" width:760px;">
				<table id="J_DataTable" style=" width:760px;">
					<thead id="J_ProductsHead" style=" width:760px;">
						<tr style=" width:760px;">
							<th>活动名称</th>
							<th>活动时间</th>
							<th>优惠类型</th>
							<th>优惠幅度</th>
							<th>目标客户</th>
					  	 
							<th>添加商品</th>
							<th>管理商品</th>
							<th>活动管理</th>
						</tr>

                        <asp:Repeater ID="rptArticle" runat="server">
                            <ItemTemplate>
                                 <tr style=" width:760px;">
                                    <td><%#Eval("Name").ToString()%></td>
                                    <td>
                                    开始：<%#Eval("startDate").ToString()%><br />
                                    结束：<%#Eval("endDate").ToString()%>
                                    </td>
                                    <td><%# Eval("itemType").ToString() != "same" ? "不同促销形式" : Eval("discountType").ToString() == "DISCOUNT"?"打折":"减价"%></td>
                                    <td><%# Eval("itemType").ToString() != "same" ? "不同促销力度" : Eval("discountType").ToString() == "DISCOUNT" ? Eval("discountValue").ToString() + "折" : Eval("discountValue").ToString() + "元"%></td>
                                    <td><%#Eval("tagId").ToString() == "1" ? "全网淘宝用户" : "全网淘宝用户"%></td>
                                    <td><a href="activitytaobaoItem.aspx?activityID=<%#  Eval("ID").ToString() %>">添加促销宝贝</a>  </td>
                                    <td><a href="activitygetitem.aspx?activityID=<%#  Eval("ID").ToString() %>">管理促销宝贝</a>   </td>
                                    <td>
                                       <%# outShowHtml(Eval("Status").ToString(), Eval("ID").ToString()) %>
                                    </td>
                                 </tr>
                            </ItemTemplate>
                        </asp:Repeater>
					</thead>
                     
                </table>
                 <div>
                        <asp:Label ID="lbPage" runat="server"></asp:Label>
                    </div>
                    <div>注：促销活动结束时间超过10天的活动，将会被自动删除</div>
              </div>
         </div>
</div>
<input type=hidden name="liID" id="liID" value="li1"  runat="server"/>
<script type="text/javascript">
    function setSelect() {
        var id = document.getElementById("liID").value;
        document.getElementById('li1').className = "";
        document.getElementById('li2').className = "";
        document.getElementById('li3').className = "";
        document.getElementById('li4').className = "";
        document.getElementById('li5').className = "";
        document.getElementById(id).className = "selected";
    }
    setSelect();
                </script>
       </div>
       </div>
</form>
</body>
</html>
