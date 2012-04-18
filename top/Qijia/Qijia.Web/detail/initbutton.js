// JavaScript Document
var nick = 'test';
var str = '<img src="u3.png" onclick="showTeteDialog()" style="cursor:pointer" /> <br />';
str += '<div id="showArea" style="display:none; position:absolute; top:50px; left:100px; border:solid 1px #000; width:724px; height:1000px;"><iframe id="showFrame" src="dialog.html" width="724" height="1000" frameborder="0"></iframe></div>';
document.write(str);


function showTeteDialog(){
	document.getElementById("showArea").style.display = "";
	document.getElementById("showFrame").src = "dialog.html";
}