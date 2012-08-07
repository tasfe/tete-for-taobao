<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true"
    CodeFile="QQShare.aspx.cs" Inherits="QQShare" Title="分享到QQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 <script type="text/javascript">

     var jiathis_config = {
         boldNum: 0,
         showClose: false,
         sm: "qzone,tqq,qq,xiaoyou",
         imageUrl: '<%=GoodsPic %>',
         imageWidth: 26,
         marginTop: 150,
         url: '<%=ShowUrl %>',
         title: '<%=Keys %>',
         summary: "分享的文本摘要",
         pic: '<%=GoodsPic %>'
     } 
    </script>
    <div id="ContentLeft">
        <div id="ContentLeftTop">
            <div class="BarLeft">
            </div>
            <div id="ContentLeftTopText">
                QQ推广</div>
            <div class="BarRight">
            </div>
            <div class="Cal">
            </div>
        </div>
        <div id="ContentLeftBox">
            <ul>
                <li><a href="AddQQ.aspx">&gt; 一键推广</a></li>
                <li><a href="TuiList.aspx?type=2">&gt; 推广列表</a></li>
            </ul>
        </div>
    </div>
    <div id="ContentRight">
        <div id="ContentRightTop">
            <div class="BarLeft">
            </div>
            <div id="ContentRightTopText">
                一键推广</div>
            <div class="BarRight">
            </div>
            <div class="Cal">
            </div>
        </div>
        <div id="ContentRightBox">
            <h2>
                QQ推广&nbsp;&gt;&gt;&nbsp;一键推广：</h2>
            <br />
            <table width="100%" border="1" class="t1" id="mytab">
                <tr class="a1">
                    <td style="background-color:#E8F2FF;font-weight:bold;">
                        第二步：立即推广
                    </td>
                </tr>
            </table>
            <div>
                <table width="90%" border="1" style="margin: 30px;" class="t1" id="mytab">
                    <tr class="a1">
                        <td style="width: 140px;">
                            <a target="_blank" href="<%=GoodsUrl %>">
                                <img src="<%=GoodsPic %>" style="width: 100px; height: 100px;" class="taobaoimg"></a>
                        </td>
                        <td>
                            <a target="_blank" href="<%=GoodsUrl %>" >
                                <%=GoodsName %></a>
                        </td>
                    </tr>
                    <tr class="a1">
                        <td style="height: 35px;" colspan="2">
                            <span class="red">关键词：</span>
                            <asp:TextBox ID="TB_KeyWords" runat="server" ReadOnly="true" Text="" Width="285px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br>
                            <span class="red">推广到：</span><div class="red">
                                 1、将您要推广的宝贝提交到以下地址</div>
                            <div id="ckepop">
<!-- JiaThis Button BEGIN -->
<div id="Div1">
<span class="jiathis_txt">
	<a class="jiathis_button_qzone">QQ空间</a>
	<a class="jiathis_button_tqq">腾讯微博</a>
	<a class="jiathis_button_qq">QQ书签</a>
	<a class="jiathis_button_xiaoyou">朋友网</a>
	</span>
</div>
<script type="text/javascript" src="http://v3.jiathis.com/code_mini/jia.js" charset="utf-8"></script>
<!-- JiaThis Button END -->
                                        
                                     </div>
                        </td>
                    </tr>
                    <tr class="a1">
                        <td style="height: 35px;" colspan="2">
                            <span class="red">2、将推广宝贝后的链接地址提交到</span>&nbsp;<a target="_blank" href="http://www.soso.com/help/usb/urlsubmit.shtml">http://www.soso.com/help/usb/urlsubmit.shtml</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
