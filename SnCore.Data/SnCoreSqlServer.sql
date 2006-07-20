SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountPlaceType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountPlaceType](
	[AccountPlaceType_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](24) NOT NULL,
	[CanWrite] [bit] NOT NULL,
	[Description] [ntext] NULL,
 CONSTRAINT [PK_AccountPlaceType] PRIMARY KEY CLUSTERED 
(
	[AccountPlaceType_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountPlaceType] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugResolution]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugResolution](
	[BugResolution_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_BugResolution] PRIMARY KEY CLUSTERED 
(
	[BugResolution_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_BugResolution] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugSeverity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugSeverity](
	[BugSeverity_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_BugSeverity] PRIMARY KEY CLUSTERED 
(
	[BugSeverity_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_BugSeverity] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugStatus](
	[BugStatus_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_BugStatus] PRIMARY KEY CLUSTERED 
(
	[BugStatus_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_BugStatus] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugType](
	[BugType_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_BugType] PRIMARY KEY CLUSTERED 
(
	[BugType_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_BugType] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Country]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Country](
	[Country_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Country_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Country] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bookmark]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Bookmark](
	[Bookmark_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Description] [ntext] NULL,
	[Url] [nvarchar](256) NULL,
	[FullBitmap] [image] NULL,
	[LinkBitmap] [image] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_Bookmark] PRIMARY KEY CLUSTERED 
(
	[Bookmark_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Bookmark]') AND name = N'UK_Bookmark')
CREATE UNIQUE NONCLUSTERED INDEX [UK_Bookmark] ON [dbo].[Bookmark] 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataObject]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataObject](
	[DataObject_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](128) NOT NULL,
 CONSTRAINT [PK_Object] PRIMARY KEY CLUSTERED 
(
	[DataObject_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_DataObject] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugPriority]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugPriority](
	[BugPriority_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_BugPriority] PRIMARY KEY CLUSTERED 
(
	[BugPriority_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_BugPriority] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugProject]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugProject](
	[BugProject_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Description] [ntext] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_BugProject] PRIMARY KEY CLUSTERED 
(
	[BugProject_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_BugProject] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeedType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FeedType](
	[FeedType_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Xsl] [ntext] NULL,
	[SpanRows] [int] NOT NULL CONSTRAINT [DF_FeedType_SpanRows]  DEFAULT (10),
	[SpanColumns] [int] NOT NULL CONSTRAINT [DF_FeedType_SpanColumns]  DEFAULT (1),
	[SpanRowsPreview] [int] NOT NULL CONSTRAINT [DF_FeedType_SpanRowsPreview]  DEFAULT (3),
	[SpanColumnsPreview] [int] NOT NULL CONSTRAINT [DF_FeedType_SpanColumnsPreview]  DEFAULT (1),
 CONSTRAINT [PK_FeedType] PRIMARY KEY CLUSTERED 
(
	[FeedType_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_FeedType] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PictureType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PictureType](
	[PictureType_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_PictureType] PRIMARY KEY CLUSTERED 
(
	[PictureType_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_PictureType] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlaceType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PlaceType](
	[PlaceType_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_PlaceType] PRIMARY KEY CLUSTERED 
(
	[PlaceType_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_PlaceType] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Survey](
	[Survey_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Survey] PRIMARY KEY CLUSTERED 
(
	[Survey_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Survey] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterDaily]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CounterDaily](
	[CounterDaily_Id] [int] IDENTITY(1,1) NOT NULL,
	[Total] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CounterDaily] PRIMARY KEY CLUSTERED 
(
	[CounterDaily_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CounterDaily_Timestamp] UNIQUE NONCLUSTERED 
(
	[Timestamp] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterHourly]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CounterHourly](
	[CounterHourly_Id] [int] IDENTITY(1,1) NOT NULL,
	[Total] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CounterHourly] PRIMARY KEY CLUSTERED 
(
	[CounterHourly_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CounterHourly_Timestamps] UNIQUE NONCLUSTERED 
(
	[Timestamp] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterWeekly]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CounterWeekly](
	[CounterWeekly_Id] [int] IDENTITY(1,1) NOT NULL,
	[Total] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CounterWeekly] PRIMARY KEY CLUSTERED 
(
	[CounterWeekly_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CounterWeekly_Timestamp] UNIQUE NONCLUSTERED 
(
	[Timestamp] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterMonthly]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CounterMonthly](
	[CounterMonthly_Id] [int] IDENTITY(1,1) NOT NULL,
	[Total] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CounterMonthly] PRIMARY KEY CLUSTERED 
(
	[CounterMonthly_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CounterMonthly_Timestamp] UNIQUE NONCLUSTERED 
(
	[Timestamp] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterYearly]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CounterYearly](
	[CounterYearly_Id] [int] IDENTITY(1,1) NOT NULL,
	[Total] [bigint] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CounterYearly] PRIMARY KEY CLUSTERED 
(
	[CounterYearly_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_CounterYearly_Timestamp] UNIQUE NONCLUSTERED 
(
	[Timestamp] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefererQuery]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RefererQuery](
	[RefererQuery_Id] [int] IDENTITY(1,1) NOT NULL,
	[Keywords] [nvarchar](128) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Total] [int] NOT NULL,
 CONSTRAINT [PK_RefererQuery] PRIMARY KEY CLUSTERED 
(
	[RefererQuery_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_RefererQuery] UNIQUE NONCLUSTERED 
(
	[Keywords] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RefererQuery]') AND name = N'IX_RefererQuery_Total')
CREATE NONCLUSTERED INDEX [IX_RefererQuery_Total] ON [dbo].[RefererQuery] 
(
	[Total] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TagWord]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TagWord](
	[TagWord_Id] [int] IDENTITY(1,1) NOT NULL,
	[Word] [nvarchar](64) NOT NULL,
	[Promoted] [bit] NOT NULL CONSTRAINT [DF_TagWord_Promoted]  DEFAULT (0),
	[Excluded] [bit] NOT NULL CONSTRAINT [DF_TagWord_Excluded]  DEFAULT (0),
 CONSTRAINT [PK_TagWord] PRIMARY KEY CLUSTERED 
(
	[TagWord_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_TagWord] UNIQUE NONCLUSTERED 
(
	[Word] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[TagWord]'))
CREATE FULLTEXT INDEX ON [dbo].[TagWord](
[Word] LANGUAGE [English])
KEY INDEX [PK_TagWord] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefererHost]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RefererHost](
	[RefererHost_Id] [int] IDENTITY(1,1) NOT NULL,
	[Host] [varchar](128) NOT NULL,
	[LastRefererUri] [varchar](256) NOT NULL,
	[LastRequestUri] [varchar](256) NOT NULL,
	[Total] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_RefererHost] PRIMARY KEY CLUSTERED 
(
	[RefererHost_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RefererHost]') AND name = N'IX_RefererHost_Total')
CREATE NONCLUSTERED INDEX [IX_RefererHost_Total] ON [dbo].[RefererHost] 
(
	[Total] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RefererHost]') AND name = N'UK_RefererHost')
CREATE UNIQUE NONCLUSTERED INDEX [UK_RefererHost] ON [dbo].[RefererHost] 
(
	[Host] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Configuration]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Configuration](
	[Configuration_Id] [int] IDENTITY(1,1) NOT NULL,
	[OptionName] [varchar](128) NOT NULL,
	[OptionValue] [ntext] NULL,
	[Password] [bit] NOT NULL CONSTRAINT [DF_Configuration_Password]  DEFAULT (0),
 CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED 
(
	[Configuration_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Configuration] UNIQUE NONCLUSTERED 
(
	[OptionName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Counter]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Counter](
	[Counter_Id] [int] IDENTITY(1,1) NOT NULL,
	[Uri] [nvarchar](256) NOT NULL,
	[Total] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_Counter] PRIMARY KEY CLUSTERED 
(
	[Counter_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Counter]') AND name = N'IX_Counter_Total')
CREATE NONCLUSTERED INDEX [IX_Counter_Total] ON [dbo].[Counter] 
(
	[Total] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Counter]') AND name = N'UK_Counter')
CREATE UNIQUE NONCLUSTERED INDEX [UK_Counter] ON [dbo].[Counter] 
(
	[Uri] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountEventType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountEventType](
	[AccountEventType_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_AccountEventType] PRIMARY KEY CLUSTERED 
(
	[AccountEventType_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterReturningDaily]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CounterReturningDaily](
	[CounterReturningDaily_Id] [int] IDENTITY(1,1) NOT NULL,
	[ReturningTotal] [int] NOT NULL,
	[NewTotal] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CounterReturningDaily] PRIMARY KEY CLUSTERED 
(
	[CounterReturningDaily_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_CounterReturningDaily] UNIQUE NONCLUSTERED 
(
	[Timestamp] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountMessage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountMessage](
	[AccountMessage_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Sent] [datetime] NOT NULL,
	[SenderAccount_Id] [int] NOT NULL,
	[Unread] [bit] NOT NULL CONSTRAINT [DF_Message_MessageRead]  DEFAULT (0),
	[Subject] [nvarchar](256) NOT NULL,
	[Body] [ntext] NOT NULL,
	[AccountMessageFolder_Id] [int] NOT NULL,
	[RecepientAccount_Id] [int] NOT NULL,
 CONSTRAINT [PK_AccountMessage] PRIMARY KEY CLUSTERED 
(
	[AccountMessage_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlacePicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PlacePicture](
	[PlacePicture_Id] [int] IDENTITY(1,1) NOT NULL,
	[Place_Id] [int] NOT NULL,
	[Bitmap] [image] NOT NULL,
	[Name] [nvarchar](64) NULL,
	[Description] [ntext] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_PlacePicture] PRIMARY KEY CLUSTERED 
(
	[PlacePicture_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlaceName]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PlaceName](
	[PlaceName_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Place_Id] [int] NOT NULL,
 CONSTRAINT [PK_PlaceName] PRIMARY KEY CLUSTERED 
(
	[PlaceName_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PlaceName]') AND name = N'UK_PlaceName')
CREATE UNIQUE NONCLUSTERED INDEX [UK_PlaceName] ON [dbo].[PlaceName] 
(
	[Name] ASC,
	[Place_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[PlaceName]'))
CREATE FULLTEXT INDEX ON [dbo].[PlaceName]
KEY INDEX [PK_PlaceName] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountPlaceRequest]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountPlaceRequest](
	[AccountPlaceRequest_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Place_Id] [int] NOT NULL,
	[Message] [ntext] NOT NULL,
	[Submitted] [datetime] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_AccountPlaceRequest] PRIMARY KEY CLUSTERED 
(
	[AccountPlaceRequest_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountPlaceRequest] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[Place_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountEvent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountEvent](
	[AccountEvent_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[AccountEventType_Id] [int] NOT NULL,
	[Place_Id] [int] NULL,
	[Phone] [varchar](24) NULL,
	[Email] [varchar](128) NULL,
	[Website] [varchar](128) NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Description] [ntext] NULL,
	[Cost] [nvarchar](128) NULL,
	[Publish] [bit] NOT NULL CONSTRAINT [DF_Event_Publish]  DEFAULT ((1)),
	[Schedule_Id] [int] NULL,
 CONSTRAINT [PK_AccountEvent] PRIMARY KEY CLUSTERED 
(
	[AccountEvent_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountPlace]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountPlace](
	[AccountPlace_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Place_Id] [int] NOT NULL,
	[Description] [ntext] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Type_Id] [int] NOT NULL,
 CONSTRAINT [PK_AccountPlace] PRIMARY KEY CLUSTERED 
(
	[AccountPlace_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountPlace] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[Place_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountPlaceFavorite]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountPlaceFavorite](
	[AccountPlaceFavorite_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Place_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountPlaceFavorite] PRIMARY KEY CLUSTERED 
(
	[AccountPlaceFavorite_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountPlaceFavorite] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[Place_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountEventPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountEventPicture](
	[AccountEventPicture_Id] [int] IDENTITY(1,1) NOT NULL,
	[Picture] [image] NULL,
	[AccountEvent_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Description] [ntext] NULL,
 CONSTRAINT [PK_AccountEventPicture] PRIMARY KEY CLUSTERED 
(
	[AccountEventPicture_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountEventPicture]') AND name = N'IX_AccountEventPicture')
CREATE NONCLUSTERED INDEX [IX_AccountEventPicture] ON [dbo].[AccountEventPicture] 
(
	[AccountEvent_Id] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountEventPicture]') AND name = N'UK_AccountEventPicture')
CREATE UNIQUE NONCLUSTERED INDEX [UK_AccountEventPicture] ON [dbo].[AccountEventPicture] 
(
	[AccountEvent_Id] ASC,
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountStoryPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountStoryPicture](
	[AccountStoryPicture_Id] [int] IDENTITY(1,1) NOT NULL,
	[Picture] [image] NULL,
	[Location] [int] NOT NULL,
	[AccountStory_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modifed] [datetime] NOT NULL,
	[Name] [nvarchar](128) NULL,
 CONSTRAINT [PK_AccountStoryPicture] PRIMARY KEY CLUSTERED 
(
	[AccountStoryPicture_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugLink]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugLink](
	[BugLink_Id] [int] IDENTITY(1,1) NOT NULL,
	[Bug_Id] [int] NOT NULL,
	[RelatedBug_Id] [int] NOT NULL,
 CONSTRAINT [PK_BugLink] PRIMARY KEY CLUSTERED 
(
	[BugLink_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_BugLink] UNIQUE NONCLUSTERED 
(
	[Bug_Id] ASC,
	[RelatedBug_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNote]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BugNote](
	[BugNote_Id] [int] IDENTITY(1,1) NOT NULL,
	[Details] [ntext] NULL,
	[Account_Id] [int] NULL,
	[Bug_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_BugNote] PRIMARY KEY CLUSTERED 
(
	[BugNote_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bug]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Bug](
	[Bug_Id] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Details] [ntext] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Status_Id] [int] NOT NULL,
	[Type_Id] [int] NOT NULL,
	[Account_Id] [int] NULL,
	[Priority_Id] [int] NOT NULL,
	[Severity_Id] [int] NOT NULL,
	[Resolution_Id] [int] NULL,
	[Project_Id] [int] NOT NULL,
 CONSTRAINT [PK_Bug] PRIMARY KEY CLUSTERED 
(
	[Bug_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Place]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Place](
	[Place_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[City_Id] [int] NOT NULL,
	[Street] [nvarchar](128) NULL,
	[Zip] [nvarchar](24) NULL,
	[CrossStreet] [nvarchar](128) NULL,
	[Description] [ntext] NULL,
	[Phone] [varchar](24) NULL,
	[Fax] [varchar](24) NULL,
	[Email] [varchar](128) NULL,
	[Website] [varchar](128) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_Place] PRIMARY KEY CLUSTERED 
(
	[Place_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Place]') AND name = N'IX_Place_Name')
CREATE NONCLUSTERED INDEX [IX_Place_Name] ON [dbo].[Place] 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[Place]'))
CREATE FULLTEXT INDEX ON [dbo].[Place](
[CrossStreet] LANGUAGE [English], 
[Description] LANGUAGE [English], 
[Email] LANGUAGE [English], 
[Fax] LANGUAGE [English], 
[Name] LANGUAGE [English], 
[Phone] LANGUAGE [English], 
[Street] LANGUAGE [English], 
[Website] LANGUAGE [English], 
[Zip] LANGUAGE [English])
KEY INDEX [PK_Place] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[City]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[City](
	[City_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Country_Id] [int] NOT NULL,
	[State_Id] [int] NULL,
	[Tag] [nvarchar](24) NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[City_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_City] UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[Country_Id] ASC,
	[State_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[State]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[State](
	[State_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Country_Id] [int] NOT NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[State_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_State] UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[Country_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Account](
	[Account_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Password] [varchar](16) NOT NULL,
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_Account_Enabled]  DEFAULT (1),
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Account_Created]  DEFAULT (getdate()),
	[LastLogin] [datetime] NULL,
	[Modified] [datetime] NOT NULL CONSTRAINT [DF_Account_Modified]  DEFAULT (getdate()),
	[Birthday] [datetime] NULL,
	[City] [nvarchar](64) NULL,
	[State_Id] [int] NULL,
	[Country_Id] [int] NULL,
	[UtcOffset] [int] NULL,
	[Signature] [nvarchar](128) NULL,
	[IsAdministrator] [bit] NULL CONSTRAINT [DF_Account_IsAdministrator]  DEFAULT ((0)),
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = N'IX_LastLogin')
CREATE NONCLUSTERED INDEX [IX_LastLogin] ON [dbo].[Account] 
(
	[LastLogin] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[Account]'))
CREATE FULLTEXT INDEX ON [dbo].[Account](
[Name] LANGUAGE [English])
KEY INDEX [PK_Account] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountAddress]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountAddress](
	[AccountAddress_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Address_Id] [int] NOT NULL,
	[Apt] [nvarchar](24) NULL,
	[City] [nvarchar](64) NULL,
	[Country_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_AccountAddress_Created]  DEFAULT (getdate()),
	[Modified] [datetime] NOT NULL CONSTRAINT [DF_AccountAddress_Modified]  DEFAULT (getdate()),
	[State_Id] [int] NULL,
	[Street] [nvarchar](128) NULL,
	[Zip] [nvarchar](24) NULL,
	[Name] [nvarchar](64) NULL,
 CONSTRAINT [PK_AccountAddress] PRIMARY KEY CLUSTERED 
(
	[AccountAddress_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reminder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Reminder](
	[Reminder_Id] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](128) NOT NULL,
	[Body] [ntext] NOT NULL,
	[DeltaHours] [int] NOT NULL,
	[DataObjectField] [varchar](64) NOT NULL,
	[DataObject_Id] [int] NOT NULL,
	[Recurrent] [bit] NOT NULL CONSTRAINT [DF_Reminder_Recurrent]  DEFAULT (1),
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_Reminder_Enabled]  DEFAULT (0),
	[LastRun] [datetime] NULL,
	[LastRunError] [ntext] NULL,
 CONSTRAINT [PK_Reminder] PRIMARY KEY CLUSTERED 
(
	[Reminder_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feature]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Feature](
	[Feature_Id] [int] IDENTITY(1,1) NOT NULL,
	[DataObject_Id] [int] NOT NULL,
	[DataRow_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_Feature] PRIMARY KEY CLUSTERED 
(
	[Feature_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Feature]') AND name = N'IX_Feature')
CREATE NONCLUSTERED INDEX [IX_Feature] ON [dbo].[Feature] 
(
	[DataRow_Id] ASC,
	[DataObject_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiscussionPost]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DiscussionPost](
	[DiscussionPost_Id] [int] IDENTITY(1,1) NOT NULL,
	[DiscussionThread_Id] [int] NOT NULL,
	[DiscussionPostParent_Id] [int] NULL,
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Body] [ntext] NULL,
	[Subject] [nvarchar](256) NULL,
 CONSTRAINT [PK_DiscussionPost] PRIMARY KEY CLUSTERED 
(
	[DiscussionPost_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DiscussionPost]') AND name = N'IX_DiscussionThread_Id')
CREATE NONCLUSTERED INDEX [IX_DiscussionThread_Id] ON [dbo].[DiscussionPost] 
(
	[DiscussionThread_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[DiscussionPost]') AND name = N'IX_Modified_DiscussionPost_Id')
CREATE NONCLUSTERED INDEX [IX_Modified_DiscussionPost_Id] ON [dbo].[DiscussionPost] 
(
	[DiscussionPost_Id] ASC,
	[Modified] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[DiscussionPost]'))
CREATE FULLTEXT INDEX ON [dbo].[DiscussionPost](
[Body] LANGUAGE [English], 
[Subject] LANGUAGE [English])
KEY INDEX [PK_DiscussionPost] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeed]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountFeed](
	[AccountFeed_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [ntext] NULL,
	[FeedUrl] [varchar](256) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[FeedType_Id] [int] NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Username] [varchar](64) NULL,
	[Password] [varchar](64) NULL,
	[UpdateFrequency] [int] NOT NULL,
	[LastError] [ntext] NULL,
	[LinkUrl] [varchar](256) NULL,
	[Publish] [bit] NOT NULL CONSTRAINT [DF_AccountFeed_Publish]  DEFAULT ((1)),
 CONSTRAINT [PK_AccountFeed] PRIMARY KEY CLUSTERED 
(
	[AccountFeed_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountFeed] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Picture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Picture](
	[Picture_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Description] [ntext] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Bitmap] [image] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Picture] PRIMARY KEY CLUSTERED 
(
	[Picture_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountContent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountContent](
	[AccountContent_Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountContentGroup_Id] [int] NOT NULL,
	[Tag] [nvarchar](128) NOT NULL,
	[Text] [ntext] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountContent] PRIMARY KEY CLUSTERED 
(
	[AccountContent_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountContent]') AND name = N'IX_AccountContent')
CREATE NONCLUSTERED INDEX [IX_AccountContent] ON [dbo].[AccountContent] 
(
	[AccountContentGroup_Id] ASC,
	[Timestamp] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReminderEvent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ReminderEvent](
	[ReminderEvent_Id] [int] IDENTITY(1,1) NOT NULL,
	[Reminder_Id] [int] NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_ReminderEvent] PRIMARY KEY CLUSTERED 
(
	[ReminderEvent_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ReminderEvent]') AND name = N'UK_ReminderEvent')
CREATE NONCLUSTERED INDEX [UK_ReminderEvent] ON [dbo].[ReminderEvent] 
(
	[Reminder_Id] ASC,
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ScheduleInstance]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ScheduleInstance](
	[ScheduleInstance_Id] [int] IDENTITY(1,1) NOT NULL,
	[Schedule_Id] [int] NOT NULL,
	[Instance] [int] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ScheduleInstance] PRIMARY KEY CLUSTERED 
(
	[ScheduleInstance_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ScheduleInstance]') AND name = N'IX_ScheduleInstance_DateTime')
CREATE NONCLUSTERED INDEX [IX_ScheduleInstance_DateTime] ON [dbo].[ScheduleInstance] 
(
	[StartDateTime] ASC,
	[EndDateTime] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ScheduleInstance]') AND name = N'UK_ScheduleInstance')
CREATE UNIQUE NONCLUSTERED INDEX [UK_ScheduleInstance] ON [dbo].[ScheduleInstance] 
(
	[Schedule_Id] ASC,
	[Instance] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SurveyQuestion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SurveyQuestion](
	[SurveyQuestion_Id] [int] IDENTITY(1,1) NOT NULL,
	[Question] [nvarchar](256) NOT NULL,
	[Survey_Id] [int] NOT NULL,
 CONSTRAINT [PK_SurveyQuestion] PRIMARY KEY CLUSTERED 
(
	[SurveyQuestion_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_SurveyQuestion] UNIQUE NONCLUSTERED 
(
	[Question] ASC,
	[Survey_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiscussionThread]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DiscussionThread](
	[DiscussionThread_Id] [int] IDENTITY(1,1) NOT NULL,
	[Discussion_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_DiscussionThread] PRIMARY KEY CLUSTERED 
(
	[DiscussionThread_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountSurveyAnswer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountSurveyAnswer](
	[AccountSurveyAnswer_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_AccountProfile_Created]  DEFAULT (getdate()),
	[Answer] [ntext] NULL,
	[Modified] [datetime] NOT NULL CONSTRAINT [DF_AccountProfile_Modified]  DEFAULT (getdate()),
	[SurveyQuestion_Id] [int] NOT NULL,
 CONSTRAINT [PK_AccountSurveyAnswer] PRIMARY KEY CLUSTERED 
(
	[AccountSurveyAnswer_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountSurveyAnswer] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[SurveyQuestion_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[AccountSurveyAnswer]'))
CREATE FULLTEXT INDEX ON [dbo].[AccountSurveyAnswer](
[Answer] LANGUAGE [English])
KEY INDEX [PK_AccountSurveyAnswer] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TagWordAccount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TagWordAccount](
	[TagWordAccount_Id] [int] IDENTITY(1,1) NOT NULL,
	[TagWord_Id] [int] NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_TagWordAccount] PRIMARY KEY CLUSTERED 
(
	[TagWordAccount_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_TagWordAccount] UNIQUE NONCLUSTERED 
(
	[TagWord_Id] ASC,
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlogPost]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountBlogPost](
	[AccountBlogPost_Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountBlog_Id] [int] NOT NULL,
	[Title] [nvarchar](128) NULL,
	[Body] [ntext] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Account_Id] [int] NOT NULL,
	[AccountName] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_AccountBlogPost] PRIMARY KEY CLUSTERED 
(
	[AccountBlogPost_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlogPost]') AND name = N'IX_AccountBlogPost')
CREATE NONCLUSTERED INDEX [IX_AccountBlogPost] ON [dbo].[AccountBlogPost] 
(
	[Modified] DESC,
	[AccountBlog_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[AccountBlogPost]'))
CREATE FULLTEXT INDEX ON [dbo].[AccountBlogPost](
[Body] LANGUAGE [English], 
[Title] LANGUAGE [English])
KEY INDEX [PK_AccountBlogPost] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlogAuthor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountBlogAuthor](
	[AccountBlogAuthor_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[AccountBlog_Id] [int] NOT NULL,
	[AllowPost] [bit] NOT NULL CONSTRAINT [DF_AccountBlogAuthor_AllowPost]  DEFAULT ((1)),
	[AllowDelete] [bit] NOT NULL CONSTRAINT [DF_AccountBlogAuthor_AllowDelete]  DEFAULT ((0)),
	[AllowEdit] [bit] NOT NULL CONSTRAINT [DF_AccountBlogAuthor_AllowEdit]  DEFAULT ((0)),
 CONSTRAINT [PK_AccountBlogAuthor] PRIMARY KEY CLUSTERED 
(
	[AccountBlogAuthor_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlogAuthor]') AND name = N'UK_AccountBlogAuthor')
CREATE UNIQUE NONCLUSTERED INDEX [UK_AccountBlogAuthor] ON [dbo].[AccountBlogAuthor] 
(
	[Account_Id] ASC,
	[AccountBlog_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefererHostDup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RefererHostDup](
	[RefererHostDup_Id] [int] IDENTITY(1,1) NOT NULL,
	[RefererHost_Id] [int] NOT NULL,
	[Host] [varchar](128) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_RefererHostDup] PRIMARY KEY CLUSTERED 
(
	[RefererHostDup_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_RefererHostDup] UNIQUE NONCLUSTERED 
(
	[RefererHost_Id] ASC,
	[Host] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RefererHostDup]') AND name = N'IX_RefererHostDup')
CREATE NONCLUSTERED INDEX [IX_RefererHostDup] ON [dbo].[RefererHostDup] 
(
	[Host] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefererAccount]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RefererAccount](
	[RefererAccount_Id] [int] IDENTITY(1,1) NOT NULL,
	[RefererHost_Id] [int] NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_RefererAccount] PRIMARY KEY CLUSTERED 
(
	[RefererAccount_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RefererAccount]') AND name = N'UK_RefererAccount')
CREATE UNIQUE NONCLUSTERED INDEX [UK_RefererAccount] ON [dbo].[RefererAccount] 
(
	[RefererHost_Id] ASC,
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountEmailMessage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountEmailMessage](
	[AccountEmailMessage_Id] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Body] [ntext] NOT NULL,
	[Account_Id] [int] NOT NULL,
	[DeleteSent] [bit] NOT NULL,
	[Sent] [bit] NOT NULL,
	[SendError] [varchar](128) NULL,
	[MailTo] [varchar](128) NOT NULL,
	[MailFrom] [varchar](128) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountEmailMessage] PRIMARY KEY CLUSTERED 
(
	[AccountEmailMessage_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountFriend]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountFriend](
	[AccountFriend_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Keen_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountFriend] PRIMARY KEY CLUSTERED 
(
	[AccountFriend_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountFriend] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[Keen_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountFriendRequest]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountFriendRequest](
	[AccountFriendRequest_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Keen_Id] [int] NOT NULL,
	[Message] [ntext] NULL,
	[Created] [datetime] NOT NULL,
	[Rejected] [bit] NOT NULL CONSTRAINT [DF_AccountFriendRequest_Rejected]  DEFAULT (0),
 CONSTRAINT [PK_AccountFriendRequest] PRIMARY KEY CLUSTERED 
(
	[AccountFriendRequest_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountFriendRequest] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[Keen_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountInvitation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountInvitation](
	[AccountInvitation_Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](128) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Code] [varchar](38) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Message] [text] NULL,
 CONSTRAINT [PK_AccountInvitation] PRIMARY KEY CLUSTERED 
(
	[AccountInvitation_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountInvitation] UNIQUE NONCLUSTERED 
(
	[Email] ASC,
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountWebsite]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountWebsite](
	[AccountWebsite_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Description] [ntext] NULL,
	[Url] [varchar](128) NOT NULL,
	[Account_Id] [int] NOT NULL,
 CONSTRAINT [PK_AccountWebsite] PRIMARY KEY CLUSTERED 
(
	[AccountWebsite_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountWebsite] UNIQUE NONCLUSTERED 
(
	[Url] ASC,
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountEmail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountEmail](
	[AccountEmail_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Address] [varchar](64) NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_AccountEmail_Created]  DEFAULT (getdate()),
	[Modified] [datetime] NULL,
	[Verified] [bit] NOT NULL CONSTRAINT [DF_AccountEmail_Verified]  DEFAULT (0),
	[Principal] [bit] NOT NULL CONSTRAINT [DF_AccountEmail_Primary]  DEFAULT (0),
 CONSTRAINT [PK_AccountEmail] PRIMARY KEY CLUSTERED 
(
	[AccountEmail_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountEmail] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[Address] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountOpenId]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountOpenId](
	[AccountOpenId_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[IdentityUrl] [nvarchar](256) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountOpenId] PRIMARY KEY CLUSTERED 
(
	[AccountOpenId_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountOpenId]') AND name = N'UK_AccountOpenId')
CREATE UNIQUE NONCLUSTERED INDEX [UK_AccountOpenId] ON [dbo].[AccountOpenId] 
(
	[IdentityUrl] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountBlog](
	[AccountBlog_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Description] [ntext] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountBlog] PRIMARY KEY CLUSTERED 
(
	[AccountBlog_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountBlog] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC,
	[Name] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlog]') AND name = N'IX_AccountBlog')
CREATE NONCLUSTERED INDEX [IX_AccountBlog] ON [dbo].[AccountBlog] 
(
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Discussion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Discussion](
	[Discussion_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Description] [ntext] NULL,
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Personal] [bit] NOT NULL CONSTRAINT [DF_Discussion_Private]  DEFAULT ((0)),
	[Object_Id] [int] NULL,
 CONSTRAINT [PK_Discussion] PRIMARY KEY CLUSTERED 
(
	[Discussion_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Discussion] UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[Account_Id] ASC,
	[Object_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Schedule]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Schedule](
	[Schedule_Id] [int] IDENTITY(1,1) NOT NULL,
	[RecurrencePattern] [smallint] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[AllDay] [bit] NULL CONSTRAINT [DF_EventSchedule_AllDay]  DEFAULT ((0)),
	[Daily_EveryNDays] [int] NULL,
	[Weekly_EveryNWeeks] [int] NULL,
	[Weekly_DaysOfWeek] [smallint] NULL,
	[Monthly_Day] [int] NULL,
	[Monthly_Month] [int] NULL,
	[MonthlyEx_DayIndex] [int] NULL,
	[MonthlyEx_DayName] [int] NULL,
	[MonthlyEx_Month] [int] NULL,
	[Yearly_Month] [int] NULL,
	[Yearly_Day] [int] NULL,
	[YearlyEx_DayIndex] [int] NULL,
	[YearlyEx_DayName] [int] NULL,
	[YearlyEx_Month] [int] NULL,
	[Endless] [bit] NULL CONSTRAINT [DF_EventSchedule_Endless]  DEFAULT ((0)),
	[EndOccurrences] [int] NULL,
	[EndDateTime] [datetime] NULL,
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_EventSchedule] PRIMARY KEY CLUSTERED 
(
	[Schedule_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Schedule]') AND name = N'IX_StartDateTime')
CREATE NONCLUSTERED INDEX [IX_StartDateTime] ON [dbo].[Schedule] 
(
	[StartDateTime] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountContentGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountContentGroup](
	[AccountContentGroup_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Description] [ntext] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Trusted] [bit] NOT NULL,
 CONSTRAINT [PK_AccountContentGroup] PRIMARY KEY CLUSTERED 
(
	[AccountContentGroup_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountProfile]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountProfile](
	[AccountProfile_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NULL,
	[AboutSelf] [ntext] NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountProfile] PRIMARY KEY CLUSTERED 
(
	[AccountProfile_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountProfile] UNIQUE NONCLUSTERED 
(
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[AccountProfile]'))
CREATE FULLTEXT INDEX ON [dbo].[AccountProfile](
[AboutSelf] LANGUAGE [English])
KEY INDEX [PK_AccountProfile] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountStory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountStory](
	[AccountStory_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Summary] [ntext] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Account_Id] [int] NOT NULL,
 CONSTRAINT [PK_Story] PRIMARY KEY CLUSTERED 
(
	[AccountStory_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountStory] UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountStory]') AND name = N'IX_Created')
CREATE NONCLUSTERED INDEX [IX_Created] ON [dbo].[AccountStory] 
(
	[Created] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[AccountStory]'))
CREATE FULLTEXT INDEX ON [dbo].[AccountStory](
[Name] LANGUAGE [English], 
[Summary] LANGUAGE [English])
KEY INDEX [PK_Story] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountMessageFolder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountMessageFolder](
	[AccountMessageFolder_Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[AccountMessageFolderParent_Id] [int] NULL,
	[System] [bit] NOT NULL CONSTRAINT [DF_MessageFolder_System]  DEFAULT (0),
	[Account_Id] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountMessageFolder] PRIMARY KEY CLUSTERED 
(
	[AccountMessageFolder_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountMessageFolder] UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[AccountMessageFolderParent_Id] ASC,
	[Account_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountPicture]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountPicture](
	[AccountPicture_Id] [int] IDENTITY(1,1) NOT NULL,
	[Account_Id] [int] NOT NULL,
	[Bitmap] [image] NOT NULL,
	[Name] [nvarchar](64) NULL,
	[Description] [ntext] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
 CONSTRAINT [PK_AccountPicture] PRIMARY KEY CLUSTERED 
(
	[AccountPicture_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountEmailConfirmation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountEmailConfirmation](
	[AccountEmailConfirmation_Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountEmail_Id] [int] NOT NULL,
	[Code] [varchar](38) NOT NULL,
 CONSTRAINT [PK_AccountEmailConfirmation] PRIMARY KEY CLUSTERED 
(
	[AccountEmailConfirmation_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountEmailConfirmation] UNIQUE NONCLUSTERED 
(
	[AccountEmail_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeedItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountFeedItem](
	[AccountFeedItem_Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountFeed_Id] [int] NOT NULL,
	[Title] [ntext] NOT NULL,
	[Description] [ntext] NULL,
	[Link] [varchar](256) NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Guid] [varchar](256) NULL,
 CONSTRAINT [PK_AccountFeedItem] PRIMARY KEY CLUSTERED 
(
	[AccountFeedItem_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeedItem]') AND name = N'IX_Created')
CREATE NONCLUSTERED INDEX [IX_Created] ON [dbo].[AccountFeedItem] 
(
	[Created] DESC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
GO
IF not EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[AccountFeedItem]'))
CREATE FULLTEXT INDEX ON [dbo].[AccountFeedItem](
[Description] LANGUAGE [English], 
[Title] LANGUAGE [English])
KEY INDEX [PK_AccountFeedItem] ON [SnCore]
WITH CHANGE_TRACKING AUTO

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeedItemImg]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AccountFeedItemImg](
	[AccountFeedItemImg_Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountFeedItem_Id] [int] NOT NULL,
	[Url] [varchar](256) NOT NULL,
	[Description] [ntext] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Visible] [bit] NOT NULL CONSTRAINT [DF_AccountFeedItemImg_Visible]  DEFAULT ((1)),
	[Interesting] [bit] NOT NULL CONSTRAINT [DF_AccountFeedItemImg_Interesting]  DEFAULT ((0)),
	[LastError] [ntext] NULL,
	[Thumbnail] [image] NULL,
 CONSTRAINT [PK_AccountFeedItemImg] PRIMARY KEY CLUSTERED 
(
	[AccountFeedItemImg_Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_AccountFeedItemImg] UNIQUE NONCLUSTERED 
(
	[Url] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountMessage_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountMessage]'))
ALTER TABLE [dbo].[AccountMessage]  WITH CHECK ADD  CONSTRAINT [FK_AccountMessage_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountMessage_AccountMessageFolder]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountMessage]'))
ALTER TABLE [dbo].[AccountMessage]  WITH CHECK ADD  CONSTRAINT [FK_AccountMessage_AccountMessageFolder] FOREIGN KEY([AccountMessageFolder_Id])
REFERENCES [dbo].[AccountMessageFolder] ([AccountMessageFolder_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PlacePicture_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[PlacePicture]'))
ALTER TABLE [dbo].[PlacePicture]  WITH CHECK ADD  CONSTRAINT [FK_PlacePicture_Place] FOREIGN KEY([Place_Id])
REFERENCES [dbo].[Place] ([Place_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_PlaceName]') AND parent_object_id = OBJECT_ID(N'[dbo].[PlaceName]'))
ALTER TABLE [dbo].[PlaceName]  WITH CHECK ADD  CONSTRAINT [FK_Place_PlaceName] FOREIGN KEY([Place_Id])
REFERENCES [dbo].[Place] ([Place_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlaceRequest_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlaceRequest]'))
ALTER TABLE [dbo].[AccountPlaceRequest]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlaceRequest_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlaceRequest_AccountPlaceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlaceRequest]'))
ALTER TABLE [dbo].[AccountPlaceRequest]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlaceRequest_AccountPlaceType] FOREIGN KEY([Type])
REFERENCES [dbo].[AccountPlaceType] ([AccountPlaceType_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlaceRequest_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlaceRequest]'))
ALTER TABLE [dbo].[AccountPlaceRequest]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlaceRequest_Place] FOREIGN KEY([Place_Id])
REFERENCES [dbo].[Place] ([Place_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEvent_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEvent]'))
ALTER TABLE [dbo].[AccountEvent]  WITH CHECK ADD  CONSTRAINT [FK_AccountEvent_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEvent_AccountEventType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEvent]'))
ALTER TABLE [dbo].[AccountEvent]  WITH CHECK ADD  CONSTRAINT [FK_AccountEvent_AccountEventType] FOREIGN KEY([AccountEventType_Id])
REFERENCES [dbo].[AccountEventType] ([AccountEventType_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEvent_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEvent]'))
ALTER TABLE [dbo].[AccountEvent]  WITH CHECK ADD  CONSTRAINT [FK_AccountEvent_Place] FOREIGN KEY([Place_Id])
REFERENCES [dbo].[Place] ([Place_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEvent_Schedule]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEvent]'))
ALTER TABLE [dbo].[AccountEvent]  WITH CHECK ADD  CONSTRAINT [FK_AccountEvent_Schedule] FOREIGN KEY([Schedule_Id])
REFERENCES [dbo].[Schedule] ([Schedule_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlace_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlace]'))
ALTER TABLE [dbo].[AccountPlace]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlace_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlace_AccountPlaceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlace]'))
ALTER TABLE [dbo].[AccountPlace]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlace_AccountPlaceType] FOREIGN KEY([Type_Id])
REFERENCES [dbo].[AccountPlaceType] ([AccountPlaceType_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlace_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlace]'))
ALTER TABLE [dbo].[AccountPlace]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlace_Place] FOREIGN KEY([Place_Id])
REFERENCES [dbo].[Place] ([Place_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlaceFavorite_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlaceFavorite]'))
ALTER TABLE [dbo].[AccountPlaceFavorite]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlaceFavorite_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPlaceFavorite_Place]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPlaceFavorite]'))
ALTER TABLE [dbo].[AccountPlaceFavorite]  WITH CHECK ADD  CONSTRAINT [FK_AccountPlaceFavorite_Place] FOREIGN KEY([Place_Id])
REFERENCES [dbo].[Place] ([Place_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEventPicture_AccountEvent]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEventPicture]'))
ALTER TABLE [dbo].[AccountEventPicture]  WITH CHECK ADD  CONSTRAINT [FK_AccountEventPicture_AccountEvent] FOREIGN KEY([AccountEvent_Id])
REFERENCES [dbo].[AccountEvent] ([AccountEvent_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountStoryPicture_AccountStory]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountStoryPicture]'))
ALTER TABLE [dbo].[AccountStoryPicture]  WITH CHECK ADD  CONSTRAINT [FK_AccountStoryPicture_AccountStory] FOREIGN KEY([AccountStory_Id])
REFERENCES [dbo].[AccountStory] ([AccountStory_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BugLink_Bug]') AND parent_object_id = OBJECT_ID(N'[dbo].[BugLink]'))
ALTER TABLE [dbo].[BugLink]  WITH CHECK ADD  CONSTRAINT [FK_BugLink_Bug] FOREIGN KEY([Bug_Id])
REFERENCES [dbo].[Bug] ([Bug_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BugLink_Bug1]') AND parent_object_id = OBJECT_ID(N'[dbo].[BugLink]'))
ALTER TABLE [dbo].[BugLink]  WITH CHECK ADD  CONSTRAINT [FK_BugLink_Bug1] FOREIGN KEY([RelatedBug_Id])
REFERENCES [dbo].[Bug] ([Bug_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BugNote_Bug]') AND parent_object_id = OBJECT_ID(N'[dbo].[BugNote]'))
ALTER TABLE [dbo].[BugNote]  WITH CHECK ADD  CONSTRAINT [FK_BugNote_Bug] FOREIGN KEY([Bug_Id])
REFERENCES [dbo].[Bug] ([Bug_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_BugPriority]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_BugPriority] FOREIGN KEY([Priority_Id])
REFERENCES [dbo].[BugPriority] ([BugPriority_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_BugProject]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_BugProject] FOREIGN KEY([Project_Id])
REFERENCES [dbo].[BugProject] ([BugProject_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_BugResolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_BugResolution] FOREIGN KEY([Resolution_Id])
REFERENCES [dbo].[BugResolution] ([BugResolution_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_BugSeverity]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_BugSeverity] FOREIGN KEY([Severity_Id])
REFERENCES [dbo].[BugSeverity] ([BugSeverity_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_BugStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_BugStatus] FOREIGN KEY([Status_Id])
REFERENCES [dbo].[BugStatus] ([BugStatus_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_BugType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_BugType] FOREIGN KEY([Type_Id])
REFERENCES [dbo].[BugType] ([BugType_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place]  WITH CHECK ADD  CONSTRAINT [FK_Place_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_City]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place]  WITH CHECK ADD  CONSTRAINT [FK_Place_City] FOREIGN KEY([City_Id])
REFERENCES [dbo].[City] ([City_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Place_PlaceType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Place]'))
ALTER TABLE [dbo].[Place]  WITH CHECK ADD  CONSTRAINT [FK_Place_PlaceType] FOREIGN KEY([Type])
REFERENCES [dbo].[PlaceType] ([PlaceType_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_City_Country]') AND parent_object_id = OBJECT_ID(N'[dbo].[City]'))
ALTER TABLE [dbo].[City]  WITH NOCHECK ADD  CONSTRAINT [FK_City_Country] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[Country] ([Country_Id])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [FK_City_Country]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_City_State]') AND parent_object_id = OBJECT_ID(N'[dbo].[City]'))
ALTER TABLE [dbo].[City]  WITH NOCHECK ADD  CONSTRAINT [FK_City_State] FOREIGN KEY([State_Id])
REFERENCES [dbo].[State] ([State_Id])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [FK_City_State]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_State_Country]') AND parent_object_id = OBJECT_ID(N'[dbo].[State]'))
ALTER TABLE [dbo].[State]  WITH NOCHECK ADD  CONSTRAINT [FK_State_Country] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[Country] ([Country_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[State] CHECK CONSTRAINT [FK_State_Country]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Account_Country]') AND parent_object_id = OBJECT_ID(N'[dbo].[Account]'))
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Country] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[Country] ([Country_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Account_State]') AND parent_object_id = OBJECT_ID(N'[dbo].[Account]'))
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_State] FOREIGN KEY([State_Id])
REFERENCES [dbo].[State] ([State_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountAddress_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountAddress]'))
ALTER TABLE [dbo].[AccountAddress]  WITH CHECK ADD  CONSTRAINT [FK_AccountAddress_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Address_Country]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountAddress]'))
ALTER TABLE [dbo].[AccountAddress]  WITH CHECK ADD  CONSTRAINT [FK_Address_Country] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[Country] ([Country_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Address_State]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountAddress]'))
ALTER TABLE [dbo].[AccountAddress]  WITH CHECK ADD  CONSTRAINT [FK_Address_State] FOREIGN KEY([State_Id])
REFERENCES [dbo].[State] ([State_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reminder_DataObject]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reminder]'))
ALTER TABLE [dbo].[Reminder]  WITH CHECK ADD  CONSTRAINT [FK_Reminder_DataObject] FOREIGN KEY([DataObject_Id])
REFERENCES [dbo].[DataObject] ([DataObject_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Feature_DataObject]') AND parent_object_id = OBJECT_ID(N'[dbo].[Feature]'))
ALTER TABLE [dbo].[Feature]  WITH CHECK ADD  CONSTRAINT [FK_Feature_DataObject] FOREIGN KEY([DataObject_Id])
REFERENCES [dbo].[DataObject] ([DataObject_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DiscussionPost_DiscussionPost]') AND parent_object_id = OBJECT_ID(N'[dbo].[DiscussionPost]'))
ALTER TABLE [dbo].[DiscussionPost]  WITH CHECK ADD  CONSTRAINT [FK_DiscussionPost_DiscussionPost] FOREIGN KEY([DiscussionPostParent_Id])
REFERENCES [dbo].[DiscussionPost] ([DiscussionPost_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DiscussionPost_DiscussionThread]') AND parent_object_id = OBJECT_ID(N'[dbo].[DiscussionPost]'))
ALTER TABLE [dbo].[DiscussionPost]  WITH CHECK ADD  CONSTRAINT [FK_DiscussionPost_DiscussionThread] FOREIGN KEY([DiscussionThread_Id])
REFERENCES [dbo].[DiscussionThread] ([DiscussionThread_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFeed_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFeed]'))
ALTER TABLE [dbo].[AccountFeed]  WITH CHECK ADD  CONSTRAINT [FK_AccountFeed_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFeed_FeedType]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFeed]'))
ALTER TABLE [dbo].[AccountFeed]  WITH CHECK ADD  CONSTRAINT [FK_AccountFeed_FeedType] FOREIGN KEY([FeedType_Id])
REFERENCES [dbo].[FeedType] ([FeedType_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Picture_PictureType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Picture]'))
ALTER TABLE [dbo].[Picture]  WITH CHECK ADD  CONSTRAINT [FK_Picture_PictureType] FOREIGN KEY([Type])
REFERENCES [dbo].[PictureType] ([PictureType_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountContent_AccountContentGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountContent]'))
ALTER TABLE [dbo].[AccountContent]  WITH CHECK ADD  CONSTRAINT [FK_AccountContent_AccountContentGroup] FOREIGN KEY([AccountContentGroup_Id])
REFERENCES [dbo].[AccountContentGroup] ([AccountContentGroup_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReminderEvent_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderEvent]'))
ALTER TABLE [dbo].[ReminderEvent]  WITH CHECK ADD  CONSTRAINT [FK_ReminderEvent_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReminderEvent_Reminder]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReminderEvent]'))
ALTER TABLE [dbo].[ReminderEvent]  WITH CHECK ADD  CONSTRAINT [FK_ReminderEvent_Reminder] FOREIGN KEY([Reminder_Id])
REFERENCES [dbo].[Reminder] ([Reminder_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ScheduleInstance_Schedule]') AND parent_object_id = OBJECT_ID(N'[dbo].[ScheduleInstance]'))
ALTER TABLE [dbo].[ScheduleInstance]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleInstance_Schedule] FOREIGN KEY([Schedule_Id])
REFERENCES [dbo].[Schedule] ([Schedule_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SurveyQuestion_Survey]') AND parent_object_id = OBJECT_ID(N'[dbo].[SurveyQuestion]'))
ALTER TABLE [dbo].[SurveyQuestion]  WITH CHECK ADD  CONSTRAINT [FK_SurveyQuestion_Survey] FOREIGN KEY([Survey_Id])
REFERENCES [dbo].[Survey] ([Survey_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DiscussionThread_Discussion]') AND parent_object_id = OBJECT_ID(N'[dbo].[DiscussionThread]'))
ALTER TABLE [dbo].[DiscussionThread]  WITH CHECK ADD  CONSTRAINT [FK_DiscussionThread_Discussion] FOREIGN KEY([Discussion_Id])
REFERENCES [dbo].[Discussion] ([Discussion_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountSurveyAnswer_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountSurveyAnswer]'))
ALTER TABLE [dbo].[AccountSurveyAnswer]  WITH CHECK ADD  CONSTRAINT [FK_AccountSurveyAnswer_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountSurveyAnswer_SurveyQuestion]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountSurveyAnswer]'))
ALTER TABLE [dbo].[AccountSurveyAnswer]  WITH CHECK ADD  CONSTRAINT [FK_AccountSurveyAnswer_SurveyQuestion] FOREIGN KEY([SurveyQuestion_Id])
REFERENCES [dbo].[SurveyQuestion] ([SurveyQuestion_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TagWordAccount_TagWord]') AND parent_object_id = OBJECT_ID(N'[dbo].[TagWordAccount]'))
ALTER TABLE [dbo].[TagWordAccount]  WITH CHECK ADD  CONSTRAINT [FK_TagWordAccount_TagWord] FOREIGN KEY([TagWord_Id])
REFERENCES [dbo].[TagWord] ([TagWord_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountBlogPost_AccountBlog]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountBlogPost]'))
ALTER TABLE [dbo].[AccountBlogPost]  WITH CHECK ADD  CONSTRAINT [FK_AccountBlogPost_AccountBlog] FOREIGN KEY([AccountBlog_Id])
REFERENCES [dbo].[AccountBlog] ([AccountBlog_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountBlogAuthor_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountBlogAuthor]'))
ALTER TABLE [dbo].[AccountBlogAuthor]  WITH CHECK ADD  CONSTRAINT [FK_AccountBlogAuthor_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountBlogAuthor_AccountBlog]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountBlogAuthor]'))
ALTER TABLE [dbo].[AccountBlogAuthor]  WITH CHECK ADD  CONSTRAINT [FK_AccountBlogAuthor_AccountBlog] FOREIGN KEY([AccountBlog_Id])
REFERENCES [dbo].[AccountBlog] ([AccountBlog_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RefererHostDup_RefererHost]') AND parent_object_id = OBJECT_ID(N'[dbo].[RefererHostDup]'))
ALTER TABLE [dbo].[RefererHostDup]  WITH CHECK ADD  CONSTRAINT [FK_RefererHostDup_RefererHost] FOREIGN KEY([RefererHost_Id])
REFERENCES [dbo].[RefererHost] ([RefererHost_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RefererAccount_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[RefererAccount]'))
ALTER TABLE [dbo].[RefererAccount]  WITH CHECK ADD  CONSTRAINT [FK_RefererAccount_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RefererAccount_RefererHost]') AND parent_object_id = OBJECT_ID(N'[dbo].[RefererAccount]'))
ALTER TABLE [dbo].[RefererAccount]  WITH CHECK ADD  CONSTRAINT [FK_RefererAccount_RefererHost] FOREIGN KEY([RefererHost_Id])
REFERENCES [dbo].[RefererHost] ([RefererHost_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEmailMessage_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEmailMessage]'))
ALTER TABLE [dbo].[AccountEmailMessage]  WITH CHECK ADD  CONSTRAINT [FK_AccountEmailMessage_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFriend_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFriend]'))
ALTER TABLE [dbo].[AccountFriend]  WITH CHECK ADD  CONSTRAINT [FK_AccountFriend_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFriend_Account1]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFriend]'))
ALTER TABLE [dbo].[AccountFriend]  WITH CHECK ADD  CONSTRAINT [FK_AccountFriend_Account1] FOREIGN KEY([Keen_Id])
REFERENCES [dbo].[Account] ([Account_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFriendRequest_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFriendRequest]'))
ALTER TABLE [dbo].[AccountFriendRequest]  WITH CHECK ADD  CONSTRAINT [FK_AccountFriendRequest_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFriendRequest_Account1]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFriendRequest]'))
ALTER TABLE [dbo].[AccountFriendRequest]  WITH CHECK ADD  CONSTRAINT [FK_AccountFriendRequest_Account1] FOREIGN KEY([Keen_Id])
REFERENCES [dbo].[Account] ([Account_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountInvitation_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountInvitation]'))
ALTER TABLE [dbo].[AccountInvitation]  WITH CHECK ADD  CONSTRAINT [FK_AccountInvitation_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountWebsite_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountWebsite]'))
ALTER TABLE [dbo].[AccountWebsite]  WITH CHECK ADD  CONSTRAINT [FK_AccountWebsite_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEmail_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEmail]'))
ALTER TABLE [dbo].[AccountEmail]  WITH CHECK ADD  CONSTRAINT [FK_AccountEmail_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountOpenId_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountOpenId]'))
ALTER TABLE [dbo].[AccountOpenId]  WITH CHECK ADD  CONSTRAINT [FK_AccountOpenId_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountBlog_AccountBlog]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountBlog]'))
ALTER TABLE [dbo].[AccountBlog]  WITH CHECK ADD  CONSTRAINT [FK_AccountBlog_AccountBlog] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Discussion_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[Discussion]'))
ALTER TABLE [dbo].[Discussion]  WITH CHECK ADD  CONSTRAINT [FK_Discussion_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Schedule_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[Schedule]'))
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountContentGroup_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountContentGroup]'))
ALTER TABLE [dbo].[AccountContentGroup]  WITH CHECK ADD  CONSTRAINT [FK_AccountContentGroup_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountProfile_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountProfile]'))
ALTER TABLE [dbo].[AccountProfile]  WITH CHECK ADD  CONSTRAINT [FK_AccountProfile_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountStory_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountStory]'))
ALTER TABLE [dbo].[AccountStory]  WITH CHECK ADD  CONSTRAINT [FK_AccountStory_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountMessageFolder_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountMessageFolder]'))
ALTER TABLE [dbo].[AccountMessageFolder]  WITH CHECK ADD  CONSTRAINT [FK_AccountMessageFolder_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountMessageFolder_AccountMessageFolder]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountMessageFolder]'))
ALTER TABLE [dbo].[AccountMessageFolder]  WITH CHECK ADD  CONSTRAINT [FK_AccountMessageFolder_AccountMessageFolder] FOREIGN KEY([AccountMessageFolderParent_Id])
REFERENCES [dbo].[AccountMessageFolder] ([AccountMessageFolder_Id])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountPicture_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountPicture]'))
ALTER TABLE [dbo].[AccountPicture]  WITH CHECK ADD  CONSTRAINT [FK_AccountPicture_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEmailConfirmation_AccountEmail]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEmailConfirmation]'))
ALTER TABLE [dbo].[AccountEmailConfirmation]  WITH CHECK ADD  CONSTRAINT [FK_AccountEmailConfirmation_AccountEmail] FOREIGN KEY([AccountEmail_Id])
REFERENCES [dbo].[AccountEmail] ([AccountEmail_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFeedItem_AccountFeed]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFeedItem]'))
ALTER TABLE [dbo].[AccountFeedItem]  WITH CHECK ADD  CONSTRAINT [FK_AccountFeedItem_AccountFeed] FOREIGN KEY([AccountFeed_Id])
REFERENCES [dbo].[AccountFeed] ([AccountFeed_Id])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountFeedItemImg_AccountFeedItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountFeedItemImg]'))
ALTER TABLE [dbo].[AccountFeedItemImg]  WITH CHECK ADD  CONSTRAINT [FK_AccountFeedItemImg_AccountFeedItem] FOREIGN KEY([AccountFeedItem_Id])
REFERENCES [dbo].[AccountFeedItem] ([AccountFeedItem_Id])
ON DELETE CASCADE
