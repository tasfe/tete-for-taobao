<%@ Page Language="C#" AutoEventWireup="true" CodeFile="missionadd.aspx.cs" Inherits="top_crm_missionadd" %>

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

  <div class="crumbs"><a href="javascript:;" class="nolink">特特CRM_客户关系营销</a> 营销计划创建 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>


    <div id="main-content">
    
  <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
      您可以创建多个客户营销计划，具体请见教程
  </div>

        <table width="100%">
            <tr>
                <td align="left" width="120">任务类型：</td>
                <td>
                    <select name="typ" onchange="InitHiddenArea(this)">
                        <option value="unpay">未付款订单催单</option>
                        <option value="birthday">客户生日关怀</option>
                        <option value="back">买家定期回访</option>
                        <option value="act">新品活动营销</option>
                    </select>
                </td>
            </tr>
            </table>
            <!--催单设置-->
            <div id="area1">
            <table width="100%">
                <tr>
                    <td align="left" height="30" width="120">催单短信内容：</td>
                    <td>
                        <textarea name="cuicontent" cols="40" rows="3"></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">延迟发送时间：</td>
                    <td>
                        <input name="cuidate" type="text" value="1" size="4" />小时
                    </td>
                </tr>
            </table>
            </div>
            
            <!--生日关怀-->
            <div id="area2" style="display:none">
            <table width="100%">
                <tr>
                    <td align="left" height="30" width="120">生日短信内容：</td>
                    <td>
                        <textarea name="birthdaycontent" cols="40" rows="3"></textarea>
                    </td>
                </tr>
            </table>
            </div>
            

            <!--定期回访-->
            <div id="area3" style="display:none">
            <table width="100%">
                <tr>
                    <td align="left" height="30" width="120">定期回访内容：</td>
                    <td>
                        <textarea name="backcontent" cols="40" rows="3"></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">回访时间：</td>
                    <td>
                        <input name="cuidate" type="text" value="30" size="4" />天
                    </td>
                </tr>
            </table>
            </div>

            

            <!--新品活动营销-->
            <div id="area4" style="display:none">
            <table width="100%">
                <tr>
                    <td align="left" height="30" width="120">活动营销内容：</td>
                    <td>
                        <textarea name="actcontent" cols="40" rows="3"></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">发送时间：</td>
                    <td>
                        <input name="actdate" type="text" value="<%=now %>" size="6" />
                    </td>
                </tr>
            </table>
            </div>
            
            <table width="100%">
            <tr>
                <td align="left" height="30" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建营销计划" />
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

            for(var i=1;i<5;i++)
            {
                document.getElementById("area" + i).style.display = "none";
            }

            obj.style.display = "";
        }
    </script>
</body>
</html>