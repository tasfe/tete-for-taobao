<%@ Page Language="C#" AutoEventWireup="true" CodeFile="missionmodify.aspx.cs" Inherits="top_crm_missionmodify" %>

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
      您可以创建多个客户营销计划，短信内容最多64个字，超过的话短信将分成多条发送。
  </div>

        <table width="100%">
            <tr>
                <td align="left" width="120" height="30">任务类型：</td>
                <td>
                    <input type="hidden" name="typ" value="<%=typ %>" />
                    <%=gettypinfo(typ)%>
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
                        <textarea id="content1" name="cuicontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c1');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"><%=content %></textarea>
                        <br />每条短信最多<span id="Span1" style="color:Red">64</span>个字，超出部分不发送，剩余：<b id="msg_c1" style="color:Red">64</b>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">延迟发送时间：</td>
                    <td>
                        <input name="cuidate" type="text" value="<%=timecount %>" size="4" />分钟
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
                        <textarea id="content2" name="birthdaycontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c2');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"><%=content %></textarea>
                        <br />每条短信最多<span id="Span3" style="color:Red">64</span>个字，超出部分不发送，剩余：<b id="msg_c2" style="color:Red">64</b>
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
                        <textarea id="content3" name="backcontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c3');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"><%=content %></textarea>
                        <br />每条短信最多<span id="Span2" style="color:Red">64</span>个字，超出部分不发送，剩余：<b id="msg_c3" style="color:Red">64</b>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">回访时间：</td>
                    <td>
                        <input name="backdate" type="text" value="<%=timecount %>" size="4" />天
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
                        <textarea id="content4" name="actcontent" cols="40" rows="3" onkeyup="gettextc(this, findObj('max_m').value, 'msg_c4');if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);" onkeydown="if(this.value.length>findObj('max_m').value)this.value=this.value.substring(0, findObj('max_m').value);"><%=content %></textarea>
                        <br />每条短信最多<span id="Span4" style="color:Red">64</span>个字，超出部分不发送，剩余：<b id="msg_c4" style="color:Red">64</b>
                    </td>
                </tr>
                <tr>
                    <td align="left" height="30">发送时间：</td>
                    <td>
                        <input name="actdate" type="text" value="<%=senddate %>" size="20" />
                    </td>
                </tr>
            </table>
            </div>
            
            <input id="max_m" value="64" type="hidden"/>
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
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="修改营销计划" />
                </td>
            </tr>
        </table>

    </div>
    </div>
    </div>
    </form>

    <script language="javascript" type="text/javascript">
        function InitHiddenAreaText(index) {
            var obj = document.getElementById("area" + index);

            for (var i = 1; i < 5; i++) {
                document.getElementById("area" + i).style.display = "none";
            }

            obj.style.display = "";
        }

        InitHiddenAreaText('<%=index %>');
        gettextc(document.getElementById("content<%=index %>"), findObj('max_m').value, 'msg_c<%=index %>');
    </script>

</body>
</html>