<%@ Page Language="C#" AutoEventWireup="true" CodeFile="success.aspx.cs" Inherits="top_market_success" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 推广管理 </div>
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
    
    <font style="font-size:14px; font-weight:bold">推广更新成功！！</font><br> 
    
    <br />
    下面为更新的图片广告效果：<br />
    <img src="http://www.7fshop.com/show/html2jpg.aspx?id=<%=id %>" />
    
    <a href='../../show/plist.aspx?id=<%=id %>' target='_blank'>查看更新过的文字广告效果</a> <br> 
    <a href='idealist.aspx'>返回列表</a>

  </div>
</div>

<script language="javascript" type="text/javascript">
function del(id){
    if(confirm('你确定要删除吗，该操作不可恢复?')){
        window.location.href = 'idealist.aspx?act=del&id=' + id;
    }
}

function copyToClipBoard(id)
{
    var clipBoardContent="<script src=\"http://www.7fshop.com/show/index.aspx?id="+id+"\" language=\"javascript\" type=\"text/javascript\">\<\/script>";
    if(window.clipboardData){
        window.clipboardData.setData("Text",clipBoardContent);
        alert("复制成功");
    }else{
        alert("FireFox浏览器不支持此功能");
    }
}
</script>


<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>
