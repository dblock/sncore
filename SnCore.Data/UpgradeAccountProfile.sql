DECLARE @group_id int
SELECT @group_id = g.AccountPropertyGroup_Id FROM AccountPropertyGroup g WHERE g.Name = 'Personal Profile'
IF @group_Id IS NULL
BEGIN
 INSERT INTO AccountPropertyGroup ( [Name], [Description] ) 
 VALUES ( 'Personal Profile', 'profile information available to other users' )
 SET @group_id = SCOPE_IDENTITY()
END

DECLARE @property_id int
SELECT @property_id = p.AccountProperty_Id FROM AccountProperty p
WHERE p.Name = 'About Me' AND p.AccountPropertyGroup_Id = @group_id
IF @property_id IS NULL
BEGIN
 INSERT INTO AccountProperty ( [Name], [Description], [TypeName], [DefaultValue], [Publish], [AccountPropertyGroup_Id]) 
 VALUES ( 'About Me', 'personal information available to other users', 'System.String', '', 1, @group_id )
 SET @property_id = SCOPE_IDENTITY()
END

IF EXISTS (SELECT * FROM sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[AccountProfile'))
DROP FULLTEXT INDEX ON [dbo].[AccountProfile]

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountProfile]') AND type in (N'U'))
BEGIN
 INSERT INTO AccountPropertyValue ( [Account_Id], [AccountProperty_Id], [Value], [Created], [Modified] )
 SELECT Account_Id, @property_id, AboutSelf, Updated, Updated FROM AccountProfile
 DROP TABLE AccountProfile
END

