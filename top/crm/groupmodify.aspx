<%@ Page Language="C#" AutoEventWireup="true" CodeFile="groupmodify.aspx.cs" Inherits="top_crm_groupmodify" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特特CRM_客户营销</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height:600px;">
            
  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 修改客户分组 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">
    
        <input type="button" value="返回列表" onclick="window.location.href='grouplist.aspx'" />

        <hr />

      <table width="100%">
            <tr>
                <td align="left" width="120">分组名称：</td>
                <td>
                    <input name="name" type="text" value="<%=name %>" />
                </td>
            </tr>
            <tr>
                <td align="left" width="120">满足金额：</td>
                <td>
                    <input name="price" type="text" value="<%=price %>" size="4" /> 元
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="修改会员分组" OnClientClick="return check()" />
                </td>
            </tr>
        </table>

        <script>
            function check() {
                if (document.getElementById("groupname").value == '') {
                    alert('请收入分组名称！');
                    document.getElementById("groupname").focus();
                    return false;
                }
                return true;
            }
        </script>

    </div>

        </div>
    </div>
    </form>
</body>
</html>
