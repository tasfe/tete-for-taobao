<%@ Page Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true"
    CodeFile="AddBaiDu.aspx.cs" Inherits="AddBaiDu" Title="�ٶ��ƹ�" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
      <div id="ContentLeft">
    <div id="ContentLeftTop">
      <div class="BarLeft"></div>
      <div id="ContentLeftTopText">�ٶ��ƹ�</div>
      <div class="BarRight"></div>
      <div class="Cal"> </div>
    </div>
    <div id="ContentLeftBox">
      <ul>
        <li><a href="#"> &gt; һ���ƹ�</a></li>
         <li><a href="TuiList.aspx?type=1"> &gt; �ƹ��б�</a></li>
      </ul>
    </div>
  </div>
<div id="ContentRight">
    <div id="ContentRightTop">
      <div class="BarLeft"></div>
      <div id="ContentRightTopText">һ���ƹ�</div>
      <div class="BarRight"></div>
      <div class="Cal"></div>
    </div>
    <div id="ContentRightBox">
            <h2>
                �ٶ��ƹ�&nbsp;&gt;&gt;&nbsp; һ���ƹ㣺</h2>
                
            <br />
            <table width="100%" border="1" class="t1" id="mytab">
   
                    <tr class="a1">
                        <td style="background-color: #E8F2FF;font-weight:bold;">
                            ��һ����ѡ�񱦱�
                        </td>
                    </tr>
                
            </table>
            <br />
            <div>
                    <span>�������ࣺ</span><span id="Category">
                        <asp:DropDownList ID="DDL_SellCate" runat="server">
                        </asp:DropDownList>
                    </span>&nbsp;&nbsp;&nbsp; <span>������: </span><span>
                        <asp:TextBox ID="TB_GoodsName" Width="200px" runat="server" />
                    </span>&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Btn_Search" runat="server" Text="�� ��" OnClick="Btn_Search_Click" />
                
            </div>
            <br>
            <span class="red">�ؼ���(�˹ؼ������ڰٶ����������Ż� �ؼ�����Ϻ󽫳�Ϊ�µı����������)��</span>
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
                    <span style="font-size: 14px;">��1ҳ</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                        class="current">1</span></div>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="Btn_Next" Text="�����ƹ�" runat="server" OnClick="Btn_Next_Click" />
            </div>
    </div>
</asp:Content>