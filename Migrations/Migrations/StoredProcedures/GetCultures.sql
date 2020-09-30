CREATE OR ALTER PROCEDURE [dbo].GetCultures
	@SearchTerm NVARCHAR(MAX)
AS
BEGIN
	DECLARE @query NVARCHAR(MAX);
	SET @query = 'SELECT 
					[Id], 
					[Name], 
					[DisplayName], 
					[IsDefault]
				  FROM SupportedCultures ';

	IF @SearchTerm IS NOT NULL
		SET @query = @query + 'WHERE ([Name] LIKE @SearchTerm OR [DisplayName] LIKE @SearchTerm) ';

	SET @query = @query + 'ORDER BY [IsDefault] DESC, [DisplayName];';

	EXEC sp_executesql @query, N'@SearchTerm NVARCHAR(MAX)', @SearchTerm;
END