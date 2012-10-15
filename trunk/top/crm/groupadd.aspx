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
          您可以在这里创建可以客户组，满足您设置条件的客户都将加入到该组内。
      </div>

      <table width="100%">
            <tr>
                <td align="left" width="120">分组名称：</td>
                <td>
                    <input id="groupname" name="name" type="text" value="" />
                </td>
            </tr>
            <tr>
                <td align="left" width="120">满足金额：</td>
                <td>
                    <input name="price" type="text" value="0" size="4" />  - <input name="priceend" type="text" value="" size="4" /> 元
                </td>
            </tr>
            <tr>
                <td align="left" height="30">地区限制：</td>
                <td width="500">
                    <input type=checkbox name=area value="安徽">安徽
          	        <input type=checkbox name=area value="北京">北京
                    <input type=checkbox name=area value="重庆">重庆
                    <input type=checkbox name=area value="福建">福建
                    <input type=checkbox name=area value="甘肃">甘肃
                    <input type=checkbox name=area value="广东">广东
                    <input type=checkbox name=area value="广西">广西
                    <input type=checkbox name=area value="贵州">贵州
                    <input type=checkbox name=area value="海南">海南
                    <input type=checkbox name=area value="河北">河北<br />
                    <input type=checkbox name=area value="黑龙江">黑龙江
                    <input type=checkbox name=area value="河南">河南 
                    <input type=checkbox name=area value="湖北">湖北
                    <input type=checkbox name=area value="湖南">湖南
                    <input type=checkbox name=area value="江苏">江苏
                    <input type=checkbox name=area value="江西">江西
                    <input type=checkbox name=area value="吉林">吉林
                    <input type=checkbox name=area value="辽宁">辽宁
                    <input type=checkbox name=area value="内蒙古">内蒙古<br />
                    <input type=checkbox name=area value="宁夏">宁夏
                    <input type=checkbox name=area value="青海">青海
                    <input type=checkbox name=area value="山东">山东
                    <input type=checkbox name=area value="上海">上海
                    <input type=checkbox name=area value="山西">山西
                    <input type=checkbox name=area value="陕西">陕西
                    <input type=checkbox name=area value="四川">四川
                    <input type=checkbox name=area value="天津">天津
                    <input type=checkbox name=area value="新疆">新疆
                    <input type=checkbox name=area value="西藏">西藏<br />
                    <input type=checkbox name=area value="云南">云南
                    <input type=checkbox name=area value="浙江">浙江
                    <input type=checkbox name=area value="香港">香港
                    <input type=checkbox name=area value="澳门">澳门
                    <input type=checkbox name=area value="台湾">台湾
                    <input type=checkbox name=area value="海外">海外
                </td>
            </tr>
            <tr>
                <td align="left" width="120">成交时间：</td>
                <td>
                    <input name="orderdate" type="text" value="" size="8" />  - <input name="orderdateend" type="text" value="" size="8" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建会员分组" OnClientClick="return check()" />
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
