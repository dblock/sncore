EXEC sp_fulltext_database enable
GO
IF NOT EXISTS ( SELECT * FROM sys.fulltext_catalogs WHERE name = 'SnCore' )
BEGIN
 CREATE FULLTEXT CATALOG [SnCore] WITH ACCENT_SENSITIVITY = ON
 END
GO
