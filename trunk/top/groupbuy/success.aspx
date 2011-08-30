<%@ Page Language="C#" AutoEventWireup="true" CodeFile="success.aspx.cs" Inherits="top_groupbuy_success" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="javascript:;" class="nolink">特特团购</a> 团购创建成功 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
          <!--<p class="help"><a href="http://service.taobao.com/support/help.htm" target="_blank">查看帮助</a></p>-->
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content">
    
    <img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/68.gif' />
    <span style="font-size:20px; font-weight:bold">恭喜您，您已经成功创建了一个店内的团购活动！</span>
    
    <br />
    <br />
    您可以选择以下操作： <br />
    
    <a href='addtotaobao-1.aspx?id=<%=id %>'>更新到宝贝描述</a> <br />

    <a href='getcode.aspx?id=<%=id %>'>获取站外推广代码</a> <br />
    
    <a href='grouplist.aspx'>返回列表</a>

  </div>
</div>

<script language="javascript" type="text/javascript">
    function del(id) {
        if (confirm('你确定要删除吗，该操作不可恢复?')) {
            window.location.href = 'idealist.aspx?act=del&id=' + id;
        }
    }

    function copyToClipBoard(id) {
        var clipBoardContent = "<script src=\"http://www.7fshop.com/show/index.aspx?id=" + id + "\" language=\"javascript\" type=\"text/javascript\">\<\/script>";
        if (window.clipboardData) {
            window.clipboardData.setData("Text", clipBoardContent);
            alert("复制成功");
        } else {
            alert("FireFox浏览器不支持此功能");
        }
    }
</script>
</body>
</html>
