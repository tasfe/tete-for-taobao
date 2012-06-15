<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllTalkContent.aspx.cs" Inherits="AllTalkContent" %>

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
<style type="text/css">
  table{ border:0;}
  tr{ border:0}
  td{ border:0}
</style>
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
        
            <h2>聊天记录</h2>
            <div id="topmenu">
                <ul>
                    <li><a href="default2.aspx">主页</a></li>
                    <li><a href="ReceiveCustomer.aspx">日统计</a></li>
                    <li><a href="YejiTotal2.aspx">业绩统计</a></li>
                    <li class="current"><a href="#">聊天记录</a></li>
                    <li><a href="GroupReceiveCustomer.aspx">客服对比</a></li>
                </ul>
            </div>
        </div>
        <div id="top-panel">
            <div id="panel">
                <ul>
                   <li><a href="#" class="invoices">超长响应聊天</a></li>
                   <li><a href="#" class="invoices">流失客户聊天</a></li>
                   <li><a href="#" class="invoices">成功客户聊天</a></li>
                   <li><a href="#" class="invoices">未回复聊天</a></li>
                   <li><a href="#" class="invoices">内部聊天</a></li>
                </ul>
            </div>
        </div>
        <div id="wrapper">
            <div id="content">
            <form id="form1" runat="server">
             <div id="rightnow">
                    <asp:TextBox ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"
                        class="Wdate" Width="120px"></asp:TextBox>&nbsp;
                    <asp:Button ID="Btn_Select" runat="server" Text="查 看" OnClick="Btn_Select_Click" />
                </div>
            <div>
              <table style="margin:0;width:740px;margin-top:5px;padding:0;">
                 <tr>
                 <td valign="top">
                    <table width="180px" style="margin:0;padding:0;">
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
                    <table width="180px" style="margin:0;padding:0;">
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
                  <td valign="top" style="width:370px">
                    <table width="360px" style="margin:0;padding:0;">
                      <tr>
                        <td style="width:350px;">对话内容</td>
                      </tr>
                       <asp:Repeater ID="Rpt_TalkList" runat="server">
                         <ItemTemplate>
                            <tr>
                               <td>
                                 <%# GetNick(Eval("direction").ToString(), Eval("FromNick").ToString(), Eval("ToNick").ToString())%>
                                  &nbsp;<%# Eval("time")%>
                                  <br  />
                                <div style="width:349px;word-wrap: break-word;word-break:break-all;">
                                <%# Eval("content")%></div>
                               </td>
                            </tr>
                         </ItemTemplate>
                       </asp:Repeater>
                    </table>
                  </td>
                 </tr>
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
                            <li><a href="/OrderTotal.aspx" class="report">付款金额</a></li>
                        </ul>
                    </li>
                    <li>
                        <h3>
                            <a href="#" class="folder_table">业绩统计</a></h3>
                        <ul>
                            <li><a href="/GoodsOrderList.aspx" class="folder_table">成功订单</a></li>
                            <li><a href="/CustomerList.aspx" class="folder_table">接待人次</a></li>
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
