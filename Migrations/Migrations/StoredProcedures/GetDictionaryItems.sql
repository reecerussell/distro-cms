CREATE OR ALTER PROCEDURE [dbo].GetDictionaryItems
	@CultureName NVARCHAR(14)
AS
	SELECT
		Id,
		CultureName,
		[Key],
		[Value],
		DateCreated,
		DateUpdated
	FROM DictionaryItems
	WHERE CultureName = @CultureName
	ORDER BY [Key];