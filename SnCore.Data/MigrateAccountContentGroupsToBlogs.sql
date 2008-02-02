IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountContentGroup]') AND type in (N'U'))
BEGIN

	DECLARE AccountContentGroupIterator CURSOR
	FOR SELECT AccountContentGroup_Id FROM AccountContentGroup

	OPEN AccountContentGroupIterator
	DECLARE @AccountContentGroupId int

	FETCH NEXT FROM AccountContentGroupIterator INTO @AccountContentGroupId
	WHILE (@@FETCH_STATUS <> -1)
	BEGIN
		PRINT STR(@AccountContentGroupId)
		DECLARE @AccountBlogId int
		DECLARE @AccountId int
		DECLARE @AccountName nvarchar(128)

		-- fetch the account name of the content owner
		SELECT @AccountId = Account.Account_Id, @AccountName = Account.[Name] 
		FROM Account, AccountContentGroup WHERE Account.Account_Id = AccountContentGroup.Account_Id
		AND AccountContentGroup_Id = @AccountContentGroupId

		-- create the blog	
		INSERT INTO AccountBlog ( Account_Id, [Name], [Description], Created, Updated, EnableComments, DefaultViewRows )
		SELECT Account_Id, [Name], [Description], Created, Modified, 1, 5 FROM AccountContentGroup WHERE AccountContentGroup_Id = @AccountContentGroupId
		SET @AccountBlogId = SCOPE_IDENTITY()
		PRINT STR(@AccountBlogId) + ' (' + @AccountName + ')'

		-- copy all content
		INSERT INTO AccountBlogPost ( AccountBlog_Id, Title, Body, Created, Modified, Account_Id, AccountName, EnableComments, Sticky )
		SELECT @AccountBlogId, Tag, [Text], [Timestamp], Modified,  @AccountId, @AccountName, 1, 0
		FROM AccountContent WHERE AccountContentGroup_Id = @AccountContentGroupId	

		DELETE AccountContentGroup WHERE AccountContentGroup_Id = @AccountContentGroupId

		FETCH NEXT FROM AccountContentGroupIterator INTO @AccountContentGroupId
	END

	CLOSE AccountContentGroupIterator
	DEALLOCATE AccountContentGroupIterator

	DROP TABLE AccountContent
	DROP TABLE AccountContentGroup
	DELETE Configuration WHERE OptionName='SnCore.AddContentGroup.Id'
	DELETE Configuration WHERE OptionName='SnCore.PressContentGroup.Id'
	
END