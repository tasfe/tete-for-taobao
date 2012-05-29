<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateAPK.aspx.cs" Inherits="CreateAPK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    Logo：
                </td>
                <td>
                    <asp:FileUpload ID="Fud_logo" runat="server" /> jpg图片
                </td>
            </tr>
            <tr>
                <td>
                    加载图片：
                </td>
                <td>
                    <asp:FileUpload ID="Fud_load" runat="server" /> png图片
                </td>
            </tr>
            <tr>
                <td>
                    头部图片：
                </td>
                <td>
                    <asp:FileUpload ID="Fud_head" runat="server" /> png图片
                </td>
            </tr>
            <tr>
                <td>
                    安装名称：
                </td>
                <td>
                    <asp:TextBox ID="Tb_AppName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:Button runat="server" Text="确定提交" 
                        onclick="Unnamed1_Click" /></td>
            </tr>
            <tr>
              <td colspan="2">
                 <asp:Button runat="server" ID="Btn_Create" Text="重新生成手机安装文件" onclick="Unnamed2_Click" Visible="false" />
              </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
