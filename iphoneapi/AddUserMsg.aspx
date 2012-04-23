<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddUserMsg.aspx.cs" Inherits="AddUserMsg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
      td{ vertical-align:top}
        #Tt_Html
        {
            height: 125px;
            width: 410px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <table width="100%">
          <tr>
             <td>标题：</td>
             <td><asp:TextBox ID="Tb_Title" runat="server" Width="410px"></asp:TextBox></td>
          </tr>
          <tr>
             <td>内容：</td>
             <td><textarea id="Tt_Html" runat="server"></textarea> </td>
          </tr>
          <tr>
             <td><asp:Button ID="Btn_AddAll" Text="发送所有客户端" runat="server" 
                     onclick="Btn_AddAll_Click" /> </td>
             <td><asp:Button ID="Button1" Text="发送部分" runat="server" /> </td>
          </tr>
          <tr>
             <td></td>
             <td></td>
          </tr>
          <tr>
             <td></td>
             <td></td>
          </tr>
       </table>
    </div>
    </form>
</body>
</html>
