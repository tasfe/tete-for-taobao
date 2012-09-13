<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VisitorNow.aspx.cs" Inherits="visitor_VisitorNow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>实时客户访问</title>
     <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>    
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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特统计</a> 实时客户访问 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

        <div>
       <asp:TextBox 
                ID="TB_Start" runat="server" onFocus="WdatePicker({minDate:'%y-%(M-1)-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd HH'})" class="Wdate" Width="120px"></asp:TextBox> 至 
            <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd HH'})"></asp:TextBox>
       <asp:Button ID="Btn_Select" runat="server" Text="检 索"  onclick="Btn_Select_Click" />
       <br />
        </div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center" width="30%">
                    <b>受访页面</b>
                </td>
                <td align="center" width="15%">
                    <b>访问者IP</b>
                </td>
                <td align="center" width="10%">
                    <b>IP所在地区</b>
                </td>
                 <td align="center" width="15%">
                    <b>访问时间</b>
                </td>
                 <td align="center" width="30%">
                    <b>访问来路</b>
                </td>
            </tr>
            <asp:Repeater ID="Rpt_PageVisit" runat="server">
                <ItemTemplate>
                    <tr>
                        <td valign="top">
                           <a href='<%#Eval("VisitURL")%>' target="_blank">
                            <%# GetSubUrl(Eval("VisitURL").ToString()) %>
                           </a>
                        </td>
                        <td align="center" valign="top">
                            <%#Eval("VisitIP")%>
                        </td>
                        <td align="center" valign="top">
                            <%#Eval("IPLocation")%>
                        </td>
                         <td align="center" valign="top">
                            <%#Eval("VisitTime")%>
                        </td>
                        <td align="center" valign="top">
                            <%# GetSubUrl(Eval("LastURL").ToString())%>
                        </td>
                    </tr>
                </ItemTemplate>
                
                 <SeparatorTemplate> 
                     <tr> 
                           <td colspan="5"><hr /></td> 
                      </tr> 
                 </SeparatorTemplate> 
            </asp:Repeater>
        </table>
       
        <div style="background-color:#dedede; margin-top:15px">
            <asp:label ID="lblCurrentPage" runat="server"></asp:label>
            <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
            <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
            <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
            <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
        </div>
    </div>

    </div>
    </div>
    </form>
</body>
</html>
