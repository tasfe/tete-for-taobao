﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="alipayadd.aspx.cs" Inherits="top_reviewnew_alipayadd" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>二次销售魔方</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">二次销售魔方</a> 支付宝红包 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
    支付宝是通过短信将卡号密码发送到客户手机上的，如果您账户中的短信数量为0，则无法赠送支付宝红包<br />
    <a href='htmlalipay.html'>点击此处查看支付宝红包创建教程</a>
</div>
                <input type="button" value="返回列表" onclick="window.location.href='alipay.aspx'" />
    
    <hr />
        <table width="700">
            <tr>
                <td align="left" height="30">红包名称：</td>
                <td>
                    <input name="name" type="text" value="支付宝红包" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">红包金额：</td>
                <td>
                    <input name="num" type="text" value="1" size="2" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">红包TXT文件：</td>
                <td>
                    <asp:FileUpload ID="fuAlipay" Width="100px" runat="server" /> 请上传支付宝生成的红包TXT文件，如：1331522192414.txt
                </td>
            </tr>
            <tr>
                <td align="left" height="30">红包有效天数：</td>
                <td>
                    <input name="end_time" type="text" value="7" size="2" /> 请填写您刚才创建红包时写的有效天数
                </td>
            </tr>
            <tr>
                <td align="left" height="30">每人限领数量：</td>
                <td>
                    <select name="per">
                        <option value="5">5</option>
                        <option value="4">4</option>
                        <option value="3">3</option>
                        <option value="2">2</option>
                        <option value="1" selected>1</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td align="left" height="30" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建支付宝红包" />
                </td>
            </tr>
        </table>
    </div>

</div>
    </form>
</body>
</html>
