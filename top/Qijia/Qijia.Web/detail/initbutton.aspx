﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="initbutton.aspx.cs" Inherits="Web_detail_initbutton" %>
// JavaScript Document
var nick = 'test';
var str = '<img src="http://qijia.7fshop.com/detail/u3.png" onclick="showTeteDialog()" style="cursor:pointer" /> <br />';
str += '<div id="showArea" style="display:none; position:absolute; top:1050px; left:290px; border:solid 1px #000; width:724px; height:1000px;"><iframe id="showFrame" src="http://qijia.7fshop.com/detail/dialog.html" width="724" height="1000" frameborder="0"></iframe></div>';
document.write(str);


function showTeteDialog(){
	document.getElementById("showArea").style.display = "";
	document.getElementById("showFrame").src = "http://qijia.7fshop.com/detail/dialog.html";
}