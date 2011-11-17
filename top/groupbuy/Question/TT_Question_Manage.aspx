<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TT_Question_Manage.aspx.cs" Inherits="TT_Question_Manage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>问题列表</title>
    <script src="JS/jquery-1.3.2.min.js" type="text/javascript"></script>
<link href="../top/css/common.css" rel="stylesheet" />
</head>
<body style="padding:0px; margin:0px;">

<div class="navigation" style="height:600px;">

  <div class="crumbs"><a href="../show.html">我的特特</a><a href="javascript:;" class="nolink">博客营销</a> 我的售后问题 </div>
  <div class="absright">
    <ul>
      <li>
        <div class="msg">
            
        </div>
      </li>
    </ul>
  </div>
    <div id="main-content">

    <form id="form1" runat="server">
    <div>
        
       <a href="TT_Question.aspx"> <img src="images/subnewques.gif" style="border:0" /></a>
        &nbsp;
        <br />
        
           <table style=" width:760px;">
            <tr style=" height:35px; background-color:#81979D; color:White; text-align:center" >
                <td style=" width:102px; height: 35px;">工单ID</td>
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
                 <span onclick="getQuestionList('<%#Eval("ID")%>')" style="cursor:pointer"> <%#Eval("title")%> </span> 
                   <a href="TT_Question_Manage.aspx?colose=1&id=<%#Eval("ID")%>">
                                    <%#Eval("state").ToString() == "1" ? "" : "<img src=images/close1.gif style='border:0;'  />"%></a>
                    
                  </td>
                  <td>
                  <%#Eval("state").ToString() == "0" ? "<font color=blue size=4>等待处理</font>" : Eval("state").ToString() == "2" ? "<font color=blue size=4>已回复</font>" : "已关闭"%> <%# Eval("count") %>
                  </td>
                  <td>
                  <%#Eval("Date")%>
                  </td>
              </tr>
              <tr id="<%# Eval("ID") %>_tr" style="display:none;">
                  <td colspan=4>
                    <div id="<%# Eval("ID") %>_div" name="divhtml">
                        正在加载。。。。
                    </div>
                    </td>
              </tr>
              </ItemTemplate>
             
            </asp:Repeater> 
              <tr  style=" height:35px;">
              <td style="width: 102px">
                  <a href="TT_Question.aspx"> <img src="images/subnewques.gif" style="border:0" /></a></td>
              <td colspan="4" align="right">
              
               <asp:label ID="lblCurrentPage" runat="server"></asp:label> 
                  &nbsp;
  
                  <asp:HyperLink id="lnkPrev" runat="server">上一页</asp:HyperLink>
                  <asp:HyperLink id="lnkNext" runat="server">下一页</asp:HyperLink>&nbsp;
                  共&nbsp;<asp:label ID="lbcountPage" runat="server" Text="1"></asp:label>&nbsp;页&nbsp;&nbsp;
                 
                  </td>
                   
              </tr>
              <tr>
              <td colspan=5>
              <hr  style="color:Green;"/>
              <input type="hidden" id="divid"value="" />
              <input type="hidden" id="trid"value="" />
              </td>
              </tr>
           </table>
    </div>
</div>
</div>
 <script type="text/javascript">
     //获取问题html
     function getQuestionList(obj) {
         var id = obj;
         var objDiv = obj + "_div";
         var obj = obj + '_tr';

         if (document.getElementById(obj).style.display != "") {
             if (document.getElementById('trid').value != "" && document.getElementById('trid').value != obj) {
                 var did = document.getElementById('divid').value;
                 var tid = document.getElementById('trid').value;
                 document.getElementById(did).innerHTML = "";
                 document.getElementById(tid).style.display = "none";
             }
         
                 document.getElementById('trid').value = obj;
                 document.getElementById('divid').value = objDiv;
              
           
             document.getElementById(obj).style.display = "";
      
             $.ajax({
                 type: "Get",
                 url: "QuestionAjax.aspx?d=" + new Date() + "&type=list&wd=" + id,
                 success: function(msg) {

                     document.getElementById(objDiv).innerHTML = msg;
                 }
             });

         }
         else {
             document.getElementById(obj).style.display = "none";
             document.getElementById(objDiv).innerHTML = "";
         }
     }

     //继续问题
     function addQuestsub() {
         if (document.getElementById("TextBox1").value == "") {
             alert("请填写提交内容");
             return false;
         }
            
         document.getElementById("loading_span").style.display = "";
         $.ajax({
         type: "Get",
             url: "QuestionAjax.aspx?d="+new Date()+"&type=andQA&FamilyID=" + document.getElementById("FamilyID").value + "&wd=" + escape(document.getElementById("TextBox1").value),
             success: function(msg) {
             getQuestionList(document.getElementById("FamilyID").value)
             }
         });
     }
 </script>
    </form>

</body>
</html>
