ALTER TABLE dbo.AccountStory ADD [Publish] bit NOT NULL DEFAULT 1
ALTER TABLE dbo.Account ADD [IsPasswordExpired] bit NOT NULL DEFAULT 0
ALTER TABLE dbo.Reminder ADD [Url] nvarchar(128) NOT NULL
ALTER TABLE dbo.Reminder DROP COLUMN Subject
ALTER TABLE dbo.Reminder DROP COLUMN Body
