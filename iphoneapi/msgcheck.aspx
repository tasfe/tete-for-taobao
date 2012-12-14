<%@ Page Language="C#" AutoEventWireup="true" CodeFile="msgcheck.aspx.cs" Inherits="iphoneapi_msgcheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    请输入4位验证码：
    <asp:TextBox ID="txtVerify" runat="server"></asp:TextBox>

    <asp:Button ID="Button3" runat="server" 
            onclick="Button3_Click" Text="评价短信赠送" />

    <hr />

        <asp:Button ID="Button1" runat="server" 
            onclick="Button1_Click" Text="审核通过" />
        <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="审核不通过" />
        
        <input type="button" onclick="window.location.href='msgcheck.aspx?ispass=0'" value="查看历史审核结果" />
        <input type="button" onclick="window.location.href='msgcheck.aspx'" value="审核中内容" />
        
        <br />
        <br />
        
        <table><tr>
        <td><input type="checkbox" onclick="selectAll()" /></td>
        <td>内容</td>
        <td>添加时间</td>
         <td>是否通过</td></tr>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
            <tr>
                <td><input type="checkbox" name="ids" value="<%#Eval("guid") %>" /></td> 
                <td><%#Eval("content") %></td>
                <td><%#Eval("adddate") %></td>
                <td><%#check(Eval("ispass").ToString())%></td>
            </ItemTemplate>
        </asp:Repeater>
        </table>
    </div>
    </form>

    
<script language="javascript" type="text/javascript">
    function setOK() {
        document.getElementById("t").value = "ok";
        document.getElementById("form1").submit();
    }

    function setNotOK() {
        document.getElementById("t").value = "no";
        document.getElementById("form1").submit();
    }

    function passContent() {
        document.getElementById("t").value = "pass";
        document.getElementById("form1").submit();
    }

    function selectAll() {
        var ids = document.getElementsByName("ids");
        for (i = 0; i < ids.length; i++) {
            if (ids[i].checked == true) {
                ids[i].checked = false;
            } else {
                ids[i].checked = true;
            }
        }
    }
</script>
</body>
</html>
