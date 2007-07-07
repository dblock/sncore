DECLARE DiscussionIterator Cursor
FOR SELECT Discussion_Id, MAX(Modified) AS 'Modified' from DiscussionThread
GROUP by Discussion_Id

OPEN DiscussionIterator
DECLARE @Discussion_Id int
DECLARE @Modified datetime

FETCH NEXT FROM DiscussionIterator INTO @Discussion_Id, @Modified
WHILE (@@FETCH_STATUS <> -1)
BEGIN
	PRINT STR(@Discussion_Id)
	UPDATE Discussion SET Modified = @Modified WHERE Discussion_Id = @Discussion_Id
	FETCH NEXT FROM DiscussionIterator INTO @Discussion_Id, @Modified
END

CLOSE DiscussionIterator
DEALLOCATE DiscussionIterator
