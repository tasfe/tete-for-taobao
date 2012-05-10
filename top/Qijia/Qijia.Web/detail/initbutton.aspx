<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initbutton.aspx.cs" Inherits="Web_detail_initbutton" %>
// JavaScript Document
var nick = 'test';
var str = '<img src="http://qijia.7fshop.com/detail/u3.png" onclick="showTeteDialog(event)" style="cursor:pointer" /> <br />';
str += '<div id="showArea" style="display:none; position:absolute; top:493px; left:89px; border:solid 2px #f48120; width:870px; height:1000px;"><iframe id="showFrame" src="dialog.html" width="866" height="1000" frameborder="0"></iframe></div>';
document.write(str);


 
function showTeteDialog(event){
if(navigator.userAgent.indexOf("MSIE")>0) {
	teteheight = event.clientY + document.documentElement.scrollTop;
}else{
	teteheight = event.clientY + document.documentElement.scrollTop;
}
    document.getElementById("showArea").style.top = teteheight + "px";
	document.getElementById("showArea").style.display = "";
	document.getElementById("showFrame").src = "http://qijia.7fshop.com/detail/dialog.aspx?nick=<%=nick %>&id=<%=id %>";
}

function CloseShowWindow(){
	document.getElementById("showArea").style.display = "none";
}


function setContentTete(aa){
	alert("tete-"+ aa);
	document.getElementById("showArea").style.display = "none";
}


function test(){
    alert(1);
}
