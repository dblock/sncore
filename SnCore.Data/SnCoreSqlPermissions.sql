CREATE PROCEDURE dbo.[sp_sncore_adduser] 
 @username nvarchar(128)
AS
BEGIN
 SET NOCOUNT ON;
 print 'Checking ' + @username + ' ...'
 IF NOT EXISTS ( SELECT * FROM master.dbo.syslogins where name = @username )
 BEGIN
  print 'Creating server principal ' + @username + ' ...' 
  EXECUTE('CREATE LOGIN [' + @username + '] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]')
  END
 IF NOT EXISTS ( SELECT * FROM sysusers where name = @username )
 BEGIN
  print 'Creating user ' + @username + ' ...' 
  EXECUTE('CREATE USER [' + @username + '] FOR LOGIN [' + @username + ']')
  END
 EXECUTE('ALTER AUTHORIZATION ON SCHEMA::[db_owner] TO [' + @username + ']')
 EXECUTE sp_addrolemember N'db_owner', @username
END
GO

DECLARE @machine_name nvarchar(20)
SELECT @machine_name = RTRIM(CONVERT(char(20), SERVERPROPERTY('MachineName')))
DECLARE @aspnet_username nvarchar(128)
SELECT @aspnet_username = @machine_name + '\ASPNET'
EXEC sp_sncore_adduser @username = @aspnet_username
EXEC sp_sncore_adduser @username = 'NT AUTHORITY\NETWORK SERVICE'
EXEC sp_sncore_adduser @username = 'NT AUTHORITY\SYSTEM'
GO
DROP PROCEDURE [sp_sncore_adduser]
GO
