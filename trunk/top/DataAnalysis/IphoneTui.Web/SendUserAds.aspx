<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendUserAds.aspx.cs" Inherits="SendUserAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
     <style type="text/css">
        td{height:20px}
     </style>   
</head>
<body>
    <form id="form1" runat="server">
    <div>
      店铺ID：<asp:TextBox ID="TB_Nick" runat="server"></asp:TextBox>
      <p />
      添加密码：<asp:TextBox ID="TB_Password" TextMode="Password" runat="server" />
      <p />
      
      <asp:Button ID="Btn_AddAds" runat="server" Text="确定添加" onclick="Btn_AddAds_Click" />
      
      <table cellpadding="0" cellspacing="0" width="100%">
       <tr>
          <td>获赠者ID</td>
          <td>赠送次数</td>
          <td>赠送时间</td>
          <td>赠送IP</td>
       </tr>
          <asp:Repeater ID="Rpt_SendList" runat="server">
            <ItemTemplate>
               <tr>
                  <td><%# Eval("Nick") %></td>
                  <td><%# Eval("PingTimes")%></td>
                  <td><%# Eval("PingDate")%></td>
                  <td><%# Eval("AddIP")%></td>
               </tr>
            </ItemTemplate>
            <SeparatorTemplate>
              <tr>
                <td colspan="4"><hr /></td>
              </tr>
            </SeparatorTemplate>
          </asp:Repeater>
      </table>
    </div>
    </form>
</body>
</html>
