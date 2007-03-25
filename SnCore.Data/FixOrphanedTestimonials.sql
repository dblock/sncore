DECLARE DiscussionIterator Cursor
FOR SELECT source.Discussion_Id, target.Discussion_Id 
FROM Discussion source, Discussion target
WHERE source.Name = 'Testimonials' AND target.Name = 'Testimonials'
AND source.Object_Id = 0 AND target.Object_Id = source.Account_Id
AND source.Account_Id = target.Account_Id

OPEN DiscussionIterator
DECLARE @SourceDiscussion_id int
DECLARE @TargetDiscussion_id int

FETCH NEXT FROM DiscussionIterator INTO @SourceDiscussion_id, @TargetDiscussion_id
WHILE (@@FETCH_STATUS <> -1)
BEGIN
	PRINT STR(@SourceDiscussion_id) + ' -> ' + STR(@TargetDiscussion_id)
	UPDATE DiscussionThread SET Discussion_Id = @TargetDiscussion_id WHERE Discussion_Id = @SourceDiscussion_id
	DELETE Discussion WHERE Discussion_Id = @SourceDiscussion_id
	FETCH NEXT FROM DiscussionIterator INTO @SourceDiscussion_id, @TargetDiscussion_id
END

CLOSE DiscussionIterator
DEALLOCATE DiscussionIterator
