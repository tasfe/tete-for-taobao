﻿<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" Title="首页" %>

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
                     <div style="height:10px"></div>
                  <a href="Images/tesousuo.avi" target="_blank" style="font-size:20px">
                    点击下载视频教程
                  </a>
                    <div style="height:10px"></div>
                    目前可以推广的网站有：<span style="color: Blue;">QQ空间、QQ收藏、腾讯微博、朋友网、百度搜藏、百度空间、百度贴吧。</span>。
                    <br>
                    <span class="red" style="font-weight: bold;">建议每次分享到一个网站的数量不要超过5个,百度账户绑定邮箱和手机</span>
                    <div style="height:10px"></div>
                    使用步骤：<br>
                    <span class="red" style="font-weight: bold;">
                    <p />
                    1、点击打<a href="AddBaiDu.aspx">百度推广</a>或<a href="AddQQ.aspx">QQ推广</a>页面，按下图所示操作
                    <div style="height:10px"></div>
                    <img src="Images/bai1.jpg" width="670px" />
                    
                    
                    2、点击推广后，在打开的页面中按下图操作
                     <div style="height:10px"></div>
                     <img src="Images/bai2.jpg" width="670px" />
                     
                       3、操作成功后，以后每次只需要点击<a href="TuiList.aspx">推广列表</a>,按下图操作
                     <div style="height:10px"></div>
                     <img src="Images/bai3.jpg" width="670px" />
                    </span>
                    
                    
                     <div style="font-size:20px;">
想要将您的店铺、宝贝展示到各个知名网站吗？
<br />详情请点击
<a href="http://fuwu.taobao.com/ser/detail.htm?service_code=ts-22655&tracelog=sousuo" target="_blank" style="color:Blue; font-size:20px">
  特推广
</a></div>
                </p>		
                <br />
            
    </div>
  </div>
    
</asp:Content>

