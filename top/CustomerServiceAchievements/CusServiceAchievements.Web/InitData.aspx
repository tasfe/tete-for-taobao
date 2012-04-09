<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InitData.aspx.cs" Inherits="InitData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>正在初始化，请稍候...</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    正在初始化，请稍候...
       <script type="text/javascript">

        var XMLHttpReq;
        function createXMLHttpRequest() {
            if (window.XMLHttpRequest) { //Mozilla 浏览器
                XMLHttpReq = new XMLHttpRequest();
            }
            else if (window.ActiveXObject) { // IE浏览器
                try {
                    XMLHttpReq = new ActiveXObject("Msxml2.XMLHTTP");
                } catch (e) {
                    try {
                        XMLHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
                    } catch (e) { }
                }
            }
        }

        //发送匹配请求函数
        function DoFetch() {
            createXMLHttpRequest();
            var url = "Default.aspx";
            XMLHttpReq.open("GET", url, true);
            XMLHttpReq.onreadystatechange = processFetchResponse; //指定响应函数
            XMLHttpReq.send(null); // 发送请求
        }

        // 处理返回匹配信息函数
        function processFetchResponse() {
            if (XMLHttpReq.readyState == 4) { // 判断对象状态
                if (XMLHttpReq.status == 200) { // 信息已经成功返回，开始处理信息
                    closeDiv();
                    if (XMLHttpReq.responseText == "true")
                        location.href = "Default2.aspx";
                    else
                        alert(XMLHttpReq.responseText);
                } else { //页面不正常
                    window.alert("您所请求的页面有异常。");
                }
            }
        }

        window.onload = function() {
            DoFetch();
        }
      </script>
    </div>
    </form>
</body>
</html>
