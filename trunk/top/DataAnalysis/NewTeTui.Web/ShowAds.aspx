<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowAds.aspx.cs" Inherits="ShowAds" Title="查看投放" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="right01">
                    <img src="images/04.gif" />
                    广告投放 &gt; <span>查看投放</span></div>
                   
                    <input type="button" onclick="window.location.href='useradslist.aspx?istou=1'" value="投放中的广告" />
            <input type="button" onclick="window.location.href='useradslist.aspx'" value="等待投放的广告" />
            <input type="button" onclick="window.location.href='UserAddAds.aspx'" value="投放新广告" />
            <hr />
            <img src='<%=ImgUrl %>' height="450px" width="670px">
            <br />
            <div style="font-size:20px">
            1、点击：<a href='<%=SiteUrl %>'target="_blank"><asp:Label runat="server" ID="LB_SiteUrl1" /></a>
            <br />
            2、在打开的网页中找到上图圈到的图片位置，点击
            <br />
           就能<a href="UserAdsList.aspx?istou=1">查看您的广告投放</a>
            </div>
</asp:Content>

