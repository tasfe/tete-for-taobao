<%@ Page Language="C#" AutoEventWireup="true" CodeFile="autosend.aspx.cs" Inherits="top_blog_autosend" validateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 自动发送设置 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    
<div id="errArea" runat="server" style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold">
    
</div>
    
        您可以在这里设置自动发送，这样以后您就算不登录，程序也会为您自动发送博客进行推广：<br />
        <span style="color:red">备注：设置自动发送后第2天才会生效为您自动发送，如果在第2天的自动发送过程中，您手动发送了文章，则当天的自动发送取消，次日自动发送继续生效~</span><br />
        <input type="radio" name="isauto" value="1" id="open" onclick="showhidden(1)" <%=open %> /><label for="open">开启</label>
        <input type="radio" name="isauto" value="0" id="close" onclick="showhidden(0)" <%=close %> /><label for="close">关闭</label>
        <br /><br />
        <div id="panel1"></div>
        <div id="panel2" style="display:none">
        <b>历史搜索关键字：</b>   <a href='linklistnew.aspx'>设置</a><br />
        <asp:Repeater ID="rptSearch" runat="server">
            <ItemTemplate>
                <input id="searchkey<%#Eval("id") %>" type="checkbox" <%#Eval("nick") %> name="searchkey" value="<%#Eval("searchkey") %>" /> <label for="searchkey<%#Eval("id") %>"><%#Eval("searchkey") %></label> &nbsp;&nbsp;
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <br />
         <div id="Div1" style="height:150px; overflow:scroll;">
        <b>发送帐号：</b>   <a href='accountlist.aspx'>设置</a><br />
        <asp:Repeater ID="rptAccount" runat="server">
            <ItemTemplate>
                <input id="account<%#Eval("id") %>" type="checkbox" <%#Eval("nick") %> name="account" value="<%#Eval("uid") %>" /> <label for="account<%#Eval("id") %>"><%#Eval("uid") %></label> <br />
            </ItemTemplate>
        </asp:Repeater>
        </div>
        <br />
        <b>关联广告：</b> <a href='http://seller.taobao.com/fuwu/service.htm?service_id=764' target="_blank">设置</a> <br />
        
        <table>
        
        <tr>
               <td><input type="radio" id="radio0" name="ads" value="0" title="" checked="checked" /> <label for="radio0">不关联广告</label> </td>
               <td>不关联广告</td>
        </tr>
        
         <asp:Repeater ID="rptAds" runat="server">
            <ItemTemplate>
        <tr>
               <td><input type="radio" id="radio<%#Eval("id") %>" name="ads" value="<%#Eval("id") %>" title="<%#Eval("size") %>" /> <label for="radio<%#Eval("id") %>"><%#Eval("name") %></label> </td>
               <td><a href='http://www.7fshop.com/show/plist.aspx?id=<%#Eval("id") %>' target="_blank" style="font-weight:bold; color:Black">预览</a></td>
        </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    
    
    
        <b>关联商品：</b>   <a href='#' onclick="OpenDialogLable('dialogProduct.aspx',650,560);">设置</a><br />
        <div id="productArea" style="height:150px; overflow:scroll;">
            <%=html %>
        </div>
            <input type="hidden" name="html" id="html" value="<%=htmlencode%>" />
        
        
        <br />
        <b>替换关键字：</b>  <a href='linklist.aspx'>设置</a><br />
        <asp:Repeater ID="rptLink" runat="server">
            <ItemTemplate>
                <input id="link<%#Eval("id") %>" type="checkbox" <%#Eval("nick") %> name="link" value="<%#Eval("keyword") %>" /> <label for="link<%#Eval("id") %>"><%#Eval("keyword")%></label> &nbsp;&nbsp; 
            </ItemTemplate>
        </asp:Repeater>
        <br />
        </div>
        <br />
        <asp:Button ID="btnSearch" runat="server" Text="保存设置" onclick="btnSearch_Click" />
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function showhidden(i) {
        if (i == "1") {
            document.getElementById("panel1").style.display = "none";
            document.getElementById("panel2").style.display = "block";
        } else {
            document.getElementById("panel1").style.display = "block";
            document.getElementById("panel2").style.display = "none";
        }
    }
    
    function InitData(){
        document.getElementById("radio<%=adsid %>").checked = true;
    }

    showhidden(<%=isautohtml %>);
    InitData();


    function OpenDialogLable(url, w, h, editTxt) {
        if (typeof (editTxt) == "undefined") {
            editTxt = "";
        }
        if (navigator.appVersion.indexOf("MSIE") == -1) {
            this.returnAction = function (strResult) {
                if (strResult != null) {
                    if (strResult != "") {
                        document.getElementById("productArea").innerHTML = strResult;
                        document.getElementById("html").value = strResult;
                    }
                }
            }
            window.open(url + '?d=' + Date() + "&t=" + escape(editTxt), 'newWin', 'modal=yes,width=' + w + ',height=' + h + ',top=200,left=300,resizable=no,scrollbars=no');
            return;
        } else {
            var GetValue = showModalDialog(url + '?d=' + Date() + "&t=" + escape(editTxt), null, 'dialogWidth:' + w + 'px; dialogHeight:' + h + 'px;')
            if (GetValue != null) {
                if (GetValue != "") {
                    document.getElementById("productArea").innerHTML = GetValue;
                    document.getElementById("html").value = GetValue;
                    //alert(document.getElementById("html").value);
                }
            }
        }
    }
</script>

<div style="display:none">
    <script src="http://s15.cnzz.com/stat.php?id=2663330&web_id=2663330&show=pic" language="JavaScript"></script>
</div>

</body>
</html>