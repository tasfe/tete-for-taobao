<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initbutton.aspx.cs" Inherits="Web_detail_initbutton" %>
// JavaScript Document
var nick = 'test';
var str = '<img src="http://qijia.7fshop.com/detail/u3.png" onclick="showTeteDialog(event)" style="cursor:pointer" /> <br />';
str += '<div id="showArea" style="display:none; position:absolute; top:1083px; left:305px; background-color:White; border:solid 1px #000; width:870px; height:800px; border:solid 2px #f48120; z-index:99999"><iframe id="showFrame" src="" width="866" height="750" frameborder="0"></iframe></div>';
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
}

function CloseShowWindow(){
	document.getElementById("showArea").style.display = "none";
}


function setContentTete(aa, id){
	document.getElementById("showArea").style.display = "none";
    setContent(aa, id);
}


function test(){
    alert(1);
}
