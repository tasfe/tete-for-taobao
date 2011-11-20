<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TT_Question.aspx.cs" Inherits="TT_Question" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>提交新问题</title>
    <meta name="Keywords" content="淘宝营销 推广插件" />
    <meta name="Description" content="淘宝营销 推广插件" />
<link href="../top/css/common.css" rel="stylesheet" />
    <style type="text/css">
    #mainBody {
 width:718px; /*设定宽度*/
 float:left; /*浮动居左*/
 clear:left; /*不允许左侧存在浮动*/
 text-align:left; /*文字左对齐*/
 padding:0;
 /*overflow:hidden; /*超出宽度部分隐藏*/
 margin:0
}
 /*以下为工单系统表格css样式 */
.faqlist
{
	border-collapse: collapse
	}
.faqlist td
{
padding:6px;
background-color:#FFF;
line-height:1.5em
}
.faqlist thead td
{
padding:6px;
text-align:center;
color:#FFF;
font-size:14px;
font-weight:500;
background-color:#81979D;
}
.faqlist tbody th
{
background-color:#EDEEE7;
padding-left:4px;
}
.faqlist thead td a:link,.faqlist thead td a:visited,.faqlist thead td a:hover,.faqlist thead td a:active{
color:#FFFFcc;
font-weight:500
}

.faq_info {
	padding:4px;
	margin-bottom:5px;
	background:#FDFBEA;
	border:1px solid #E1D897;
	line-height:16px;
}

body {
color:#333333;
font-family:arial,helvetica,sans-serif;
font-size:12px;
font-size-adjust:none;
font-style:normal;
font-variant:normal;
font-weight:normal;
line-height:1.2em;
text-align:left;
}
    </style>
</head>
<body style="padding:0px; margin:0px;">

<div class="navigation" style="height:600px;">

  <div class="crumbs"> 我的售后问题 </div>
 
    <div id="main-content">

    <form id="form1" runat="server">
    <div id="mainBody">
			<div class="faqlist">
<table cellspacing="1" border="0" bgcolor="#a6a79d" align="center" width="100%">
        <thead><tr>
                <td colspan="2">提交新的售后问题 (<a href="TT_Question_Manage.aspx">查看历史售后问题</a>)</td> </tr></thead>
              <tbody>
   <!-- <tr>
    <th width="120">问题分类</th>
    <td>
        <asp:DropDownList ID="DropDownList1" runat="server" Height="20px" Width="235px"></asp:DropDownList><font color="red"><b>请选择准确的问题分类，否则可能会得不到及时准确的回复</b></font>
    </td>
    </tr>
  --> 
  <tr>
    <th width="120">标题</th>
    <td>
                    <asp:TextBox ID="txttitle" runat="server" Height="20px" Width="224px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txttitle"
                ErrorMessage="* 非空"></asp:RequiredFieldValidator></td>
  </tr>
  <tr>
    <th>详细说明</th>
    <td><font color="blue">为尽快解决您的问题，提高工作效率，请详细说明问题.<br>
                    <asp:TextBox ID="txtxxi" runat="server" Height="183px" TextMode="MultiLine" 
                        Width="480px"></asp:TextBox></font></td>
  </tr>
                  <tr>
                    <th>联系电话</th>
                    <td>
                            &nbsp;<asp:TextBox ID="txtph" runat="server" Height="20px" Width="151px" MaxLength="11"></asp:TextBox>
                            <font color="red">*</font>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3"
                                runat="server" ErrorMessage="非空" ControlToValidate="txtph"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtph"
                                ErrorMessage="请输入正确的手机号码" ValidationExpression="\d+"></asp:RegularExpressionValidator>方便我们与您联系
			       </td>
     </tr>
                  <tr>
                    <th>验证码</th>
                    <td><!--onblur="checkSafeCode()"-->
                    <asp:TextBox ID="txtv" runat="server" Height="24px" Width="81px"></asp:TextBox>
			       <img height="18" align="absmiddle" width="55" onclick="this.src='verifyimg.aspx?d='+(new Date()).getTime()" style="cursor: pointer;" src="verifyimg.aspx?d=' + Date()" id="validateCode_img"> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtv"
                                    ErrorMessage="* 非空"></asp:RequiredFieldValidator><span id="codeHtml"></span></td>
     </tr>
  <tr>
    <td style="text-align: center; height: 36px;"  colspan="2"><span id="submit_span"><asp:Button
                            ID="btOK" runat="server" Height="24px" 
            OnClientClick="return setIsSpan();" OnClick="Button1_Click" Text="提交" 
            Width="87px" /></span><span style="display: none;" id="loading_span">&nbsp
                        <img height="16" align="absmiddle" width="16" src="images/loading_16x16.gif"> 正在提交，请稍候...</span></td>
  </tr>
  </tbody>
</table>
</div>
</div>

</div>
</div>
<script  type="text/javascript">
    function setIsSpan()
    {
        if(document.getElementById("txttitle").value=="")
        {
            
            alert("请填写问题标题！");
            return false;
        }
        
        if(document.getElementById("txttitle").value=="")
        {
           
            alert("请填写问题标题！");
            return false;
        }
        
        if(document.getElementById("txttitle").value=="")
        {
          
            alert("请填写问题标题！");
            return false;
        }
        document.getElementById('loading_span').style.display="";
        return true;
    }
</script>
    </form>
</body>
</html>
