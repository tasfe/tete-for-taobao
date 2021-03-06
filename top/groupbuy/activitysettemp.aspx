﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activitysettemp.aspx.cs" Inherits="top_groupbuy_activitysettemp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改促销活动</title>
    <link href="../css/common.css" rel="stylesheet" />
    <link href="js/common.css" rel="stylesheet" />
    <link href="js/calendar.css" rel="stylesheet" />
    <link href="js/groupbuy.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.3.2.min.js"></script> 

    <script language="javascript" type="text/javascript" src="js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="js/cal.js"></script>
    <style>
        td{font-size:12px;}
        a{color:Blue; text-decoration:none;}
        th{text-align:left; height:40px;}
    </style>
</head>
<body style="padding:0px; margin:0px;">
<form id="form1" method="post" action="activitysettemp1.aspx">

    <div class="navigation">
        <div class="crumbs"><a href="default.aspx" class="nolink">特特团购</a> 1.选择促销模板 </div>
    
        <div id="main-content">
            <table width="700">
            <tr>
                <td>活动模板名称：</td>
                <td><input name="title" id="title" value="" size=40 /> <font color=red>*</font></td>
            
            </tr>
            <tr>
                <td align="left" height="30">选择活动模板：</td>
                <td>
                    <%--宽950的（适合放在店铺首页头部位置）：
                    <hr />
                    <table width="100%" cellpadding=0 cellspacing=0>
                        <tr>
                            <td>风格1（每行3个） <br /><input type="radio" name="style" value="1" /></td>
                            <td>风格2（每行2个） <br /><input type="radio" name="style" value="2" /></td>
                            <td>风格3（每行4个） <br /><input type="radio" name="style" value="3" /></td>
                            <td>风格4（每行1个） <br /><input type="radio" name="style" value="4" /></td>
                        </tr>
                    </table>
                    <br />--%>
                    宽750的（适合放在宝贝描述里面）：
                    <hr />
                    <table width="100%" cellpadding=0 cellspacing=0>
                        <tr>
                            <td>风格1（默认团购模板） <br />
                                <span onclick="selectRd('templateID1')"   onMouseOver="toolTip('<img width=400px  src=images/groupbuy1.jpg>')" onMouseOut="toolTip()" > 
                                <input type='radio' name='templateID' id="Radio1" checked="checked" value='1' />
                            </td>
                            <td>风格2（第一行1个，下面每行3个） <br />
                                <span  onclick="selectRd('templateID2')"  onMouseOver="toolTip('<img width=400px  src=images/groupbuy1.jpg>')" onMouseOut="toolTip()">
                                <input type='radio' name='templateID' id="templateID2" value='2'  />
                            </td>
                            <td>风格3（一排三列） <br />
                                <span  onclick="selectRd('templateID3')"  onMouseOver="toolTip('<img width=400px  src=images/image3.jpg>')" onMouseOut="toolTip()">
                                <input type='radio' name='templateID' id="templateID3" value='3'  />
                                  <input type="hidden" id="template" name="template" value="1" />   
                            </td>
                            <td> </td>
                        </tr>
    
                 <script type="text/javascript">
                     function selectRd(obj) {
                         document.getElementById(obj).checked = true;
                         document.getElementById('template').value = document.getElementById(obj).value;
                     }
                </script>
                                      
                    </table>
                   <%-- <br />
                    宽190的（适合放在左侧菜单）：
                    <hr />
                    <table width="100%" cellpadding=0 cellspacing=0>
                        <tr>
                            <td>风格1（每行1个） <br /><input type="radio" name="style" value="12" /></td>
                            <td>风格2（每行1个） <br /><input type="radio" name="style" value="13" /></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>--%>
                </td>
            </tr>
            <tr>
                <td align="left" height="30">选择按钮风格</td>
                <td>
                    <table width="100%" cellpadding=0 cellspacing=0>
                        <tr>
                            <td><img src="images/0.png" /> <br /><input type="radio" checked=checked name="button1" value="0" /></td>
                            <td><img src="images/1.png" /> <br /><input type="radio" name="button1" value="1" /></td>
                            <td><img src="images/2.png" /> <br /><input type="radio" name="button1" value="2" /></td>
                            <td><img src="images/3.png" /> <br /><input type="radio" name="button1" value="3" /></td>
                        </tr>
                        <tr>
                            <td><img src="images/4.png" /> <br /><input type="radio" name="button1" value="4" /></td>
                            <td><img src="images/5.png" /> <br /><input type="radio" name="button1" value="5" /></td>
                            <td><img src="images/6.png" /> <br /><input type="radio" name="button1" value="6" /></td>
                            <td><img src="images/7.png" /> <br /><input type="radio" name="button1" value="7" /></td>
                        </tr>
                        <tr>
                            <td><img src="images/8.png" /> <br /><input type="radio" name="button1" value="8" /></td>
                            <td><img src="images/9.png" /> <br /><input type="radio" name="button1" value="9" /></td>
                            <td><img src="images/10.png" /> <br /><input type="radio" name="button1" value="10" /></td>
                            <td><img src="images/11.png" /> <br /><input type="radio" name="button1" value="11" /></td>
                        </tr>
                        <tr>
                            <td><img src="images/12.png" /> <br /><input type="radio" name="button1" value="12" /></td>
                            <td><img src="images/13.png" /> <br /><input type="radio" name="button1" value="13" /></td>
                            <td><img src="images/14.png" /> <br /><input type="radio" name="button1" value="14" /></td>
                            <td><img src="images/15.png" /> <br /><input type="radio" name="button1" value="15" /></td>
                        </tr>
                        <tr>
                            <td><img src="images/16.png" /> <br /><input type="radio" name="button1" value="16" /></td>
                            <td><img src="images/17.png" /> <br /><input type="radio" name="button1" value="17" /></td>
                            <td><img src="images/18.png" /> <br /><input type="radio" name="button1" value="18" /></td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            <tr>
                <td align="left" height="30">商城标志</td>
                <td>
                    <img src="images/lp_protection_icon.png" /> 
                    <br /> <input type="radio" name="showmall" checked=checked value="1" />显示  <input type="radio" name="showmall" value="0" />不显示
                </td>
            </tr>
            <tr>
                <td align="left" height="30">良品标志</td>
                <td>
                    <img src="images/mall_protection_icon.png" />
                    <br /> <input type="radio" name="showliang"  checked=checked value="1" />显示  <input type="radio"  name="showliang" value="0" />不显示
                </td>
            </tr>
            <tr>
                <td align="left" height="30">包邮标志</td>
                <td>
                    <table width="100%" cellpadding=0 cellspacing=0>
                        <tr>
                            <td> <input type="checkbox" name="flag" value="0" /><img src="images/b0.png" />消费者保障 </td>
                            <td><input type="checkbox" name="flag" value="1" /> <img src="images/b1.png" /> 七天退换</td>
                            <td><input type="checkbox" name="flag" value="2" /><img src="images/b2.png" /> 假一赔三</td>
                            <td><input type="checkbox" name="flag" value="3" /><img src="images/b3.png" /> 30天维修</td>
                                  
                        </tr>
                        <tr>
                            <td><input type="checkbox" name="flag" value="4" /><img src="images/b4.png" /> 1小时发货</td>
                            <td><input type="checkbox" name="flag" value="5" /><img src="images/b5.png" /> 闪电发货</td>
                            <td><input type="checkbox" name="flag" value="6" /><img src="images/b6.png" />正品保障</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" height="30"></td>
                <td>
                    <input type="submit" value="下一步：选择展示的宝贝" onclick="parent.scroll(0,0);" />
                </td>
            </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript" src="js/ToolTip.js"></script>
    </form>
</body>
</html>
