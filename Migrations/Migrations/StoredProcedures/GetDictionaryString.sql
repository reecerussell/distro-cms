CREATE OR ALTER PROCEDURE [dbo].GetDictionaryString
	@Key NVARCHAR(45),
	@CultureName NVARCHAR(14)
AS
	SELECT i.[Value] 
	FROM DictionaryItems AS i
	INNER JOIN SupportedCultures AS c ON c.id = i.CultureId
	WHERE i.[Key] = @Key
		AND c.[Name] = @CultureName;