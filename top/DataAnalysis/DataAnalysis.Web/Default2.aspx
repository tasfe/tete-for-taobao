<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .all
        {
            width: 1200px;
        }
        .detail
        {
            width: 60px;
            float: left;
        }
        .detail div
        {
            font-size: 12px;
            height: 30px;
        }
        .detail1
        {
            width: 90px;
            float: left;
        }
        .detail1 div
        {
            font-size: 12px;
            height: 30px;
        }
        
        a{ color:Black;}
        .numalink{ text-decoration:none; color:#AF4A92}
        
    </style>
    
    <style type="text/css">  
.tooltips{  position:relative; 
z-index:2;  }  .tooltips:hover{  z-index:3;  background:none; 
}  .tooltips span{  display: none;  }  .tooltips:hover span{ 
display:block;  position:absolute;  top:21px;  left:9px;  width:15em;  border:1px solid black;  background-color:#ccFFFF;  padding: 3px;  color:black;  }
</style> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="Btn_AddCookie"  Text="点击查看客户真实数据" runat="server" 
            onclick="Btn_AddCookie_Click" /><font size="2">此处只是为了让审核人员能够更直观的看到软件真实效果</font>
        <div class="all">
            <div class="detail1">
                <div>
                </div>
                <div>
                    <a href="VisitTotal.aspx" class="tooltips">PV
                       <span>店铺浏览页面数量</span>
                    </a>
                </div>
                <div>
                    <a href="VisitTotal.aspx" class="tooltips">UV
                       <span>店铺浏览人数</span>
                    </a>
                </div>
                <div>
                     <a href="ZhiTongPV.aspx" class="tooltips">直通车流量
                       <span>淘宝直通车是为淘宝卖家量身定制的，按点击付费的效果营销工具，实现宝贝的精准推广。用如何开网店的小编一席话总结：淘宝直通车推广，在给宝贝带来曝光量的同时，精准的搜索匹配也给宝贝带来了精准的潜在买家。淘宝直通车推广，用一个点击，让买家进入你的店铺，产生一次甚至多次的店铺内跳转流量，这种以点带面的关联效应可以降低整体推广的成本和提高整店的关联营销效果。同时，淘宝直通车还给用户提供了淘宝首页热卖单品活动和各个频道的热卖单品活动以及不定期的淘宝各类资源整合的直通车用户专享活动。</span>
                    </a>
                </div>
                <div>
                     <a href="ZuanZhanPV.aspx" class="tooltips">钻展流量
                       <span>淘宝钻石展位,是淘宝图片类广告位竞价平台,就是花钱在淘宝首页,或者其他位置做广告</span>
                     </a>
                </div>
                <div>
                   <a href="AskOrderTotal.aspx" class="tooltips">
                      询单数
                       <span>通过旺旺询问客服的客户数量</span>
                   </a>
                   
                </div>
                <div>
                     <a href="AskOrderTotal.aspx" class="tooltips">丢单率
                     <span>丢单率等于询问客服人中成功下订单(含货到)的人数除以询问客服的总人数</span>
                     </a>
                </div>
                <div>
                    <a href="OrderTotal.aspx" class="tooltips">
                      订单数
                       <span>成功付款的订单(含货到)数量</span>
                   </a>
                </div>
                <div>
                   <a href="OrderTotal.aspx" class="tooltips">
                      订单总价
                       <span>客户为订单支付的总金额(含运费)</span>
                   </a>
                </div>
                <div>
                    <a href="GoodsCountTotal.aspx" class="tooltips">
                      销售单价
                       <span>客户为订单支付的总金额(含运费)除以售出的商品总数</span>
                   </a>
                </div>
                <div>
                   <a href="CustomerBuyTotal.aspx" class="tooltips">
                      客单价
                       <span>客户为订单支付的总金额(含运费)除以客户总数</span>
                   </a>
                </div>
                <div>
                    <a href="ZhuanHuaTotal.aspx" class="tooltips">
                      转化率
                       <span>成功付款的客户总数(含货到)除以浏览网站的总人数</span>
                   </a>
                </div>
                <div>
                     <a href="#" class="tooltips">
                      回头客
                       <span>不只一次在网站购买过商品的客户数量</span>
                   </a>
                </div>
                <div>
                     <a href="VisitTotal.aspx" class="tooltips">
                      浏览回头率
                       <span>不只一天浏览过网站的客户数量</span>
                   </a>
                </div>
                <div>
                    <a href="#" class="tooltips">
                      退款率
                       <span>退款订单数量除以成功付款订单数量(含货到)</span>
                   </a>
                </div>
                <div>
                    <a href="VisitTotal.aspx" class="tooltips">
                      平均访问深度
                       <span>浏览页面总数量除以浏览网站用户的总数量</span>
                   </a>
                </div>
                <div style="height:60px">
                <a href="TopGoods.aspx" class="tooltips">
                      浏览排行
                       <span>被浏览最多的商品</span>
                   </a>
                    
                </div>
                <div style="height:60px">
                     <a href="GoodsBuyTotal.aspx" class="tooltips">
                      销售排行
                       <span>销售最多的商品</span>
                   </a>
                </div>
                <%--<div>
                    收藏量
                </div>--%>
            </div>
            <asp:Repeater runat="server" ID="Rpt_TotalList">
                <ItemTemplate>
                    <div class="detail">
                        <div style="color:#103667;">
                            <%# GetMonthDay(Eval("SiteTotalDate").ToString()) %>
                        </div>
                        <div>
                           <a class="numalink" href='HourTotal.aspx?day=<%# Eval("SiteTotalDate") %>'><%# Eval("SitePVCount")%></a>
                        </div>
                        <div>
                           <a class="numalink" href='HourTotal.aspx?day=<%# Eval("SiteTotalDate") %>'><%# Eval("SiteUVCount")%></a>
                        </div>
                        <div>
                            <a class="numalink" href='ZhiTongHourPV.aspx?day=<%# Eval("SiteTotalDate") %>'><%# Eval("ZhiTongFlow")%></a>
                        </div>
                        <div>
                            <a class="numalink" href='ZhiTongHourPV.aspx?day=<%# Eval("SiteTotalDate") %>'><%# Eval("SiteZuanZhan")%></a>
                        </div>
                        <div>
                            <a class="numalink" href='AskOrderCustomer.aspx?day=<%# Eval("SiteTotalDate") %>'><%# Eval("AskOrder")%></a>
                        </div>
                        <div>
                            <%# Eval("LostOrder")%>
                        </div>
                        <div>
                            <%# Eval("SiteOrderCount")%>
                        </div>
                        <div>
                            <%# Eval("SiteOrderPay")%>
                        </div>
                        <div>
                            <%# Eval("SellAvg")%>
                        </div>
                        <div>
                            <%# Eval("OneCustomerPrice")%>
                        </div>
                        <div>
                            <%# Eval("CreateAVG")%>
                        </div>
                        <div>
                            <%# Eval("SiteSecondBuy")%>
                        </div>
                        <div>
                            <%# Eval("BackSee") %>
                        </div>
                        <div>
                            <%# Eval("Refund")%>
                        </div>
                        <div>
                            <%# Eval("SeeDeepAVG")%>
                        </div>
                        <div style="height:60px">
                          <a href='TopGoods.aspx?day=<%# Eval("SiteTotalDate") %>' class="numalink">
                           <img style="border:0" src='<%# Eval("SeeTop.pic_url")%>_60x60.jpg' alt='<%# Eval("SeeTop.title")%>' height="50px" />
                          </a>
                        </div>
                        <div style="height:60px">
                          <a href='GoodsBuyTotal.aspx?day=<%# Eval("SiteTotalDate") %>' class="numalink">
                           <img style="border:0" src='<%# Eval("SellTop.pic_url")%>_60x60.jpg' alt='<%# Eval("SellTop.title")%>' height="50px" />
                          </a>
                        </div>
                       <%--     <div>
                                <%# Eval("Collection")%>
                            </div>--%>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        
    </div>
    </form>
</body>
</html>
