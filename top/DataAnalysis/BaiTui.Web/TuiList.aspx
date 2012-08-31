<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true"
    CodeFile="TuiList.aspx.cs" Inherits="TuiList" Title="推广列表" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="ContentLeft">
        <div id="ContentLeftTop">
            <div class="BarLeft">
            </div>
            <div id="ContentLeftTopText">
                推广列表</div>
            <div class="BarRight">
            </div>
            <div class="Cal">
            </div>
        </div>
        <div id="ContentLeftBox">
            <ul>
                <li><a href="TuiList.aspx?type=1">&gt; 百度推广列表</a></li>
                <li><a href="TuiList.aspx?type=2">&gt; QQ推广列表</a></li>
            </ul>
        </div>
    </div>
    <div id="ContentRight">
        <div id="ContentRightTop">
            <div class="BarLeft">
            </div>
            <div id="ContentRightTopText">
                推广列表</div>
            <div class="BarRight">
            </div>
            <div class="Cal">
            </div>
        </div>
        <div id="ContentRightBox">
            <h2>
                推广列表&nbsp;&gt;&gt;&nbsp;<asp:Label Text="一键推广：" runat="server" ID="Lbl_TuiType" /></h2>
            <br />
                <table>
                   <asp:Repeater runat="server" ID="Rpt_TuiList">
                     <ItemTemplate>
                        <tr>
                            <td class="taobaotitle" style="width: 500px;">
                                <a target="_blank" href='http://item.taobao.com/item.htm?id=<%# Eval("GoodsId") %>'>
                                    <%# Eval("GoodsName")%></a>
                            </td>
                            <td class="taobaodate" style="width: 120px; text-align: center;">
                                <a class="red" href='BaiShare.aspx?id=<%# Eval("TuiId") %>'>去快速推广</a>
                            </td>
                        </tr>
                      </ItemTemplate>
                   </asp:Repeater>    
                   
                </table>
            </div>
            <div id="CommonPager" class="meneame">
               <asp:label ID="lblCurrentPage" runat="server" Font-Size="Large"></asp:label>
            <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
            <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
            <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
            <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
        </div>
</asp:Content>
