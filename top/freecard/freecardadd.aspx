﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freecardadd.aspx.cs" Inherits="top_freecard_freecardadd" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../reviewnew/default.aspx" class="nolink">好评有礼</a> 创建包邮卡 </div>
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
    友情提醒：本服务到期后，此包邮卡将失效！！
</div>
                <input type="button" value="返回列表" onclick="window.location.href='freecardlist.aspx'" />
    
    <hr />
        <table width="700">
            <tr>
                <td align="left" height="30">名称：</td>
                <td>
                    <input name="name" type="text" value="包邮卡" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">包邮时间：</td>
                <td>
                    <input name="carddate" type="text" value="3" size="2" />月 （这里的时间是指客户收到此包邮卡起几个月内下单免邮费，填0为终身包邮卡）
                </td>
            </tr>
            <tr>
                <td align="left" height="30">满多少金额可用：</td>
                <td>
                    满<input name="price" type="text" value="0" size="6" />元可用 （写0则为不限制）
                </td>
            </tr>
            <tr>
                <td align="left" height="30">使用次数限制：</td>
                <td>
                    <input name="usecount" type="text" value="0" size="2" /> （此处指您创建的包邮卡可以免邮费的次数）
                </td>
            </tr>
           
            <tr>
                <td align="left" height="30" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建包邮卡" />
                </td>
            </tr>
        </table>
    </div>

</div>
    </form>
</body>
</html>
