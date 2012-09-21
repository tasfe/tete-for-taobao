<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitytempmanage.aspx.cs" Inherits="top_groupbuy_activitytempmanage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改促销活动</title>
    <link href="../css/common.css" rel="stylesheet" />
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 
     
    <script type="text/javascript" src="js/cal.js"></script>
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        th{text-align:left; height:40px;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
<form id="form1" action="activitysettemp3.aspx">

    <div class="navigation">

        <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 模板管理 <font color="red">注：图片更新用于模板图片不显示，点击图片更新后，请清除宝贝描述，重新同步！</font></div>
    
        <div id="main-content">
          <table width="800px;" border="0" cellspacing="0" cellpadding="0" style="margin:4px; padding:0px">			
                <tr>
                 
					   <td  width="200px">模板名称</td> 
		                <td  width="150px">风格</td>
                        <td  width="150px">创建时间</td>
		                <td width="300px"> 操作</td>
						</tr>
           </table>
				  <hr/>
            <table width="800px;" border="0" cellspacing="0" cellpadding="0" style="margin:4px; padding:0px">	
                     
                            <asp:Repeater ID="rptItems" runat="server">
                                <ItemTemplate>
        	                      <tr style=" height:30px; border-bottom:1px; border-bottom-color:Green;">
                                    <td width="200px"><%#Eval("title").ToString()%></td> 
                                    <td  width="150px"><%#Eval("name").ToString()%></td>
                                    <td  width="150px">
                                     <%#Eval("careteDate").ToString()%> 
                                    </td>
                                    <td  width="300px">
                                        <div id="del"></div>
                                        <%# outShowHtml(Eval("ID").ToString(), Eval("ActivityID").ToString(), Eval("templetID").ToString())%>
                                    </td>
                                 </tr>
                                
                                </ItemTemplate>
                            </asp:Repeater>
 
    </table>
           <div>
                        <asp:Label ID="lbPage" runat="server"></asp:Label>
                    </div>
        </div>
    </div>
    <script type="text/javascript">
     function delItemtemp(act,aid,iid) {
         var actionID = aid;
                        $.ajax({
                            url: 'deletetaobaoitems.aspx?act=' + act + '&aid=' + actionID + '&id=' + iid + '&t=' + new Date().getTime() + '',
                            type: 'GET',
                            dataType: 'text',
                            async: true,
                            timeout: 2000000,
                            beforeSend: function () {
                                $('#del' + iid).html('正在清除...');
                            },
                            error: function () {
                                alert('网络错误，请重试！');
                            },
                            success: function (msg) {
                                if (msg == 'true') {
                                    $('#del' + iid).html('清除成功...');
 
                                } else {
                                    $('#del' + iid).html('清除失败:' + msg + '<a href="javascript:delItemtemp(' + act + ',' + aid + ',' + iid + ')">重试</a>');
                                }
                            }
                        });
                    } 

            </script>
    </form>
</body>
</html>