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
    
        <input type="button" value="返回列表" onclick="window.location.href='missionlist.aspx'" />

        <hr />
  <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 5px 0px; color:Red; font-weight:bold; width:700px">
      您可以创建多个客户营销计划，短信内容最多66个字，超过的话短信将分成多条发送。<br />
      创建新品活动营销的时候请您耐心等待，不要一直点击导致短信多次发送。
  </div>

        <table width="100%">
            <tr>
                <td align="left" width="120">任务类型：</td>
                <td>
                    <select name="typ" id="typ" onchange="InitHiddenArea(this)">
                        <option value="unpay">未付款订单催单</option>
                        <option value="birthday">客户生日关怀</option>
                        <option value="back">买家定期回访</option>
                        <option value="act">新品活动营销</option>
                    </select>
                </td>
            </tr>
            <tr style="display:none;">
                <td align="left" width="120">会员组：</td>
                <td>
                    <select name="group">
                        <option value="0">全部会员</option>
                        
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
                        <textarea name="cuicontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c3');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"></textarea>
                        <br />每条短信最多<span id="Span3" style="color:Red">66</span>个字，超出部分不发送，剩余：<b id="msg_c3" style="color:Red">66</b>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">延迟发送时间：</td>
                    <td>
                        <input name="cuidate" type="text" value="60" size="4" />分钟
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
                        <textarea name="birthdaycontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c2');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"></textarea>
                        <br />每条短信最多<span id="Span2" style="color:Red">66</span>个字，超出部分不发送，剩余：<b id="msg_c2" style="color:Red">66</b>
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
                        <textarea name="backcontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c1');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"></textarea>
                        <br />每条短信最多<span id="Span1" style="color:Red">66</span>个字，超出部分不发送，剩余：<b id="msg_c1" style="color:Red">66</b>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">回访时间：</td>
                    <td>
                        <input name="backdate" type="text" value="30" size="4" />天
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
                        <textarea name="actcontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"></textarea>
                        <br />每条短信最多<span id="msg_t" style="color:Red">66</span>个字，超出部分不发送，剩余：<b id="msg_c" style="color:Red">66</b>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30" width="120">会员组</td>
                    <td>
                        <select name="groupguid" onchange="InitUserCount(this)">
                            <option value="" title="<%=totalcustomer %>">所有会员</option>
                            <optgroup label="自定义会员组">
                                <asp:Repeater ID="rptGroup" runat="server">
                                    <ItemTemplate>
                                        <option value="<%#Eval("guid") %>" title="<%#Eval("count") %>"><%#Eval("name") %></option>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </optgroup>
                            <optgroup label="按购买次数区别">
                                <option value="0" title="--">未成功购买的会员</option>
                                <option value="1" title="--">购买过一次的会员</option>
                                <option value="2" title="--">购买过多次的会员</option>
                            </optgroup>
                            <optgroup label="按用户组区别">
                                <option value="a" title="--">未购买</option>
                                <option value="b" title="--">普通会员</option>
                                <option value="c" title="--">高级会员</option>
                                <option value="d" title="--">VIP会员</option>
                                <option value="e" title="--">至尊VIP会员</option>
                            </optgroup>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30" width="120">会员总数：</td>
                    <td>
                        该组内共有<span id="total"><%=totalcustomer%></span>名有手机号码的会员，请确保账户内有足够的短信，否则无法正常发送。
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">是否马上发送：</td>
                    <td>
                        <select name="sendnow" onchange="ShowDate(this)">
                            <option value="1">马上发送</option>
                            <option value="0">在指定时间发送</option>
                        </select>
                        会员多的情况下发送时间会比较长，请您耐心等待~
                    </td>
                </tr>
                <tr id="senddatearea" style="display:none;">
                    <td align="left" height="30">计划发送时间：</td>
                    <td>
                        <input name="actdate" type="text" value="<%=now %>" size="20"  />
                    </td>
                </tr>
            </table>
            </div>
            <input id="max_m" value="66" type="hidden"/>
            <script>
                function findObj(n, d) {
                    var p, i, x;
                    if (!d) d = document;
                    if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
                        d = parent.frames[n.substring(p + 1)].document;
                        n = n.substring(0, p);
                    }
                    if (!(x = d[n]) && d.all) x = d.all[n];
                    for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
                    for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
                    if (!x && d.getElementById) x = d.getElementById(n);
                    return x;
                }

                function gettextc(o, mc, show) {
                    var c_i = o.value.length;
                    var t_i = c_i <= mc ? (mc - c_i) : '0';
                    findObj(show).innerHTML = t_i;
                }
            </script>

            <script>
                function InitUserCount(obj) {
                    document.getElementById("total").innerHTML = obj.options[obj.options.selectedIndex].title;
                }
</script>
            
            <table width="100%">
                <tr>
                    <td align="left" height="30" width="120">活动是否启用：</td>
                    <td>
                        <select name="isstop">
                            <option value="0">启动</option>
                            <option value="1">暂停</option>
                        </select>
                    </td>
                </tr>
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
        function StartSend() { 
            if (document.getElementById("typ").options.selectedIndex == 3) { 
                //if(confirm("您确定要立即发送吗"))
            }
        }

        function InitHiddenArea(obj) {
            var obj = document.getElementById("area" + (obj.options.selectedIndex + 1));

            for(var i=1;i<5;i++)
            {
                document.getElementById("area" + i).style.display = "none";
            }

            obj.style.display = "";
        }

        function ShowDate(obj) {
            if (obj.options.selectedIndex == 1) {
                document.getElementById("senddatearea").style.display = "";
            } else {
                document.getElementById("senddatearea").style.display = "none";
            }
        }
    </script>

</body>
</html>