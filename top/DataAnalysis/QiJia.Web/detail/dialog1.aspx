<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dialog1.aspx.cs" Inherits="Web_detail_dialog1" ValidateRequest="false" %>

<html>
<head runat="server">
    <title></title>
    <link href="http://qijia.7fshop.com/detail/global.css" rel="stylesheet" />
</head>
<body style="margin:12px; padding:0px;">
    <form id="form1" runat="server">

    <div id="alphaArea" style="width:866px; height:2250px; background-color:#ccc; position:absolute; display:none; top:0px; left:0px;filter:alpha(opacity=50);-moz-opacity:0.5;-khtml-opacity: 0.5;opacity: 0.5;">
        图片上传中，请您稍等...
    </div>

    <div style="height:500px;font-family: tahoma, Arial, Helvetica, Verdana, '微软雅黑';">
    
<table width="100%" style="line-height:24px; color:#555; font-size:12px;">
<tr>
<td valign="top" width="500">
<strong>请上传商品图片：</strong><br />

<div style=" padding:4px; background:#fafafa url(title_bg02.png) no-repeat; margin:0 3px 6px 3px; color:red;line-height:25px; padding:10px; font-size:12px; width:364px;">
　　您可以在右侧的效果图上看到每个图片展示的位置，我们会<br />根据您传的图片自动帮您生成合适尺寸的图片，不用您手动去调<br />整图片大小。(注：每张图片大小不能超过1M)
</div>

<br />
【商品图片1】：
<span id="addFileUpload1">
    <asp:FileUpload ID="FileUpload1" runat="server" unselectable="on" /> 
    <input id="returnFileUpload1" type="button" style="display:none;" value="返回" onclick="ShowViewArea('FileUpload1')" /> 
</span> 

<span id="modifyFileUpload1" style="display:none;"> 
    <a href='<%=item1 %>' target="_blank">点击查看图片</a> 
    <input type="button" value="更改图片" onclick="ShowUploadButton('FileUpload1')" /> 
</span> 

<br />
<span>第1张大图，图片尺寸720*583</span><br /><br />

【商品图片2】：
<span id="addFileUpload2">
    <asp:FileUpload ID="FileUpload2" runat="server" unselectable="on" /> 
    <input id="returnFileUpload2" type="button" style="display:none;" value="返回" onclick="ShowViewArea('FileUpload2')" /> 
</span> 

<span id="modifyFileUpload2" style="display:none;"> 
    <a href='<%=item2 %>' target="_blank">点击查看图片</a> 
    <input type="button" value="更改图片" onclick="ShowUploadButton('FileUpload2')" /> 
</span> 
<br />
<span>商品详情图，图片尺寸324*316</span><br /><br />

【商品图片3】：

<span id="addFileUpload3">
    <asp:FileUpload ID="FileUpload3" runat="server" unselectable="on" /> 
    <input id="returnFileUpload3" type="button" style="display:none;" value="返回" onclick="ShowViewArea('FileUpload3')" /> 
</span> 

<span id="modifyFileUpload3" style="display:none;"> 
    <a href='<%=item3 %>' target="_blank">点击查看图片</a> 
    <input type="button" value="更改图片" onclick="ShowUploadButton('FileUpload3')" /> 
</span> 

<br />
<span>第1张局部小图，图片尺寸190*130</span><br /><br />

【商品图片4】：

<span id="addFileUpload4">
    <asp:FileUpload ID="FileUpload4" runat="server" unselectable="on" /> 
    <input id="returnFileUpload4" type="button" style="display:none;" value="返回" onclick="ShowViewArea('FileUpload4')" /> 
</span> 

<span id="modifyFileUpload4" style="display:none;"> 
    <a href='<%=item4 %>' target="_blank">点击查看图片</a> 
    <input type="button" value="更改图片" onclick="ShowUploadButton('FileUpload4')" /> 
</span> 

<br />
<span>第1张局部大图，图片尺寸480*310</span><br /><br />

【商品图片5】：

<span id="addFileUpload5">
    <asp:FileUpload ID="FileUpload5" runat="server" unselectable="on" /> 
    <input id="returnFileUpload5" type="button" style="display:none;" value="返回" onclick="ShowViewArea('FileUpload5')" /> 
</span> 

<span id="modifyFileUpload5" style="display:none;"> 
    <a href='<%=item5 %>' target="_blank">点击查看图片</a> 
    <input type="button" value="更改图片" onclick="ShowUploadButton('FileUpload5')" /> 
</span> 

<br />
<span>第2张局部小图，图片尺寸190*130</span><br /><br />

【商品图片6】：

<span id="addFileUpload6">
    <asp:FileUpload ID="FileUpload6" runat="server" unselectable="on" /> 
    <input id="returnFileUpload6" type="button" style="display:none;" value="返回" onclick="ShowViewArea('FileUpload6')" /> 
</span> 

<span id="modifyFileUpload6" style="display:none;"> 
    <a href='<%=item6 %>' target="_blank">点击查看图片</a> 
    <input type="button" value="更改图片" onclick="ShowUploadButton('FileUpload6')" /> 
</span> 

<br />
<span>第2张局部大图，图片尺寸480*310</span><br /><br />


</td>
<td align="left" valign="top">
<strong>效果图：</strong><br />
<img src="tip.gif" />
</td>
</tr>
</table>

<br />

<table width="100%" style="font-size:12px; color:#555;">
<tr>
<td valign="top" width="500">
<strong>请输入商品属性：</strong><br />
<input name="text111" value="NO" type="hidden" />
【　商品编号　】：
<input name="text121" id="txt1" maxlength="15" value="" /><br />

<input name="property1" value="【产品品牌】" type="hidden" />
【　产品品牌　】：
<input name="property2" id="txt2" maxlength="15" value="" /><br />

<input name="property3" value="【产品名称】" type="hidden" />
【　产品名称　】：
<input name="property4" id="txt3" maxlength="15" value="" /><br />

<input name="property5" value="【材质】" type="hidden" />
【　材质　】：
<input name="property6" id="txt4" maxlength="15" value="" /><br />

<input name="property7" value="【规格】" type="hidden" />
【　规格　】：
<input name="property8" id="txt5" maxlength="15" value="" /><br />

<input name="property9" value="【图层工艺】" type="hidden" />
【　图层工艺　】：
<input name="property10" id="txt6" maxlength="15" value="" /><br />

<input name="property11" value="【适用范围】" type="hidden" />
【　适用范围　】：
<input name="property12" id="txt7" maxlength="15" value="" /><br />

<input name="property13" value="【计价单位】" type="hidden" />
【　计价单位　】：
<input name="property14" id="txt8" maxlength="15" value="" /><br />

<input name="property15" value="【产地】" type="hidden" />
【　产地　】：
<input name="property16" id="txt9" maxlength="15" value="" /><br />

<input name="property17" value="【是否带不干胶】" type="hidden" />
【　是否带不干胶　】：
<input name="property18" id="txt10" maxlength="15" value="" /><br />

<input name="property19" value="【产品介绍】" type="hidden" />
【　产品介绍　】：
<input name="property20" id="txt11" maxlength="20" value="" /><br /><br /><br />
</td>
<td align="left" valign="top">
<strong>效果图：</strong><br />
<img src="tip1.gif" />
</td>
</tr>
</table>

<table width="100%" style="font-size:12px; color:#555;">
<tr>
<td valign="top" width="500">
<strong>请输入长度属性：</strong><br />
<input name="text1" value="text1" type="hidden" />
【文字1】：
<input name="text2" id="txt12" maxlength="50" value="" /><br />

<input name="text3" value="text2" type="hidden" />
【文字2】：
<input name="text4" id="txt13" maxlength="50" value="" /><br />

<input name="text5" value="text3" type="hidden" />
【文字3】：
<input name="text6" id="txt14" maxlength="50" value="" /><br />

<input name="text7" value="text4" type="hidden" />
【文字4】：
<input name="text8" id="txt15" maxlength="50" value="" /><br />

<input name="text9" value="text5" type="hidden" />
【文字5】：
<input name="text10" id="txt16" maxlength="50" value="" /><br />

</td>
<td align="left" valign="top">
<strong>效果图：</strong><br />
<img src="tip2.gif" />
</td>
</tr>
<tr>
	<td colspan="2">
    	<hr />
    </td>
</tr>
<tr>
<td valign="top" width="500">
<input name="text11" value="text6" type="hidden" />
【文字6】：
<textarea id="textarea1" name="text12" rows="4" cols="26" ></textarea><br /><b>(不超过50个字符)</b><br /><br />

<textarea name="text137" style="display:none">text7</textarea>
【文字7】：
<textarea id="textarea2" name="text14" rows="4" cols="26" ></textarea><br /><b>(不超过50个字符)</b>
</td>
<td align="left" valign="top">
<img src="tip6.gif" />
</td>
</tr>
</table>
<br />

<table width="100%">
<tr>
<td valign="top" width="400">
<img src="tip3.gif" /> <br />
<img src="tip4.gif" /> 
</td>
<td valign="top">
<img src="tip5.gif" /> 
</td>
</tr>
</table>

        <asp:Button ID="Button1" runat="server" Text=""  style="width:120px; height:40px; background:url(btn_02.jpg) no-repeat;border:0; cursor:pointer; margin-right:40px; margin-left:230px;" onclick="Button1_Click" OnClientClick="return CheckInitData()"  />
<input type="button" value="" style="width:90px; height:40px; background:url(btn_03.jpg) no-repeat;border:0; cursor:pointer; display:none" />

</div>
    </form>

    <script language="javascript" type="text/javascript">
        function CheckInitData() {
            if (Obj("FileUpload1").value == "" && '<%=item1 %>' == '') {
                alert("请上传商品图片1！");
                Obj("FileUpload1").focus();
                return false;
            }
            if (Obj("FileUpload2").value == "" && '<%=item2 %>' == '') {
                alert("请上传商品图片2！");
                Obj("FileUpload2").focus();
                return false;
            }
            if (Obj("FileUpload3").value == "" && '<%=item3 %>' == '') {
                alert("请上传商品图片3！");
                Obj("FileUpload3").focus();
                return false;
            }
            if (Obj("FileUpload4").value == "" && '<%=item4 %>' == '') {
                alert("请上传商品图片4！");
                Obj("FileUpload4").focus();
                return false;
            }
            if (Obj("FileUpload5").value == "" && '<%=item5 %>' == '') {
                alert("请上传商品图片5！");
                Obj("FileUpload5").focus();
                return false;
            }
            if (Obj("FileUpload6").value == "" && '<%=item6 %>' == '') {
                alert("请上传商品图片6！");
                Obj("FileUpload6").focus();
                return false;
            }

            if (checkNull("txt1")) {
                alert("请输入产品编号！");
                Obj("txt1").focus();
                return false;
            }
            if (Obj("txt1").value.length > 15) {
                alert("产品编号不能超过15个字符！");
                Obj("txt1").focus();
                return false;
            }
            if (Obj("txt1").value.indexOf("<") != -1 || Obj("txt1").value.indexOf(">") != -1) {
                alert("产品编号里不能包含尖括号！");
                Obj("txt1").focus();
                return false;
            }

            if (checkNull("txt2")) {
                alert("请输入产品品牌！");
                Obj("txt2").focus();
                return false;
            }
            if (Obj("txt2").value.length > 15) {
                alert("产品品牌不能超过15个字符！");
                Obj("txt2").focus();
                return false;
            }
            if (Obj("txt2").value.indexOf("<") != -1 || Obj("txt2").value.indexOf(">") != -1) {
                alert("产品品牌里不能包含尖括号！");
                Obj("txt2").focus();
                return false;
            }

            if (checkNull("txt3")) {
                alert("请输入产品名称！");
                Obj("txt3").focus();
                return false;
            }
            if (Obj("txt3").value.length > 15) {
                alert("产品名称不能超过15个字符！");
                Obj("txt3").focus();
                return false;
            }
            if (Obj("txt3").value.indexOf("<") != -1 || Obj("txt3").value.indexOf(">") != -1) {
                alert("产品名称里不能包含尖括号！");
                Obj("txt3").focus();
                return false;
            }

            if (checkNull("txt4")) {
                alert("请输入材质！");
                Obj("txt4").focus();
                return false;
            }
            if (Obj("txt4").value.length > 15) {
                alert("材质不能超过15个字符！");
                Obj("txt4").focus();
                return false;
            }
            if (Obj("txt4").value.indexOf("<") != -1 || Obj("txt4").value.indexOf(">") != -1) {
                alert("材质里不能包含尖括号！");
                Obj("txt4").focus();
                return false;
            }


            if (checkNull("txt5")) {
                alert("请输入规格！");
                Obj("txt5").focus();
                return false;
            }
            if (Obj("txt5").value.length > 15) {
                alert("规格不能超过15个字符！");
                Obj("txt5").focus();
                return false;
            }
            if (Obj("txt5").value.indexOf("<") != -1 || Obj("txt5").value.indexOf(">") != -1) {
                alert("规格里不能包含尖括号！");
                Obj("txt5").focus();
                return false;
            }

            if (checkNull("txt6")) {
                alert("请输入图层工艺！");
                Obj("txt6").focus();
                return false;
            }
            if (Obj("txt6").value.length > 15) {
                alert("图层工艺不能超过15个字符！");
                Obj("txt6").focus();
                return false;
            }
            if (Obj("txt6").value.indexOf("<") != -1 || Obj("txt6").value.indexOf(">") != -1) {
                alert("图层工艺里不能包含尖括号！");
                Obj("txt6").focus();
                return false;
            }

            if (checkNull("txt7")) {
                alert("请输入适用范围！");
                Obj("txt7").focus();
                return false;
            }
            if (Obj("txt7").value.length > 15) {
                alert("适用范围不能超过15个字符！");
                Obj("txt7").focus();
                return false;
            }
            if (Obj("txt7").value.indexOf("<") != -1 || Obj("txt7").value.indexOf(">") != -1) {
                alert("适用范围里不能包含尖括号！");
                Obj("txt7").focus();
                return false;
            }

            if (checkNull("txt8")) {
                alert("请输入计价单位！");
                Obj("txt8").focus();
                return false;
            }
            if (Obj("txt8").value.length > 15) {
                alert("计价单位不能超过15个字符！");
                Obj("txt8").focus();
                return false;
            }
            if (Obj("txt8").value.indexOf("<") != -1 || Obj("txt8").value.indexOf(">") != -1) {
                alert("计价单位里不能包含尖括号！");
                Obj("txt8").focus();
                return false;
            }

            if (checkNull("txt9")) {
                alert("请输入产地！");
                Obj("txt9").focus();
                return false;
            }
            if (Obj("txt9").value.length > 15) {
                alert("产地不能超过15个字符！");
                Obj("txt9").focus();
                return false;
            }
            if (Obj("txt9").value.indexOf("<") != -1 || Obj("txt9").value.indexOf(">") != -1) {
                alert("产地里不能包含尖括号！");
                Obj("txt9").focus();
                return false;
            }

            if (checkNull("txt10")) {
                alert("请输入是否带不干胶！");
                Obj("txt10").focus();
                return false;
            }
            if (Obj("txt10").value.length > 15) {
                alert("是否带不干胶不能超过15个字符！");
                Obj("txt10").focus();
                return false;
            }
            if (Obj("txt10").value.indexOf("<") != -1 || Obj("txt10").value.indexOf(">") != -1) {
                alert("是否带不干胶里不能包含尖括号！");
                Obj("txt10").focus();
                return false;
            }

            if (checkNull("txt11")) {
                alert("请输入产品介绍！");
                Obj("txt11").focus();
                return false;
            }
            if (Obj("txt11").value.length > 20) {
                alert("产品介绍不能超过20个字符！");
                Obj("txt11").focus();
                return false;
            }
            if (Obj("txt11").value.indexOf("<") != -1 || Obj("txt11").value.indexOf(">") != -1) {
                alert("内容里不能包含尖括号！");
                Obj("txt11").focus();
                return false;
            }

            if (checkNull("txt12")) {
                alert("请输入文字1！");
                Obj("txt12").focus();
                return false;
            }
            if (Obj("txt12").value.length > 50) {
                alert("文字1不能超过50个字符！");
                Obj("txt12").focus();
                return false;
            }
            if (Obj("txt12").value.indexOf("<") != -1 || Obj("txt12").value.indexOf(">") != -1) {
                alert("内容里不能包含尖括号！");
                Obj("txt12").focus();
                return false;
            }

            if (checkNull("txt13")) {
                alert("请输入文字2！");
                Obj("txt13").focus();
                return false;
            }
            if (Obj("txt13").value.length > 50) {
                alert("文字2不能超过50个字符！");
                Obj("txt13").focus();
                return false;
            }
            if (Obj("txt13").value.indexOf("<") != -1 || Obj("txt13").value.indexOf(">") != -1) {
                alert("内容里不能包含尖括号！");
                Obj("txt13").focus();
                return false;
            }

            if (checkNull("txt14")) {
                alert("请输入文字3！");
                Obj("txt14").focus();
                return false;
            }
            if (Obj("txt14").value.length > 50) {
                alert("文字3不能超过50个字符！");
                Obj("txt14").focus();
                return false;
            }
            if (Obj("txt14").value.indexOf("<") != -1 || Obj("txt14").value.indexOf(">") != -1) {
                alert("内容里不能包含尖括号！");
                Obj("txt14").focus();
                return false;
            }

            if (checkNull("txt15")) {
                alert("请输入文字4！");
                Obj("txt15").focus();
                return false;
            }
            if (Obj("txt15").value.length > 50) {
                alert("文字4不能超过50个字符！");
                Obj("txt15").focus();
                return false;
            }
            if (Obj("txt15").value.indexOf("<") != -1 || Obj("txt15").value.indexOf(">") != -1) {
                alert("内容里不能包含尖括号！");
                Obj("txt15").focus();
                return false;
            }

            if (checkNull("txt16")) {
                alert("请输入文字5！");
                Obj("txt16").focus();
                return false;
            }
            if (Obj("txt16").value.length > 50) {
                alert("文字5不能超过50个字符！");
                Obj("txt16").focus();
                return false;
            }
            if (Obj("txt16").value.indexOf("<") != -1 || Obj("txt16").value.indexOf(">") != -1) {
                alert("内容里不能包含尖括号！");
                Obj("txt16").focus();
                return false;
            }

            if (checkNull("textarea1")) {
                alert("请输入文字6！");
                return false;
            }
            if (Obj("textarea1").value.length > 50) {
                alert("文字6不能超过50个字符！");
                return false;
            }
            if (Obj("textarea1").value.indexOf("<") != -1 || Obj("textarea1").value.indexOf(">") != -1) {
                alert("文字6里不能包含尖括号！");
                Obj("textarea1").focus();
                return false;
            }

            if (checkNull("textarea2")) {
                alert("请输入文字7！");
                return false;
            }
            if (Obj("textarea2").value.length > 50) {
                alert("文字7不能超过50个字符！");
                return false;
            }
            if (Obj("textarea2").value.indexOf("<") != -1 || Obj("textarea2").value.indexOf(">") != -1) {
                alert("文字7里不能包含尖括号！");
                Obj("textarea2").focus();
                return false;
            }


            ShowUpload();
            return true;
        }

        function Obj(id) {
            return document.getElementById(id);
        }

        function ShowUpload() {
            document.getElementById("alphaArea").style.display = "";
        }

        function ShowUploadButton(id) {
            document.getElementById("add" + id).style.display = "";
            document.getElementById("modify" + id).style.display = "none";
            document.getElementById("return" + id).style.display = "";
        }

        function ShowViewArea(id) {
            document.getElementById("add" + id).style.display = "none";
            document.getElementById("modify" + id).style.display = "";
        }

        function InitPropertyText(str, tag) {
            var index = 0;
            var indexAry = 0;
            var strArray = str.split("{||}");
            if(str.length == 0){
                return;
            }else{
                var obj = document.getElementsByTagName("input");
                for(var i=0;i<obj.length;i++){
                    if (obj[i].name.indexOf(tag) != -1) {
                        index++;
                        if (index % 2 == 0) {
                            obj[i].value = strArray[indexAry];
                            indexAry++;
                        }
                    }
                }
            }

            if (tag == 'text') {
                document.getElementById("textarea1").value = strArray[6];
                document.getElementById("textarea2").value = strArray[7];
            }
        }

        if ('<%=item1 %>' != '') {
            ShowViewArea('FileUpload1');
        }
        if ('<%=item2 %>' != '') {
            ShowViewArea('FileUpload2');
        }
        if ('<%=item3 %>' != '') {
            ShowViewArea('FileUpload3');
        }
        if ('<%=item4 %>' != '') {
            ShowViewArea('FileUpload4');
        }
        if ('<%=item5 %>' != '') {
            ShowViewArea('FileUpload5');
        }
        if ('<%=item6 %>' != '') {
            ShowViewArea('FileUpload6');
        }

        InitPropertyText('<%=property %>','property');
        InitPropertyText('<%=text %>', 'text');

        function checkNull(id) {
            if (Obj(id).value.replace(/\s/g, "") == "") {
                return true;
            }
            return false;
        }
    </script>

</body>
</html>
