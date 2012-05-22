<%@ Page Language="C#" AutoEventWireup="true" CodeFile="groupadd.aspx.cs" Inherits="top_crm_groupadd" %>

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
            
  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 创建客户分组 </div>
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
      <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
          您可以在这里创建可以客户组，满足您设置条件的客户都将加入到该组内
      </div>

      <table width="100%">
            <tr>
                <td align="left" width="120">分组名称：</td>
                <td>
                    <input name="cuidate" type="text" value="" />
                </td>
            </tr>
            <tr>
                <td align="left" width="120">满足金额：</td>
                <td>
                    <input name="cuidate" type="text" value="0" size="4" /> 元
                </td>
            </tr>
            <tr>
                <td align="left" width="120">满足交易数：</td>
                <td>
                    <input name="cuidate" type="text" value="0" size="4" /> 笔
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建会员分组" />
                </td>
            </tr>
        </table>

    </div>

        </div>
    </div>
    </form>
</body>
</html>
