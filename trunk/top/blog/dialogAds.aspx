<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dialogAds.aspx.cs" Inherits="top_blog_dialogAds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" value="关闭窗口" class="button" onclick="CloseWindow()" />
	<a href='redirect.aspx' target="_blank">创建免费广告</a>
    <table>
         <asp:Repeater ID="rptAds" runat="server">
            <ItemTemplate>
        <tr>
               <td><input type="hidden" id="radio<%#Eval("id") %>" name="ads" value="<%#Eval("id") %>" title="<%#Eval("size") %>" /> <label for="radio<%#Eval("id") %>"><%#Eval("name") %></label> </td>
               <td><a href='http://www.7fshop.com/show/plist.aspx?id=<%#Eval("id") %>' target="_blank" style="font-weight:bold; color:Black">预览</a></td>
               <td><a href='#' onclick="InitData('radio<%#Eval("id") %>')" style="font-weight:bold; color:Black">插入此广告</a> </td>
        </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    </div>
    </form>

        <script language="javascript">
            function InitData(id) {
                var obj = document.getElementById(id);
                var str = '<a href="<%=nickid %>" target=_blank><img src="http://www.7fshop.com/show/html2jpg.aspx?id=' + obj.value + '" border=0 /></a>';
                
                if(obj.title == "743*308" || obj.title == "714*160"){
                    str = '<a href="<%=nickid %>" target=_blank><img src="http://www.7fshop.com/show/html2jpg.aspx?id=' + obj.value + '" width="650" border=0 /></a>';
                }

                if (navigator.appVersion.indexOf("MSIE") == -1) {
                    window.opener.returnAction(str);
                    window.close();
                } else {
                    window.returnValue = str;
                    window.close();
                }
            }

            function CloseWindow() {
                if (navigator.appVersion.indexOf("MSIE") == -1) {
                    window.close();
                } else {
                    window.close();
                }
            }
    </script>

</body>
</html>
