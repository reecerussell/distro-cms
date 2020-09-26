CREATE OR ALTER PROCEDURE [dbo].GetDictionaryItems
	@CultureName NVARCHAR(14)
AS
	SELECT
		i.Id,
		i.[DisplayName],
		i.[Value]
	FROM DictionaryItems AS [i]
	INNER JOIN SupportedCultures AS [c] ON c.Id = i.CultureId
	WHERE c.[Name] = @CultureName
	ORDER BY [DisplayName] ASC;