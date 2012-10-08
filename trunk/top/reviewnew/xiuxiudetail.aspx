<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xiuxiudetail.aspx.cs" Inherits="top_reviewnew_xiuxiudetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>好评有礼</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 基本设置 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
        <table width="700">
            <tr>
                <td align="left" width="180" height="30">好评秀秀模块标题名称：</td>
                <td>
                    <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">好评秀秀间隔时间：</td>
                <td>
                    <asp:TextBox ID="tbTime" runat="server"></asp:TextBox> 秒
                </td>
            </tr>
            <tr>
                <td align="left" height="30">是否展示头部广告图片：</td>
                <td>
                    <asp:DropDownList ID="ddlShow" runat="server">
                        <asp:ListItem Value="0" Text="不展示"></asp:ListItem>
                        <asp:ListItem Value="1" Text="展示"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            
            <tr>
                <td align="left" height="30">自定义广告图片地址：</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Width="400px"></asp:TextBox> <br /> （留空则显示默认广告图片，图片高度120，宽度为您添加模块的宽度【550、750、950三种】）
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存设置" />
                </td>
            </tr>
        </table>
    </div>
</div>
    </form>
</body>
</html>
