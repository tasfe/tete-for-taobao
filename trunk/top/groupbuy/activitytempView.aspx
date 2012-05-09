<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitytempView.aspx.cs" Inherits="top_groupbuy_activitytempView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <%=html%>
    <div runat=server id=div1>
    <asp:TextBox ID="TextBox1" runat="server" Height="178px" TextMode="MultiLine" 
                Width="602px"></asp:TextBox>
            <br />
            <input type="button" value="复制代码" onclick="jsCopy();" />
            </div>
              <script type="text/javascript">
                  function jsCopy() {
                      var e = document.getElementById("TextBox1"); //对象是content 
                      e.select(); //选择对象 
                      document.execCommand("Copy"); //执行浏览器复制命令

                      alert("已复制好，可贴粘。");
                  } 
</script> 
    </form>
</body>
</html>
