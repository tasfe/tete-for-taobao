<%@ Page Language="C#" AutoEventWireup="true" CodeFile="activityAddsuccess.aspx.cs" Inherits="top_groupbuy_activityAddsuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="javascript:;" class="nolink">特特促销活动</a> 促销活动创建成功 </div>
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
    <span style="font-size:20px; font-weight:bold">恭喜您，您已经成功创建了一个促销活动！</span><a href='activityadd.aspx'>继续添加促销活动</a> <br />
    
    <br />
    <br />
    您还可以： <br />
    
    
    
    <a href='activityList.aspx'>管理创建的活动 </a>

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

