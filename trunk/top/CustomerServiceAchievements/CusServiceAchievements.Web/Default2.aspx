<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>客服绩效考核</title>
    <link href="css/css1.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="jxkh/css/theme.css" />
    <link rel="stylesheet" type="text/css" href="jxkh/css/style.css" />
    
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
            
           <div id="site-nav" style="margin-bottom:-15px;">
                <p class="login-info">
                    <font color="white">在线客服：</font> <a target="_blank" href="http://amos.im.alisoft.com/msg.aw?v=2&uid=叶儿随清风&site=cntaobao&s=1&charset=utf-8">
                        <img border="0" align="absmiddle" src="http://amos.im.alisoft.com/online.aw?v=2&uid=叶儿随清风&site=cntaobao&s=1&charset=utf-8"
                            alt="有问题请点这里" /></a>
                </p>
                <ul class="quick-menu">
                    <li><a href="help/help.html" style="color:White">帮助教程</a></li>
                </ul>
            </div>
        
            <h2>客服绩效考核</h2>
            <div id="topmenu">
                <ul>
                    <li class="current"><a href="default2.aspx">主页</a></li>
                    <li><a href="ReceiveCustomer.aspx">日统计</a></li>
                    <li><a href="YejiTotal2.aspx">业绩统计</a></li>
                    <li><a href="AllTalkContent.aspx">聊天记录</a></li>
                    <li><a href="GroupReceiveCustomer.aspx">客服对比</a></li>
                </ul>
            </div>
        </div>
        <div id="top-panel">
            <div id="panel">
                <ul>
                   <li><a href="/ReceiveCustomer.aspx" class="report">日统计</a></li>
                   <li><a href="/YejiTotal2.aspx" class="folder_table">业绩统计</a></li>
                   <li><a href="/AllTalkContent.aspx" class="invoices">聊天记录</a></li>
                   <li><a href="/GroupReceiveCustomer.aspx" class="user">客服对比</a></li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
            <div id="content">
             <form id="form1" runat="server">
                <div id="rightnow">
                    <asp:Button ID="Btn_AddCookie" runat="server" Text="点击测试真实数据" 
                        onclick="Btn_AddCookie_Click" />
                </div>
                <div>
                 <p class="youhave">
                  <a href="CustomerList.aspx">客服今天接待情况</a>
                 </p>
                    <table style="margin:0; width:740px; margin-top:5px;">
                      <thead>
                        <tr>
                            <th style="width:15%">
                                序号
                            </th>
                            <th style="width:15%">
                                买家
                            </th>
                            <th style="width:25%">
                                接待人
                            </th>
                            <th style="width:15%">
                                接待时间
                            </th>
                            <th style="width:15%">
                                接待时长
                            </th>
                            <th style="width:15%">
                                交易
                            </th>
                        </tr>
                      </thead>
                        <asp:Repeater ID="Rpt_Jie" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td align="center">
                                        <%# Container.ItemIndex + 1%>
                                    </td>
                                    <td>
                                        <%# Eval("CustomerNick") %>
                                    </td>
                                    <td>
                                        <%# Eval("FromNick") %>
                                    </td>
                                    <td>
                                        <%# Eval("StartTime")%>
                                    </td>
                                    <td>
                                        <%# Eval("TimeSpan")%>
                                    </td>
                                    <td>
                                        <%# Eval("ShowJ")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
               </form>
            </div>
            <div id="sidebar">
               <ul>
                    <li>
                        <h3>
                            <a href="#" class="report">日统计</a></h3>
                        <ul>
                            <li><a href="/ReceiveCustomer.aspx" class="report">接待人数</a></li>
                            <li><a href="/GoodsOrderList.aspx" class="report">成功订单</a></li>
                            <li><a href="#" class="report">付款金额</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="folder_table">业绩统计</a></h3>
                        <ul>
                            <li><a href="/GoodsOrderList.aspx" class="folder_table">成功订单</a></li>
                            <li><a href="CustomerList.aspx" class="folder_table">接待人次</a></li>
                            <li><a href="#" class="folder_table">日统计</a></li>
                            <li><a href="#" class="folder_table">月统计</a></li>
                            <li><a href="#" class="folder_table">年统计</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="invoices">聊天记录</a></h3>
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
                            <a href="#" class="user">客服对比</a></h3>
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
