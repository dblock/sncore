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
-- add a neighborhood to places
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Place]') AND name = N'Neighborhood_Id') 
ALTER TABLE dbo.Place ADD [Neighborhood_Id] int NULL
GO
-- publish media content from the account feed (2007-01-27)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeed]') AND name = N'PublishMedia') 
ALTER TABLE dbo.AccountFeed ADD [PublishMedia] bit NULL
GO
UPDATE dbo.AccountFeed SET PublishMedia = Publish WHERE PublishMedia IS NULL
ALTER TABLE dbo.AccountFeed ALTER COLUMN [PublishMedia] bit NOT NULL
GO
-- object id in discussions cannot be null (2007-03-25)
ALTER TABLE dbo.Discussion ALTER COLUMN [Object_Id] int NOT NULL
GO
-- add an account identity to account event pictures (2006-11-16)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountEventPicture]') AND name = N'Account_Id') 
ALTER TABLE dbo.AccountEventPicture ADD [Account_Id] int NOT NULL DEFAULT 0
GO
UPDATE dbo.AccountEventPicture SET [Account_Id] = dbo.AccountEvent.Account_Id 
FROM dbo.AccountEvent WHERE dbo.AccountEvent.AccountEvent_Id = dbo.AccountEventPicture.AccountEvent_Id
AND dbo.AccountEventPicture.Account_Id = 0
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountEventPicture_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountEventPicture]'))
ALTER TABLE [dbo].[AccountEventPicture] WITH CHECK ADD CONSTRAINT [FK_AccountEventPicture_Account] FOREIGN KEY([Account_Id])
REFERENCES [dbo].[Account] ([Account_Id])
GO
-- update frequency wasn't properly set
UPDATE dbo.AccountFeed SET UpdateFrequency = 12 WHERE UpdateFrequency = 0
-- add a no end date/time to schedules
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Schedule]') AND name = N'NoEndDateTime')
ALTER TABLE dbo.Schedule ADD [NoEndDateTime] bit NOT NULL DEFAULT 0
GO
-- set precompute data to 1 for faster FREETEXTTABLE queries
EXEC sp_configure 'show advanced options', '1'
RECONFIGURE
GO
EXEC sp_configure 'show advanced options', '1'
EXEC sp_configure 'precompute rank', '1'
RECONFIGURE
GO
-- delete old constraint for AccountFeed, now two: Name and Uri
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeed]') AND name = N'UK_AccountFeed')
ALTER TABLE [dbo].[AccountFeed] DROP CONSTRAINT [UK_AccountFeed]
GO
-- create a DefaultView discussion column that defines how to show a discussion board by default (2007-08-29)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Discussion]') AND name = N'DefaultView')
ALTER TABLE dbo.Discussion ADD [DefaultView] nvarchar(64) NULL
GO
-- create an AccountBlog_Id column that defines a group blog (2007-08-29)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountGroup]') AND name = N'AccountBlog_Id')
ALTER TABLE dbo.AccountGroup ADD [AccountBlog_Id] int NULL
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountGroup_AccountBlog]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountGroup]'))
ALTER TABLE [dbo].[AccountGroup] WITH CHECK ADD CONSTRAINT [FK_AccountGroup_AccountBlog] FOREIGN KEY([AccountBlog_Id])
REFERENCES [dbo].[AccountBlog] ([AccountBlog_Id])
GO
-- create a default type in AccountEventType
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountEventType]') AND name = N'DefaultType') 
ALTER TABLE dbo.AccountEventType ADD [DefaultType] bit NULL
GO
UPDATE dbo.AccountEventType SET DefaultType = 0 WHERE DefaultType IS NULL
ALTER TABLE dbo.AccountEventType ALTER COLUMN [DefaultType] bit NOT NULL
GO
-- create a default type in PlaceType
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[PlaceType]') AND name = N'DefaultType') 
ALTER TABLE dbo.PlaceType ADD [DefaultType] bit NULL
GO
UPDATE dbo.PlaceType SET DefaultType = 0 WHERE DefaultType IS NULL
ALTER TABLE dbo.PlaceType ALTER COLUMN [DefaultType] bit NOT NULL
GO
-- create a default type in FeedType
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[FeedType]') AND name = N'DefaultType') 
ALTER TABLE dbo.FeedType ADD [DefaultType] bit NULL
GO
UPDATE dbo.FeedType SET DefaultType = 0 WHERE DefaultType IS NULL
ALTER TABLE dbo.FeedType ALTER COLUMN [DefaultType] bit NOT NULL
GO
-- create a failure option for AccountEmail
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountEmail]') AND name = N'Failed') 
ALTER TABLE dbo.AccountEmail ADD [Failed] bit NULL
GO
UPDATE dbo.AccountEmail SET [Failed] = 0 WHERE Failed IS NULL
ALTER TABLE dbo.AccountEmail ALTER COLUMN [Failed] bit NOT NULL
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountEmail]') AND name = N'LastError') 
ALTER TABLE dbo.AccountEmail ADD [LastError] varchar(128) NULL
GO
-- create a failure option for AccountInvitation
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountInvitation]') AND name = N'Failed') 
ALTER TABLE dbo.AccountInvitation ADD [Failed] bit NULL
GO
UPDATE dbo.AccountInvitation SET [Failed] = 0 WHERE Failed IS NULL
ALTER TABLE dbo.AccountInvitation ALTER COLUMN [Failed] bit NOT NULL
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountInvitation]') AND name = N'LastError') 
ALTER TABLE dbo.AccountInvitation ADD [LastError] varchar(128) NULL
GO
-- create a sticky option for DiscussionPost
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DiscussionPost]') AND name = N'Sticky') 
ALTER TABLE dbo.DiscussionPost ADD [Sticky] bit NULL
GO
UPDATE dbo.DiscussionPost SET [Sticky] = 0 WHERE Sticky IS NULL
ALTER TABLE dbo.DiscussionPost ALTER COLUMN [Sticky] bit NOT NULL
GO
-- Discussions that belong to specific objects are no longer identified by name, but by DataObject_Id
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Discussion]') AND name = N'DataObject_Id') 
BEGIN
ALTER TABLE dbo.Discussion ADD [DataObject_Id] int NULL
ALTER TABLE dbo.Discussion DROP CONSTRAINT [UK_Discussion]
END
GO
CREATE PROCEDURE dbo.[sp_migrate_discussion_post]
	  @data_object_name VARCHAR(128)
	, @discussion_name VARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @comments_id int
	SELECT @comments_id = DataObject_Id FROM DataObject WHERE [Name] = @data_object_name
	UPDATE Discussion SET DataObject_Id = @comments_id
	 WHERE [DataObject_Id] IS NULL 
	 AND [Name] = @discussion_name 
	 AND [Object_Id] != 0 
	 AND [Personal] = 1
END
GO
EXEC [sp_migrate_discussion_post] @data_object_name = 'Picture', @discussion_name = 'Picture Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'Story', @discussion_name = 'Story Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'Place', @discussion_name = 'Place Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'Account', @discussion_name = 'Testimonials'
EXEC [sp_migrate_discussion_post] @data_object_name = 'PlacePicture', @discussion_name = 'Place Picture Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'AccountFeedItem', @discussion_name = 'Feed Entry Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'AccountBlogPost', @discussion_name = 'Blog Post Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'AccountStoryPicture', @discussion_name = 'Story Picture Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'AccountEvent', @discussion_name = 'Event Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'AccountEventPicture', @discussion_name = 'Event Picture Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'AccountGroupPicture', @discussion_name = 'Group Picture Comments'
EXEC [sp_migrate_discussion_post] @data_object_name = 'AccountGroup', @discussion_name = 'Group Discussion'
GO
DROP PROCEDURE [sp_migrate_discussion_post]
GO
-- create a DefaultViewRows option for Discussion
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Discussion]') AND name = N'DefaultViewRows') 
ALTER TABLE dbo.Discussion ADD [DefaultViewRows] int NULL
GO
UPDATE dbo.Discussion SET [DefaultViewRows] = 5 WHERE DefaultViewRows IS NULL
ALTER TABLE dbo.Discussion ALTER COLUMN [DefaultViewRows] int NOT NULL
GO
-- create an EnableComments option for AccountBlog
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlog]') AND name = N'EnableComments') 
ALTER TABLE dbo.AccountBlog ADD [EnableComments] bit NULL
GO
UPDATE dbo.AccountBlog SET [EnableComments] = 1 WHERE EnableComments IS NULL
ALTER TABLE dbo.AccountBlog ALTER COLUMN [EnableComments] bit NOT NULL
GO
-- create an EnableComments option for AccountBlogPost
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlogPost]') AND name = N'EnableComments') 
ALTER TABLE dbo.AccountBlogPost ADD [EnableComments] bit NULL
GO
UPDATE dbo.AccountBlogPost SET [EnableComments] = 1 WHERE EnableComments IS NULL
ALTER TABLE dbo.AccountBlogPost ALTER COLUMN [EnableComments] bit NOT NULL
GO
-- create a sticky option for AccountBlogPost
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlogPost]') AND name = N'Sticky') 
ALTER TABLE dbo.AccountBlogPost ADD [Sticky] bit NULL
GO
UPDATE dbo.AccountBlogPost SET [Sticky] = 0 WHERE Sticky IS NULL
ALTER TABLE dbo.AccountBlogPost ALTER COLUMN [Sticky] bit NOT NULL
GO
-- create a DefaultViewRows option for AccountBlog
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountBlog]') AND name = N'DefaultViewRows') 
ALTER TABLE dbo.AccountBlog ADD [DefaultViewRows] int NULL
GO
UPDATE dbo.AccountBlog SET [DefaultViewRows] = 1 WHERE DefaultViewRows IS NULL
ALTER TABLE dbo.AccountBlog ALTER COLUMN [DefaultViewRows] int NOT NULL
GO
-- create an Hidden option for RefererHost
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[RefererHost]') AND name = N'Hidden') 
ALTER TABLE dbo.RefererHost ADD [Hidden] bit NULL
GO
UPDATE dbo.RefererHost SET [Hidden] = 0 WHERE Hidden IS NULL
ALTER TABLE dbo.RefererHost ALTER COLUMN [Hidden] bit NOT NULL
GO
-- resize Description column of AccountAuditEntry
ALTER TABLE dbo.AccountAuditEntry ALTER COLUMN [Description] nvarchar(384) NOT NULL
GO
-- create an Hidden option for AccountFeed
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountFeed]') AND name = N'Hidden') 
ALTER TABLE dbo.AccountFeed ADD [Hidden] bit NULL
GO
UPDATE dbo.AccountFeed SET [Hidden] = 0 WHERE Hidden IS NULL
ALTER TABLE dbo.AccountFeed ALTER COLUMN [Hidden] bit NOT NULL
GO
-- delete old constraint for AccountEventPicture
IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AccountEventPicture]') AND name = N'UK_AccountEventPicture')
DROP INDEX UK_AccountEventPicture ON [dbo].[AccountEventPicture]
GO
-- create a default type in AccountPlaceType
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountPlaceType]') AND name = N'DefaultType') 
ALTER TABLE dbo.AccountPlaceType ADD [DefaultType] bit NULL
GO
UPDATE dbo.AccountPlaceType SET DefaultType = 0 WHERE DefaultType IS NULL
ALTER TABLE dbo.AccountPlaceType ALTER COLUMN [DefaultType] bit NOT NULL
GO
-- create a Position for pictures
CREATE PROCEDURE dbo.[sp_add_picture_position]
	  @table_name VARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[' + @table_name + ']') AND name = N'Position') 
    BEGIN
		PRINT 'Altering ' + @table_name + ' ...'
		EXEC('ALTER TABLE dbo.' + @table_name + ' ADD [Position] int NULL')
		EXEC('UPDATE dbo.' + @table_name + ' SET [Position] = 0 WHERE Position IS NULL')
		EXEC('ALTER TABLE dbo.' + @table_name + ' ALTER COLUMN [Position] int NOT NULL')
	END
END
GO
EXEC [sp_add_picture_position] @table_name = 'AccountEventPicture'
EXEC [sp_add_picture_position] @table_name = 'AccountGroupPicture'
EXEC [sp_add_picture_position] @table_name = 'AccountPicture'
EXEC [sp_add_picture_position] @table_name = 'AccountStoryPicture'
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountStoryPicture]') AND name = N'Location')
EXEC('UPDATE dbo.AccountStoryPicture SET Position = Location')
GO
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AccountStoryPicture]') AND name = N'Location') 
ALTER TABLE dbo.AccountStoryPicture DROP COLUMN Location
GO
EXEC [sp_add_picture_position] @table_name = 'PlacePicture'
GO
DROP PROCEDURE dbo.[sp_add_picture_position]
GO
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Account]') AND name = N'LCID') 
ALTER TABLE dbo.Account ADD [LCID] int NULL
GO
