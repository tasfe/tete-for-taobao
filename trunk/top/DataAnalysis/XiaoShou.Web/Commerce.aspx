<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Commerce.aspx.cs" Inherits="Commerce" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>查看盈利</title>

    <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <link href="css/common.css" rel="stylesheet" />
    <style>
        td
        {
            font-size: 12px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
    </style>
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height: 1000px;">
            <div class="crumbs">
                <a href="javascript:;" class="nolink">销售分析王</a> 查看盈利
            </div>
            <div class="absright">
                <ul>
                    <li>
                        <div class="msg">
                        </div>
                    </li>
                </ul>
            </div>
            <div id="main-content" style="height: 1000px;">
                <div>
                   <div>
                       <asp:TextBox 
                ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" class="Wdate" Width="120px"></asp:TextBox> 至 
            <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
       &nbsp;<asp:Button ID="Btn_Select" runat="server" Text="查 看"  onclick="Btn_Select_Click" />
                   <br /></div>
                    <table width="750px">
                       <tr><td>日期</td><td>销售金额</td><td>销售成本</td><td>邮费</td><td>盈利</td></tr>
                       <asp:Repeater ID="Rpt_RealTotal" runat="server">
                          <ItemTemplate>
                             <tr>
                                <td><%# GetDate(Eval("SiteTotalDate").ToString())%></td>
                                <td><%# Eval("SiteOrderPay")%></td>
                                <td><%# Eval("RealTotalFee")%></td>
                                <td><%# Eval("RealPostFee")%></td>
                                <td><%# Eval("Commerce")%></td>
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
    </div>
    </form>
</body>
</html>