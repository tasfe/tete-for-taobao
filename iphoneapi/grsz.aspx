<%@ Page Language="C#" AutoEventWireup="true" CodeFile="grsz.aspx.cs" Inherits="iphoneapi_grsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<meta name="viewport" content="width=device-width,
 initial-scale=0.5, minimum-scale=0.1, 
maximum-scale=2, user-scalable=no" />
</head>
<body  style="background:url(gerenziliao.png) no-repeat; width:640px; height:666px; margin:0px; padding:0px;">
    <form id="form1" runat="server">
    <input type="hidden" name="act" value="save" />
    <asp:Repeater ID="rptDetail" runat="server">
    <ItemTemplate>
    
    <div style="margin:40px 0 0 155px;">
        <input type="text" name="truename" value='<%#Eval("truename") %>' style="height:26px; width:300px; font-size:20px" />
    </div>
    <div style="margin:42px 0 0 155px;">
        <select name="sex" style="height:30px; font-size:20px">
            <option value="男">男</option>
            <option value="女">女</option>
        </select>
    </div>
    <div style="margin:42px 0 0 155px;">
        <input name="address" type="text" value='<%#Eval("address") %>' style="height:26px; width:300px; font-size:20px" />
    </div>
    <div style="margin:22px 0 0 155px;">
        <input name="birthday" type="text" value='<%#Eval("birthday") %>' style="height:26px; width:300px; font-size:20px" />
    </div>
    <div style="margin:42px 0 0 155px;">
        <input name="email" type="text" value='<%#Eval("email") %>' style="height:26px; width:300px; font-size:20px" />
    </div>
    <div style="margin:42px 0 0 155px;">
        <input name="weibo" type="text" value='<%#Eval("weibo") %>' style="height:26px; width:300px; font-size:20px" />
    </div>
    <div style="margin:42px 0 0 155px;">
        <input name="mobile" type="text" value='<%#Eval("mobile") %>' style="height:26px; width:300px; font-size:20px" />
    </div>
    <div style="margin:42px 0 0 155px;">
        <input type="submit" value="确定修改" style="height:36px; width:200px; font-size:20px;" />
    </div>
    
    </ItemTemplate>
    </asp:Repeater>
    </form>
</body>
</html>
