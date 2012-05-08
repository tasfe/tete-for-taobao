<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dialog1.aspx.cs" Inherits="Web_detail_dialog1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div style="height:1000px;">

<table width="100%">
<tr>
<td valign="top" width="370">
请上传商品图片：<br />

<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
&nbsp;&nbsp;&nbsp;&nbsp;您可以在右侧的效果图上看到每个图片展示的位置，我们会根据您传的图片自动帮您生成合适尺寸的图片，不用您手动去调整图片大小。(注：每张图片大小不能超过1M)
</div>
<br />
【商品图片1】：<asp:FileUpload ID="FileUpload1" runat="server" /><br />
&nbsp;第1张大图，图片尺寸720*583<br /><br />

【商品图片2】：<asp:FileUpload ID="FileUpload2" runat="server" /><br />
&nbsp;商品详情图，图片尺寸324*316<br /><br />

【商品图片3】：<asp:FileUpload ID="FileUpload3" runat="server" /><br />
&nbsp;第1张局部小图，图片尺寸190*130<br /><br />

【商品图片4】：<asp:FileUpload ID="FileUpload4" runat="server" /><br />
&nbsp;第1张局部大图，图片尺寸480*310<br /><br />

【商品图片5】：<asp:FileUpload ID="FileUpload5" runat="server" /><br />
&nbsp;第2张局部小图，图片尺寸190*130<br /><br />

【商品图片6】：<asp:FileUpload ID="FileUpload6" runat="server" /><br />
&nbsp;第2张局部大图，图片尺寸480*310<br /><br />


</td>
<td align="left" valign="top">
效果图：<br />
<img src="tip.gif" />
</td>
</tr>
</table>

<br />

<table width="100%">
<tr>
<td valign="top" width="370">
请输入商品属性：<br />
<input name="text111" value="NO" type="hidden" />
【商品编号】：
<input name="text121" value="" /><br />

<input name="property1" value="【产品品牌】" type="hidden" />
【产品品牌】：
<input name="property2" value="" /><br />

<input name="property3" value="【产品名称】" type="hidden" />
【产品名称】：
<input name="property4" value="" /><br />

<input name="property5" value="【材质】" type="hidden" />
【材质】：
<input name="property6" value="" /><br />

<input name="property7" value="【规格】" type="hidden" />
【规格】：
<input name="property8" value="" /><br />

<input name="property9" value="【图层工艺】" type="hidden" />
【图层工艺】：
<input name="property10" value="" /><br />

<input name="property11" value="【适用范围】" type="hidden" />
【适用范围】：
<input name="property12" value="" /><br />

<input name="property13" value="【计价单位】" type="hidden" />
【计价单位】：
<input name="property14" value="" /><br />

<input name="property15" value="【产地】" type="hidden" />
【产地】：
<input name="property16" value="" /><br />

<input name="property17" value="【是否带不干胶】" type="hidden" />
【是否带不干胶】：
<input name="property18" value="" /><br />

<input name="property19" value="【产品介绍】" type="hidden" />
【产品介绍】：
<input name="property20" value="" /><br /><br /><br />
</td>
<td align="left" valign="top">
效果图：<br />
<img src="tip1.gif" />
</td>
</tr>
</table>

<table width="100%">
<tr>
<td valign="top" width="370">
请输入长度属性：<br />
<input name="text1" value="text1" type="hidden" />
【文字1】：
<input name="text2" value="" /><br />

<input name="text3" value="text2" type="hidden" />
【文字2】：
<input name="text4" value="" /><br />

<input name="text5" value="text3" type="hidden" />
【文字3】：
<input name="text6" value="" /><br />

<input name="text7" value="text4" type="hidden" />
【文字4】：
<input name="text8" value="" /><br />

<input name="text9" value="text5" type="hidden" />
【文字5】：
<input name="text10" value="" /><br />

</td>
<td align="left" valign="top">
效果图：<br />
<img src="tip2.gif" />
</td>
</tr>
<tr>
	<td colspan="2">
    	<hr />
    </td>
</tr>
<tr>
<td valign="top" width="370">
<input name="text11" value="text6" type="hidden" />
【文字6】：
<textarea name="text12" value="" rows="4" cols="26"></textarea><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(不超过50个字符)<br /><br />

<input name="text13" value="text7" type="hidden" />
【文字7】：
<textarea name="text14" value="" rows="4" cols="26" ></textarea><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(不超过50个字符)
</td>
<td align="left" valign="top">
<img src="tip6.gif" />
</td>
</tr>
</table>
<br />

<table width="100%">
<tr>
<td valign="top" width="280">
<img src="tip3.gif" /> <br />
<img src="tip4.gif" /> 
</td>
<td valign="top">
<img src="tip5.gif" /> 
</td>
</tr>
</table>

        <asp:Button ID="Button1" runat="server" Text="生成商品描述" 
            style="width:200px; height:40px;" onclick="Button1_Click"  />
<input type="button" value="效果预览" style="width:100px; height:40px;" />

</div>
    </div>
    </form>
</body>
</html>
