<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initbutton.aspx.cs" Inherits="Web_detail_initbutton" %>
// JavaScript Document
var nick = 'test';
var str = '<img src="http://qijia.7fshop.com/detail/u3.png" onclick="showTeteDialog()" style="cursor:pointer" /> <br />';
str += '<div id="showArea" style="display:none; position:absolute; top:1083px; left:305px; background-color:White; border:solid 1px #000; width:724px; height:570px; z-index:99999"><iframe id="showFrame" src="http://qijia.7fshop.com/detail/dialog.html" width="724" height="570" frameborder="0"></iframe></div>';
document.write(str);


function showTeteDialog(){
	document.getElementById("showArea").style.display = "";
	document.getElementById("showFrame").src = "http://qijia.7fshop.com/detail/dialog.aspx?nick=<%=nick %>&id=<%=id %>";
}

function test(){
    alert(1);
}