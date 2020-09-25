CREATE OR ALTER PROCEDURE [dbo].GetDictionaryItems
	@CultureName NVARCHAR(14)
AS
	SELECT
		Id,
		[DisplayName] AS [Name],
		[Value]
	FROM DictionaryItems
	WHERE CultureName = @CultureName
	ORDER BY [DisplayName] ASC;