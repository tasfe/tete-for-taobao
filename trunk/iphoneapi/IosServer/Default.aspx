<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form action="api/default.aspx?act=login" method=post>
    <div>
        <input type="" name="uid" />
        <br />
        <input type="" name="pass" />
        <br />
        <input type="" name="verify" /> <img src='api/default.aspx?act=verify' />
        <br />
        <input type="submit" value="test" />
    </div>
    </form>
</body>
</html>
