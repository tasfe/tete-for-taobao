<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitytempmanage.aspx.cs" Inherits="top_groupbuy_activitytempmanage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改促销活动</title>
    <link href="../css/common.css" rel="stylesheet" />
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 

    <script language="javascript" type="text/javascript" src="js/My97DatePicker/WdatePicker.js"></script>
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

        <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 模板管理 </div>
    
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
                                        <%# outShowHtml(Eval("ID").ToString(), Eval("ActivityID").ToString())%>
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
    </form>
</body>
</html>