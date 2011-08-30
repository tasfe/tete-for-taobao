<%@ Page Language="C#" AutoEventWireup="true" CodeFile="blogadd1.aspx.cs" Inherits="top_blog_blogadd1" ValidateRequest="false" EnableViewStateMac="false" %>

<%@ Register assembly="FredCK.FCKeditorV2" namespace="FredCK.FCKeditorV2" tagprefix="FCKeditorV2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />

</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 我要推广 (2.修改软文) </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

        <b>您可以在此修改软文的标题和内容：</b><br />
        标 题： <asp:TextBox ID="tbTitle" runat="server" Width="400px"></asp:TextBox><br />
        内 容：
        <input type="button" value="插入图片" onclick="OpenDialogLable('dialogPic.aspx',450,160);" />
        <input type="button" value="插入商品" onclick="OpenDialogLable('dialogProduct.aspx',650,560);" />
        <input type="button" value="插入免费广告" onclick="OpenDialogLable('dialogAds.aspx',550,460);" />
<a href='redirect.aspx' target="_blank">创建免费广告</a>
        <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" Width="650px" Height="260px">
        </FCKeditorV2:FCKeditor>

        <br />
        
        <!--<b>您可以选择已经建好的广告：</b>  <a href='http://seller.taobao.com/fuwu/service.htm?service_id=764' target="_blank">创建广告</a>  <br />  
        <span style="color:#aaa">该广告会自动插入到您文章的头部</span>
        <br />
        <asp:Repeater ID="rptAds" runat="server">
            <ItemTemplate>
               <input type="radio" id="radio<%#Eval("id") %>" name="ads" value="<%#Eval("id") %>" checked /> <label for="radio<%#Eval("id") %>"><%#Eval("name") %></label> 
               <a href='http://www.7fshop.com/show/plist.aspx?id=<%#Eval("id") %>' target="_blank">预览</a> <br />
            </ItemTemplate>
        </asp:Repeater>
        <br />

        <b>程序自动根据您的关键字帮您匹配符合条件的商品：</b>
        <br />
        <span style="color:#aaa">程序会自动在您的文章后面显示您设置的推荐商品~</span><br />
        <div>

        <table width="500" border="0">
        <tr>
        <asp:Repeater ID="rptProduct" runat="server">
            <ItemTemplate>
               <td>
                <a href="http://item.taobao.com/item.htm?id=<%#Eval("numiid") %>" title="<%#Eval("title") %>" target="_blank"><img src="<%#Eval("picurl") %>_80x80.jpg" border="0" /></a><br />
                <a href="http://item.taobao.com/item.htm?id=<%#Eval("numiid") %>" title="<%#Eval("title") %>" target="_blank"><%#Eval("title").ToString()%></a>
               </td> 
            </ItemTemplate>
        </asp:Repeater>
        </tr>
        </table>
        </div>
        <br />-->
        <b>关键字加超链接：</b>   <a href='linklist.aspx' target="_blank">设置关键字</a>  
        <br />
        <span style="color:#aaa">文章中的关键字会被直接链接到您设置的链接地址~</span>
        <br />
        <asp:TextBox ID="tbKey" runat="server" Width="140px" Visible="false"></asp:TextBox>
        <div style="height:100px; overflow:scroll">
        <asp:Repeater ID="rptLink" runat="server">
            <ItemTemplate>
              &nbsp;&nbsp;关键字“<b><%#Eval("keyword") %></b>”链接到“<%#Eval("link") %>” <a href='linklist.aspx' target="_blank">修改</a> <br>
            </ItemTemplate>
        </asp:Repeater>
        </div>
        <!--<br />
        <span style="color:#ccc">文章中的关键字会被直接链接到您的店铺，多个关键字之间用空格分开</span>-->
        <br />
        <input type="button" value="后退" onclick="history.go(-1)" />
        <input type="button" value="下一步" onclick="submitForm()" />
    </div>
</div>
</form>

<script language="javascript" type="text/javascript">
    function submitForm() {
        document.getElementById("form1").action = "blogadd2.aspx";
        document.getElementById("form1").submit();
        parent.scroll(0, 0);
    }


    function OpenDialogLable(url, w, h, editTxt) {
        if (typeof (editTxt) == "undefined") {
            editTxt = "";
        }
        if (navigator.appVersion.indexOf("MSIE") == -1) {
            this.returnAction = function (strResult) {
                if (strResult != null) {
                    if (strResult != "") {
                        var oEditor = FCKeditorAPI.GetInstance('FCKeditor1');
                        oEditor.InsertHtml(strResult);
                    }
                }
            }
            window.open(url + '?d=' + Date() + "&t=" + escape(editTxt), 'newWin', 'modal=yes,width=' + w + ',height=' + h + ',top=200,left=300,resizable=no,scrollbars=no');
            return;
        } else {
            var GetValue = showModalDialog(url + '?d=' + Date() + "&t=" + escape(editTxt), null, 'dialogWidth:' + w + 'px; dialogHeight:' + h + 'px;')
            if (GetValue != null) {
                if (GetValue != "") {
                    var oEditor = FCKeditorAPI.GetInstance('FCKeditor1');
                    oEditor.InsertHtml(GetValue);
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
