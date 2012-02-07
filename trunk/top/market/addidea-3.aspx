<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addidea-3.aspx.cs" Inherits="top_market_addidea_3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>我要推广</title>
<link href="../css/common.css" rel="stylesheet" />
<link href="images/tab.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">
<div class="navigation" style="height:500px;">
  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">营销推广</a> 我要推广 (4.获取发布代码)  </div>
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

  
<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold">
    若您不想自己去推广，可使用腾讯微博推广自动帮您推广 
    <a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22762-9:1' target="_blank">点此订购</a>
</div>

    <br />
    <img src='http://a.tbcdn.cn/sys/wangwang/smiley/48x48/68.gif' />
    <span style="font-size:20px; font-weight:bold">恭喜您，您已经成功创建了一个属于您自己的推广页！<br />特特推荐您使用以下几种推广方式：</span><br />
    <a href='showcode.aspx?id=<%=id %>' style="color:Red; font-weight:bold; font-size:16px;">【点击查看详细的推广教程】</a>
    <a href='../../show/plist.aspx?id=<%=id %>' target="_blank" style="color:Red; font-weight:bold; font-size:16px;">【查看广告效果】</a>
    <br />

    

<!--tab ads start-->
<div id="main" style="overflow:hidden; margin-left:6px;">
<div id="tabCot_product" class="tab" style="margin-left:30px;">
<div class="tabContainer">
<ul class="tabHead" id="tabCot_product-li-currentBtn-">
   <li class="currentBtn"><a href="javascript:void(0)" title="网站HTML代码" rel="1">网站HTML代码</a></li>
   <li><a href="javascript:void(0)" title="论坛推广代码" rel="2">论坛推广</a></li>
   <li><a href="javascript:void(0)" title="博客推广代码" rel="3">博客推广</a></li>
   <li><a href="javascript:void(0)" title="图片HTML代码" rel="4">图片HTML代码</a></li>
   <li><a href="javascript:void(0)" title="淘帮派推广代码" rel="5">淘帮派推广</a></li>
   <li><a href="javascript:void(0)" title="百度贴吧推广代码" rel="6">百度贴吧推广</a></li>
</ul>
</div>

<div id="tabCot_product_1" class="tabCot">
<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">1、可以以内嵌HTML形式放在自己论坛或者网站页面里面</span>
        </td>
      </tr>
      <tr>
        <td>
        	<textarea cols="80" rows="2" id="Textarea6"><script src="http://www.7fshop.com/show/index.aspx?id=<%=id %>" language="javascript" type="text/javascript"></script></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea1')" />
        	<br />
    <br />
        </td>
      </tr>
</table>
<div class="clear"></div>
</div>
<div id="tabCot_product_2" class="tabCot" style="display: none;">

<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">2、作为论坛或者淘帮派的个人签名，复制下面的代码作为远程图片地址即可</span>
        </td>
      </tr>
      <tr>
        <td>
            <strong>百度空间等其它论坛可用：</strong><br />
        	<textarea cols="80" rows="2" id="Textarea1">http://www.7fshop.com/show/html2jpg.aspx?id=<%=id %></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea2')" />
        	<br /><br />
        	<strong>百度贴吧专用：</strong><br />
        	<textarea cols="80" rows="2" id="Textarea5">http://www.7fshop.com/show/html2jpg/?id=<%=id %>.png</textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea5')" />
        	<br />
    <br />
        </td>
      </tr>
</table>
<div class="clear"></div>
</div>
<div id="tabCot_product_3" class="tabCot" style="display: none;">

<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">3、您可以直接将下面的图片用鼠标括起来右键点复制，然后粘贴到自己的博客日志里面，也可以将此图片右键下载<br />到本地发到您的微博里面</span>  <a href='http://blog.jianghu.taobao.com/u/MjA0MjAwODU2/blog/blog_detail.htm?aid=46415639' target="_blank" style="font-weight:bold; color:Red">客户案例</a>
        </td>
      </tr>
      <tr>
        <td>
        	<a href='<%=nickidEncode %>' target="_blank"><img src='http://www.7fshop.com/show/html2jpg.aspx?id=<%=id %>' border=0 /></a><br />
    <br />
        </td>
      </tr>
</table>

<div class="clear"></div>
</div>
<div id="tabCot_product_4" class="tabCot" style="display: none;">

<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">4、这个是图片的HTML代码，适于用会编辑HTML源代码的客户操作</span>
        </td>
      </tr>
      <tr>
        <td>
        	<textarea cols="80" rows="3" id="Textarea3"><a href='<%=nickidEncode %>' target="_blank"><img src='http://www.7fshop.com/show/html2jpg.aspx?id=<%=id %>' border=0 /></a></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea3')" />
        	<br />
    <br />
        </td>
      </tr>
</table>

<div class="clear"></div>
</div>
<div id="tabCot_product_5" class="tabCot" style="display: none;">

<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">5、这个是HTML形式的推广代码，适于用淘帮派等只能使用HTML发布不能使用脚本的论坛</span>
        </td>
      </tr>
      <tr>
        <td>
        	<textarea cols="80" rows="3" id="Textarea4">
<asp:Panel ID="panel1" runat="server">
<table width="<%=width %>" height="<%=height %>" cellpadding=0 cellspacing=0 style="margin:0; background:#e6f3fb; padding:0 6px 6px;">
<tr>
    <td style="padding:5px 0 4px 10px;height:15px;line-height:15px;background:#e6f3fb;font-size:12px;"><%=tabletitle%></td>
</tr>
<tr>
    <td>
    
    <table style="margin:0px 0px 4px 6px;padding:0px;background-color: #b2dcf4; " cellpadding=1 cellspacing=1>
        <tr>
            <td style="padding:0px;">

                    <table style="margin: 0px;background: #fff;padding: 0px 2px;border:0;" cellpadding=0 cellspacing=0 width="<%=width %>" height="<%=height %>">
                        <tr>
                    <asp:Repeater ID="rptProduct" runat="server">
                        <ItemTemplate>
                            <td height="120" width="100" align="center"><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" title="<%#Eval("itemname") %>" target="_blank"><img src="<%#Eval("itempicurl") %>_sum.jpg" width="80" height="80" border="0" /></a><br /><a href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" title="<%#Eval("itemname") %>" style="color:#0394ef;text-decoration:none; font-size:12px;" target="_blank"><%#Eval("itemname") %></a></td><%#Eval("html") %>
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
</asp:Panel>
<asp:Panel ID="panel2" runat="server" Visible="false"><table background="http://img02.taobaocdn.com/sns_album/i2/T1RllQXi4eXXartXjX.gif" border="0" cellpadding="0"
                                        cellspacing="0" height="30" style="border-right: #999999 1px solid; border-top: #999999 1px solid;
                                        border-left: #999999 1px solid; border-bottom: #999999 1px solid" width="740">
                                        <tr>
                                            <td>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table align="left" border="0" cellpadding="0" cellspacing="0" height="30">
                    <tr>
                        <td width="10">
                        </td>
                        <td background="http://img01.taobaocdn.com/sns_album/i1/T1UQtQXgXrXXartXjX.gif" width="24">
                        </td>
                        <td background="http://img04.taobaocdn.com/sns_album/i4/T1q7pQXXFtXXartXjX.gif">
                           <strong style="font-size: 13px; color:#ffffff"><%=tabletitle%></strong></td>
                        <td>
                            <img src="http://img04.taobaocdn.com/sns_album/i4/T1cjRQXnJEXXartXjX.gif" /></td>
                    </tr>
                </table>
            </td>
            <td align="right">
           
            </td>
        </tr>
    </table>
    
  </td></tr></table>  
    <table border="0" cellpadding="0" cellspacing="0" style="border-right: #999999 1px solid;
        border-top: #999999 1px solid; overflow: hidden; border-left: #999999 1px solid;
        border-bottom: #999999 1px solid" width="740">
        <tr>
            <td valign="top">
                <TABLE cellSpacing=0 cellPadding=0 width=730 
        border=0><TBODY><TR><TD align="middle">
        
        <TABLE cellSpacing=8 
            cellPadding=0 align=center border=0><TBODY><TR>
            
            
      <asp:Repeater ID="rptProduct2" runat="server">
            <ItemTemplate>
            
            <TD vAlign=top 
                align="middle" width=175 bgColor=white>
                <TABLE cellSpacing=0 
                  cellPadding=0 align=center border=0><TBODY><TR><TD 
                      align="middle"><DIV 
                        style="BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; MARGIN-TOP: 4px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 160px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 160px"><DIV 
                        style="OVERFLOW: hidden; WIDTH: 160px; HEIGHT: 160px"><A 
                        href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" 
                        target="_blank"><IMG 
                        src="<%#Eval("itempicurl") %>_160x160.jpg" 
                        border=0 /></A></DIV></DIV></TD></TR><TR><TD 
                      align="middle"><DIV 
                        style="PADDING-RIGHT: 4px; PADDING-LEFT: 4px; FONT-SIZE: 12px; PADDING-BOTTOM: 4px; PADDING-TOP: 4px"><A 
                        style="FONT-SIZE: 12px; COLOR: #3f3f3f; TEXT-DECORATION: none" 
                        href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" 
                        target="_blank"><%#Eval("itemname")%></A><BR /><FONT 
                        style="COLOR: #fe596a"><B>￥ <%#Eval("itemprice")%>元</B></FONT> 
                        </DIV><A 
                        href="http://item.taobao.com/item.htm?id=<%#Eval("itemid") %>" 
                        target="_blank"><IMG 
                        src="http://img02.taobaocdn.com/sns_album/i2/T1t8dQXgtgXXartXjX.gif" 
                        border=0 /></A> <DIV></DIV></TD></TR></TBODY></TABLE></TD>
                      
                     </ItemTemplate>
      </asp:Repeater>       
              
              
              </TR>
              

              
              
              </TBODY></TABLE>
         
         
         
         </TD></TR></TBODY></TABLE>
            </td>
        </tr>
                       <tr>
                                            <td align="right" height="24" style="border-bottom: #999999 1px solid" valign="center">
                                                <a href="<%=nickid %>" style="text-decoration: none"
                                                    target="_blank"><font style="font-size: 13px; color: #ff6600"><strong>更多详情请见 <%=nickid %></strong></font></a>
                                            </td>
                                        </tr>
    </table>
</asp:Panel>
        	</textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea4')" />
        	<br />
    <br />
        </td>
      </tr>
</table>

<div class="clear"></div>
</div>


<div id="tabCot_product_6" class="tabCot" style="display: none;">

<table style="border-style:none;">
<tr>
        <td>
        	<span style="font-size:14px;">6、这个是用于百度贴吧，空间等无法给图片上加链接的文字型连接</span>
        </td>
      </tr>
      <tr>
        <td>
            <textarea cols="80" rows="2" id="Textarea7" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea>
            <br />
        	<input type="submit" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea7')" />
        	<br />
    <br />
        </td>
      </tr>
</table>


<div class="clear"></div>
</div>

<div class="modBottom">
    <span class="modABL"></span>
    <span class="modABR"></span>
</div>
</div>
</div>
<div class="noprint"> 
<script type="text/javascript" language="jscript">
    function tab(o, s, cb, ev) {//tab切换类
        var $ = function (o) { return document.getElementById(o) };
        var css = o.split((s || '_'));
        if (css.length != 4) return;
        this.event = ev || 'onclick';
        o = $(o);
        if (o) {
            this.ITEM = [];
            o.id = css[0];
            var item = o.getElementsByTagName(css[1]);
            var j = 1;
            for (var i = 0; i < item.length; i++) {
                if (item[i].className.indexOf(css[2]) >= 0 || item[i].className.indexOf(css[3]) >= 0) {
                    if (item[i].className == css[2]) o['cur'] = item[i];
                    item[i].callBack = cb || function () { };
                    item[i]['css'] = css;
                    item[i]['link'] = o;
                    this.ITEM[j] = item[i];
                    item[i]['Index'] = j++;
                    item[i][this.event] = this.ACTIVE;
                }
            }
            return o;
        }
    }
    tab.prototype = {
        ACTIVE: function () {
            var $ = function (o) { return document.getElementById(o) };
            this['link']['cur'].className = this['css'][3];
            this.className = this['css'][2];
            try {
                $(this['link']['id'] + '_' + this['link']['cur']['Index']).style.display = 'none';
                $(this['link']['id'] + '_' + this['Index']).style.display = 'block';
            } catch (e) { }
            this.callBack.call(this);
            this['link']['cur'] = this;
        }
    }
    /*
    tab 使用方法:
    new tab(标签ID, id分隔符, 单击事触发函数, 什么事件触发TAB切换);
    标签ID:        ID命名格式为: 前缀+分隔符+TAB标签的HTML标签名+激活状态下标签样式+分隔符+非激活状态下标签样式(必须)
    id分隔符:       分隔符(必须)
    TAB切换时触发函数: TAB切换时触发函数(可选)
    什么事件触发TAB切换:可选(默认为onclick)
    注: 标签ID命名时的前缀将做为 该标签的新ID值,所以前缀不要与现在任何元素的ID值相同.
    返回值为: 标签ID所对象的对象.
    切换标签时对应的 项目名称命名规则:
    前缀+_+顺序值
    具体理解,可以看上面的代码,比如
    ID为 "test3_li_now_" 的对象代表的意思是:
    前缀为 test3
    li 为 id为 "test3_li_now_" 标签下面的 li 标签 做为TAB项.
    now 为 标签激活时的样式
    "" 最后的空为 非激活状态下的样式
    每个标签项 激活 状态下对应的元素的ID应该命名为:
    test3_1   第一个标签项对应项目
    test3_2   第二个标签项对应项目
    test3_3   第三个标签项对应项目
    等等
    */
    new tab('tabCot_product-li-currentBtn-', '-');
</script>
</div>


<!--tab ads end-->

<!--  <br />   
    <img src="http://a.tbcdn.cn/sys/wangwang/smiley/48x48/28.gif" /> <a href='redirect.aspx?t=2' target="_blank" style="font-size:20px;">我没有时间去博客发文章推广，能不能自动帮我发文章呢？</a>-->

    <br />  

 <!--<a href='javascript:GoToHelp();' style="color:Red; font-weight:bold; font-size:16px;">【如果您还是不会操作，请点此查看更加详细的教程】</a> <br />-->
<input type="button" value="返回列表" onclick="backToList()" />
<input type="button" value="继续添加" onclick="backToAdd()" />



   <!--<table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
        	<span style="font-size:14px;">1、可以以内嵌HTML形式放在自己论坛或者网站页面里面</span>
        </td>
      </tr>
      <tr>
        <td>
        	<textarea cols="80" rows="2" id="Textarea1"><script src="http://www.7fshop.com/show/index.aspx?id=<%=id %>" language="javascript" type="text/javascript"></script></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea1')" />
        	<br />
    <br />
        </td>
      </tr>
      <tr>
        <td>
        	<span style="font-size:14px;">2、作为论坛或者淘帮派的个人签名，复制下面的代码作为远程图片地址即可</span>
        </td>
      </tr>
      <tr>
        <td>
            <strong>百度空间等其它论坛可用：</strong><br />
        	<textarea cols="80" rows="2" id="Textarea2">http://www.7fshop.com/show/html2jpg.aspx?id=<%=id %></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea2')" />
        	<br /><br />
        	<strong>百度贴吧专用：</strong><br />
        	<textarea cols="80" rows="2" id="Textarea5">http://www.7fshop.com/show/html2jpg/?id=<%=id %>.png</textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea5')" />
        	<br />
    <br />
        </td>
      </tr>
      <tr>
        <td>
        	<span style="font-size:14px;">3、您可以直接将下面的图片用鼠标括起来右键点复制，然后粘贴到自己的博客日志里面，也可以将此图片右键下载<br />到本地发到您的微博里面</span>  <a href='http://blog.jianghu.taobao.com/u/MjA0MjAwODU2/blog/blog_detail.htm?aid=46415639' target="_blank" style="font-weight:bold; color:Red">客户案例</a>
        </td>
      </tr>
      <tr>
        <td>
        	<a href='<%=nickid %>' target="_blank"><img src='http://www.7fshop.com/show/html2jpg.aspx?id=<%=id %>' border=0 /></a><br />
    <br />
        </td>
      </tr>
      <tr>
        <td>
        	<span style="font-size:14px;">4、这个是图片的HTML代码，适于用会编辑HTML源代码的客户操作</span>
        </td>
      </tr>
      <tr>
        <td>
        	<textarea cols="80" rows="3" id="Textarea3"><a href='<%=nickid %>' target="_blank"><img src='http://www.7fshop.com/show/html2jpg.aspx?id=<%=id %>' border=0 /></a></textarea><br />
        	<input type="submit" value="复制代码" onclick="copyToClipBoard('Textarea3')" />
        	<br />
    <br />
        </td>
      </tr>
      
      <tr>
        <td>
        	<span style="font-size:14px;">6、这个是用于百度贴吧，空间等无法给图片上加链接的文字型连接</span>
        </td>
      </tr>
      <tr>
        <td>
            <textarea cols="80" rows="2" id="Textarea7" onclick="this.value=='请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”'?this.value='':void(0)">请在这里写上您推广的广告语，如“秋季特惠，买一送一，详情请见店铺公告~”</textarea>
            <br />
        	<input type="submit" value="生成有链接的广告词" onclick="copyToClipBoardAdd('Textarea7')" />
        	<br />
    <br />
        </td>
      </tr>
      <tr>
        <td>
            <a href='javascript:GoToHelp();' style="color:Red; font-weight:bold; font-size:16px;">【如果您还是不会操作，请点此查看更加详细的教程】</a> <br />
        	<input type="button" value="返回列表" onclick="backToList()" />
        	<input type="button" value="继续添加" onclick="backToAdd()" />
        </td>
      </tr>
    </table>-->


  </div>

</div>

<script language="javascript" type="text/javascript">
    function GoToHelp()
    {
        window.location.href='showcode.aspx?id=<%=id %>';
        parent.scroll(0,0);
    }
    
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
</script>

<div style="display:none">
<script src="http://s15.cnzz.com/stat.php?id=2663320&web_id=2663320&show=pic" language="JavaScript"></script>
</div>


</body>
</html>
