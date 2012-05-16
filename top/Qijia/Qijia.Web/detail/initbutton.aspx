<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initbutton.aspx.cs" Inherits="Web_detail_initbutton" %>
// JavaScript Document
var nick = 'test';
var str = '<img src="http://qijia.7fshop.com/detail/u3.png" onclick="showTeteDialog(event)" style="cursor:pointer" /> <br />';
str += '<div id="alphaArea" style="display:none; position:absolute;margin:0px padding:0px; top:0px; left:0px;z-index:99999;filter:alpha(opacity=50); opacity:0.5; width:100%; height:3000px; background-color:#ccc;"></div>';
str += '<div id="showArea" style="display:none; position:absolute; top:1083px; left:265px; background-color:White; border:solid 1px #000; width:870px; height:800px; border:solid 2px #f48120; z-index:99999"><div style="text-align:right"><a href="javascript:CloseShowWindow();" style="font-size:14px; font-weight:bold;">关闭窗口</a></div><iframe id="showFrame" src="" width="866" height="750" frameborder="0"></iframe></div>';
document.write(str);



function showTeteDialog(event){
if(navigator.userAgent.indexOf("MSIE")>0) {
	teteheight = event.clientY + document.documentElement.scrollTop;
}else{
	teteheight = event.clientY + document.documentElement.scrollTop;
}
    document.getElementById("showArea").style.top = teteheight + "px";
	document.getElementById("showFrame").src = "http://qijia.7fshop.com/detail/dialog.aspx?nick=<%=nick %>&nickid=<%=nickid %>&id=<%=id %>";
	document.getElementById("showArea").style.display = "";
    document.getElementById("alphaArea").style.display = "";
}

function CloseShowWindow(){
	document.getElementById("alphaArea").style.display = "none";
	document.getElementById("showArea").style.display = "none";
}


function setContentTete(aa, id){
	document.getElementById("alphaArea").style.display = "none";
	document.getElementById("showArea").style.display = "none";
    setContent(aa, id);
}


function test(){
    alert(1);
}
