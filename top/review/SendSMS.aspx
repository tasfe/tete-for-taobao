<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendSMS.aspx.cs" Inherits="Server.SendSMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <input  type="hidden" id="username" name="username" value="请联系我们" runat="server"/>
    <input  type="hidden" id="password" name="password" value="请联络我们" runat="server"/>
    <input  type="hidden" id="method" name="method" value="sendsms" runat="server"/> 
    <input  type="hidden" id="mobile" name="mobile" value=" " runat="server"/> 
    <input  type="hidden" id="msg" name="msg" runat="server" value="测试信息,请查收。[丹佛斯中国]"/>
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
