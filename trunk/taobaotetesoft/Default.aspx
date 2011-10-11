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
top: 30%; /* 距顶部50%*/ 
left: 40%; 
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <input type="hidden" id="paramHidden" name="paramHidden" runat="server" />

    <div class="b2" id="area1" runat="server">
				<strong>特特营销推广 V1.0 【腾讯微博互粉系统】</strong>
				<table width="240" cellspacing="0" cellpadding="4" border="0">
				<tbody><tr><td align="left" style="color: red;" colspan="2" id="errortd"></td></tr>
                        <!--<tr> 
				                <td width="50" class="f14">帐　号:</td>
				                <td width="156"><input type="text" class="ip" title="用户名/验证邮箱" value="" id="Text1" name="uid"></td>
				        </tr>
				        <tr style="" id="trPassNorm"> 
				                <td width="50" valign="top" class="f14">密　码:</td>
				                <td width="156"><input type="password" class="ip" value="" id="normModPsp" name="pass"></td>
				        </tr>
				     <tr> 
							<td>&nbsp;</td>
							<td> 
                                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text=" 登录 " /></td>
					 </tr>-->
                     
                     <tr> 
							<td>
                                这是一款免费有效的帮您增加粉丝数量的软件..
                            </td>
					 </tr>
                     <tr> 
							<td>
                                <input type="button" value="立即使用" onclick="" />
                            </td>
					 </tr>
				
				</tbody></table>
    </div>

    <div class="b2" id="area2" runat="server" visible="false">
        <strong>请输入您收到的手机验证码</strong>
        <table width="240" cellspacing="0" cellpadding="4" border="0">
				<tbody><tr><td align="left" style="color: red;" colspan="2" id="Td1"></td></tr>
                        <tr> 
				                <td width="50" class="f14">验证码:</td>
				                <td width="156"><input type="text" class="ip" title="请输入您手机收到的验证码" maxlength="6" value="" id="Text2" name="code"></td>
				        </tr>
				     <tr> 
							<td>&nbsp;</td>
							<td> 
                                    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text=" 继续 " /></td>
					 </tr>
				
				</tbody></table>
    </div>

    

    <div class="b2" id="area3" runat="server" visible="false">
        <strong>请输入下面验证码图片上的字符</strong>
        <table width="240" cellspacing="0" cellpadding="4" border="0">
				<tbody><tr><td align="left" style="color: red;" colspan="2" id="Td2"></td></tr>
                        <tr> 
				                <td width="50" class="f14">验证码:</td>
				                <td width="156"><input type="text" class="ip" title="请输入图片上显示的验证码" maxlength="6" value="" id="Text3" name="code">
                                    <img src="https://regcheckcode.taobao.com/auction/checkcode?sessionID=5375632f5f5f85b97f4c65e42cba3210" />
                                </td>
				        </tr>
				     <tr> 
							<td>&nbsp;</td>
							<td> 
                                    <asp:Button ID="Button3" runat="server" onclick="Button2_Click" Text=" 继续 " /></td>
					 </tr>
				
				</tbody></table>
    </div>
    </form>
</body>
</html>
