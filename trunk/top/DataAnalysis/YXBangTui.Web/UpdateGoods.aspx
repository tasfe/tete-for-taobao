<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateGoods.aspx.cs" Inherits="UpdateGoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>更新商品</title>
    <link href="css/common.css" rel="stylesheet" />
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div class="navigation" style="height: 600px;">
        <div class="crumbs">
            <a href="#" class="nolink">特推广</a> 更新商品
        </div>
        <div class="absright">
            <ul>
                <li>
                    <div class="msg">
                        
                    </div>
                </li>
            </ul>
        </div>
       
        <div id="main-content">
          <div style="border:solid 1px #CCE2FF; padding:4px; background-color:#E8F2FF; margin:0 3px 6px 3px; color:Red; font-weight:bold">
         如果您重新编辑过商品，请点击更新商品； 如果您还修改过分类，请点击更新分类。
         </div>
            <asp:Button ID="Btn_UpdateGoods" Text="更新商品" runat="server" OnClick="Btn_UpdateGoods_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="Btn_UpdateClass" Text="更新分类" runat="server" OnClick="Btn_UpdateClass_Click" />   
        </div>
    </div>
    </form>
</body>
</html>
