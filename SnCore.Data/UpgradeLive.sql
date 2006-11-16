-- add publish flag to stories
ALTER TABLE dbo.AccountStory ADD [Publish] bit NOT NULL DEFAULT 1

-- add password expired flag to accounts
ALTER TABLE dbo.Account ADD [IsPasswordExpired] bit NOT NULL DEFAULT 0

-- change reminders from hardcoded text to return a url: must drop existing reminders
ALTER TABLE dbo.Reminder ADD [Url] nvarchar(128) NOT NULL
ALTER TABLE dbo.Reminder DROP COLUMN Subject
ALTER TABLE dbo.Reminder DROP COLUMN Body

-- switch UtcOffset to TimeZone, an index into TimeZoneInformation (2006-10-01)
ALTER TABLE dbo.Account ADD [TimeZone] int NOT NULL DEFAULT -1
ALTER TABLE dbo.Account DROP COLUMN UtcOffset

-- publish images from the account feed
ALTER TABLE dbo.AccountFeed ADD [PublishImgs] bit NULL
GO
UPDATE dbo.AccountFeed SET PublishImgs = Publish
ALTER TABLE dbo.AccountFeed ALTER COLUMN [PublishImgs] bit NOT NULL

-- add a hidden from profile field
ALTER TABLE dbo.AccountPicture ADD [Hidden] bit NOT NULL DEFAULT 0

-- add a requirelogin field to content groups (2006-11-04)
ALTER TABLE dbo.AccountContentGroup ADD [Login] bit NOT NULL DEFAULT 0

-- add an account identity to place pictures (2006-11-16)
ALTER TABLE dbo.PlacePicture ADD [Account_Id] int NOT NULL DEFAULT 0
GO
UPDATE dbo.PlacePicture SET [Account_Id] = dbo.Place.Account_Id 
FROM dbo.Place WHERE dbo.Place.Place_Id = dbo.PlacePicture.Place_Id
