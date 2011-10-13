<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特特营销推广 V1.0 【腾讯微博互粉系统】</title>
    <style>
        .b2 {
            background:none repeat scroll 0 0 #F9F9F9;
            border:1px solid #AAAAAA;
            margin-bottom:12px;
            font-size:14px;
line-height:22px;
padding:10px;
width:300px;
position: absolute; /*绝对定位*/ 
top: 10%; /* 距顶部50%*/ 
left: 20%; 
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <input type="hidden" id="paramHidden" name="paramHidden" runat="server" />

    <div class="b2" id="area1" runat="server">
				<strong>特特营销推广 V1.0 【腾讯微博互粉系统】</strong>
				<table width="280" cellspacing="0" cellpadding="4" border="0">
				<tbody><tr><td align="left" style="color: red;" colspan="2" id="errortd"></td></tr>
                     
                     <tr> 
							<td>
                                这是一款免费有效的帮您增加粉丝数量的软件，目前已经有<%=count %>人正在使用该软件...
                            </td>
					 </tr>
                     <tr> 
							<td>
                           <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text=" 立即使用 " /></td>
                            </td>
					 </tr>
				
				</tbody></table>
    </div>
    </form>
</body>
</html>
