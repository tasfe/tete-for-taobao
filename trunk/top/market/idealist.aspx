<%@ Page Language="C#" AutoEventWireup="true" CodeFile="idealist.aspx.cs" Inherits="top_market_idealist" %>

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
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="30"><strong>广告名称</strong></td>
        <td><strong>创建时间</strong></td>
        <td><strong>访问次数</strong></td>
        <td><strong>点击次数</strong></td>
        <td><strong>操作</strong></td>
      </tr>
      
      <asp:Repeater ID="rptIdeaList" runat="server">
        <ItemTemplate>
        
      <tr>
        <td height="30"><a href="addidea.aspx?id=<%#Eval("id") %>" style="color:#333; text-decoration:none"><%#Eval("name") %></a></td>
        <td style="color:#333"><%#Eval("date") %></td>
        <td><a href="result.aspx" style="color:#333"><%#Eval("viewcount") %></a></td>
        <td><a href="result.aspx" style="color:#333"><%#Eval("hitcount") %></a></td>
        <td>
        	<a href='addidea.aspx?id=<%#Eval("id") %>' style="color:#333; text-decoration:none">修改</a> 
        	<a href="addidea-3.aspx?id=<%#Eval("id") %>" style="color:#333; text-decoration:none">获取代码</a> 
        	<a href='../../show/plist.aspx?id=<%#Eval("id") %>' style="color:#333; text-decoration:none" target="_blank">预览</a> 
        	<a href='idealist.aspx?id=<%#Eval("id") %>&act=update' style="color:#333; text-decoration:none">更新</a> 
            <a href="#" onclick="del('<%#Eval("id") %>')" style="color:#333; text-decoration:none">删除</a>
        </td>
      </tr>
        
        </ItemTemplate>
      </asp:Repeater>
    </table>

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
