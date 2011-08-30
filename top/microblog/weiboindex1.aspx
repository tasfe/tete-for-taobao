<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weiboindex1.aspx.cs" Inherits="top_microblog_weiboindex1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微博营销</title>
    <link href="../css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">

    <form id="form1" runat="server">
    <input type="hidden" name="t" id="t" value="" />
<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">微博营销</a> 自动微博营销 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">
    
    <asp:Panel ID="panel2" runat="server" Visible="true">
<div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px; color:Red; font-weight:bold; margin-bottom:6px;">
尊敬的用户您好，您在关联了新浪微博以后可以自动将淘宝商品推广到您的店铺里面
</div>
    
    <asp:Button ID="Button6" runat="server" Text="进入新浪微博授权" onclick="Button6_Click" />
    
    <br />
    </asp:Panel>



    <asp:Panel ID="panel1" runat="server" Visible="false">
    

    目前已经关联新浪微博：

    <asp:Repeater ID="rptMicroBlog" runat="server">
        <ItemTemplate>
            http://t.sina.com.cn/<%#Eval("uid") %> <a href='<%#Eval("pass") %>' target="_blank">查看微博</a> <br />
        </ItemTemplate>
    </asp:Repeater>

    <hr />

        <div style="height:22px;">
            新品上架自动发微博：
            <!--<input type="radio" name="auto1" value="1" checked="checked" />是 
            <input type="radio" name="auto1" value="0" />否 -->
            已发布：0条 
        </div>
        <div id="area1">
            <textarea id="text1" name="text1" runat="server" cols="50" rows="3"></textarea>
        </div>
        <br />
        <div style="height:22px;">
            宝贝售出自动发微博：
            <!--<input type="radio" name="auto2" value="1" checked="checked" />是 
            <input type="radio" name="auto2" value="0" />否  -->
            已发布：0条
        </div>
        <div id="area2">
            <textarea id="text2" name="text2" runat="server" cols="50" rows="3"></textarea>
        </div>
        <br />
        <div style="height:22px;">
            买家好评自动发微博：
            <!--<input type="radio" name="auto3" value="1" checked="checked" />是 
            <input type="radio" name="auto3" value="0" />否  -->
            已发布：0条
        </div>
        <div id="area3">
            <textarea id="text3" name="text3" runat="server" cols="50" rows="3"></textarea>
        </div>
        <br />
        <div style="height:22px;">
            橱窗推荐自动发微博：
            <!--<input type="radio" name="auto4" value="1" checked="checked" />是 
            <input type="radio" name="auto4" value="0" />否  -->
             已发布：0条
        </div>
        <div id="area4">
            <textarea id="text4" name="text4" runat="server" cols="50" rows="3"></textarea>
        </div>
        <hr />
    <br />

        <div>
            &nbsp;<asp:Button ID="Button3" runat="server" Text="保 存" 
                onclick="Button3_Click" />
        </div>

        </asp:Panel>
    </div>
</div>
</form>

</body>
</html>
