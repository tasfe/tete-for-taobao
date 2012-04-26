<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsOrderList.aspx.cs" Inherits="GoodsOrderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>客服绩效考核</title>
    <link href="css/css1.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="jxkh/css/theme.css" />
    <link rel="stylesheet" type="text/css" href="jxkh/css/style.css" />

    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        var start = document.cookie.indexOf('theme=');
        var StyleFile = "theme" + document.cookie.charAt(start + 6) + ".css";
        document.writeln('<link rel="stylesheet" type="text/css" href="jxkh/css/' + StyleFile + '">');
    </script>

    <!--[if IE]>
<link rel="stylesheet" type="text/css" href="jxkh/css/ie-sucks.css" />
<![endif]-->
</head>
<body>
    <div id="container">
        <div id="header">
            <div id="site-nav" style="margin-bottom: -15px;">
                <p class="login-info">
                    <font color="white">在线客服：</font> <a target="_blank" href="http://amos.im.alisoft.com/msg.aw?v=2&uid=叶儿随清风&site=cntaobao&s=1&charset=utf-8">
                        <img border="0" align="absmiddle" src="http://amos.im.alisoft.com/online.aw?v=2&uid=叶儿随清风&site=cntaobao&s=1&charset=utf-8"
                            alt="有问题请点这里" /></a>
                </p>
                <ul class="quick-menu">
                    <li><a href="help/help.html" style="color: White">帮助教程</a></li>
                </ul>
            </div>
            <h2>
                聊天记录</h2>
            <div id="topmenu">
                <ul>
                    <li><a href="default2.aspx">主页</a></li>
                    <li class="current"><a href="ReceiveCustomer.aspx">日统计</a></li>
                    <li><a href="YejiTotal2.aspx">业绩统计</a></li>
                    <li><a href="AllTalkContent.aspx">聊天记录</a></li>
                    <li><a href="GroupReceiveCustomer.aspx">客服对比</a></li>
                </ul>
            </div>
        </div>
        <div id="top-panel">
            <div id="panel">
                <ul>
                    <li><a href="CustomerList.aspx" class="report">接待人数</a></li>
                    <li><a href="#" class="report_seo">成功订单</a></li>
                    <li><a href="#" class="report_seo">付款金额</a></li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
            <div id="content">
                <form id="form1" runat="server">
                <div id="rightnow">
                    <asp:TextBox ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"
                        class="Wdate" Width="120px"></asp:TextBox>
                    至
                    <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
                    <asp:Button ID="Btn_Select" runat="server" Text="检索" OnClick="Btn_Select_Click" />
                </div>
                <div>
                    <table style="margin: 0; width: 740px; margin-top: 5px;">
                        <thead>
                            <tr>
                                <th style="width:100px">
                                    订单号
                                </th>
                                <th style="width:115px">
                                    购买者
                                </th>
                                <th style="width:40px">
                                    邮费
                                </th>
                                <th style="width:85px">
                                    商品总价
                                </th>
                                <th style="width:85px">
                                    实际支付
                                </th>
                                <th style="width:200px">
                                    收获地址
                                </th>
                                <th style="width:115px">
                                    接待人
                                </th>
                            </tr>
                        </thead>
                        <asp:Repeater ID="Rpt_PageVisit" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Eval("tid") %>
                                    </td>
                                    <td>
                                        <%# Eval("buyer_nick")%>
                                    </td>
                                    <td>
                                        <%#Eval("post_fee")%>
                                    </td>
                                    <td>
                                        <%#Eval("total_fee")%>
                                    </td>
                                    <td>
                                        <%#Eval("payment")%>
                                    </td>
                                    <td>
                                        <%#Eval("receiver_state")%><%#Eval("receiver_city")%>
                                    </td>
                                    <td>
                                        <%# Eval("seller_nick")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <div style="background-color: #dedede; margin-top: 15px">
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:HyperLink ID="lnkFrist" runat="server">首页</asp:HyperLink>
                        <asp:HyperLink ID="lnkPrev" runat="server">上一页</asp:HyperLink>
                        <asp:HyperLink ID="lnkNext" runat="server">下一页</asp:HyperLink>
                        <asp:HyperLink ID="lnkEnd" runat="server">尾页</asp:HyperLink>
                    </div>
                </div>
                </form>
                
            </div>
            <div id="sidebar">
                <ul>
                    <li>
                        <h3>
                            <a href="ReceiveCustomer.aspx" class="report">日统计</a></h3>
                        <ul>
                            <li><a href="ReceiveCustomer.aspx" class="report">接待人数</a></li>
                            <li><a href="GoodsOrderList.aspx" class="report">成功订单</a></li>
                            <li><a href="#" class="report">付款金额</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="YejiTotal2.aspx" class="folder_table">业绩统计</a></h3>
                        <ul>
                            <li><a href="GoodsOrderList.aspx" class="folder_table">成功订单</a></li>
                            <li><a href="#" class="folder_table">接待人次</a></li>
                            <li><a href="#" class="folder_table">日统计</a></li>
                            <li><a href="#" class="folder_table">月统计</a></li>
                            <li><a href="#" class="folder_table">年统计</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="AllTalkContent.aspx" class="invoices">聊天记录</a></h3>
                        <ul>
                            <li><a href="#" class="invoices">超长响应聊天</a></li>
                            <li><a href="#" class="invoices">流失客户聊天</a></li>
                            <li><a href="#" class="invoices">成功客户聊天</a></li>
                            <li><a href="#" class="invoices">未回复聊天</a></li>
                            <li><a href="#" class="invoices">内部聊天</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="GroupReceiveCustomer.aspx" class="user">客服对比</a></h3>
                        <ul>
                            <li><a href="#" class="user">接待人数</a></li>
                            <li><a href="#" class="user">平均首次响应速度</a></li>
                            <li><a href="#" class="user">平均响应速度s</a></li>
                            <li><a href="#" class="user">未回复人数</a></li>
                            <li><a href="#" class="user">回复次数</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div id="footer">
            <div id="credits">
                Made by <a href="http://www.7fshop.com/">TeTeSoft</a>
            </div>
            <div id="styleswitcher">
                <ul>
                    <li><a href="javascript: document.cookie='theme='; window.location.reload();" title="Default"
                        id="defswitch">d</a></li>
                    <li><a href="javascript: document.cookie='theme=1'; window.location.reload();" title="Blue"
                        id="blueswitch">b</a></li>
                    <li><a href="javascript: document.cookie='theme=2'; window.location.reload();" title="Green"
                        id="greenswitch">g</a></li>
                    <li><a href="javascript: document.cookie='theme=3'; window.location.reload();" title="Brown"
                        id="brownswitch">b</a></li>
                    <li><a href="javascript: document.cookie='theme=4'; window.location.reload();" title="Mix"
                        id="mixswitch">m</a></li>
                    <li><a href="javascript: document.cookie='theme=5'; window.location.reload();" title="Mix"
                        id="defswitch">m</a></li>
                </ul>
            </div>
            <br />
        </div>
    </div>
</body>
</html>
