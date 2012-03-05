<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateCode.aspx.cs" Inherits="CreateCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>获取统计代码</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <asp:FileUpload ID="FUp_Img" Width="400" runat="server" />
        <asp:Button ID="Btn_Upload" runat="server" Text="确定上传"  onclick="Btn_Upload_Click" />
       <br />
        统计代码：
    
        <textarea id="TA_Code" runat="server" cols="70" rows="3" />
        <%
              if(true){
             %>
                 <img src="<%=HasImg %>"  />
             <%} %>
    </div>
    </form>
</body>
</html>
