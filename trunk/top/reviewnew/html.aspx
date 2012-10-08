<%@ Page Language="C#" AutoEventWireup="true" CodeFile="html.aspx.cs" Inherits="top_review_html" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>好评有礼</title>
<link href="../css/common.css" rel="stylesheet" />
<style>
    td{font-size:12px;}
    a{color:Blue; text-decoration:none;}
    img{border:none}
</style>


</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="default.aspx" class="nolink">好评有礼</a> 活动展示 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
            <b style="font-size:16px;">好评有礼活动展示页和包邮卡客户查询链接：</b> <br />
            <textarea cols=50 rows=4>http://container.api.taobao.com/container?action=freecard&appkey=12690739&newnick=<%=nickencode%></textarea>
   
     <hr />

            <table width="700">
            <tr>
                <td align="left" colspan="4"><b style="font-size:16px;">请选择左侧分类的展示图片：</b></td>
            </tr>
            <tr>
                <td width="140"><a href="images/left2.jpg" target="_blank"><img src="images/left2.jpg" width="120" /></a> <br /> <input id="left2" name="left" type="radio" value="left2" /><label for="left2">图片1</label> </td>
                <td width="140"><a href="images/left3.jpg" target="_blank"><img src="images/left3.jpg" width="120" /></a> <br /> <input id="left3" name="left" type="radio" value="left3" /><label for="left3">图片2</label> </td>
                <td width="140"><a href="images/left4.jpg" target="_blank"><img src="images/left4.jpg" width="120" /></a> <br /> <input id="left4" name="left" type="radio" value="left4" /><label for="left4">图片3</label> </td>
                <td width="140"><a href="images/left5.jpg" target="_blank"><img src="images/left5.jpg" width="120" /></a> <br /> <input id="left5" name="left" type="radio" value="left5" /><label for="left5">图片4</label> </td>
            </tr>
            </table>
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="一键同步到左侧分类" />
            
            <hr />

            <table width="700" style="margin-top:6px">
            <tr>
                <td align="left" colspan="3"><b style="font-size:16px;">请选择宝贝描述插入的展示图片：</b></td>
            </tr>
            <tr>
                <td><a href="images/750a.jpg" target="_blank"><img src="images/750am.jpg" height="90" /></a> <br /> <input id="750a" name="detail" type="radio" value="750a" /><label for="750a">图片1</label> </td>
                <td><a href="images/750b.jpg" target="_blank"><img src="images/750bm.jpg" height="90" /></a> <br /> <input id="750b" name="detail" type="radio" value="750b" /><label for="750b">图片2</label> </td>
                <td><a href="images/750c.jpg" target="_blank"><img src="images/750cm.jpg" height="90" /></a> <br /> <input id="750c" name="detail" type="radio" value="750c" /><label for="750c">图片3</label> </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    展示位置：
                    <select name="detailimgistop" id="detailimgistop">
                        <option value="1">顶部↑</option>
                        <option value="0">底部↓</option>
                    </select>
                </td>
            </tr>
            </table>
            
                    <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="一键同步到宝贝描述" OnClientClick="check(this)" />
                    <asp:Button ID="Button4" runat="server" onclick="Button4_Click" Text="一键清除" OnClientClick="check(this)" /><br />
                    <b>亲，如果您的店铺宝贝很多，请您耐心等待一会~</b>
          

     </div>
</div>
</form>

<script>
    function check(obj) {
        obj.value = "同步中，请您耐心等待..";
        //obj.disabled = true;
    }

    document.getElementById("<%=leftimgname %>").checked = true;
    document.getElementById("<%=detailimgname %>").checked = true;

    if ('0' == '<%=detailimgistop %>') {
        document.getElementById("detailimgistop").selectedIndex = 1;
    }
</script>

</body>
</html>