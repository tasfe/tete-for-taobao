<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" Title="首页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="ContentLeft">
    <div id="ContentLeftTop">
      <div class="BarLeft"></div>
      <div id="ContentLeftTopText">首页</div>
      <div class="BarRight"></div>
      <div class="Cal"> </div>
    </div>
    <div id="ContentLeftBox">
      <ul>
        <li><a href="#"> &gt; 软件使用说明</a></li>
      </ul>
    </div>
  </div>
<div id="ContentRight">
    <div id="ContentRightTop">
      <div class="BarLeft"></div>
      <div id="ContentRightTopText">软件使用说明</div>
      <div class="BarRight"></div>
      <div class="Cal"></div>
    </div>
    <div id="ContentRightBox">
                <br>
                <h2>
                    首页&nbsp;&gt;&gt;&nbsp;软件使用说明：</h2>
                <br>
		<p>
                    
                <p>
                    <span class="red" style="font-weight: bold;">软件说明：将宝贝一键推广到QQ、百度等旗下各大网站。</span>
                    <br>
                    <br>
                    目前可以推广的网站有：<span style="color: Blue;">QQ空间、QQ收藏、腾讯微博、朋友网、百度搜藏、百度空间、百度贴吧。</span>。
                    <br>
                    <br>
                    使用步骤：<br>
                    <span class="red" style="font-weight: bold;">
                    1、勾选宝贝，根据宝贝分类选择需要QQ推广或是百度推广的宝贝。<br>
                    2、一键QQ或百度推广，可以一键推广到指定的网站中去。腾讯微博、QQ空间等网站支持分享图片。
                    </span>
                </p>		
                <br />
            
    </div>
  </div>
    
</asp:Content>

