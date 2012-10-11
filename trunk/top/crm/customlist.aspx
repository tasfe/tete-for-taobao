<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customlist.aspx.cs" Inherits="top_crm_customlist" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特特CRM_客户营销</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 客户列表 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
    系统默认自动获取您订购软件后产生的会员数据，如需店铺以前会员数据，可参考<a href='http://bangpai.taobao.com/group/thread/1091516-277392666.htm' target="_blank">教程</a>导入哦~
    <br />该分组下共有<%=total %>名会员
</div>

买家类型：
<select name="typ">
    <option value="all">所有会员</option>
    <optgroup label="按购买次数区别">
        <option value="0">未成功购买的会员</option>
        <option value="1">购买过一次的会员</option>
        <option value="2">购买过多次的会员</option>
    </optgroup>
    <optgroup label="按用户组区别">
        <option value="a">未购买</option>
        <option value="b">普通会员</option>
        <option value="c">高级会员</option>
        <option value="d">VIP会员</option>
        <option value="e">至尊VIP会员</option>
    </optgroup>
</select>
    请输入买家昵称：<asp:TextBox ID="search" Width="90px" runat="server"></asp:TextBox>
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="搜索" />
        <asp:Button ID="Button1" runat="server" Text="导出全部" onclick="Button1_Click" />
        <input type=button value="订单CSV批量导入" onclick="window.location.href='initcustombycsv.aspx'" />
        <hr />

        <table width="740" cellpadding="0" cellspacing="0">
        <tr>
                <td width="100"><b>客户昵称</b></td>
                <td width="50"><b>省</b></td>
                <!--<td width="50"><b>市</b></td>
                <td width="50"><b>区</b></td>-->
                <td width="85"><b>手机</b></td>
                <td width="40"><b>性别</b></td>
                <td width="50"><b>等级</b></td>
                <td width="50"><b>交易量</b></td>
                <td width="50"><b>交易额</b></td>
                <td width="55"><b>最后交易</b></td>
                <td width="55"><a href="customlist.aspx?isbirth=1"><b>生日</b></a></td>
                <td width="60"><b>操作</b></td>
            </tr>
        <asp:Repeater ID="rptArticle" runat="server">
            <ItemTemplate>
            <tr>
                <td height="35"><%#Eval("buynick") %> <img src='level/<%#Eval("buyerlevel") %>.gif' valign="middle" /></td>
                <td><%#Eval("sheng")%></td>
                <!--<td><%#Eval("shi")%></td>
                <td><%#Eval("qu")%></td>-->
                <td><%#Eval("mobile")%></td>
                <td><%#getsex(Eval("sex").ToString())%></td>
                <td><%#getgrade(Eval("grade").ToString())%></td>
                <td><%#Eval("tradecount")%></td>
                <td><%#Eval("tradeamount")%></td>
                <td><%#DateTime.Parse(Eval("lastorderdate").ToString()).ToString("yyyy-MM-dd")%></td>
                <td><%#getdateshort(Eval("birthday").ToString())%></td>
                <td> <a href='custommodify.aspx?id=<%#Eval("guid")%>' target="_blank">编辑</a> </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <div>
        <asp:Label ID="lbPage" runat="server"></asp:Label>
    </div>

    </div>
</div>
    </div>
    </form>
</body>
</html>
