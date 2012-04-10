<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mylist.aspx.cs" Inherits="top_bao_mylist" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>活动报名助手</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">活动报名助手</a> 适合我的免费活动 </div>
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
此处程序会自动调用接口分析您的店铺信用，最近销量等数据分析给出的适合您的免费活动</div>

                  <input type="button" value="返回免费活动列表" onclick="window.location.href='freelist.aspx'" />
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="80"><b>活动名称</b></td>
                <td width="220"><b>报名要求</b></td>
                <td width="80"><b>报名方式</b> </td>
                <td width="140"><b>相关地址</b> </td>
            </tr>

            <tr>
                <td height="35">新人专享活动</td>
                <td>新人专享</td>
                <td>论坛报名</td>
                <td><a href='http://bangpai.taobao.com/group/367527.htm' target="_blank">http://bangpai.taobao.com/group/367527.htm</a></td>
            </tr>
    </table>
    </div>
    </form>
</body>
</html>
