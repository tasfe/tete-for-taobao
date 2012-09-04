<%@ Page Language="C#" AutoEventWireup="true" CodeFile="freecardadd.aspx.cs" Inherits="top_freecard_freecardadd" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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

  <div class="crumbs"><a href="../reviewnew/default.aspx" class="nolink">好评有礼</a> 创建包邮卡 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>

    <div id="main-content">

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
    友情提醒：本服务到期后，此包邮卡将失效！！
</div>
                <input type="button" value="返回列表" onclick="window.location.href='freecardlist.aspx'" />
    
    <hr />
        <table width="700">
            <tr>
                <td align="left" height="30" width=120>名称：</td>
                <td>
                    <input id="freecard" name="name" type="text" value="" size="15" />
                </td>
            </tr>
            <tr>
                <td align="left" height="30">包邮时间：</td>
                <td>
                    <input name="carddate" type="text" value="3" size="2" />月 （这里的时间是指客户收到此包邮卡起几个月内下单免邮费，填0为终身包邮卡）
                </td>
            </tr>
            <tr>
                <td align="left" height="30">满多少金额可用：</td>
                <td>
                    满<input name="price" type="text" value="0" size="6" />元可用 （写0则为不限制）
                </td>
            </tr>
            <tr>
                <td align="left" height="30">使用次数限制：</td>
                <td>
                    <input name="usecount" type="text" value="0" size="2" /> （此处指您创建的包邮卡可以免邮费的次数）
                </td>
            </tr>
            <tr>
                <td align="left" height="30">地区限制：</td>
                <td width="500">
                    <input name="areaisfree" type="radio" value="0" checked />选中地区不包邮
                    <input name="areaisfree" type="radio" value="1" />只有选中地区包邮 <br />

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
                <td align="left" height="30" colspan="2">
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="创建包邮卡" OnClientClick="return check()" />
                </td>
            </tr>
        </table>
    </div>
    <script>
        function check() {
            freecard = document.getElementById("freecard");

            if (freecard.value == "") {
                alert("请输入包邮卡的名称");
                freecard.focus();
                return false;
            }

            return true;
        }
    
    </script>

</div>
    </form>
</body>
</html>
