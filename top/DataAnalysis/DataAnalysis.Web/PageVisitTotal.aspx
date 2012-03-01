﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PageVisitTotal.aspx.cs" Inherits="PageVisitTotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看具体页面访问</title>
     <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
       <asp:TextBox 
                ID="TB_Start" runat="server" onFocus="WdatePicker({minDate:'%y-%(M-1)-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd HH'})" class="Wdate" Width="120px"></asp:TextBox> 至 
            <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd HH'})"></asp:TextBox>
       <asp:Button ID="Btn_Select" runat="server" Text="检索"  onclick="Btn_Select_Click" />
       <br />
        </div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center">
                    <b>受访页面</b>
                </td>
                <td width="220" align="center">
                    <b>浏览次数</b>
                </td>
                <td width="220" align="center">
                    <b>人均浏览次数 </b>
                </td>
            </tr>
            <asp:Repeater ID="Rpt_PageVisit" runat="server">
                <ItemTemplate>
                    <tr>
                        <td height="35">
                            <%#Eval("VisitURL")%>
                        </td>
                        <td align="center">
                            <%#Eval("VisitCount")%>
                        </td>
                        <td align="center">
                            <%#Eval("VisitAvg")%>
                        </td>
                    </tr>
                </ItemTemplate>
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
    </form>
</body>
</html>
