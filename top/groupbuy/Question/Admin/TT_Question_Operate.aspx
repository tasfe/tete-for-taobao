<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TT_Question_Operate.aspx.cs" Inherits="Admin_TT_Question_Operate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>提问回复</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
        提问时间：<asp:Label ID="Label1" runat="server"></asp:Label>最后回复时间：<asp:Label ID="Label2" runat="server"></asp:Label><table width="600px;">
            <tr style=" height:35px;  background-color:#81979D; color:White; text-align:center">
  <td  >提问,回复</td>
                
            </tr>
                         <asp:Repeater ID="Repeater1" runat="server">
            
              <ItemTemplate>
              <tr style="height:30px ; color:Green;" >
                   <td> 
                  [<%#decode(Eval("userID").ToString())%>]
                  客户提问： <%#Eval("title")%>
                  问题描述： <%#Eval("details")%>
                  </td>
                  </tr>
                  <tr>
                   <td> 
                 <%#Eval("Answer").ToString() == "" ? "" : "系统回复："%>  <%#Eval("Answer")%>
                  </td>
                  
                 
              </tr>
             
              </ItemTemplate>
             
            </asp:Repeater> 
            <tr>
                <td align=center>
                    <asp:TextBox ID="TextBox1" runat="server" Height="149px" TextMode="MultiLine" Width="372px"></asp:TextBox><br />
                    <br />
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/img-12.gif"
                        OnClick="ImageButton1_Click" /></td>
            </tr>
                          
  <tr>
              <td  >
              <hr  style="color:Green;"/>
              </td>
              </tr>      </table>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    </form>
</body>
</html>
