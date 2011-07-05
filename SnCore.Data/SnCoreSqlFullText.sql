EXEC sp_fulltext_database enable
GO
IF NOT EXISTS ( SELECT * FROM sys.fulltext_catalogs WHERE name = 'FoodCandy' )
BEGIN
 CREATE FULLTEXT CATALOG [FoodCandy] WITH ACCENT_SENSITIVITY = ON
 END
GO
