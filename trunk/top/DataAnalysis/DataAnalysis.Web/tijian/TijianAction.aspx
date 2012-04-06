<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TijianAction.aspx.cs" Inherits="tijian_TijianAction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>体检参数设置</title>
    <link href="../css/common.css" rel="stylesheet" />
    <style type="text/css">
        td
        {
            font-size: 12px;
            height: 20px;
        }
        a
        {
            color: Blue;
            text-decoration: none;
        }
        .paramname{ color:#AF4A92}
    </style>
</head>
<body style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div>
        <div class="navigation" style="height: 600px;">
            <div class="crumbs">
                <a href="javascript:;" class="nolink">营销决策</a> 体检参数设置
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
1.	客户浏览比率是否健康的处理提醒
a)	一个PV等于一次页面刷新，以UV比较比重越高，店铺越健康，如果UV和PV比率低于
<asp:Label ID="Lb_L_Liulan" runat="server" CssClass="paramname" />
以下说明店铺不健康，您的店铺只有
<asp:Label ID="Lb_Liulan" runat="server" CssClass="paramname"  />
，说明您店铺的装修比较差，客户都不愿意来看了，推荐您使用以下服务：
i.	淘宝官方
ii.	卖家分享经验帖子(1-5)
2.	最近7天销售客单价是否正常提醒
a)	环比订单量大实际销售额低，说明客单价有问题，同样证明店铺关联销售有问题，连带购买低，营销成本就高，则会出现亏损，同时客单价如果不能高于销售产品均价
<asp:Label ID="Lb_L_SellGuanlian" runat="server" CssClass="paramname" />，则代表客单价过低。您的客单价只有销售均价的
<asp:Label ID="Lb_SellGuanlian" runat="server" CssClass="paramname"  />，连带购买太低了，推荐您使用以下服务：
i.	满就送
ii.	关联搭配
iii.	卖家分享经验帖子
3.	检查店铺浏览转换率是否健康
a)	转换率过低说明促销方式、产品款式、性价比、界面视觉有问题则需要优化。您的转换率只有
<asp:Label ID="Lb_Zhuanhuan" runat="server" CssClass="paramname"  />，说明宝贝描述页的装修有问题，推荐您使用以下服务来优化：
i.	宝贝装修
ii.	分享经验帖
4.	检查店铺浏览回头率是否健康
a)	浏览回头显示一个店铺的老客户增长情况，过低低于
<asp:Label ID="Lb_L_SeeBack" runat="server" CssClass="paramname" />则说明产品、售后有问题，则老客户增长就需要优化。您的回头率只有
<asp:Label ID="Lb_SeeBack" runat="server" CssClass="paramname"  />，您的产品质量或者售后有问题，您可以通过以下服务去管理客服售后的问题：
i.	客服绩效管理
ii.	淘关怀
5.	检查店铺二次购买率是否健康
a)	二次购买率过低说明您的老客户的关心程度和促进二次消费手段不够，您的二次购买率只有
<asp:Label ID="Lb_BuyBack" runat="server" CssClass="paramname"  />，推荐您使用以下服务增加二次购买率：
i.	淘关怀
ii.	评价有礼
6.	检查店铺页面访问深度是否健康
a)	平均访问深度过低，一般不低于
<asp:Label ID="Lb_L_SeeDeep" runat="server" CssClass="paramname" />，如果低于说明产品、界面、或者产品结构或者促销活动有问题，您的平均访问深度只有
<asp:Label ID="Lb_SeeDeep" runat="server" CssClass="paramname" />，推荐您使用以下服务增加访问深度：
i.	促销布局装修
ii.	经验帖
7.	检查爆款商品购买率是否健康
a)	有产品流量很高，但是没有销售量，那么需要查看是收藏量很高，如果很高可能是价格问题，如果没有收藏则可能是界面问题，如果流量突然增长需要分析展示位置。您目前的爆款商品购买率只有
<asp:Label ID="Lb_TopGoods" runat="server" CssClass="paramname" />，您需要加大对此宝贝的推广力度和宣传方式，推荐您使用以下服务：
i.	宝贝装修
ii.	经验帖
            </div>
        </div>
    </div>
    </form>
</body>
</html>

