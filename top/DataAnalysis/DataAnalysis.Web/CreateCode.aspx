<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateCode.aspx.cs" Inherits="CreateCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>获取统计代码</title>
    <link href="css/common.css" rel="stylesheet" />
    <style>
        td
        {
            font-size: 12px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
    </style>
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height: 600px;">
            <div class="crumbs">
                <a href="javascript:;" class="nolink">特特店铺销售分析</a> 获取统计代码
            </div>
            <div class="absright">
                <ul>
                    <li>
                        <div class="msg">
                        </div>
                    </li>
                </ul>
            </div>
            <div id="main-content">
                <asp:FileUpload ID="FUp_Img" Width="400" runat="server" />
                <asp:Button ID="Btn_Upload" runat="server" Text="确定上传" OnClick="Btn_Upload_Click" />
                <br />
                统计代码：
                <textarea id="TA_Code" runat="server" cols="70" rows="3" />
                <img src="<%=UserImage %>" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
