<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true"
    CodeFile="AddQQ.aspx.cs" Inherits="AddQQ" Title="QQ推广" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div id="ContentLeft">
    <div id="ContentLeftTop">
      <div class="BarLeft"></div>
      <div id="ContentLeftTopText">QQ推广</div>
      <div class="BarRight"></div>
      <div class="Cal"> </div>
    </div>
    <div id="ContentLeftBox">
      <ul>
        <li><a href="#"> &gt; 一键推广</a></li>
         <li><a href="TuiList.aspx?type=2"> &gt; 推广列表</a></li>
      </ul>
    </div>
  </div>
<div id="ContentRight">
    <div id="ContentRightTop">
      <div class="BarLeft"></div>
      <div id="ContentRightTopText">一键推广</div>
      <div class="BarRight"></div>
      <div class="Cal"></div>
    </div>
    <div id="ContentRightBox">
            <h2>
                QQ推广&nbsp;&gt;&gt;&nbsp; 一键推广：</h2>
                
            <br />
            <table width="100%" border="1" class="t1" id="mytab">
   
                    <tr class="a1">
                        <td style="background-color:#E8F2FF;font-weight:bold">
                            第一步：选择宝贝
                        </td>
                    </tr>
                
            </table>
            <br />
            <div>
                    <span>宝贝分类：</span><span id="Category">
                        <asp:DropDownList ID="DDL_SellCate" runat="server">
                        </asp:DropDownList>
                    </span>&nbsp;&nbsp;&nbsp; <span>宝贝名: </span><span>
                        <asp:TextBox ID="TB_GoodsName" Width="200px" runat="server" />
                    </span>&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Btn_Search" runat="server" Text="查 找" OnClick="Btn_Search_Click" />
                
            </div>
            <br>
            <span class="red">请输入关键字(至少输入三个，这些关键字用于搜搜搜索引擎优化)：</span>
            <div>
                <span>
                    <asp:TextBox ID="TB_Key1" Width="110px" runat="server" MaxLength="15" /></span>&nbsp;
                <span>
                    <asp:TextBox ID="TB_Key2" Width="110px" runat="server" MaxLength="15" /></span>&nbsp;
                <span>
                    <asp:TextBox ID="TB_Key3" Width="110px" runat="server" MaxLength="15" /></span>&nbsp;
                <span>
                    <asp:TextBox ID="TB_Key4" Width="110px" runat="server" MaxLength="15" /></span>&nbsp;
                <span>
                    <asp:TextBox ID="TB_Key5" Width="110px" runat="server" MaxLength="15" /></span>&nbsp;
            </div>
            <div style="margin-top:5px">
                    <table>
                        <asp:Repeater ID="Rpt_GoodsList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                      <asp:RadioButton ID="RBtn_Goods" runat="server" />
                                      <asp:Label Text='<%# Eval("GoodsId") %>' runat="server" Visible="false" ID="Lbl_GoodsId"></asp:Label>
                                      <asp:Label Text='<%# Eval("GoodsPic") %>' runat="server" Visible="false" ID="Lbl_GoodsPic"></asp:Label>
                                    </td>
                                    <td>
                                        <a target="_blank" href='http://item.taobao.com/item.htm?id=<%# Eval("GoodsId") %>'>
                                            <img src='<%# Eval("GoodsPic") %>' class="taobaoimg"></a>
                                    </td>
                                    <td style="width: 540px;" class="taobaotitle">
                                        <span id="title_7591614139">
                                         <asp:Label ID="Lbl_GoodsName" Text='<%# Eval("GoodsName") %>' runat="server"></asp:Label></span><br>
                                        <br>
                                        <span id="result_7591614139"></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                <div class="meneame" id="CommonPager">
                    <asp:label ID="lblCurrentPage" runat="server"></asp:label>
            <asp:HyperLink id="lnkFrist" runat="server">首页</asp:HyperLink>
            <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
            <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink> 
            <asp:HyperLink id="lnkEnd" runat="server">尾页</asp:HyperLink>
                    </div>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="Btn_Next" Text="立即推广" runat="server" OnClick="Btn_Next_Click" />
            </div>
    </div>
</asp:Content>
