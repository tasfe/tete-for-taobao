<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testTopImgAdd.aspx.cs" Inherits="top_groupbuy_testTopImgAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table >
            <tr style=" height:100px; vertical-align:middle;" >
                <td valign=middle>
                 商城,良品 ：<input type=checkbox name="img2" id="Checkbox7"  value="mall_icon.png"/><img src="images/mall_icon.png" style="width:80px; height:45px;" />消费者保障
                   <input type=checkbox name="img2" id="Checkbox8"   value="lp_icon.png"/><img src="images/lp_icon.png" style="width:75px; height:18px;"/>七天退换
                </td>
            </tr>
            <tr>
                <td>
                        店铺保障：<input type=checkbox name="img1" id="img1" value="0.png" /><img src="images/0.png" />消费者保障
                  <input type=checkbox name="img1" id="Checkbox1"  value="1.png" /><img src="images/1.png" />七天退换
                  <input type=checkbox name="img1" id="Checkbox2"  value="2.png" /><img src="images/2.png" />假一赔三
                    <input type=checkbox name="img1" id="Checkbox3"  value="3.png" /><img src="images/3.png" />30天维修
                    <input type=checkbox name="img1" id="Checkbox4" value="4.png"  /><img src="images/4.png" />1小时发货
                    <input type=checkbox name="img1" id="Checkbox5"  value="5.png" /><img src="images/5.png" />闪电发货
                    <input type=checkbox name="img1" id="Checkbox6" value="6.png"  /><img src="images/6.png" />正品保障
                </td>
            </tr>
        </table>
        


    
        </div>
    <br />
    <asp:Button ID="Button1" runat="server" Height="25px" onclick="Button1_Click" 
        Text="提交" Width="69px" />
    </form>
</body>
</html>
