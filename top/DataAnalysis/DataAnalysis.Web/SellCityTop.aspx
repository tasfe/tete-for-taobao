<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SellCityTop.aspx.cs" Inherits="SellCityTop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售城市排行</title>
     <script src="js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>    <link href="css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
</style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div>

         <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="javascript:;" class="nolink">特特店铺销售分析</a> 销售城市排行 </div>
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
        <asp:Button ID="Btn_3Days" runat="server" OnClick="Btn_3Days_Click" Text="最近3天" />&nbsp;
        <asp:Button ID="Btn_7Days" runat="server" OnClick="Btn_7Days_Click" Text="最近7天" />&nbsp;
        <asp:Button ID="Btn_30Days" runat="server" OnClick="Btn_30Days_Click" Text="最近30天" />&nbsp;
        <asp:TextBox ID="TB_Start" runat="server" onFocus="WdatePicker({startDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})" class="Wdate" Width="120px"></asp:TextBox> 至 
            <asp:TextBox ID="TB_End" runat="server" Width="120px" class="Wdate" onFocus="WdatePicker({minDate:'%y-%M-01',maxDate:'%y-%M-%ld',dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
       <asp:Button ID="Btn_Select" runat="server" Text="检索"  onclick="Btn_Select_Click" />
       <br />
        </div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center">
                    <b>省份/直辖市</b>
                </td>
                <td width="220" align="center">
                    <b>市/区</b>
                </td>
                <td width="200" align="center">
                    <b>订单总量</b>
                </td>
                <td width="200" align="center">
                    <b>订单总金额</b>
                </td>
            </tr>
            <asp:Repeater ID="Rpt_PageVisit" runat="server">
                <ItemTemplate>
                    <tr>
                        <td height="35" align="center">
                            <%#Eval("receiver_state")%>
                        </td>
                        <td align="center">
                            <%#Eval("receiver_city")%>
                        </td>
                        <td align="center">
                            <%#Eval("OrderTotal")%>
                        </td>
                        <td align="center">
                            <%#Eval("payment")%>
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

    </div>
    </div>
    </form>
</body>
</html>
