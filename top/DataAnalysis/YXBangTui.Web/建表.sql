CREATE TABLE [dbo].[BangT_Ads](
	[AdsId] [uniqueidentifier] NOT NULL,
	[SiteId] [uniqueidentifier] NULL,
	[AdsName] [nvarchar](150) NULL,
	[AdsSize] [varchar](50) NULL,
	[AdsType] [int] NULL,
	[AdsPic] [varchar](100) NULL,
 CONSTRAINT [PK_BangT_Ads] PRIMARY KEY CLUSTERED 
(
	[AdsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[BangT_Ads]  WITH CHECK ADD  CONSTRAINT [FK_BangT_Ads_BangT_Ads] FOREIGN KEY([SiteId])
REFERENCES [dbo].[BangT_Sites] ([SiteId])
GO

ALTER TABLE [dbo].[BangT_Ads] CHECK CONSTRAINT [FK_BangT_Ads_BangT_Ads]
GO

CREATE TABLE [dbo].[BangT_Buys](
	[Nick] [nvarchar](50) NOT NULL,
	[FeeId] [uniqueidentifier] NOT NULL,
	[BuyTime] [datetime] NULL,
	[IsExpied] [bit] NULL,
	[ExpiedTime] [datetime] NULL,
 CONSTRAINT [PK_BangT_Buys] PRIMARY KEY CLUSTERED 
(
	[Nick] ASC,
	[FeeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_Category](
	[CateId] [varchar](50) NOT NULL,
	[CateName] [nvarchar](50) NULL,
	[ParentId] [varchar](50) NULL,
	[Nick] [nvarchar](50) NULL,
 CONSTRAINT [PK_BangT_Category] PRIMARY KEY CLUSTERED 
(
	[CateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_Click](
	[UserAdsId] [uniqueidentifier] NOT NULL,
	[ClickDate] [varchar](8) NOT NULL,
	[ClickCount] [int] NULL,
	[ClickType] [int] NOT NULL,
 CONSTRAINT [PK_BangT_Click] PRIMARY KEY CLUSTERED 
(
	[UserAdsId] ASC,
	[ClickDate] ASC,
	[ClickType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_ClickIP](
	[ClickId] [uniqueidentifier] NOT NULL,
	[UserAdsId] [uniqueidentifier] NOT NULL,
	[VisitIP] [varchar](50) NOT NULL,
	[VisitDate] [varchar](8) NOT NULL,
 CONSTRAINT [PK_BangT_ClickIP_1] PRIMARY KEY CLUSTERED 
(
	[ClickId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_Fee](
	[FeeId] [uniqueidentifier] NOT NULL,
	[Fee] [decimal](9, 2) NULL,
	[SiteCount] [int] NULL,
	[AdsType] [int] NULL,
	[AdsCount] [int] NULL,
	[ShowDays] [int] NULL,
 CONSTRAINT [PK_BangT_Fee] PRIMARY KEY CLUSTERED 
(
	[FeeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_Goods](
	[GoodsId] [varchar](50) NOT NULL,
	[GoodsName] [nvarchar](300) NULL,
	[GoodsPrice] [decimal](9, 2) NULL,
	[GoodsCount] [int] NULL,
	[GoodsPic] [varchar](250) NULL,
	[Modified] [datetime] NULL,
	[CateId] [varchar](250) NULL,
	[Nick] [nvarchar](50) NULL,
	[TaoBaoCId] [varchar](150) NULL,
 CONSTRAINT [PK_BangT_Goods] PRIMARY KEY CLUSTERED 
(
	[GoodsId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_ShopInfo](
	[sid] [nvarchar](50) NOT NULL,
	[cid] [nvarchar](50) NULL,
	[nick] [nvarchar](50) NULL,
	[title] [nvarchar](50) NULL,
	[desc] [nvarchar](500) NULL,
	[bulletin] [nvarchar](500) NULL,
	[pic_path] [nvarchar](150) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[sid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_Sites](
	[SiteId] [uniqueidentifier] NOT NULL,
	[SiteName] [nvarchar](50) NULL,
	[SiteUrl] [varchar](100) NULL,
 CONSTRAINT [PK_BangT_Sites] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[BangT_TaoBaoGoodsClass](
	[CId] [varchar](50) NOT NULL,
	[name] [nvarchar](100) NULL,
	[is_parent] [bit] NULL,
	[parent_cid] [varchar](50) NULL,
 CONSTRAINT [PK_BangT_TaoBaoGoodsClass] PRIMARY KEY CLUSTERED 
(
	[CId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_UsedInfo](
	[Nick] [nvarchar](50) NOT NULL,
	[UsedTimes] [int] NULL,
 CONSTRAINT [PK_BangT_UsedInfo] PRIMARY KEY CLUSTERED 
(
	[Nick] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_UserAds](
	[Id] [uniqueidentifier] NOT NULL,
	[AdsTitle] [nvarchar](100) NULL,
	[AdsUrl] [varchar](150) NULL,
	[AdsId] [uniqueidentifier] NULL,
	[UserAdsState] [int] NULL,
	[AdsShowStartTime] [datetime] NULL,
	[AdsShowFinishTime] [datetime] NULL,
	[AliWang] [nvarchar](50) NULL,
	[SellCateName] [nvarchar](50) NULL,
	[CateIds] [varchar](150) NULL,
	[AddTime] [datetime] NULL,
	[Nick] [nvarchar](50) NULL,
	[FeeId] [uniqueidentifier] NULL,
	[AdsPic] [varchar](150) NULL,
	[Price] [decimal](9, 2) NULL,
 CONSTRAINT [PK_BangT_UserAds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[TopLoginLog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nick] [nvarchar](150) NULL,
	[logDate] [datetime] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TopLoginLog] ADD  CONSTRAINT [DF_TopLoginLog_logDate]  DEFAULT (getdate()) FOR [logDate]
GO

CREATE TABLE [dbo].[TopTaobaoShop](
	[sid] [int] NOT NULL,
	[cid] [int] NULL,
	[title] [nvarchar](200) NULL,
	[nick] [nvarchar](150) NULL,
	[desc] [ntext] NULL,
	[bulletin] [ntext] NULL,
	[pic_path] [nvarchar](250) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[shop_score] [int] NULL,
	[remain_count] [int] NULL,
	[date] [datetime] NULL,
	[lastlogin] [datetime] NULL,
	[logintimes] [int] NULL,
	[session] [nvarchar](150) NULL,
	[versionNo] [int] NULL,
	[versionNoBlog] [int] NULL,
	[enddate] [datetime] NULL,
	[isover] [int] NULL,
	[enddateblog] [datetime] NULL,
	[isoverblog] [int] NULL,
	[sessionblog] [nvarchar](250) NULL,
	[sessionmircoblog] [nvarchar](250) NULL,
	[sessiongroupbuy] [nvarchar](250) NULL,
	[sessionmarket] [nvarchar](250) NULL,
	[typ] [nvarchar](50) NULL,
	[ip] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[TopTaobaoShop] ADD  CONSTRAINT [DF_TopTaobaoShop_date]  DEFAULT (getdate()) FOR [date]
GO

ALTER TABLE [dbo].[TopTaobaoShop] ADD  CONSTRAINT [DF_TopTaobaoShop_logintimes]  DEFAULT ((0)) FOR [logintimes]
GO

ALTER TABLE [dbo].[TopTaobaoShop] ADD  CONSTRAINT [DF_TopTaobaoShop_versionNo]  DEFAULT ((1)) FOR [versionNo]
GO

ALTER TABLE [dbo].[TopTaobaoShop] ADD  CONSTRAINT [DF_TopTaobaoShop_versionNoBlog]  DEFAULT ((1)) FOR [versionNoBlog]
GO

ALTER TABLE [dbo].[TopTaobaoShop] ADD  CONSTRAINT [DF_TopTaobaoShop_isover]  DEFAULT ((0)) FOR [isover]
GO

ALTER TABLE [dbo].[TopTaobaoShop] ADD  CONSTRAINT [DF_TopTaobaoShop_isoverblog]  DEFAULT ((0)) FOR [isoverblog]
GO


CREATE TABLE [dbo].[TopTuijian](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nickfrom] [nvarchar](50) NULL,
	[nickto] [nvarchar](50) NULL,
	[isok] [int] NULL,
	[okdate] [datetime] NULL,
 CONSTRAINT [PK_TopTuijian] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TopTuijian] ADD  CONSTRAINT [DF_TopTuijian_isok]  DEFAULT ((0)) FOR [isok]
GO


