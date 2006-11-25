-- add publish flag to stories
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountStory]') AND name = N'Publish')
ALTER TABLE dbo.AccountStory ADD [Publish] bit NOT NULL DEFAULT 1
GO
-- add password expired flag to accounts
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = N'IsPasswordExpired')
ALTER TABLE dbo.Account ADD [IsPasswordExpired] bit NOT NULL DEFAULT 0
GO
-- change reminders from hardcoded text to return a url: must drop existing reminders
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Reminder]') AND name = N'Url')
ALTER TABLE dbo.Reminder ADD [Url] nvarchar(128) NOT NULL
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Reminder]') AND name = N'Subject')
ALTER TABLE dbo.Reminder DROP COLUMN Subject
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Reminder]') AND name = N'Body')
ALTER TABLE dbo.Reminder DROP COLUMN Body
GO
-- switch UtcOffset to TimeZone, an index into TimeZoneInformation (2006-10-01)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = N'TimeZone')
ALTER TABLE dbo.Account ADD [TimeZone] int NOT NULL DEFAULT -1
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = N'UtcOffset')
ALTER TABLE dbo.Account DROP COLUMN UtcOffset
GO
-- publish images from the account feed
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeed]') AND name = N'PublishImgs') 
ALTER TABLE dbo.AccountFeed ADD [PublishImgs] bit NULL
GO
UPDATE dbo.AccountFeed SET PublishImgs = Publish WHERE PublishImgs IS NULL
ALTER TABLE dbo.AccountFeed ALTER COLUMN [PublishImgs] bit NOT NULL
GO
-- add a hidden from profile field
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountPicture]') AND name = N'Hidden') 
ALTER TABLE dbo.AccountPicture ADD [Hidden] bit NOT NULL DEFAULT 0
GO
-- add a requirelogin field to content groups (2006-11-04)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountContentGroup]') AND name = N'Login') 
ALTER TABLE dbo.AccountContentGroup ADD [Login] bit NOT NULL DEFAULT 0
GO
-- add an account identity to place pictures (2006-11-16)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PlacePicture]') AND name = N'Account_Id') 
ALTER TABLE dbo.PlacePicture ADD [Account_Id] int NOT NULL DEFAULT 0
GO

UPDATE dbo.PlacePicture SET [Account_Id] = dbo.Place.Account_Id 
FROM dbo.Place WHERE dbo.Place.Place_Id = dbo.PlacePicture.Place_Id
AND dbo.PlacePicture.Account_Id = 0
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PlacePicture_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[PlacePicture]'))
ALTER TABLE [dbo].[PlacePicture] WITH CHECK ADD CONSTRAINT [FK_PlacePicture_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
GO
