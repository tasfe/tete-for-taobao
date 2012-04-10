<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freelist.aspx.cs" Inherits="top_bao_freelist" %>

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
此处是审核的时候给您看到的部分数据，后期我们的报名数据会实时更新，保证都是有效的免费活动</div>

                   <input type="button" value="查看适合我的活动" onclick="window.location.href='mylist.aspx'" />
    <hr />

    <table width="700" cellpadding="0" cellspacing="0">
        <tr>
                <td width="80"><b>活动名称</b></td>
                <td width="220"><b>报名要求</b></td>
                <td width="80"><b>报名方式</b> </td>
                <td width="140"><b>相关地址</b> </td>
            </tr>

            <tr>
                <td height="35">免费试用</td>
                <td>首页- try.taobao.com   报名要求：应季热销产品，或者市场需求大的产品。</td>
                <td>论坛报名</td>
                <td><a href='http://try.taobao.com/shoper_index.htm' target="_blank">http://try.taobao.com/shoper_index.htm</a></td>
            </tr>

            <tr>
                <td height="35">淘博园</td>
                <td>首页-淘宝天下</td>
                <td>-</td>
                <td>http://bo.tianxia.taobao.com/</td>
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
