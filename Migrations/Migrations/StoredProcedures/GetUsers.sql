CREATE OR ALTER PROCEDURE [dbo].GetUsers
	@SearchTerm NVARCHAR(MAX),
	@RoleId NVARCHAR(36)
AS
BEGIN
	DECLARE @query NVARCHAR(MAX);
	DECLARE @conditionKeyword NVARCHAR(20);

	IF @RoleId IS NULL 
		BEGIN
			SET @query = 'SELECT
							[u].Id,
							[u].Firstname,
							[u].Lastname,
							[u].Email,
							[u].DateCreated,
							[u].DateUpdated
						  FROM  [Users] AS [u] ';
			SET @conditionKeyword = ' WHERE ';
		END
	ELSE 
		BEGIN
			SET @query = 'SELECT
							[u].Id,
							[u].Firstname,
							[u].Lastname,
							[u].Email,
							[u].DateCreated,
							[u].DateUpdated
						  FROM  [UserRoles] AS [ur] 
							LEFT JOIN [Users] AS [u] ON [ur].[UserId] = [u].[Id]
							WHERE [ur].[Roleid] = @RoleId ';
			SET @conditionKeyword = ' AND ';
		END;

	IF @SearchTerm IS NOT NULL BEGIN
		SET @query = @query + @conditionKeyword + ' (
				[u].[Firstname] LIKE @SearchTerm OR
				[u].[Lastname] LIKE @SearchTerm OR
				[u].[Email] LIKE @SearchTerm
			) ';
	END;

	SET @query = @query + ' ORDER BY [u].Firstname, [u].Lastname ;';

	EXEC sp_executesql @query, N'@SearchTerm NVARCHAR(MAX), @RoleId NVARCHAR(20)', @SearchTerm, @RoleId;
END;