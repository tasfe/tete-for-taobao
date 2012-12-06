<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gift.aspx.cs" Inherits="iphoneapi_gift" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Repeater ID="rptDetail" runat="server">
            <ItemTemplate>
                <%#Eval("title") %> <br />
                <img src='<%#Eval("imgurl") %>' /> <br />
                <%#Eval("score") %>分兑换 <br /><br />

                <input type="button" value="马上兑换" onclick="ok()" />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </form>

    <script>
        function ok() {
            alert('兑换成功'); 
            window.location.href = 'wddd.aspx';
        }
    </script>
</body>
</html>
