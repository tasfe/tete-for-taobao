<%@ Page Language="C#" AutoEventWireup="true" CodeFile="custommodify.aspx.cs" Inherits="top_crm_custommodify" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 编辑客户资料 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>


    <div id="main-content">
    
        <input type="button" value="返回列表" onclick="window.location.href='customlist.aspx'" />

        <hr />

        <table width="100%">
            <tr>
                <td align="left" width="120" height="30">客户昵称：</td>
                <td>
                    <%=buynick %>
                </td>
            </tr>
            <tr>
                <td align="left" width="120" height="30">省：</td>
                <td>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="120" height="30">市：</td>
                <td>
                    <asp:Label ID="Label2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="120" height="30">区：</td>
                <td>
                    <asp:Label ID="Label3" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="120" height="30">地址：</td>
                <td>
                    <asp:Label ID="lbAddress" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="120" height="30">生日：</td>
                <td>
                    <input name="birthday" type="text" value="<%=birthday %>" size="20" />
                </td>
            </tr>
        
            <tr>
                <td align="left" height="30" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="编辑客户资料" />
                </td>
            </tr>
        </table>

    </div>
    </div>
    </div>
    </form>

    <script language="javascript" type="text/javascript">
        function InitHiddenArea(obj) {
            var obj = document.getElementById("area" + (obj.options.selectedIndex + 1));

            for (var i = 1; i < 5; i++) {
                document.getElementById("area" + i).style.display = "none";
            }

            obj.style.display = "";
        }
    </script>

</body>
</html>
