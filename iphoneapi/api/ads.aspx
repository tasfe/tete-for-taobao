<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ads.aspx.cs" Inherits="iphoneapi_api_cate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b>新品广告3张</b><br />
        广告图片1：<input name="pic_index_1" value="<%=logo1 %>" />
        链接地址1：<input name="url_index_1" value="<%=url1 %>" />
        分类ID：<input name="cate_index_1" value="<%=cate1 %>" /><br />
        
        广告图片2：<input name="pic_index_2" value="<%=logo2 %>" />
        链接地址2：<input name="url_index_2" value="<%=url2 %>" />
        分类ID：<input name="cate_index_2" value="<%=cate2 %>" /><br />
        
        广告图片3：<input name="pic_index_3" value="<%=logo3 %>" />
        链接地址3：<input name="url_index_3" value="<%=url3 %>" />
        分类ID：<input name="cate_index_3" value="<%=cate3 %>" /><br />
        <br />

        <asp:Repeater ID="rpt1" runat="server" OnItemDataBound="rpt1_ItemDataBound">
            <ItemTemplate>
                <b>【<%#Eval("oldname") %>】分类广告3张</b><br />
                <input name="id" type="hidden" value="<%#Eval("cateid") %>" />
                <asp:Label ID="lb1" runat="server" Text='<%#Eval("cateid") %>' Visible="false"></asp:Label>
                <asp:Repeater ID="rpt2" runat="server">
                    <ItemTemplate>
                         广告图片：<input name="pic_<%#Eval("typ") %>_<%#Eval("orderid") %>" value="<%#Eval("logo") %>" />
                         链接地址：<input name="url_<%#Eval("typ") %>_<%#Eval("orderid") %>" value="<%#Eval("url") %>" />
                         分类ID：<input name="cate_<%#Eval("typ") %>_<%#Eval("orderid") %>" value="<%#Eval("cateid") %>" /><br />
                    </ItemTemplate>
                </asp:Repeater>
                <br />
            </ItemTemplate>
        </asp:Repeater>

        <br /><br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="保存" />
    </div>
    </form>
</body>
</html>
