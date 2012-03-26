<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllTalkContent.aspx.cs" Inherits="AllTalkContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>接待人次</title>
    <link rel="stylesheet" type="text/css" href="jxkh/css/theme.css" />
    <link rel="stylesheet" type="text/css" href="jxkh/css/style.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

    <script type="text/javascript">
        var StyleFile = "theme" + document.cookie.charAt(6) + ".css";
        document.writeln('<link rel="stylesheet" type="text/css" href="jxkh/css/' + StyleFile + '">');
    </script>

    <!--[if IE]>
<link rel="stylesheet" type="text/css" href="css/ie-sucks.css" />
<![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="header">
            <h2>客服绩效考核</h2>
            <div id="topmenu">
                <ul>
                    <li><a href="default.aspx">主页</a></li>
                    <li class="current"><a href="#">日统计</a></li>
                    <li><a href="YejiTotal.aspx">业绩统计</a></li>
                    <li><a href="#">聊天记录</a></li>
                    <li><a href="#">客服对比</a></li>
                </ul>
            </div>
        </div>
        <div id="top-panel">
            <div id="panel">
                <ul>
                   <li><a href="#" class="report">接待人数</a></li>
                   <li><a href="#" class="report_seo">成功订单</a></li>
                   <li><a href="#" class="report_seo">付款金额</a></li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
        
            <div id="content">
              <table style="margin:0; width:740px; margin-top:5px;">
                 <tr>
                 <td valign="top">
                    <table>
                      <tr><td>
                        全部客服
                      </td></tr>
                       <asp:Repeater ID="Rp_KefuList" runat="server">
                         <ItemTemplate>
                            <tr>
                               <td><asp:LinkButton Text=' <%# Eval("FromNick") %>' runat="server" OnClick ="ShowCus"></asp:LinkButton></td>
                            </tr>
                         </ItemTemplate>
                       </asp:Repeater>
                    </table>
                 </td>
                 <td valign="top">
                    <table>
                     <tr>
                        <td>接待客户名单</td>
                     </tr>
                      <asp:Repeater ID="Rpt_CustomerList" runat="server">
                         <ItemTemplate>
                            <tr>
                               <td><asp:LinkButton ID="LinkButton1" Text=' <%# Eval("ToNick") %>' runat="server" OnClick="ShowTalk"></asp:LinkButton></td>
                            </tr>
                         </ItemTemplate>
                       </asp:Repeater>
                    </table>
                 </td>
                  <td valign="top">
                    <table>
                      <tr>
                        <td colspan="2">对话内容</td>
                      </tr>
                       <asp:Repeater ID="Rpt_TalkList" runat="server">
                         <ItemTemplate>
                            <tr>
                               <td><%# Eval("content")%></td>
                               <td><%# Eval("time")%></td>
                            </tr>
                         </ItemTemplate>
                       </asp:Repeater>
                    </table>
                  </td>
                 
                 </tr>
              </table>
            </div>
        
            <div id="sidebar">
                <ul>
                    <li>
                        <h3>
                            <a href="#" class="house">日统计</a></h3>
                        <ul>
                            <li><a href="#" class="report">接待人数</a></li>
                            <li><a href="#" class="report_seo">成功订单</a></li>
                            <li><a href="#" class="report_seo">付款金额</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="folder_table">业绩统计</a></h3>
                        <ul>
                            <li><a href="#" class="addorder">成功订单</a></li>
                            <li><a href="#" class="shipping">接待人次</a></li>
                            <li><a href="#" class="invoices">日统计</a></li>
                            <li><a href="#" class="invoices">月统计</a></li>
                            <li><a href="#" class="invoices">年统计</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="manage">聊天记录</a></h3>
                        <ul>
                            <li><a href="#" class="manage_page">超长响应聊天</a></li>
                            <li><a href="#" class="manage_page">流失客户聊天</a></li>
                            <li><a href="#" class="cart">成功客户聊天</a></li>
                            <li><a href="#" class="folder">未回复聊天</a></li>
                            <li><a href="#" class="promotions">内部聊天</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="user">客服对比</a></h3>
                        <ul>
                            <li><a href="#" class="useradd">接待人数</a></li>
                            <li><a href="#" class="useradd">平均首次响应速度</a></li>
                            <li><a href="#" class="group">平均响应速度s</a></li>
                            <li><a href="#" class="search">未回复人数</a></li>
                            <li><a href="#" class="online">回复次数</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <asp:HiddenField ID="Hf_User" runat="server" />
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
    </form>
</body>
</html>
