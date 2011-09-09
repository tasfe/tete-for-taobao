<%@ Page Language="C#" AutoEventWireup="true" CodeFile="grouplist.aspx.cs" Inherits="top_groupbuy_grouplist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特团购</a> 团购列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold">
    尊敬的用户您好，店铺首页的FLASH目前只显示最新一个商品的团购信息， 如果有到期的团购,请取消团购！谢谢！
     
</div>
        
        <input type="button" value="添加团购" onclick="window.location.href='groupbuyadd.aspx'" />

        <!--<a href='http://www.7fshop.com/testflash/callback.aspx?decode=utf-8&top_appkey=12154705&top_parameters=aWZyYW1lPTEmdHM9MTMwMjc1OTA0MDcyNyZ2aXNpdG9yX2lkPTIwNDIwMDg1NiZ2aXNpdG9yX25pY2s9w8C2xcmv1q7QxA3D3D&top_session=20856ceae6936aa0495654d7db5e1c65766c&time=2011-04-14+13%3A30%3A11&envMode=prod&shop_id=65114941&viewer_nick=%E7%BE%8E%E6%9D%9C%E8%8E%8E%E4%B9%8B%E5%BF%83&seller_nick=<%=nickencode %>&top_sign=roaRUKGZYsQhn6rWH1QWXQ%3D%3D' target="_blank">
        预览效果
        </a>-->
    
    <hr />

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
                <td width="120"><b>活动名称</b></td>
                <td width="100"><b>宝贝</b></td>
                <td width="60"><b>原价</b></td>
                <td width="60"><b>团购价</b></td>
                <td width="50"><b>已参团</b></td>
                <td width="80"><b>开始时间</b></td>
                <td width="80"><b>结束时间</b></td>
                <td width="160"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="100" style="font-size:14px;"><%#Eval("name").ToString()%></td>
                <td>
                    <a href='http://item.taobao.com/item.htm?id=<%#Eval("productid").ToString() %>' target="_blank"><img style="border:solid 1px #000;" width="80" height="80" src='<%#Eval("productimg").ToString() %>_80x80.jpg' alt='<%#Eval("productname").ToString() %>' border="0" /></a> 
                </td>
                <td><s><%#Eval("productprice").ToString() %></s></td>
                <td style="color:Red; font-weight:bold; font-size:14px"><%#Eval("groupbuyprice").ToString() %></td>
                <td><%#Eval("buycount").ToString() %></td>
                <td><%#Eval("starttime")%></td>
                <td><%#Eval("endtime").ToString() %></td>
                <td>
                    <a href='groupbuydetail.aspx?id=<%#Eval("id").ToString()%>'>查看团购订单</a> | 
                    <a href='grouplist.aspx?id=<%#Eval("id").ToString()%>&act=del' onclick="return confirm('您确认要取消团购活动，该操作不可恢复？')">取消团购</a> <br />
                    <a href='addtotaobao-1.aspx?id=<%#Eval("id").ToString()%>'>同步到描述</a> | 
                    <a href='deletetaobao.aspx?id=<%#Eval("id").ToString()%>' onclick="return confirm('您确认要清除关联描述，该操作不可恢复？')">清除关联描述</a>
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
