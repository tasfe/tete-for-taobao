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

CREATE TABLE [dbo].[BangT_Buys](
	[Nick] [nvarchar](50) NOT NULL,
	[FeeId] [uniqueidentifier] NOT NULL,
	[BuyTime] [datetime] NULL,
	[IsExpied] [bit] NULL,
 CONSTRAINT [PK_BangT_Buys] PRIMARY KEY CLUSTERED 
(
	[Nick] ASC,
	[FeeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[BangT_Goods](
	[GoodsId] [varchar](50) NOT NULL,
	[GoodsName] [nvarchar](150) NULL,
	[GoodsPrice] [decimal](9, 2) NULL,
	[GoodsCount] [int] NULL,
	[GoodsPic] [varchar](150) NULL,
	[Modified] [datetime] NULL,
	[CateId] [varchar](50) NULL,
	[Nick] [nvarchar](50) NULL,
	[TaoBaoCId] [varchar](50) NULL,
 CONSTRAINT [PK_BangT_Goods] PRIMARY KEY CLUSTERED 
(
	[GoodsId] ASC
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
 CONSTRAINT [PK_BangT_UserAds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO