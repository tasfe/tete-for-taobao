<%@ Page Language="C#" AutoEventWireup="true" CodeFile="plist.aspx.cs" Inherits="show_plist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>特特推广</title>
<link type="text/css" href="css/css.css" rel="stylesheet"/>
</head>
<body>
<asp:Panel ID="panel1" runat="server">
<div id="container" style="width:<%=width %>px; height:<%=height %>px; overflow:hidden">
  <div class="navigation"><%=title %></div>
  <div class="outer">
    <div class="inner">
    <asp:Repeater ID="test" runat="server">
            <ItemTemplate>
      <dl>
        <dt><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" title="<%#Eval("itemname") %>" onclick="javascript:spreadStat('<%=id %>','<%#Eval("itemid") %>');" target="_blank"><img src="<%#Eval("itempicurl") %>_80x80.jpg" border="0" /></a></dt>
        <dd><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" title="<%#Eval("itemname") %>" onclick="javascript:spreadStat('<%=id %>','<%#Eval("itemid") %>');" target="_blank"><%#left(Eval("itemname").ToString(), 16)%></a></dd>
      </dl>
      </ItemTemplate>
      </asp:Repeater>
      
          <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
      <dl>
        <dt><a href="http://item.taobao.com/item.htm?id=<%#Eval("Created") %>" title="<%#Eval("title") %>" target="_blank"><img src="<%#Eval("picurl") %>" border="0" /></a></dt>
        <dd><a href="http://item.taobao.com/item.htm?id=<%#Eval("Created") %>" title="<%#Eval("title") %>" target="_blank"><%#left(Eval("title").ToString(), 16)%></a></dd>
      </dl>
      </ItemTemplate>
      </asp:Repeater>
      
      
      <br class="clearfloat"/>
    </div>
  </div>
</div>
</asp:Panel>

<asp:Panel ID="panel2" runat="server" Visible="false">
    <table background="/top/show1/4.gif" border="0" cellpadding="0"
                                        cellspacing="0" height="30" style="border-right: #999999 1px solid; border-top: #999999 1px solid;
                                        border-left: #999999 1px solid; border-bottom: #999999 1px solid" width="740">
                                        <tr>
                                            <td>


    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table align="left" border="0" cellpadding="0" cellspacing="0" height="30">
                    <tr>
                        <td width="10">
                        </td>
                        <td background="/top/show1/1.gif" width="24">
                        </td>
                        <td background="/top/show1/2.gif">
                            <font color="white" style="font-size: 13px"><strong><%=title %></strong></font></td>
                        <td>
                            <img src="/top/show1/3.gif" /></td>
                    </tr>
                </table>
            </td>
            <td align="right">
           
            </td>
        </tr>
    </table>
    
  </td></tr></table>  
    <table border="0" cellpadding="0" cellspacing="0" style="border-right: #999999 1px solid;
        border-top: #999999 1px solid; overflow: hidden; border-left: #999999 1px solid;
        border-bottom: #999999 1px solid" width="740">
        <tr>
            <td valign="top">
                <TABLE cellSpacing=0 cellPadding=0 width=730 
        border=0><TBODY><TR><TD align="middle">
        
        <TABLE cellSpacing=8 
            cellPadding=0 align=center border=0><TBODY><TR>
            
            
      <asp:Repeater ID="Repeater2" runat="server">
            <ItemTemplate>
            
            <TD vAlign=top 
                align="middle" width=175 bgColor=white>
                <TABLE cellSpacing=0 
                  cellPadding=0 align=center border=0><TBODY><TR><TD 
                      align="middle"><DIV 
                        style="BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; MARGIN-TOP: 4px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 160px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 160px"><DIV 
                        style="OVERFLOW: hidden; WIDTH: 160px; HEIGHT: 160px"><A 
                        href="http://item.taobao.com/item.htm?id=<%#Eval("Created") %>" 
                        target="_blank"><IMG 
                        src="<%#Eval("picurl") %>" 
                        border=0 /></A></DIV></DIV></TD></TR><TR><TD 
                      align="middle"><DIV 
                        style="PADDING-RIGHT: 4px; PADDING-LEFT: 4px; FONT-SIZE: 12px; PADDING-BOTTOM: 4px; PADDING-TOP: 4px"><A 
                        style="FONT-SIZE: 12px; COLOR: #3f3f3f; TEXT-DECORATION: none" 
                        href="http://item.taobao.com/item.htm?id=<%#Eval("Created") %>" 
                        target="_blank"><%#Eval("title") %></A><BR /><FONT 
                        style="COLOR: #fe596a"><B>￥&nbsp;<%#Eval("price")%>元</B></FONT> 
                        </DIV><A 
                        href="http://item.taobao.com/item.htm?id=<%#Eval("Created") %>" 
                        target="_blank"><IMG 
                        src="/top/show1/buy1.gif" 
                        border=0 /></A> <DIV></DIV></TD></TR></TBODY></TABLE></TD>
                      
                     </ItemTemplate>
      </asp:Repeater>       
              
              
              </TR>
              

              
              
              </TBODY></TABLE>
         
         
         
         </TD></TR></TBODY></TABLE>
            </td>
        </tr>
                       <tr>
                                            <td align="right" height="24" style="border-bottom: #999999 1px solid" valign="center">
                                                <a href="<%=nickid %>" style="text-decoration: none"
                                                    target="_blank"><font style="font-size: 13px; color: #ff6600"><strong>更多详情请见 <%=nickid %></strong>&nbsp;</font></a>
                                            </td>
                                        </tr>
    </table>
</asp:Panel>

<script type="text/javascript" language="javascript" src="/show/css/common.js"></script>
<script type="text/javascript">
    var xmlHttp;
    var url = "<%=url %>";

    if("0" != "<%=id %>" && url != "")
    {
        //记录浏览量
        createxmlHttpRequest();
        var queryString = "http://www.7fshop.com/qqshow/plist.aspx?act=view&id=<%=id %>&url="+escape(url)+"&t="+new Date().getTime();
        xmlHttp.open("GET",queryString);
        xmlHttp.send(null);  
    }
</script>


</body>
</html>
