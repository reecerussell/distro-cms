CREATE OR ALTER PROCEDURE [dbo].GetRoles
	@SearchTerm NVARCHAR(45)
AS
BEGIN
	DECLARE @query NVARCHAR(MAX);
	SET @query = 'SELECT 
					Id,
					[Name]
				FROM [Roles] ';

	IF @SearchTerm IS NOT NULL
		SET @query = @query + 'WHERE [Name] LIKE @SearchTerm ';

	SET @query = @query + 'ORDER BY [Name];'

	EXEC sp_executesql @query, N'@SearchTerm NVARCHAR(45)', @SearchTerm;
END