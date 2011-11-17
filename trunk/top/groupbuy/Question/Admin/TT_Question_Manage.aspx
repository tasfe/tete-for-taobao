<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TT_Question_Manage.aspx.cs" Inherits="Admin_TT_Question_Manage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>问题列表</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
       <a href="TT_Question.aspx"> </a>
        &nbsp;
        <br />
        
           <table style=" width:760px;">
            <tr style=" height:35px; background-color:#81979D; color:White; text-align:center" >
                <td style=" width:102px; height: 35px;">工单ID</td>
                <td style=" width:102px; height: 35px;">旺旺id</td>
                <td style="height: 35px">标题</td>
                <td style=" width:170px; height: 35px;">状态</td>
                <td style=" width:150px; height: 35px;">提交时间</td>
            </tr>
             <asp:Repeater ID="Repeater1" runat="server">
            
              <ItemTemplate>
              <tr style="height:30px ;" >
                   <td> 
                  <%#Eval("ID")%>
                  </td>
                   <td> 
                  <%#  Eval("userID").ToString() %>
                  </td>
                  <td>
                  <a href="TT_Question_Operate.aspx?id=<%#Eval("ID")%>" target="_blank"> <%#Eval("title")%> </a>
                   <a href="TT_Question_Manage.aspx?colose=1&id=<%#Eval("ID")%>" target="_blank">
                                    <%#Eval("state").ToString() == "1" ? "" : "<img src=../images/close1.gif style='border:0;'  />"%></a>
                                   
                                   
                    
                  </td>
                  <td>
                  <%#Eval("state").ToString() == "0" ? "<font color=blue size=4>未处理</font>" : Eval("state").ToString() == "2" ? "<font color=blue size=4>已回复...</font>" : "已关闭"%> <%# Eval("count") %>
                  </td>
                  <td>
                  <%#Eval("Date")%>
                  </td>
              </tr>
              <tr id="<%# Eval("ID") %>_dea" style="display:none;">
                  <td colspan=4>
                    <div>
                        正在加载。。。。
                    </div>
                    </td>
              </tr>
              </ItemTemplate>
             
            </asp:Repeater> 
              <tr  style=" height:35px;">
              <td style="width: 102px">
                  <a href="TT_Question.aspx"> </a></td>
              <td colspan="4" align="right">
              
               <asp:label ID="lblCurrentPage" runat="server"></asp:label> 
                  &nbsp;
  
                  <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
                  <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink>&nbsp;
                  共&nbsp;<asp:label ID="lbcountPage" runat="server" Text="1"></asp:label>&nbsp;页&nbsp;&nbsp;</td>
              </tr>
           </table>
    </div>
 
    </form>
</body>
</html>
