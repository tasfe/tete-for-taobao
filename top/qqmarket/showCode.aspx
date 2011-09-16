<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showCode.aspx.cs" Inherits="top_market_showCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>推广方式</title>
<link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 特特推荐的推广方式 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
        </div>
      </li>
    </ul>
  </div>
  <div id="main-content">
  
  <div style="display:none">
  <form id="tiebaForm" method="get" target="_blank">
    <input type="submit" value="submit" />
    <textarea id="Textarea1" style="display:none">http://www.7fshop.com/qqshow/html2jpg/?id=<%=id %>.png</textarea>
    <textarea id="Textarea12" style="display:none">http://www.7fshop.com/qqshow/html2jpg.aspx?id=<%=id %></textarea>
    <textarea id="Textarea2" style="display:none"><a href='<%=nickid %>' target="_blank"><img src='http://www.7fshop.com/qqshow/html2jpg/?id=<%=id %>.png' /></a></textarea>
    <textarea cols="80" rows="3" id="Textarea6">
        	
<table width="514" height="168" cellpadding=0 cellspacing=0 style="margin:0; background:#e6f3fb; padding:0 6px 6px;">
<tr>
    <td style="padding:5px 0 4px 10px;height:15px;line-height:15px;background:#e6f3fb;font-size:12px;"><%=tabletitle%></td>
</tr>
<tr>
    <td>
    
    <table style="margin:0px 0px 4px 6px;padding:0px;background-color: #b2dcf4; " cellpadding=1 cellspacing=1>
        <tr>
            <td style="padding:0px;">

                    <table style="margin: 0px;background: #fff;padding: 0px 2px;border:0;" cellpadding=0 cellspacing=0 width="460" height="160">
                        <tr>
                    <asp:Repeater ID="rptProduct" runat="server">
                        <ItemTemplate>
                            <td height="120" width="100" align="center"><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" title="<%#Eval("itemname") %>" target="_blank"><img src="<%#Eval("itempicurl") %>" width="80" height="80" border="0" /></a><br /><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" title="<%#Eval("itemname") %>" style="color:#0394ef;text-decoration:none; font-size:12px;" target="_blank"><%#Eval("itemname") %></a></td>
                        </ItemTemplate>
                    </asp:Repeater>
                        </tr>
                    </table>
                    
            </td>
        </tr>
    </table>
    
    </td>
</tr>
</table><br />
更多详情请见： <a href='<%=nickid %>' target="_blank"><%=nickid %></a>
        	</textarea>
</form>
  </div>
  
  <img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/68.gif' />
    <span style="font-size:20px; font-weight:bold">特特通过对您店铺目前情况的分析，推荐您使用以下几种推广方式：</span><br />
    <br />
    
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="60">
        	1、<img src="http://img.baidu.com/img/post-jg.gif" style="cursor:pointer" onclick="showArea(1, this)" />
        	<span style="font-size:16px; cursor:pointer" onclick="showArea(1, this)">去百度贴吧发帖或回帖推广</span> 
        </td>
      </tr>
      <tr id="area1">
        <td style="padding-left:20px">
            <b>步骤一</b>、进入百度帖吧<a href='http://tieba.baidu.com/' target="_blank">http://tieba.baidu.com/</a>，点击页面页面右上的<b>注册</b>按钮，先注册个会员，因为很多贴吧是不允许非会员发帖回帖的。
            <br /><hr />
            <b>步骤二</b>、在顶部的搜索框内输入您店铺的主营商品关键字就会进入相应的贴吧
        	<br />
        	<hr />
            <b>步骤三</b>、任意点开一篇帖子，然后把页面拉到最下面，这里有一个可以让您回帖的输入框。您也可以直接发一篇推广的帖子，但是前期建议多使用回复去推广：）~
        	<br />
        	<hr />
            <b>步骤四</b>、请点击<input type="button" value="复制代码" onclick="copyToClipBoard('Textarea1')" />复制您的推广图片地址，然后点击发帖处<img src='images/insertbaidu.gif' />，并粘贴您刚才复制的地址，然后点击“插入图片”就会把你的推广图片插入
        	<br />
        	<hr />
        	<b>步骤五</b>、请输入您的广告词 <br />
        	<textarea cols="80" rows="2" id="Textarea4" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea><br />
            <input type="button" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea4')" /> ，然后把代码跟在图片后面粘贴发布即可。<br />
    <br /><hr />
     <b>步骤六</b>、如果您已经发布成功，那您需要找到更多的帖子去重复步骤三到步骤五，这样您就会发现每天店铺的流量在不断增长了，那么继续尝试一下下面的推广吧。<img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/88.gif' />
     <input type="button" value="尝试下一个推广" onclick="showArea(2, this)" />
        	<br />
        	<hr />
        </td>
      </tr>
      
      <tr>
        <td height="60">
        	2、<img src="images/taobangpai.gif" style="cursor:pointer" onclick="showArea(2, this)" />
        	<span style="font-size:16px; cursor:pointer" onclick="showArea(2, this)">去淘帮派发帖或回帖推广</span> 
        </td>
      </tr>
      <tr id="area2" style="display:none">
        <td style="padding-left:20px">
        	<b>步骤一</b>、进入淘帮派<a href='http://bangpai.taobao.com/' target="_blank">http://bangpai.taobao.com/</a>，点击页面页面右上的<b>注册</b>按钮，先注册个会员，因为很多贴吧是不允许非会员发帖回帖的。
            <br /><hr />
            <b>步骤二</b>、在顶部右上的搜索框前选择“帮派”，然后在输入框内输入您店铺的主营商品关键字就会出现适合您推广的帮派。
        	<br />
        	<hr />
            <b>步骤三</b>、进去帮派后任意点开一篇帖子，然后把页面拉到最下面，这里有一个可以让您回帖的输入框。
        	<br />
        	<hr />
            <b>步骤四</b>、请点击<input type="button" value="复制代码" onclick="copyToClipBoard('Textarea6')" />复制您的推广代码，点击输入框左下角的<img src='images/bangpaiadd.gif' />将输入框切换为源码模式粘贴，然后粘贴到输入框内然后发布即可。
        	<br />
        	<hr />
     <b>步骤五</b>、如果您已经发布成功，那您需要找到更多的帖子去重复步骤三到步骤四，这样您就会发现每天店铺的流量在不断增长了，那么继续尝试一下下面的推广吧。<img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/88.gif' />
     <input type="button" value="尝试下一个推广" onclick="showArea(3, this)" />
        	<br />
        	<hr />
        </td>
      </tr>
      <tr>
        <td height="60">
        	3、<img src="http://img.baidu.com/img/logo-hi.gif" style="cursor:pointer" onclick="showArea(3, this)" />
        	<span style="font-size:16px; cursor:pointer" onclick="showArea(3, this)">去百度空间推广</span> 
        </td>
      </tr>
      <tr id="area3" style="display:none">
        <td style="padding-left:20px">
        	<b>步骤一</b>、进入百度空间<a href='http://hi.baidu.com/index.htm' target="_blank">http://hi.baidu.com/index.htm</a>，进入<a href='http://hi.baidu.com/reg/new'>http://hi.baidu.com/reg/new</a>注册个您自己的空间。
            <br />
        	<hr />
            <b>步骤二</b>、如果您有了会员则可以直接登录百度空间，点击左侧菜单的“文章”，然后再点击右上的<img src="images/baiduhiadd.gif" />发表新文章
        	<br />
        	<hr />
            <b>步骤三</b>、请点击<input type="button" value="复制代码" onclick="copyToClipBoard('Textarea12')" />复制您的推广图片地址，然后点击发帖处<img src='images/baiduhiadd1.gif' />，并粘贴您刚才复制的地址，然后点击“插入图片”再选择“添加网上图片”就会把你的推广图片插入。
        	<br />
        	<hr />
        	<b>步骤四</b>、请输入您的广告词 <br />
        	<textarea cols="80" rows="2" id="Textarea3" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea><br />
            <input type="button" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea')" /> ，然后把代码跟在图片后面粘贴发布即可。<br />
            <br /><hr />
            <b>步骤五</b>、如果您已经发布成功，那您需要找到更多的帖子去重复步骤一到步骤四，这样您就会发现每天店铺的流量在不断增长了，祝您财源滚进。<img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/88.gif' />
     <input type="button" value="尝试下一个推广" onclick="showArea(4, this)" />
        	<br />
        </td>
      </tr>
      <tr>
        <td height="60">
        	4、<img src="images/qqweibo.gif" style="cursor:pointer" onclick="showArea(4, this)" />
        	<span style="font-size:16px;cursor:pointer" onclick="showArea(4, this)">去腾讯微博推广</span>
        </td>
      </tr>
      <tr id="area4" style="display:none">
        <td style="padding-left:20px">
        	<b>步骤一</b>、去腾讯微博首页<a href='http://t.qq.com/' target="_blank">http://t.qq.com/</a>使用QQ帐号登录微博 <br />
            <hr />
        	<b>步骤二</b>、在下面的图片上点鼠标右键下载到本地 <br />
        	<img src='http://www.7fshop.com/qqshow/html2jpg/?id=<%=id %>.png' />
            <br /><hr />
        	<b>步骤三</b>、点击腾讯微博页面头部的<img src='images/qqtadd.gif' />打开插入图片窗口，选择您刚才下载到本地的图片然后上传。
            <br /><hr />
        	<b>步骤四</b>、请输入您的广告词<br />
        	<textarea cols="80" rows="2" id="Textarea7" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea><br />
            <input type="button" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea7')" /> ，然后把代码粘贴到输入框内发布即可。
            <br /><hr />
            <b>步骤五</b>、如果您已经发布成功，那您需要更换QQ帐号去重复步骤三到步骤四，这样您就会发现每天店铺的流量在不断增长了，祝您财源滚进。<img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/88.gif' />
     <input type="button" value="尝试下一个推广" onclick="showArea(5, this)" />
 <br />
        </td>
      </tr>
      <tr>
        <td height="60">
        	5、<img src="images/qqmail.gif" style="cursor:pointer" onclick="showArea(5, this)" />
        	<span style="font-size:16px;cursor:pointer" onclick="showArea(5, this)">QQ邮件群发</span>
        </td>
      </tr>
      <tr id="area5" style="display:none">
        <td style="padding-left:20px">
        	<b>步骤一</b>、去QQ邮箱首页<a href='http://mail.qq.com/' target="_blank">http://mail.qq.com/</a>使用QQ帐号登录邮箱。
            <br /><hr />
            <b>步骤二</b>、先点击左侧的写信弹出邮件发送页面，然后在右侧的联系人列表里面选择你需要发送推广邮件的好友。
            <br /><hr />
            <b>步骤三</b>、请点击<input type="button" value="复制代码" onclick="copyToClipBoard('Textarea1')" />复制图片地址，
        	<textarea cols="80" rows="2" id="Textarea10" style="display:none">http://www.7fshop.com/qqshow/html2jpg/?id=<%=id %>.png</textarea><br />
        	然后点击输入框上面的<img src='images/qqmailadd.gif' />弹出插入商品提示框，选择“网络照片”，并粘贴您刚才复制的地址，然后点击确定就会把你的广告图片插入
 <br /><hr />
        	<b>步骤四</b>、请输入您的广告词 <br />
        	<textarea cols="80" rows="2" id="Textarea9" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea><br />
            <input type="button" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea9')" /> ，然后把代码粘贴到输入框内发布即可。
            <br /><hr />
            <b>步骤五</b>、如果您已经发布成功，那您需要更换QQ帐号去重复步骤二到步骤四，这样您就会发现每天店铺的流量在不断增长了，祝您财源滚进。<img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/88.gif' />
     <input type="button" value="尝试下一个推广" onclick="showArea(6, this)" />
        </td>
      </tr>
      <tr>
        <td height="60">
        	6、<img src="images/qzone.gif" style="cursor:pointer" onclick="showArea(6, this)" />
        	<span style="font-size:16px;cursor:pointer" onclick="showArea(6, this)">去QQ空间推广</span>
        </td>
      </tr>
      <tr id="area6" style="display:none">
        <td style="padding-left:20px">
        	<b>步骤一</b>、去QQ空间首页<a href='http://qzone.qq.com/' target="_blank">http://qzone.qq.com/</a>使用QQ帐号登录QQ空间。
            <br /><hr />
            <b>步骤二</b>、先点击页面顶部的日志栏目，然后点击下面内容左侧的“写日志”，输入您的推广标题。
            <b>步骤三</b>、请点击<input type="button" value="复制代码" onclick="copyToClipBoard('Textarea1')" />复制图片地址，
        	<textarea cols="80" rows="2" id="Textarea5" style="display:none">http://www.7fshop.com/qqshow/html2jpg/?id=<%=id %>.png</textarea><br />
        	然后所在输入框的上面菜单点击<img src='images/qzoneadd.gif' />，在弹出的提示框内选择“网络图片”并粘贴您刚才复制的图片地址，点击输入框下面的添加，稍等一下，再点击右下的“确定”插入您的推广图片。
 <br /><hr />
        	<b>步骤四</b>、请输入您的广告词 <br />
        	<textarea cols="80" rows="2" id="Textarea8" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea><br />
            <input type="button" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea8')" /> ，然后把代码粘贴到输入框内发布即可。
            <br /><hr />
            <b>步骤五</b>、如果您已经发布成功，那您需要更换QQ帐号去重复步骤二到步骤四，这样您就会发现每天店铺的流量在不断增长了，祝您财源滚进。<img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/88.gif' />
     <input type="button" value="尝试下一个推广" onclick="showArea(7, this)" />
 <br />
        </td>
      </tr>
      <tr>
        <td height="60">
        	7、<img src="images/sina.gif" style="cursor:pointer" onclick="showArea(7, this)" />
        	<span style="font-size:16px;cursor:pointer" onclick="showArea(7, this)">去新浪微博推广</span>
        </td>
      </tr>
      <tr id="area7" style="display:none">
        <td style="padding-left:20px">
        	<b>步骤一</b>、去新浪微博首页<a href='http://t.sina.com.cn/' target="_blank">http://t.sina.com.cn/</a>使用新浪帐号登录微博，如果没有帐号请点此注册<a href='http://t.sina.com.cn/reg.php?ps=u3&lang=zh' target="_blank">http://t.sina.com.cn/reg.php?ps=u3&lang=zh</a> <br />
            <hr />
        	<b>步骤二</b>、在下面的图片上点鼠标右键下载到本地 <br />
        	<img src='http://www.7fshop.com/qqshow/html2jpg/?id=<%=id %>.png' />
            <br /><hr />
        	<b>步骤三</b>、点击新浪微博页面头部的<img src='images/sinaadd.gif' />打开插入图片窗口，选择您刚才下载到本地的图片然后上传。
            <br /><hr />
        	<b>步骤四</b>、请输入您的广告词<br />
        	<textarea cols="80" rows="2" id="Textarea11" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea><br />
            <input type="button" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea11')" /> ，然后把代码粘贴到输入框内发布即可。
            <br /><hr />
            <b>步骤五</b>、如果您已经发布成功，那您需要更换新浪帐号去重复步骤三到步骤四，这样您就会发现每天店铺的流量在不断增长了，祝您财源滚进。<img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/88.gif' />
        <input type="button" value="尝试下一个推广" onclick="showArea(8, this)" />
 <br />
        </td>
      </tr>
     <tr>
        <td height="60">
        	8、<img src="images/tianya.gif" style="cursor:pointer" onclick="showArea(8, this)" />
        	<span style="font-size:16px;cursor:pointer" onclick="showArea(8, this)">嵌入自有论坛或者网站进行推广</span>
        </td>
      </tr>
      
      <tr id="area8" style="display:none">
        <td style="padding-left:20px">
        	<b>步骤一</b>、打开您论坛或者网站的FTP，下载您需要嵌入代码的HTML页面 <br />
            <br /><hr />
            <b>步骤二</b>、请点击<input type="button" value="复制代码" onclick="copyToClipBoard('Textarea16')" />复制下面地址，<br />
        	<textarea cols="80" rows="2" id="Textarea16"><script src="http://www.7fshop.com/qqshow/index.aspx?id=<%=id %>" language="javascript" type="text/javascript"></script></textarea><br />
        	并嵌入到相应的HTML页面中，就可以在论坛或者网站上推广您的店铺和商品
 <br />
        </td>
      </tr>
      
      <tr>
        <td>
            <b style="font-size:16px; color:red">通过以上几种推广方式，您就可以在互联网上推广您的产品了。<br />只有辛苦的付出才有灿烂的收获，特特祝您早日成功！</b><img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/68.gif' /><br /><br />
        </td>
      </tr>

      <tr>
        <td>
        	<input type="button" value="后退" onclick="backToGo()" />
        	<input type="button" value="返回列表" onclick="backToList()" />
        	<input type="button" value="继续添加" onclick="backToAdd()" />
        </td>
      </tr>
    </table>

  </div>
</div>
<div>

</div>

<script language="javascript" type="text/javascript">
    function backToAdd()
    {
        window.location.href='addidea.aspx';
        parent.scroll(0,0);
    }
    
    function backToList()
    {
        window.location.href='idealist.aspx';
        parent.scroll(0,0);
    }
    
    function backToGo()
    {
        history.go(-1);
        parent.scroll(0,0);
    }

    function copyToClipBoard(id)
    {
	    var clipBoardContent=document.getElementById(id).value
	    if(window.clipboardData){
	        window.clipboardData.setData("Text",clipBoardContent);
	        alert("复制成功");
	    }else{
	        alert("FireFox浏览器不支持此功能,请手动复制");
	    }
    }
    
    function copyToClipBoardAdd(id)
    {
	    var clipBoardContent= '\r\n' + document.getElementById(id).value + '\r\n更多详情请见 <%=nickid %>';
	    if(window.clipboardData){
	        window.clipboardData.setData("Text",clipBoardContent);
	        alert("复制成功");
	    }else{
	        alert("FireFox浏览器不支持此功能,请手动复制");
	    }
    }
    
    function showTieba()
    {
        var kw = document.getElementById("kwBak").value;

        document.getElementById("tiebaForm").action = "http://tieba.baidu.com/f?kw=1111";
        document.getElementById("tiebaForm").submit();
    }
    
    function showArea(i, obj)
    {
        HiddenAll();
        document.getElementById("area"+i).style.display = '';
    }
    
    function HiddenAll()
    {
        document.getElementById("area1").style.display = 'none';
        document.getElementById("area2").style.display = 'none';
        document.getElementById("area3").style.display = 'none';
        document.getElementById("area4").style.display = 'none';
        document.getElementById("area5").style.display = 'none';
        document.getElementById("area6").style.display = 'none';
        document.getElementById("area7").style.display = 'none';
        document.getElementById("area8").style.display = 'none';
    }
</script>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>

