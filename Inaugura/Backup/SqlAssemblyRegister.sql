EXEC sp_configure 'show advanced options' , '1';
go
reconfigure;
go
EXEC sp_configure 'clr enabled' , '1'
go
reconfigure;
-- Turn advanced options back off
EXEC sp_configure 'show advanced options' , '0';
go

DROP FUNCTION Distance
GO
DROP FUNCTION ConvertDateTime
GO
DROP ASSEMBLY [Inaugura.SqlServer]
GO

CREATE ASSEMBLY [Inaugura.SqlServer] FROM 'E:\Dev\Inaugura\Inaugura.SqlServer\bin\Release\Inaugura.SqlServer.dll'
GO
CREATE FUNCTION Distance(@latitude1 float, @longitude1 float, @latitude2 float, @longitude2 float) 
RETURNS float AS EXTERNAL NAME 
[Inaugura.SqlServer].[Inaugura.SqlServer.UDF].Distance

GO
CREATE FUNCTION ConvertDateTime(@str nvarchar(200)) 
RETURNS DateTime AS EXTERNAL NAME 
[Inaugura.SqlServer].[Inaugura.SqlServer.UDF].ConvertDateTime