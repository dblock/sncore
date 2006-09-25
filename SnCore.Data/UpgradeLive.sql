ALTER TABLE dbo.Reminder ADD [Url] nvarchar(128) NOT NULL
ALTER TABLE dbo.Reminder DROP COLUMN Subject
ALTER TABLE dbo.Reminder DROP COLUMN Body
