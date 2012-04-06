<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetTijianParam.aspx.cs" Inherits="tijian_SetTijianParam" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>体检参数设置</title>
    <link href="../css/common.css" rel="stylesheet" />
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
                <a href="javascript:;" class="nolink">营销决策</a> 体检参数设置
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
              <table>
                  <asp:Repeater ID="Rpt_tijian" runat="server">
                   <ItemTemplate>
                    <tr>
                       <td><asp:Label Text='<%# Eval("ParamName") %>' ID="Lb_ParamName" runat="server" /></td>
                       <td><asp:TextBox ID="Tb_ParamValue" runat="server" Text='<%# Eval("ParamValue") %>'></asp:TextBox></td>
                    </tr>
                   </ItemTemplate>
                  </asp:Repeater>
                  <tr><td colspan="2" align="center"><asp:Button runat="server" ID="btn_Up" Text="确 定" 
                          onclick="btn_Up_Click" /></td></tr>
              </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
